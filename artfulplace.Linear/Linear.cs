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
            this.ExpressionCompileCollection = new Dictionary<string, Func<MethodInfo, List<ParameterExpression>, Dictionary<string, Expression>, Expression>>();
            this.CompiledQueryable = new Dictionary<string, LinearQueryable>();
            this.CompiledExpressions = new Dictionary<string, LambdaExpression>();
            builder.LinearReference = this;
        }

        public Linear(bool safeMode)
        {
            this.IsSafeMode = safeMode;
            this.MemoriedCollection = new Dictionary<string, IEnumerable>();
            this.ExtendExpressionCollection = new Dictionary<string, Expression>();
            this.ExpressionCompileCollection = new Dictionary<string, Func<MethodInfo, List<ParameterExpression>, Dictionary<string, Expression>, Expression>>();
            this.CompiledQueryable = new Dictionary<string, LinearQueryable>();
            this.CompiledExpressions = new Dictionary<string, LambdaExpression>();
            builder.LinearReference = this;
        }

        /// <summary>
        /// Get SourceReference Can Call Runtime Method.
        /// </summary>
        public bool IsSafeMode { get; private set; }

        public Dictionary<string, Expression> ExtendExpressionCollection { get; set; }
        public Dictionary<string, Func<MethodInfo,List<ParameterExpression>,Dictionary<string,Expression>,Expression>> ExpressionCompileCollection { get; set; }

        public Action<string> WriteAction { get; set; }

        private artfulplace.Linear.Lambda.ExpressionBuilder2 builder = new artfulplace.Linear.Lambda.ExpressionBuilder2();

        /// <summary>
        /// Collection References to Use From Collection 
        /// </summary>
        internal Dictionary<string, IEnumerable> MemoriedCollection { get; set; }

        internal Dictionary<string, LinearQueryable> CompiledQueryable { get; set; }
        internal Dictionary<string, LambdaExpression> CompiledExpressions { get; set; }

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

        /// <summary>
        /// Assembly of Host Application. To use Calling Methods and Get Collection's Reference 
        /// </summary>
        public System.Reflection.Assembly CallingAssembly { get; set; }

        /// <summary>
        /// Get Applied text query Collection from Linear Object.
        /// </summary>
        /// <typeparam name="T">Result Type</typeparam>
        /// <param name="target">Text Query to apply collection</param>
        /// <returns></returns>
        public IEnumerable<T> GetResult<T>(string target)
        {
            //if (CompiledQueryable.ContainsKey(target))
            //{
            //    var lq = CompiledQueryable[target];
            //    return (IEnumerable<T>)lq.Provider.Execute(lq.Expression);
            //}
            var info = MethodParser.MethodParse(target).ToArray();
            var colInfo = info[0];
            IEnumerable col;
            switch (colInfo.Name)
            {
                case "From":
                    col = FromResolver(colInfo);
                    break;
                case "TypeMember":
                    typeMember(typeof(T));
                    return default(IEnumerable<T>);
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
            // CompiledQueryable.Add(target, source2);
            return (IEnumerable<T>)source2.Provider.Execute(sourceCache);
        }

        /// <summary>
        /// like Queryable.Where for Single Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool ElementIs<T>(T obj,string source)
        {
            if (CompiledExpressions.ContainsKey(source))
            {
                var le = CompiledExpressions[source];
                return ((Func<T, bool>)le.Compile()).Invoke(obj);
            }
            var source2 = LambdaStringComplementer.Check(source);
            var bParser = new BracketParser();
            var info = LambdaParser.Parse(source2,bParser.Parse(source2));
            var expr = builder.DynamicBuild(info, new Type[] { typeof(T) });
            CompiledExpressions.Add(source, expr);
            var dele = (Func<T,bool>)expr.Compile();
            return dele.Invoke(obj);
        }

        /// <summary>
        /// like Queryable.Select for Single Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public TResult ElementTo<T,TResult>(T obj, string source)
        {
            if (CompiledExpressions.ContainsKey(source))
            {
                var le = CompiledExpressions[source];
                return ((Func<T, TResult>)le.Compile()).Invoke(obj);
            }
            var source2 = LambdaStringComplementer.Check(source);
            var bParser = new BracketParser();
            var info = LambdaParser.Parse(source2, bParser.Parse(source2));
            var expr = builder.DynamicBuild(info, new Type[] { typeof(T) });
            CompiledExpressions.Add(source, expr);
            var dele = (Func<T, TResult>)expr.Compile();
            return dele.Invoke(obj);
        }

        private void typeMember(Type t)
        {
            var s = System.Reflection.RuntimeReflectionExtensions.GetRuntimeProperties(t).Select(x => string.Format("{0} as {1}",x.Name, x.PropertyType));
            var sb = new StringBuilder();
            foreach (var xs in s)
            {
                sb.Append(xs);
                sb.Append(Environment.NewLine);
            }
            WriteAction(sb.ToString());
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
                case "TypeMember":
                    typeMember(typeof(T));
                    return default(T);
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
        /// Invoke Lambda Expression in Linear Object
        /// </summary>
        /// <param name="source"></param>
        /// <remarks>Only Action (not Generic) is invokable. Generic Type Action can't invoke.</remarks>
        public void InvokeLambda(string source)
        {
            var bParser = new BracketParser();
            var info = LambdaParser.Parse(source, bParser.Parse(source));
            var expr = builder.DynamicBuild(info, new Type[0]);
            dynamic dele = expr.Compile();
            dele()()();
        }

        public void InvokeLambda(string source, object arg1)
        {
            var bParser = new BracketParser();
            var info = LambdaParser.Parse(source, bParser.Parse(source));
            var expr = builder.DynamicBuild(info, new Type[] {arg1.GetType()});
            dynamic dele = expr.Compile();
            dele(arg1.ToString())(arg1.ToString())(arg1.ToString());
        }

        /// <summary>
        /// Get Applied Text Query Collection From Argument collection
        /// </summary>
        /// <typeparam name="T">Type of Source</typeparam>
        /// <typeparam name="TResult">Type of Result Collection</typeparam>
        /// <param name="source">To Apply Collection</param>
        /// <param name="target">Query string</param>
        /// <returns>Result Applied query Collection</returns>
        public static IEnumerable<TResult> ToLinear<T,TResult>(IEnumerable<T> source, string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return (IEnumerable<TResult>)source;
            }
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
