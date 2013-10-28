using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace artfulplace.Linear.Core
{
    internal class MethodInfo
    {
        internal string Name { get; set; }
        internal IEnumerable<ArgumentInfo> Args { get; set; }
        internal BracketParseInfo BracketInfo { get; set; }
        internal enum MethodType
        {
            Property,
            Method
        }
        internal MethodType Type { get; set; }
        internal string baseStr { get; set; }

    }

    internal class ArgumentInfo
    {
        internal string Value { get; set; }
        internal enum ArgumentType
        {
            String,
            Char,
            Integer,
            Double,
            Boolean,
            Lambda,
            Variable,
            Method
        }
        internal ArgumentType Type { get; set; }
        internal BracketParseInfo BracketInfo { get; set; }
        internal object GetValue()
        {
            switch (this.Type)
            {
                case Core.ArgumentInfo.ArgumentType.Integer:
                    return int.Parse(this.Value);
                case Core.ArgumentInfo.ArgumentType.Double:
                    return double.Parse(this.Value);
                case Core.ArgumentInfo.ArgumentType.Boolean:
                    return bool.Parse(this.Value);
                case Core.ArgumentInfo.ArgumentType.String:
                    return this.Value;
                default :
                    return null;
            }
        }

    }
}
