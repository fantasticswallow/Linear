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
            PropertyOrField,
            Property,
            Method
        }
        internal MethodType Type { get; set; }
        internal string baseStr { get; set; }

        internal Type[] GetArgumentTypes()
        {
            if (this.Args == null)
            {
                return null;
            }
            return this.Args.Select(_ => _.GetType2()).ToArray();
        }
    }

    internal class ArgumentInfo
    {
        internal string Value { get; set; }
        internal enum ArgumentType
        {
            String,
            Char,
            Integer,
            Long,
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
                case Core.ArgumentInfo.ArgumentType.Long:
                    return long.Parse(this.Value);
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

        internal Type GetType2()
        {
            switch (this.Type)
            {
                case ArgumentInfo.ArgumentType.Boolean:
                    return typeof(bool);
                case ArgumentInfo.ArgumentType.String:
                    return typeof(string);
                case ArgumentInfo.ArgumentType.Double:
                    return typeof(double);
                case ArgumentInfo.ArgumentType.Long:
                    return typeof(long);
                case ArgumentInfo.ArgumentType.Integer:
                    return typeof(int);
                default:
                    return null;
            }
        }

        

    }
}
