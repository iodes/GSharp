using GSharp.Graphic.Holes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GSharp.Graphic.Core
{
    public abstract class ModuleBlock
    {
        public static List<BaseHole> SetContent(string name, Type[] argumentList, WrapPanel Content)
        {
            Content.Children.Clear();

            List<BaseHole> holeList = new List<BaseHole>();

            int start = -1;
            int last = 0;

            for (int i = 0; i < name.Length; i++)
            {
                char chr = name[i];

                if (chr == '{')
                {
                    start = i;
                }

                else if (chr == '}' && start >= 0)
                {
                    string text = name.Substring(last, start - last);
                    string holeNumber = name.Substring(start + 1, i - start - 1);
                    int number;
                    if (int.TryParse(holeNumber, out number) && 0 <= number && number < argumentList.Length)
                    {
                        BaseHole hole = BaseHole.CreateHole(argumentList[number]);
                        hole.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        
                        Content.Children.Add(new TextBlock
                        {
                            Text = text,
                            VerticalAlignment = System.Windows.VerticalAlignment.Center
                        });
                        Content.Children.Add(hole);
                        holeList.Add(hole);

                        start = -1;
                        last = i + 1;
                    }
                }
            }

            string lastText = name.Substring(last);
            Content.Children.Add(new TextBlock
            {
                Text = lastText,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            });

            return holeList;
        }
    }
}
