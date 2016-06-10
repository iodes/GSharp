using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using GSharp.Extension;
using GSharp.Extension.Optionals;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Scopes;
using GSharp.Graphic.Statements;
using GSharp.Extension.Abstracts;
using GSharp.Extension.Attributes;
using GSharp.Graphic.Objects.Strings;
using GSharp.Graphic.Objects.Logics;
using GSharp.Graphic.Objects.Customs;
using GSharp.Graphic.Objects.Numbers;
using GSharp.Graphic.Objects;

namespace GSharp.Manager
{
    public class ExtensionManager
    {
        #region 속성
        public string Path { get; set; }

        public List<GExtension> Extensions { get; set; } = new List<GExtension>();
        #endregion

        #region 객체
        private static Type[] numberTypes = new Type[] {
            typeof(char),

            typeof(short),
            typeof(ushort),

            typeof(int),
            typeof(uint),

            typeof(long),
            typeof(ulong),

            typeof(float),
            typeof(double),
        };
        #endregion

        #region 생성자
        public ExtensionManager()
        {

        }

        public ExtensionManager(string valuePath)
        {
            Path = valuePath;

            foreach (string path in Directory.GetFiles(valuePath, "*.ini", SearchOption.AllDirectories))
            {
                INI ini = new INI(path);
                string parentName = Directory.GetParent(path).FullName;
                string extensionPath = ini.GetValue("Assembly", "File").Replace("<%LOCAL%>", parentName);

                if (File.Exists(extensionPath))
                {
                    GExtension extension = LoadExtension(extensionPath);
                    extension.Path = extensionPath;
                    extension.Title = ini.GetValue("General", "Title");
                    extension.Author = ini.GetValue("General", "Author");
                    extension.Summary = ini.GetValue("General", "Summary");

                    string dependenciesDir = ini.GetValue("Assembly", "Dependencies").Replace("<%LOCAL%>", parentName);
                    if (dependenciesDir.Length > 0 && Directory.Exists(dependenciesDir))
                    {
                        extension.Dependencies = dependenciesDir;
                    }

                    Extensions.Add(extension);
                }
            }

            Extensions = (from extension in Extensions orderby extension.Title ascending select extension).ToList();
        }
        #endregion

        #region 내부 함수
        private T GetAttribute<T>(ICustomAttributeProvider info)
        {
            object[] attributes = info.GetCustomAttributes(true);
            if (attributes.Length > 0)
            {
                foreach (object attribute in attributes)
                {
                    if (attribute.GetType() == typeof(T))
                    {
                        return (T)attribute;
                    }
                }
            }

            return default(T);
        }

        private GParameter[] GetParameters(MethodInfo info)
        {
            List<GParameter> result = new List<GParameter>();

            foreach (ParameterInfo infoParam in info.GetParameters())
            {
                GParameterAttribute paramCommand = GetAttribute<GParameterAttribute>(infoParam);
                if (paramCommand != null)
                {
                    result.Add(new GParameter(infoParam.Name, infoParam.Name, paramCommand.Name, infoParam.ParameterType));
                }
                else
                {
                    result.Add(new GParameter(infoParam.Name, infoParam.Name, string.Empty, infoParam.ParameterType));
                }
            }

            return result.ToArray();
        }
        #endregion

