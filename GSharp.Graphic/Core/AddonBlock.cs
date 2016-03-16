using GSharp.Graphic.Holes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GSharp.Graphic.Core
{
    public abstract class AddonBlock
    {
        public static List<BaseHole> SetContent(String name, StackPanel Content)
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
                    string holeName = name.Substring(start + 1, i - start - 1);

                    BaseHole hole = BaseHole.CreateHole(holeName);

                    if (hole != null)
                    {
                        Content.Children.Add(new TextBlock
                        {
                            Text = text
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
                Text = lastText
            });

            return holeList;
        }
    }
}
