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
            //var list = TestBinder.MethodParseTest(test);
            //list.ForEach(_ => Console.WriteLine(_));
            Console.WriteLine(test7);
            Console.WriteLine("");
            var obj = TestBinder.LambdaExpressionDynamicTest(Enumerable.Range(1,100).AsQueryable<int>(),test7);
            obj.ForEach(x => Console.WriteLine(x));
        }

        
    }
}
