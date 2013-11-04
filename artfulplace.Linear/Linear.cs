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
            this.IsSafeMode = true;
            this.MemoriedCollection = new Dictionary<string, IEnumerable>();
            this.ExtendExpressionCollection = new Dictionary<string, Expression>();
            this.ExpressionCompileCollection = new Dictionary<string, Func<MethodInfo, List<ParameterExpression>, Expression>>();
            builder.LinearReference = this;
        }

        public Linear(bool safeMode)
        {
            this.IsSafeMode = safeMode;
            this.MemoriedCollection = new Dictionary<string, IEnumerable>();
            this.ExtendExpressionCollection = new Dictionary<string, Expression>();
            this.ExpressionCompileCollection = new Dictionary<string, Func<MethodInfo, List<ParameterExpression>, Expression>>();
            builder.LinearReference = this;
        }

        /// <summary>
        /// Get SourceReference Can Call Runtime Method.
        /// </summary>
        public bool IsSafeMode { get; private set; }

        public Dictionary<string, Expression> ExtendExpressionCollection { get; set; }
        public Dictionary<string, Func<MethodInfo,List<ParameterExpression>,Expression>> ExpressionCompileCollection { get; set; }

        private artfulplace.Linear.Lambda.ExpressionBuilder2 builder = new artfulplace.Linear.Lambda.ExpressionBuilder2();

        /// <summary>
        /// Collection References to Use From Collection 
        /// </summary>
        internal Dictionary<string, IEnumerable> MemoriedCollection { get; set; }
        
        public void AddCollection<T>(string name, IEnumerable<T> collection)
        {
            MemoriedCollection.Add(name, collection);
        }

        /// <summary>
        /// Infrastructure. Get Collection from Memoried Collection.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IEnumerable From(string name)
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

        private IEnumerable From(SourceReference refer)
        {
            return refer.Result(this.IsSafeMode);
        }

        internal IEnumerable FromResolver(MethodInfo i)
        {
            var arg1 = i.Args.First();
            if (arg1.Type == ArgumentInfo.ArgumentType.String)
            {
                return this.From(arg1.GetValue().ToString());
            }
            else
            {
                if ((arg1.Value.StartsWith("SourceReference(")) || (arg1.Value.StartsWith("$SR(")))
                {
                    if (CallingAssembly == null)
                    {
                        throw new NullReferenceException("SourceReferenceはLinear.CallingAssemblyが設定されていない場合は利用できません。");
                    }
                    var tName = arg1.Value.Remove(arg1.Value.Length - 1);
                    tName = tName.Replace("SourceReference(", "");
                    tName = tName.Replace("$SR(", "");
                    var sr = new SourceReference(tName, CallingAssembly);
                    return this.From(sr);
                }
                else
                {
                    throw new ArgumentException("Fromの引数はstring、またはSourceReference(Method)でなくてはいけません");
                }
            }
        }

        public System.Reflection.Assembly CallingAssembly { get; set; }

        /// <summary>
        /// Get Applied text query Collection from Linear Object.
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="target">Text Query to apply collection</param>
        /// <returns></returns>
        public IEnumerable<T> GetResult<T>(string target)
        {
            var info = MethodParser.MethodParse(target).ToArray();
            var colInfo = info[0];
            IEnumerable col;
            switch (colInfo.Name)
            {
                case "From":
                    col = FromResolver(colInfo);
                    break;
                default:
                    throw new ArgumentException("Collectionが指定されていません。クエリの最初は From(string),または From(SourceReference(Method)) から始める必要があります。");

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
                    // FromではGenericArgumentsTypeが取れないので別に与える
                    sourceCache = MethodBuilder.MethodBuild(sourceCache, _, xt,builder);
                }
                else 
                {
                    sourceCache = MethodBuilder.MethodBuild(sourceCache, _, builder);
                }
            });
            var source2 = new LinearQueryable(source1, sourceCache);
            return (IEnumerable<T>)source2.Provider.Execute(sourceCache);
        }

        public bool ElementIs<T>(T obj,string source)
        {
            var bParser = new BracketParser();
            var info = LambdaParser.Parse(source,bParser.Parse(source));
            var expr = builder.DynamicBuild(info, new Type[] { typeof(T) });
            var dele = (Func<T,bool>)expr.Compile();
            return dele.Invoke(obj);
        }

        public TResult ElementTo<T,TResult>(T obj, string source)
        {
            var bParser = new BracketParser();
            var info = LambdaParser.Parse(source, bParser.Parse(source));
            var expr = builder.DynamicBuild(info, new Type[] { typeof(T) });
            var dele = (Func<T, TResult>)expr.Compile();
            return dele.Invoke(obj);
        }

        /// <summary>
        /// Get Applied text query Element from Linear Object.
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="target">Text Query to apply collection</param>
        /// <returns></returns>
        public T GetElement<T>(string target)
        {
            var info = MethodParser.MethodParse(target).ToArray();
            var colInfo = info[0];
            IEnumerable col;
            switch (colInfo.Name)
            {
                case "From":
                    col = FromResolver(colInfo);
                    break;
                default:
                    throw new ArgumentException("Collectionが指定されていません。クエリの最初は From(string),または From(SourceReference(Method)) から始める必要があります。");

            }
            var source1 = col.AsQueryable();
            var xt = source1.GetType().GenericTypeArguments.First();
            var gent = typeof(LinearQueryable<>).MakeGenericType(xt);
            var source3 = (IQueryable)Activator.CreateInstance(gent, source1);
            Expression sourceCache;
            sourceCache = Expression.Constant(source3, gent);
            info.Skip(1).ForEach((_, idx) =>
            {
                if (idx == 0)
                {
                    // FromではGenericArgumentsTypeが取れないので別に与える
                    sourceCache = MethodBuilder.MethodBuild(sourceCache, _, xt, builder);
                }
                else
                {
                    sourceCache = MethodBuilder.MethodBuild(sourceCache, _, builder);
                }
            });
            var source2 = new LinearQueryable(source1, sourceCache);
            return (T)source2.Provider.Execute(sourceCache);
        }

        /// <summary>
        /// Get Applied Text Query Collection From Argument collection
        /// </summary>
        /// <typeparam name="T">Type of Source</typeparam>
        /// <typeparam name="TResult">Type of Result Collection</typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
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
