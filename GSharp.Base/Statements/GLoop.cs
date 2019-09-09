using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GLoop : GStatement
    {
        #region Constants
        private readonly List<GStatement> listStatement = new List<GStatement>();
        #endregion

        #region Properties
        public GObject GNumber { get; set; }
        #endregion

        #region Initializer
        public GLoop()
        {

        }

        public GLoop(GObject number)
        {
            GNumber = number;
        }
        #endregion

        public void Append(GStatement obj)
        {
            listStatement.Add(obj);
        }

        public override string ToSource()
        {
            var builderCode = new StringBuilder();

            if (GNumber != null)
            {
                var varName = "_" + GetHashCode();
                builderCode.AppendFormat("for (int {0} = 0; {0} < {1}.ToNumber(); {0}++)\n{{\n", varName, GNumber.ToSource());
            }
            else
            {
                builderCode.AppendLine("while (true)\n{\n");
            }

            foreach (var statement in listStatement)
            {
                builderCode.Append(ConvertAssistant.Indentation(statement.ToSource()));
            };

            builderCode.Append("};");

            return builderCode.ToString();
        }
    }
}
