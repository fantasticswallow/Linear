using artfulplace.Linear.Core;
using artfulplace.Linear.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace artfulplace.Linear
{
    /// <summary>
    /// Linear Collection Object
    /// </summary>
    public class Linear
    {

        public Linear()
        {
            this.MemoriedCollection = new Dictionary<string, IEnumerable>();
        }

        public Dictionary<string, IEnumerable> MemoriedCollection { get; set; }
        
        public IEnumerable From(string name)
        {
            var name2 = name;
            if (MemoriedCollection.ContainsKey(name2))
            {
                return MemoriedCollection[name2];
            }
            else
            {
                throw new ArgumentException("Linear Compile Error : From Database can't find " + name2);
            }
        }

        public IEnumerable From(SourceReference refer)
        {
            return refer.Result();
        }

        public IEnumerable<T> GetResult<T>(string target)
        {
            var info = MethodParser.MethodParse(target).ToArray();
            var colInfo = info[0];
            IEnumerable col;
            switch (colInfo.Name)
            {
                case "From":
                    col = this.From(colInfo.Args.First().GetValue().ToString());
                    break;
                default:
                    throw new ArgumentException("Collectionが指定されていません。クエリの最初は From(string) から始める必要があります。");

            }
            var source1 = col.AsQueryable();
            var xt = source1.GetType().GenericTypeArguments.First();
            var gent = typeof(LinearQueryable<>).MakeGenericType(xt);
            var source3 = (IQueryable)Activator.CreateInstance(gent,source1);
            Expression sourceCache;
            sourceCache = Expression.Constant(source3, gent);
            info.Skip(1).ForEach((_,idx) =>
            {
                if (idx == 0)
                {
                    
                    sourceCache = MethodBuilder.MethodBuild(sourceCache, _, xt);
                }
                else 
                {
                    sourceCache = MethodBuilder.MethodBuild(sourceCache, _);
                }
            });
            var source2 = new LinearQueryable(source1, sourceCache);
            return (IEnumerable<T>)source2.Provider.Execute(sourceCache);
        }

        public static IEnumerable<TResult> ToLinear<T,TResult>(IEnumerable<T> source, string target)
        {
            var source1 = source.AsQueryable<T>();
            var source3 = new LinearQueryable<T>(source1);
            var info = MethodParser.MethodParse(target);
            Expression sourceCache;
            sourceCache = Expression.Constant(source3, source3.GetType());
            info.ForEach(_ =>
            {
                sourceCache = MethodBuilder.MethodBuild(sourceCache, _);
            });
            var source2 = new LinearQueryable(source1, sourceCache);
            return (IEnumerable<TResult>)source2.Provider.Execute(sourceCache);
        }


    }
}
