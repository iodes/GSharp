using GSharp.Base.Objects;
using GSharp.Base.Objects.Customs;
using GSharp.Base.Objects.Logics;
using GSharp.Base.Objects.Numbers;
using GSharp.Base.Objects.Strings;
using GSharp.Extension;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Holes;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Objects.Customs;
using GSharp.Graphic.Objects.Logics;
using GSharp.Graphic.Objects.Numbers;
using GSharp.Graphic.Objects.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GSharp.Graphic
{
    /// <summary>
    /// Interface로 구현된 IVariable, IModule 등과 같은 객체를 생성하는 등의 몇가지 함수를 모아둔 클래스
    /// </summary>
    public static class BlockUtils
    {
        /// <summary>
        /// 숫자 관련 자료형을 담은 배열
        /// sbyte, byte, short, ushort, int, uint, long, ulong, float, double을 포함합니다.
        /// </summary>
        private static Type[] numberTypes = new Type[] {
            typeof(sbyte), // SByte
            typeof(byte), // Byte
            typeof(short), // Int16
            typeof(ushort), // UInt16
            typeof(int), // Int32
            typeof(uint), // UInt32
            typeof(long), // Int64
            typeof(ulong), // UInt64
            typeof(float), // Single
            typeof(double), // Double
        };

        /// <summary>
        /// 모듈 블럭들에게 필요한 구멍을 자동으로 찾아서 생성하고 위치시킵니다.
        /// </summary>
        /// <param name="Command">모듈을 호출하기 위해 필요한 GCommand 객체</param>
        /// <param name="Content">생성한 구멍과 내용을 추가할 Panel 객체</param>
        /// <returns>생성한 모든 구멍을 반환</returns>
        public static List<BaseHole> SetContent(GCommand Command, Panel Content)
        {
            // 기존 패널에 있는 내용 삭제
            Content.Children.Clear();

            // 반환을 위한 구멍 목록 생성
            var holeList = new List<BaseHole>();

            // Command의 FriendlyName에서 구멍 파싱
            var start = -1;
            var last = 0;

            for (int i = 0; i < Command.FriendlyName.Length; i++)
            {
                var chr = Command.FriendlyName[i];

                if (chr == '{')
                {
                    start = i;
                }

                else if (chr == '}' && start >= 0)
                {
                    var text = Command.FriendlyName.Substring(last, start - last);
                    var holeNumber = Command.FriendlyName.Substring(start + 1, i - start - 1);

                    int number;
                    if (int.TryParse(holeNumber, out number) && 0 <= number && number < Command.Optionals.Length)
                    {
                        BaseHole hole = CreateHole(Command.Optionals[number].ObjectType);
                        hole.Foreground = new BrushConverter().ConvertFromString("#086748") as Brush;
                        hole.VerticalAlignment = VerticalAlignment.Center;

                        Content.Children.Add(new TextBlock
                        {
                            Text = text,
                            FontWeight = FontWeights.Bold,
                            Foreground = Brushes.White,
                            VerticalAlignment = VerticalAlignment.Center
                        });
                        Content.Children.Add(hole);
                        holeList.Add(hole);

                        start = -1;
                        last = i + 1;
                    }
                }
            }

            var lastText = Command.FriendlyName.Substring(last);
            Content.Children.Add(new TextBlock
            {
                Text = lastText,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            });


            // 구멍 목록 반환
            return holeList;
        }

        /// <summary>
        /// 객체를 끼울 수 있는 구멍을 만드는 함수
        /// </summary>
        /// <param name="holeType">구멍에 끼울 객체의 자료형</param>
        /// <returns>
        /// 매개변수의 자료형이 string이면 StringHole을 반환
        /// 매개변수의 자료형이 bool이면 LogicHole을 반환
        /// 매개변수의 자료형이 숫자 형태(<see cref="numberTypes" />)인 경우 NumberHole을 반환
        /// 이 외의 경우 CustomHole을 반환합니다.
        /// </returns>
        public static BaseHole CreateHole(Type holeType)
        {
            // 자료형이 string인 경우
            if (holeType == typeof(string))
            {
                return new StringHole
                {
                    Foreground = new BrushConverter().ConvertFromString("#086748") as Brush
                };
            }

            // 자료형이 bool인경우
            if (holeType == typeof(bool))
            {
                return new LogicHole();
            }

            // 자료형이 숫자 형태인 경우
            if (numberTypes.Contains(holeType))
            {
                return new NumberHole();
            }

            // 그 외의 경우
            return new CustomHole(holeType);
        }


        /// <summary>
        /// 매개변수 블럭을 생성해 주는 함수
        /// </summary>
        /// <param name="variableName">
        /// 매개변수의 이름
        /// 매개변수의 이름은 "param" + "매개변수의 이름(첫번째 글자만 대문자로 변경)"이 됩니다.
        /// </param>
        /// <param name="variableType">매개변수의 자료형</param>
        /// <returns>
        /// 매개변수의 자료형이 string이면 StringVariableBlock을 반환
        /// 매개변수의 자료형이 bool이면 LogicVariableBlock을 반환
        /// 매개변수의 자료형이 숫자 형태(<see cref="numberTypes" />)인 경우 NumberVariableBlock을 반환
        /// 이 외의 경우 CustomVariableBlock을 반환합니다.
        /// </returns>
        public static IVariableBlock CreateParameterVariable(string variableName, Type variableType)
        {
            // 매개변수 이름을 첫번째 글자를 대문자로 바꾸고 param을 붙임
            variableName = "param" + variableName.First().ToString().ToUpper() + variableName.Substring(1);

            // string인 경우
            if (variableType == typeof(string))
            {
                return new StringVariableBlock(new GStringVariable(variableName));
            }

            // bool인 경우
            if (variableType == typeof(bool))
            {
                return new LogicVariableBlock(new GLogicVariable(variableName));
            }

            // 자료형이 숫자 형태인 경우
            if (numberTypes.Contains(variableType))
            {
                return new NumberVariableBlock(new GNumberVariable(variableName));
            }

            // 이 외의 경우 모두 CustomHole로
            return new CustomVariableBlock(new GCustomVariable(variableType, variableName));
        }

        /// <summary>
        /// 일반 변수 블럭을 생성하는 함수입니다.
        /// </summary>
        /// <param name="variableName">
        /// 변수의 이름
        /// 매개변수의 이름은 "var" + "매개변수의 이름(첫번째 글자만 대문자로 변경)"이 됩니다.
        /// </param>
        /// <param name="variableType">변수의 자료형</param>
        /// <returns>
        /// 매개변수의 자료형이 string이면 StringVariableBlock을 반환
        /// 매개변수의 자료형이 bool이면 LogicVariableBlock을 반환
        /// 매개변수의 자료형이 숫자 형태(<see cref="numberTypes" />)인 경우 NumberVariableBlock을 반환
        /// 이 외의 경우 CustomVariableBlock을 반환
        /// </returns>
        public static IVariableBlock CreateVariableBlock(string variableName, Type variableType)
        {
            // 매개변수 이름을 첫번째 글자를 대문자로 바꾸고 param을 붙임
            variableName = "var" + variableName.First().ToString().ToUpper() + variableName.Substring(1);

            // string인 경우
            if (variableType == typeof(string))
            {
                return new StringVariableBlock(new GStringVariable(variableName));
            }

            // bool인 경우
            if (variableType == typeof(bool))
            {
                return new LogicVariableBlock(new GLogicVariable(variableName));
            }

            // 자료형이 숫자 형태인 경우
            if (numberTypes.Contains(variableType))
            {
                return new NumberVariableBlock(new GNumberVariable(variableName));
            }

            // 이 외의 경우 모두 CustomHole로
            return new CustomVariableBlock(new GCustomVariable(variableType, variableName));
        }
    }
}
