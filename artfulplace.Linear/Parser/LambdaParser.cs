using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear.Linq;

namespace artfulplace.Linear.Core
{
    internal class LambdaParser
    {
        internal static Lambda.LambdaInfo Parse(string target,BracketParseInfo brackInfo)
        {
            brackInfo.Children.ForEach((x, idx) =>
            {
                var idx2 = target.IndexOf(x.Result);
                if (idx2 >= 0)
                {
                    target = target.Remove(idx2, x.Result.Length).Insert(idx2, "{" + idx.ToString() + "}");
                }
            });
            var goesTo = new string[] {"=>"};
            var spliter = target.Split(goesTo,StringSplitOptions.None);
            var linfo = new Lambda.LambdaInfo();
            if (spliter[0].Contains("{"))
            {
                var s = spliter[0].Trim();
                var i = int.Parse(s.Remove(s.Length - 1).Remove(0, 1));
                linfo.Arguments = ParseArg(brackInfo.Children[i].Capture);
            }
            else
            {
                linfo.Arguments = ParseArg(spliter[0]);
            }
            var target2 = spliter[1];
            brackInfo.Children.ForEach((x, idx) =>
            {
                var repOld = "{" + idx.ToString() + "}";
                target2 = target2.Replace(repOld, x.Result);
            });
            target2 = target2.Replace(Environment.NewLine, "");
            linfo.BracketInfo = brackInfo;
            linfo.Expressions = target2.Split(';').Select(_ => {
                var exprParser = new Lambda.ExpressionParser();
                return exprParser.ExpressionParse(_);
            });

            return linfo;
        }


        internal static Lambda.LambdaArgumentInfo[] ParseArg(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return null;
            }
            return target.Trim().Split(',').Select(_ =>
            {
                var info = new Lambda.LambdaArgumentInfo();
                _ = _.Trim();
                var sp = _.Split(' ');
                if (sp.Length == 1)
                {
                    info.Name = sp[0];
                    info.IsDefined = false;
                }
                else
                {
                    info.Type = sp[0];
                    info.Name = sp[1];
                    info.IsDefined = true;
                }
                return info;
            }).ToArray();
        }

    }


}
