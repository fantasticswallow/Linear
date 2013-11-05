using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear;
using artfulplace.Linear.Linq;
using artfulplace.Linear.RegexExtension;
using System.Text.RegularExpressions;

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
            //var list = TestBinder.MethodParseTest(test);
            //list.ForEach(_ => Console.WriteLine(_));
            var curLinear = new Linear(false);
            curLinear.CallingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            curLinear.AddCollection("Range1", Enumerable.Range(12, 100));
            curLinear.AddCollection("Range2", Enumerable.Range(110, 15));
            var tData = (List<testData1>)testData1.GetData();
            curLinear.AddCollection("TestData1", tData);
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
            Console.WriteLine(test12);
            Console.WriteLine("");
            var obj3 = curLinear.GetResult<testData1>(test12);
            // obj3.ForEach(x => Console.WriteLine(x));
            obj3.ForEach(x => Console.WriteLine(string.Format("{0} : {1} : {2}", x.Name, x.Lv.ToString(), x.ShipType)));
            tData.Add(testData1.Create("Yamato", 70, "BattleShip"));
            tData.Add(testData1.Create("Nagato", 63, "BattleShip"));
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(test12);
            Console.WriteLine("");
            obj3 = curLinear.GetResult<testData1>(test12);
            // obj3.ForEach(x => Console.WriteLine(x));
            obj3.ForEach(x => Console.WriteLine(string.Format("{0} : {1} : {2}", x.Name, x.Lv.ToString(), x.ShipType)));
            //obj3.ForEach(x => {
            //    Console.WriteLine(x.Key + " => ");
            //    x.ForEach(x2 => Console.WriteLine(string.Format("{0} : {1} : {2}", x2.Name, x2.Lv.ToString(), x2.ShipType)));
            //    Console.WriteLine("");
            //});
        }

        
    }
}
