using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using GSharp.Extension;
using GSharp.Graphic.Statements;
using GSharp.Graphic.Logics;
using GSharp.Graphic.Core;

namespace GSharp.Manager
{
    public class AddonManager
    {
        #region 속성
        public string Path { get; set; }

        public List<Addon> Addons { get; set; } = new List<Addon>();
        #endregion

        #region 객체
        private Type targetType;
        private object targetObject;
        private Assembly targetAssembly;
        #endregion

        #region 생성자
        public AddonManager(string valuePath)
        {
            Path = valuePath;

            foreach (string path in Directory.GetFiles(valuePath, "*.ini", SearchOption.AllDirectories))
            {
                INI ini = new INI(path);

                Addon addon = LoadAddon(ini.GetValue("Assembly", "File").Replace("<%LOCAL%>", Directory.GetParent(path).FullName));
                addon.Title = ini.GetValue("General", "Title");
                addon.Author = ini.GetValue("General", "Author");
                addon.Summary = ini.GetValue("General", "Summary");

                Addons.Add(addon);
            }
        }
        #endregion

        #region 내부 함수
        private Addon LoadAddon(string pathValue)
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

                        Addon target = (Addon)targetObject;
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
                                        target.Commands.Add(new KeyValuePair<string, string>(command.Name, method.Name));
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
        /// 애드온에 포함된 모든 함수를 블럭 배열로 변환합니다.
        /// </summary>
        /// <param name="target">블럭 배열로 변환할 애드온 객체입니다.</param>
        public BaseBlock[] ConvertToBlocks(Addon target)
        {
            List<BaseBlock> blockList = new List<BaseBlock>();
            
            foreach (KeyValuePair<string, string> command in target.Commands)
            {
                AddonLogicBlock block = new AddonLogicBlock
                {
                    Title = target.Title,
                    EXTName = command.Key,
                    EXTMethod = command.Value
                };

                blockList.Add(block);
            }

            return blockList.ToArray();
        }
        #endregion
    }
}
