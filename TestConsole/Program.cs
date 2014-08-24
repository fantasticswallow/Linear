using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear;
using artfulplace.Linear.Linq;
using artfulplace.Linear.RegexExtension;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Collections;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = "From(\"hoge\").Where(_ => _.Result.Contains(\"(])('\\\"\")).Select(_ => _.Replace(\"x1\",\"x2\"))";
            var test2 = "brackInfo.Children.ForEach((x, idx) => Console.WriteLine(x + idx.ToString()))";
            var test3 = "From(\"hoge\").Take(10,200).Where(x => x.value == 100).Test2(true, 20.1,203,\"aaa\")";
            var test4 = "_ => _.hoge() && (_.fuga() || _.piyo)";
            var test5 = "_ => _.hoge() && !(_.fuga() || _.piyo)";
            var test6 = "From(\"hoge\").Where(_ => _ + 10 > (_ - 10) && x - y< _ - 10 || ((x + 5) == (y != 5) && _ != 5) ).Select(_ => _.Replace(\"x1\",\"x2\"))";
            var test7 = "Skip(15)";
            var test8 = "Where(_ => _ % 5 == 0).Select(_ => _.ToString() + \"hoge\")";
            var test9 = "Where(_ => _ % 5 == 0).Select(_ => _ + 2).Select(_ => _.ToString() + \"hoge\")";
            var test9_2 = "Where(_ => _ % 5 == 0).Select(_ => (_ + 2).ToString() + \"hoge\")";
            var test10 = "From(\"Range1\").Where(_ => _ % 5 == 0).Select(_ => _ + 2).Select(_ => _.ToString() + \"hoge\")";
            var test11 = "From(\"Range2\").Where(_ => _ % 5 == 0).Select(_ => _ + 2).Select(_ => _.ToString() + \"hoge\")";
            var test12 = "From(\"TestData1\").Where(_ => _.Lv > 45).OrderByDescending(_ => _.Lv).Take(5)";
            var test13 = "From(\"TestData1\").Reverse()";
            var test14 = "From(SourceReference(testData1.GetData())).OrderByDescending(_ => _.Lv).GroupBy(_ => _.ShipType)";
            var test15 = "From($SR(testData1.GetData())).Reverse()";
            var test16 = "From($SR(testData1.GetData())).Select(_ => _.ShipType.ToLower().ToUpper().ToLower())";
            var test17 = "From(SourceReference(testData1.GetData())).Where(_ => RegexIsMatch(_.ShipType, \"(BattleShip|Aircraft)\"))";
            var test18 = "From(SourceReference(testData1.GetData2())).Where(_ => _.Lv > 45).OrderByDescending(_ => _.Lv).Take(5)";
            var test19 = "() => ConsoleWrite()";
            var test20 = "From(\"Range1\").Where(_ => _ % 5 == 0).Select(_ => StringFormat(\"Nemui {0}\", _.ToString()))";
            var test21 = "From(SourceReference(testData1.GetData())).Select(_ => PropChange(_, \"Lv\",int x => x + 10))";
            var test22 = "From(\"Range1\").Where(_ => _.ToString().Contains(\"6\") == false).Select(_ => _.ToString())";
            var test23 = "TypeMember()";
            var test24 = "_=>_.Contains(\"test\")&&_.Contains(\"hoge\")";
            var test25 = "_ => _.ToString()";
            var test26 = "_.Contains(\"test\")";
            var test27 = "From(\"Range1\").Where(_ => _ <= 20).Select(_ => TypeConvert(_,System.Double)).Select(_ => _$(System.Math).Pow(_,2.0))";
            //var list = TestBinder.MethodParseTest(test);
            //list.ForEach(_ => Console.WriteLine(_));
            var curLinear = new Linear(false);
            curLinear.CallingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            curLinear.AddCollection("Range1", Enumerable.Range(1, 100));
            curLinear.AddCollection("Range2", Enumerable.Range(110, 15));
            Expression<Action> e = () => Console.WriteLine("Test");
            curLinear.ExtendExpressionCollection.Add("ConsoleWrite", e);
            testData1.DataCache = (List<testData1>)testData1.GetData();
            curLinear.WriteAction = s => Console.Write(s);
            // curLinear.AddCollection("TestData1", tData);
            //Console.WriteLine(test10);
            //Console.WriteLine("");
            //var obj = curLinear.GetResult<string>(test10);
            //obj.ForEach(x => Console.WriteLine(x));
            //Console.WriteLine("");
            //Console.WriteLine("");
            //Console.WriteLine(test12);
            //Console.WriteLine("");
            //var obj2 = curLinear.GetResult<testData1>(test12);
            //obj2.ForEach(x => Console.WriteLine(string.Format("{0} : {1} : {2}",x.Name ,x.Lv.ToString(),x.ShipType)));
            //Console.WriteLine("");
            //Console.WriteLine("");
            //Console.WriteLine(test18);
            //Console.WriteLine("");
            //var obj3 = curLinear.GetResult<testData1>(test18);
            //// obj3.ForEach(x => Console.WriteLine(x));
            //obj3.ForEach(x => Console.WriteLine(string.Format("{0} : {1} : {2}", x.Name, x.Lv.ToString(), x.ShipType)));
            //testData1.DataCache.Add(testData1.Create("Yamato", 70, "BattleShip"));
            //testData1.DataCache.Add(testData1.Create("Nagato", 63, "BattleShip"));
            //Console.WriteLine("");
            //Console.WriteLine("");
            //Console.WriteLine("");
            //Console.WriteLine(test18);
            //Console.WriteLine("");
            //obj3 = curLinear.GetResult<testData1>(test18);
            //// obj3.ForEach(x => Console.WriteLine(x));
            //obj3.ForEach(x => Console.WriteLine(string.Format("{0} : {1} : {2}", x.Name, x.Lv.ToString(), x.ShipType)));
            //obj3.ForEach(x => {
            //    Console.WriteLine(x.Key + " => ");
            //    x.ForEach(x2 => Console.WriteLine(string.Format("{0} : {1} : {2}", x2.Name, x2.Lv.ToString(), x2.ShipType)));
            //    Console.WriteLine("");
            //});
            // curLinear.InvokeLambda(test19);
            //Console.WriteLine(test23);
            //Console.WriteLine();
            //var obj = curLinear.GetResult<testData1>(test23);
            // obj.ToArray().ForEach(x => Console.WriteLine(x));
            //Console.WriteLine(test24);
            //Console.WriteLine();
            //Console.WriteLine("Target : hogehoge -> Result : {0}", curLinear.ElementIs("hogehoge", test24));
            //Console.WriteLine("Target : hogetesthoge -> Result : {0}", curLinear.ElementIs("hogetesthoge", test24));
            //Console.WriteLine("Target : test -> Result : {0}", curLinear.ElementIs("test", test24));
            //Console.WriteLine(test25);
            //Console.WriteLine();
            //var res = curLinear.ElementTo<int, string>(12,test25);
            //Console.WriteLine(string.Format("Result : {0}, Type : {1}",res, res.GetType().ToString()));
            //Console.WriteLine(test26);
            //Console.WriteLine();
            //Console.WriteLine("Target : hogehoge -> Result : {0}", curLinear.ElementIs("hogehoge", test26));
            //Console.WriteLine("Target : hogetesthoge -> Result : {0}", curLinear.ElementIs("hogetesthoge", test26));
            Console.WriteLine(test27);
            Console.WriteLine();
            var obj3 = curLinear.GetResult<double>(test27);
            
            obj3.ForEach(x => Console.WriteLine(x));

        }

        
    }
}
