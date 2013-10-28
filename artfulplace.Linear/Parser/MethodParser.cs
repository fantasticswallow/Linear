using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artfulplace.Linear.Core
{
    internal class MethodParser
    {
        internal static IEnumerable<MethodInfo> MethodParse(string target)
        {
            var parser = new BracketParser();
            var info = parser.Parse(target);
            var parser2 = new MethodSpliter();
            return parser2.RootSplit(target, info);
        }


    }
}