        #region 사용자 함수
        /// <summary>
        /// 확장 모듈을 불러옵니다.
        /// </summary>
        /// <param name="pathValue">확장 모듈의 전체 경로입니다.</param>
        public GExtension LoadExtension(string pathValue)
        {
            Assembly targetAssembly = Assembly.LoadFile(pathValue);
            AssemblyName[] name = targetAssembly.GetReferencedAssemblies();

            // 객체 생성
            GExtension target = new GExtension();
            target.Namespace = targetAssembly.GetName().Name;

            // 클래스 분석
            foreach (Type value in targetAssembly.GetExportedTypes())
            {
                if (value.BaseType == typeof(GModule))
                {
                    // 속성 분석
                    foreach (PropertyInfo property in value.GetProperties())
                    {
                        GCommandAttribute command = GetAttribute<GCommandAttribute>(property);
                        if (command != null)
                        {
                            // 커멘드 목록 추가
                            target.Commands.Add(
                                new GCommand
                                (
                                    target,
                                    value.FullName,
                                    property.Name,
                                    command.Name,
                                    property.GetMethod.ReturnType,
                                    GCommand.CommandType.Property
                                )
                            );
                        }
                    }

                    // 메소드 분석
                    foreach (MethodInfo info in value.GetMethods())
                    {
                        GCommandAttribute command = GetAttribute<GCommandAttribute>(info);
                        if (command != null)
                        {
                            // 커멘드 목록 추가
                            target.Commands.Add(
                                new GCommand
                                (
                                    target,
                                    value.FullName,
                                    info.Name,
                                    command.Name,
                                    info.ReturnType,
                                    info.ReturnType == typeof(void) ? GCommand.CommandType.Call : GCommand.CommandType.Logic,
                                    GetParameters(info)
                                )
                            );
                        }
                    }

                    // 이벤트 분석
                    foreach (EventInfo info in value.GetEvents())
                    {
                        GCommandAttribute command = GetAttribute<GCommandAttribute>(info);
                        if (command != null)
                        {
                            // 대리자 검색
                            Type eventDelegate = null;
                            MethodInfo eventDelegateMethod = null;
                            foreach (Type typeDelegate in value.GetNestedTypes(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                            {
                                if (typeDelegate == info.EventHandlerType)
                                {
                                    eventDelegate = typeDelegate;
                                    break;
                                }
                            }
                            if (eventDelegate != null)
                            {
                                eventDelegateMethod = eventDelegate.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                            }

                            // 커멘드 목록 추가
                            target.Commands.Add(
                                new GCommand
                                (
                                    target,
                                    value.FullName,
                                    info.Name,
                                    command.Name,
                                    eventDelegateMethod?.ReturnType,
                                    GCommand.CommandType.Event,
                                    eventDelegateMethod != null ? GetParameters(eventDelegateMethod) : null
                                )
                            );
                        }
                    }

                    // 열거형 분석
                    foreach (Type enumeration in value.GetNestedTypes())
                    {
                        GCommandAttribute command = GetAttribute<GCommandAttribute>(enumeration);
                        if (command != null && enumeration.IsEnum)
                        {
                            // 열거형 필드 분석
                            List<GEnumeration> gEnumList = new List<GEnumeration>();
                            foreach (FieldInfo enumerationField in enumeration.GetFields())
                            {
                                GFieldAttribute fieldCommand = GetAttribute<GFieldAttribute>(enumerationField);
                                if (fieldCommand != null)
                                {
                                    gEnumList.Add(new GEnumeration(enumerationField.Name, $"{value.FullName}.{enumeration.Name}.{enumerationField.Name}", fieldCommand.Name, enumerationField.FieldType));
                                }
                            }

                            // 커멘드 목록 추가
                            target.Commands.Add(
                                new GCommand
                                (
                                    target,
                                    value.FullName,
                                    enumeration.Name,
                                    command.Name,
                                    enumeration,
                                    GCommand.CommandType.Enum,
                                    gEnumList.ToArray()
                                )
                            );
                        }
                    }
                }
                else if (value.BaseType == typeof(GView))
                {
                    target.Controls.Add(new GControl(target, value, value.FullName));
                }
            }

            target.Commands = (from command in target.Commands orderby command.MethodType descending, command.FriendlyName ascending select command).ToList();

            return target;
        }

        /// <summary>
        /// 모듈에 포함된 모든 함수를 블럭 배열로 변환합니다.
        /// </summary>
        /// <param name="target">블럭 배열로 변환할 모듈 객체입니다.</param>
        public BaseBlock[] ConvertToBlocks(GExtension target)
        {
            var blockList = new List<BaseBlock>();
            
            foreach (var command in target.Commands)
            {
                switch (command.MethodType)
                {
                    case GCommand.CommandType.Call:

                        if (command.ObjectType == typeof(void))
                        {
                            blockList.Add(new VoidCallBlock(command));
                            break;
                        }

                        blockList.Add(new ObjectCallBlock(command));
                        break;

                    case GCommand.CommandType.Logic:
                        blockList.Add(new ObjectCallBlock(command));
                        break;

                    case GCommand.CommandType.Event:
                        blockList.Add(new EventBlock(command));
                        break;

                    case GCommand.CommandType.Property:
                        blockList.Add(new PropertyBlock(command));
                        break;
                        
                    case GCommand.CommandType.Enum:
                        blockList.Add(new EnumBlock(command));
                        break;
                }
            }

            return blockList.ToArray();
        }
        #endregion
    }
}
