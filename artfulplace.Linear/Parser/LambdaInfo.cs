using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artfulplace.Linear.Lambda
{
    internal class LambdaInfo
    {
        internal LambdaArgumentInfo[] Arguments { get; set; }
        internal IEnumerable<ExpressionBasicInfo> Expressions { get; set; }
        internal Core.BracketParseInfo BracketInfo { get; set; }
    }

    internal class LambdaArgumentInfo
    {
        internal string Name { get; set; }
        internal string Type { get; set; }
        internal bool IsDefined { get; set; }
    }    
}
