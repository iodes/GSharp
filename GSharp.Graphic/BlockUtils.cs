using GSharp.Base.Objects;
using GSharp.Base.Objects.Customs;
using GSharp.Base.Objects.Logics;
using GSharp.Base.Objects.Numbers;
using GSharp.Base.Objects.Strings;
using GSharp.Base.Utilities;
using GSharp.Extension;
using GSharp.Extension.Optionals;
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
using System.Xml;

namespace GSharp.Graphic
{
    /// <summary>
    /// Interface로 구현된 IVariable, IModule 등과 같은 객체를 생성하는 등의 몇가지 함수를 모아둔 클래스
    /// </summary>
    public static class BlockUtils
    {
        /// <summary>
        /// 모듈 블럭들에게 필요한 구멍을 자동으로 찾아서 생성하고 위치시킵니다.
        /// </summary>
        /// <param name="Command">모듈을 호출하기 위해 필요한 GCommand 객체</param>
        /// <param name="Content">생성한 구멍과 내용을 추가할 Panel 객체</param>
        /// <returns>생성한 모든 구멍을 반환</returns>
        public static List<BaseHole> SetContent(GCommand Command, Panel Content, Brush brush = null)
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

                        if (brush == null)
                        {
                            brush = new BrushConverter().ConvertFromString("#086748") as Brush;
                        }

                        hole.Foreground = brush;
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
        /// 매개변수의 자료형이 숫자 형태인 경우 NumberHole을 반환
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
            if (GSharpUtils.numberTypes.Contains(holeType))
            {
                return new NumberHole();
            }

            // 그 외의 경우
            return new CustomHole(holeType);
        }

        /// <summary>
        /// 변수 블럭을 생성해 주는 함수
        /// </summary>
        /// <param name="variableName">C# 형태에서의 변수의 이름</param>
        /// <param name="variableType">매개변수의 자료형</param>
        /// <param name="friendlyName">블럭으로 노출될 변수의 이름</param>
        /// <returns>
        /// 매개변수의 자료형이 string이면 StringVariableBlock을 반환
        /// 매개변수의 자료형이 bool이면 LogicVariableBlock을 반환
        /// 매개변수의 자료형이 숫자 형태인 경우 NumberVariableBlock을 반환
        /// 이 외의 경우 CustomVariableBlock을 반환합니다.
        /// </returns>
        public static VariableBlock CreateVariableBlock(string variableName, string friendlyName = null)
        {
            GVariable variable = GSharpUtils.CreateGVariable(variableName);

            if (friendlyName == null)
            {
                friendlyName = variableName;
            }

            return new VariableBlock(friendlyName, variable);
        }

        /// <summary>
        /// 변수 블럭을 생성해 주는 함수
        /// </summary>
        /// <param name="variable">IVariable 객체</param>
        /// <param name="friendlyName">블럭으로 노출될 변수의 이름</param>
        /// <returns>
        /// variable이 GStringVariable이면 StringVariableBlock을 반환
        /// variable이 GLogicVariable이면 LogicVariableBlock을 반환
        /// variable이 GNumberVariable이면 NumberVariableBlock을 반환
        /// 이 외의 경우 CustomVariableBlock을 반환합니다.
        /// </returns>
        public static VariableBlock CreateVariableBlock(GVariable variable, string friendlyName = null)
        {
            if (friendlyName == null)
            {
                friendlyName = variable.Name;
            }

            return new VariableBlock(friendlyName, variable);
        }

        public static void SaveGCommand(XmlWriter writer, GCommand command)
        {
            writer.WriteAttributeString("NamespaceName", command.NamespaceName);
            writer.WriteAttributeString("MethodName", command.MethodName);
            writer.WriteAttributeString("FriendlyName", command.FriendlyName);
            writer.WriteAttributeString("MethodType", command.MethodType.ToString());
            writer.WriteAttributeString("ObjectType", command.ObjectType.AssemblyQualifiedName);

            if (command.Optionals != null)
            {
                writer.WriteStartElement("Optionals");

                foreach (var option in command.Optionals)
                {
                    writer.WriteStartElement("Optional");

                    writer.WriteAttributeString("Name", option.Name);
                    writer.WriteAttributeString("FullName", option.FullName);
                    writer.WriteAttributeString("FriendlyName", option.FriendlyName);
                    writer.WriteAttributeString("ObjectType", option.ObjectType.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        public static GCommand LoadGCommand(XmlElement element)
        {
            var namespaceName = element.GetAttribute("NamespaceName");
            var methodName = element.GetAttribute("MethodName");
            var friendlyName = element.GetAttribute("FriendlyName");
            var methodType = element.GetAttribute("MethodType");
            var objectTypeString = element.GetAttribute("ObjectType");
            var objectType = Type.GetType(objectTypeString);

            GCommand.CommandType methodEnum;
            if (!Enum.TryParse(methodType, out methodEnum))
            {
                throw new Exception("CommandType 로드에 실패했습니다.");
            }

            if (objectType == null)
            {
                throw new Exception("ObjectType 로드에 실패했습니다.");
            }

            var options = new List<GOptional>();

            foreach (XmlElement option in element.SelectNodes("Optionals/Optional"))
            {
                var optionName = option.GetAttribute("Name");
                var optionFullName = option.GetAttribute("FullName");
                var optionFriendlyName = option.GetAttribute("FriendlyName");
                var optionObjectTypeString = option.GetAttribute("ObjectType");
                var optionObjectType = Type.GetType(optionObjectTypeString);

                if (optionObjectType == null)
                {
                    throw new Exception("Option의 ObjectType 로드에 실패했습니다.");
                }

                options.Add(new GOptional(optionName, optionFullName, optionFriendlyName, optionObjectType));
            }

            return new GCommand(namespaceName, methodName, friendlyName, objectType, methodEnum, options.ToArray());
        }

        public static void SaveChildBlocks(XmlWriter writer, BaseBlock block, string tagName = "ChildBlocks")
        {
            writer.WriteStartElement(tagName);
            block?.SaveXML(writer);
            writer.WriteEndElement();
        }

        public static void ConnectToHole(BaseHole baseHole, BaseBlock baseBlock)
        {
            if (baseHole is BaseObjectHole)
            {
                (baseHole as BaseObjectHole).BaseObjectBlock = baseBlock as ObjectBlock;
            }
            else if (baseHole is SettableObjectHole)
            {
                (baseHole as SettableObjectHole).SettableObjectBlock = baseBlock as SettableObjectBlock;
            }
            else if (baseHole is NextConnectHole)
            {
                (baseHole as NextConnectHole).StatementBlock = baseBlock as StatementBlock;
            }
        }
    }
}
