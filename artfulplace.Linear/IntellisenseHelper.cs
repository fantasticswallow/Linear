using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace artfulplace.Linear
{
    public class IntellisenseHelper
    {

        private readonly static char[] ignoreChar = { '(', '[', '"', '#', '$', '\'' };

        public static IEnumerable<string> GetLambdaSuggests(string current, Type arg ,int cursor)
        {
            return GetLambdaSuggests(current.Remove(cursor),arg);
        }

        public static IEnumerable<string> GetLambdaSuggests(string current, Type arg)
        {
            var goesTo = new string[] { "=>" };
            var spliter = current.Split(goesTo, StringSplitOptions.None);
            if (spliter.Length == 1)
            {
                var lm = spliter[0];
                if (lm.IndexOf('.') > 0)
                {
                    var lname = lm.Remove(lm.IndexOf('.'));
                    if (lname.IndexOfAny(ignoreChar) > 0)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else
            {
                var lname = spliter[0];
                if (lname.IndexOf(' ') > 0)
                {
                    lname = lname.Substring(lname.IndexOf(' ') + 1);
                }

            }
            return null;
        }

        private static IEnumerable<string> splitMethodSuggesting(string target, string argName, Type arg)
        {
            if (string.IsNullOrEmpty(target))
            {
                return standardMethodNames.Concat(new string[] { argName });
            }
            return null;
        }

        private static exprType getCurrentType(string target)
        {
            var target2 = target.Replace("\\\"","");
            target2 = target2.Replace("\\'", "");
            var qCount = strCount(target2, "\"");
            
            if (qCount % 2 != 0)
            {
                return exprType.String;
            }
            var lc = qCount / 2;
            for (var i = 0; i < lc; i++)
            {
                target2 = omitFirstString(target2, "\"");
            }
            qCount = strCount(target2, "'");
            if (qCount % 2 != 0)
            {
                return exprType.String;
            }
            lc = qCount / 2;
            for (var i = 0; i < lc; i++)
            {
                target2 = omitFirstString(target2, "'");
            }

            return exprType.Normal;

        }

        private static int strCount(string target, string child)
        {
            return target.Length - target.Replace(child, "").Length;
        }

        /// <summary>
        /// 指定したキーワードに囲まれた部位を削除して返します
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string omitFirstString(string target, string key)
        {
            var s = target.IndexOf(key);
            var e = target.Substring(s + 1).IndexOf(key);
            return target.Replace(target.Substring(s, e - s + 1), "");
        }

        private enum exprType
        {
            Normal,
            String,
            MethodBracket
        }


        private readonly static string[] standardMethodNames = { "IsRegexAnd", "IsRegexNot", "IsRegexXor","StringFormat", "RegexIsMatch", "RegexReplace", "RegexSplit","CreateTime", "CreateTimeSpan" };

    }
}
