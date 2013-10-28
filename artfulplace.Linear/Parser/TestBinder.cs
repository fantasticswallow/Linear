using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using artfulplace.Linear.Core;
using artfulplace.Linear.Linq;
using artfulplace.Linear.Lambda;

namespace artfulplace.Linear
{
    public class TestBinder
    {
        private static List<string> retList;
        public static List<String> BracketParseTest(string target)
        {

            var parser = new BracketParser();
            var info = parser.Parse(target);
            retList = new List<string>();
            test(info);
            return retList;
        }

        private static void test(BracketParseInfo info)
        {
            retList.Add(info.Capture);
            if (info.Children != null)
            {
                info.Children.ForEach(x => test(x));
            }
        }

        public static IEnumerable<string> MethodParseTest(string target)
        {

            var parser = new MethodSpliter();
            var info = parser.RootSplit(target);
            return info;
        }

        public static IEnumerable<string> MethodParseTest2(string target)
        {
            var parser = new BracketParser();
            var info = parser.Parse(target);
            var parser2 = new MethodSpliter();
            var info2 = parser2.RootSplit(target,info);
            retList = new List<string>();
            info2.ForEach(_ => {
                retList.Add(string.Format("{0} => Type:{1}. value:{2}",_.Name,_.Type.ToString(),_.baseStr));
                if (_.Args != null)
                {
                    _.Args.ForEach(x => {
                        retList.Add(string.Format("arg => value:{0} type:{1}", x.Value, x.Type.ToString()));
                        if (x.Type == ArgumentInfo.ArgumentType.Lambda)
                        {
                            var info3 = LambdaParser.Parse(x.Value, x.BracketInfo);
                            retList.Add("lambda =>");
                            info3.Arguments.ForEach(x2 => retList.Add("lambda arg => name:" + x2.Name));
                            info3.Expressions.ForEach(x2 => {
                                retList.Add("lambda expr => ");
                                test12(x2);
                            });
                        }
                    });
                }
                retList.Add("");
            });
            return retList;
        }

        private static void test12(ExpressionBasicInfo info)
        {
            if (info == null)
            {
                return;
            }
            var str = "Expression " + info.ExpressionType.ToString() + " : " + info.ExpressionString1;
            if (!(string.IsNullOrEmpty(info.ExpressionString2)))
            {
                str += " , " + info.ExpressionString2;
            }
            retList.Add(str);
            if (info.Expression1 != null)
            {
                retList.Add("left expr => ");
                test12(info.Expression1);
            }
            if (info.Expression2 != null)
            {
                retList.Add("right expr => ");
                test12(info.Expression2);
            }
        }


        public static List<String> LambdaBracketParseTest(string target)
        {

            var parser = new BracketParser();
            var info = parser.ParseLambda(target);
            retList = new List<string>();
            test2(info);
            return retList;
        }

        private static void test2(BracketParseInfo info)
        {
            retList.Add(info.Result);
            if (info.Children != null)
            {
                info.Children.ForEach(x => test2(x));
            }
        }

        public static List<String> LambdaExpressionTest(IQueryable<int> source,string target)
        {
            var info = MethodParser.MethodParse(target);
            var li = info.First().Args.First();
            var linfo = LambdaParser.Parse(li.Value, li.BracketInfo);
            var expr = ExpressionBuilder.Build<int,bool>(linfo);
            var res = source.Where(expr);
                        
            retList = new List<string>();
            res.ForEach(x => retList.Add(x.ToString()));
            return retList;
        }

        public static List<String> LambdaExpressionDynamicTest(IQueryable<int> source, string target)
        {

            var res = MethodHost.Invoke<int>(source, target);

            retList = new List<string>();
            res.ForEach(x => retList.Add(x.ToString()));
            return retList;
        }

    }
}
