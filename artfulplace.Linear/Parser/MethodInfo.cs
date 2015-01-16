using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace artfulplace.Linear.Core
{
    public class MethodInfo
    {
        public string Name { get; set; }
        public IEnumerable<ArgumentInfo> Args { get; set; }
        internal BracketParseInfo BracketInfo { get; set; }
        public enum MethodType
        {
            PropertyOrField,
            Property,
            Method
        }
        public MethodType Type { get; set; }
        internal string baseStr { get; set; }

        public Type[] GetArgumentTypes()
        {
            if (this.Args == null)
            {
                return null;
            }
            return this.Args.Select(_ => _.GetType2()).ToArray();
        }
    }

    public class ArgumentInfo
    {
        public string Value { get; set; }
        public enum ArgumentType
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
        public ArgumentType Type { get; set; }
        internal BracketParseInfo BracketInfo { get; set; }
        public object GetValue()
        {
            switch (this.Type)
            {
                case Core.ArgumentInfo.ArgumentType.Integer:
                    return int.Parse(this.Value.Trim());
                case Core.ArgumentInfo.ArgumentType.Long:
                    return long.Parse(this.Value.Remove(this.Value.Length - 1).Trim());
                case Core.ArgumentInfo.ArgumentType.Double:
                    return double.Parse(this.Value.Trim());
                case Core.ArgumentInfo.ArgumentType.Boolean:
                    return bool.Parse(this.Value.Trim());
                case Core.ArgumentInfo.ArgumentType.String:
                    return this.Value.Trim();
                default :
                    return null;
            }
        }

        public Type GetType2()
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
