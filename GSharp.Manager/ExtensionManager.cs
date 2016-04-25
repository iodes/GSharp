using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using GSharp.Extension;
using GSharp.Graphic.Core;
using GSharp.Graphic.Logics;
using GSharp.Graphic.Scopes;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Statements;
using GSharp.Extension.Abstracts;
using GSharp.Extension.Attributes;

namespace GSharp.Manager
{
    public class ExtensionManager
    {
        #region 속성
        public string Path { get; set; }

        public List<GExtension> Extensions { get; set; } = new List<GExtension>();
        #endregion

        #region 생성자
        public ExtensionManager(string valuePath)
        {
            Path = valuePath;

            foreach (string path in Directory.GetFiles(valuePath, "*.ini", SearchOption.AllDirectories))
            {
                INI ini = new INI(path);
                string extensionPath = ini.GetValue("Assembly", "File").Replace("<%LOCAL%>", Directory.GetParent(path).FullName);

                if (File.Exists(extensionPath))
                {
                    GExtension extension = LoadExtension(extensionPath);
                    extension.Path = extensionPath;
                    extension.Title = ini.GetValue("General", "Title");
                    extension.Author = ini.GetValue("General", "Author");
                    extension.Summary = ini.GetValue("General", "Summary");

                    Extensions.Add(extension);
                }
            }
        }
        #endregion

        #region 내부 함수
        private GExtension LoadExtension(string pathValue)
        {
            Path = pathValue;

            Assembly targetAssembly = Assembly.LoadFrom(Path);
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
                                    info.ReturnType == typeof(void) ? GCommand.CommandType.Call : GCommand.CommandType.Logic,
                                    (from parameter in info.GetParameters() select parameter.ParameterType).ToArray()
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
                                    GCommand.CommandType.Event,
                                    eventDelegateMethod != null ? (from parameter in eventDelegateMethod.GetParameters() select parameter.ParameterType).ToArray() : null
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

            return target;
        }

        private T GetAttribute<T>(MemberInfo info)
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
        #endregion

        #region 사용자 함수
        /// <summary>
        /// 모듈에 포함된 모든 함수를 블럭 배열로 변환합니다.
        /// </summary>
        /// <param name="target">블럭 배열로 변환할 모듈 객체입니다.</param>
        public BaseBlock[] ConvertToBlocks(GExtension target)
        {
            List<BaseBlock> blockList = new List<BaseBlock>();
            
            foreach (GCommand command in target.Commands)
            {
                switch (command.MethodType)
                {
                    case GCommand.CommandType.Call:
                        blockList.Add(new CallBlock(command));
                        break;

                    case GCommand.CommandType.Logic:
                        blockList.Add(new LogicCallBlock(command));
                        break;

                    case GCommand.CommandType.Event:
                        blockList.Add(new EventBlock(command));
                        break;

                    case GCommand.CommandType.Property:
                        blockList.Add(new PropertyBlock(command));
                        break;
                }
            }

            return blockList.ToArray();
        }
        #endregion
    }
}
