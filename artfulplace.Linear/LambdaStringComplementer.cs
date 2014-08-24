using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artfulplace.Linear
{
    /// <summary>
    /// Lambda Expressions単体適用における一引数の式の補完を行います
    /// </summary>
    internal class LambdaStringComplementer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static string Check(string target)
        {
            if (target.Contains("=>"))
            {
                return target;
            }
            else
            {
                return Complement(target);
            }
        }

        private readonly static char[] ignoreChar = { '(', '[', '"' , '#', '$', '\'' };

        private static string Complement(string target)
        {
            var pn = target.Remove(target.IndexOf('.'));
            if (pn.IndexOfAny(ignoreChar) > 0)
            {
                if (target.Contains("_"))
                {
                    return "_ => " + target;
                }
                else
                {
                    return "x => " + target;
                }
            }
            else
            {
                return string.Format("{0} => {1}", pn, target);
            }
        }

    }
}
