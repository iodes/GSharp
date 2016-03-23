using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using GSharp.Extension;
using GSharp.Graphic.Core;
using GSharp.Graphic.Logics;
using GSharp.Graphic.Statements;
using GSharp.Graphic.Scopes;

namespace GSharp.Manager
{
    public class ModuleManager
    {
        #region 속성
        public string Path { get; set; }

        public List<GModule> Modules { get; set; } = new List<GModule>();
        #endregion

        #region 객체
        private Type targetType;
        private object targetObject;
        private Assembly targetAssembly;
        #endregion

        #region 생성자
        public ModuleManager(string valuePath)
        {
            Path = valuePath;

            foreach (string path in Directory.GetFiles(valuePath, "*.ini", SearchOption.AllDirectories))
            {
                INI ini = new INI(path);

                GModule module = LoadModule(ini.GetValue("Assembly", "File").Replace("<%LOCAL%>", Directory.GetParent(path).FullName));
                module.Title = ini.GetValue("General", "Title");
                module.Author = ini.GetValue("General", "Author");
                module.Summary = ini.GetValue("General", "Summary");

                Modules.Add(module);
            }
        }
        #endregion

        #region 내부 함수
        private GModule LoadModule(string pathValue)
        {
            if (File.Exists(pathValue))
            {
                Path = pathValue;

                targetAssembly = Assembly.LoadFrom(Path);
                AssemblyName[] name = targetAssembly.GetReferencedAssemblies();

                foreach (Type value in targetAssembly.GetExportedTypes())
                {
                    if (value.Name == "Main")
                    {
                        targetType = value;
                        targetObject = Activator.CreateInstance(targetType);

                        GModule target = (GModule)targetObject;
                        target.Package = targetType.Assembly.FullName.Split(',')[0];

                        foreach (MethodInfo method in targetType.GetMethods())
                        {
                            object[] attributes = method.GetCustomAttributes(true);
                            if (attributes.Length > 0)
                            {
                                foreach (object attribute in attributes)
                                {
                                    if (attribute.GetType() == typeof(CommandAttribute))
                                    {
                                        CommandAttribute command = attribute as CommandAttribute;
                                        target.Commands.Add(
                                            new GCommand
                                            {
                                                Parent = target,
                                                FriendlyName = command.Name,
                                                MethodName = method.Name,
                                                MethodType = command.Type,
                                                NamespaceName = targetAssembly.GetName().Name
                                            }
                                        );
                                    }
                                }
                            }
                        }

                        return target;
                    }
                }
            }

            return null;
        }
        #endregion

        #region 사용자 함수
        /// <summary>
        /// 모듈에 포함된 모든 함수를 블럭 배열로 변환합니다.
        /// </summary>
        /// <param name="target">블럭 배열로 변환할 모듈 객체입니다.</param>
        public BaseBlock[] ConvertToBlocks(GModule target)
        {
            List<BaseBlock> blockList = new List<BaseBlock>();
            
            foreach (GCommand command in target.Commands)
            {
                switch (command.MethodType)
                {
                    case CommandAttribute.CommandType.Call:
                        blockList.Add(new CallBlock(command));
                        break;

                    case CommandAttribute.CommandType.Logic:
                        blockList.Add(new LogicCallBlock(command));
                        break;

                    case CommandAttribute.CommandType.Event:
                        blockList.Add(new EventBlock(command));
                        break;
                }
            }

            return blockList.ToArray();
        }
        #endregion
    }
}
