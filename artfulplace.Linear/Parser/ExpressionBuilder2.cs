using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear.Linq;
using System.Reflection;

namespace artfulplace.Linear.Lambda
{
    internal class ExpressionBuilder2
    {

        internal Linear LinearReference { get; set; }

        /// <summary>
        /// 式の情報を利用した引数を生成します
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private ParameterExpression argExpressionDynamic(LambdaArgumentInfo info)
        {
            return Expression.Parameter(Type.GetType(info.Type), info.Name);
        }

        private ParameterExpression argExpressionDynamic(LambdaArgumentInfo info, Type t)
        {
            return Expression.Parameter(t, info.Name);
        }

        internal LambdaExpression DynamicBuild(LambdaInfo info, Type[] args)
        {
            var args2 = new List<ParameterExpression>();
            if (info.Arguments != null)
            {
                info.Arguments.ForEach((x, idx) => args2.Add(argExpressionDynamic(x, args[idx])));
            }
            var args3 = args2.ToList();
            var expr = GenerateExpression(info.Expressions.First(), args3);
            return Expression.Lambda(expr, args2);
        }

        private Expression GenerateExpression(ExpressionBasicInfo info, List<ParameterExpression> args)
        {
            switch (info.ExpressionType)
            {
                case ExpressionType.Constant:
                    object val = null;
                    Type t = null;
                    switch (info.ConstantType)
                    {
                        case Core.ArgumentInfo.ArgumentType.Integer:
                            val = int.Parse(info.ExpressionString1);
                            t = typeof(int);
                            break;
                        case Core.ArgumentInfo.ArgumentType.Long:
                            val = long.Parse(info.ExpressionString1);
                            t = typeof(long);
                            break;
                        case Core.ArgumentInfo.ArgumentType.Double:
                            val = double.Parse(info.ExpressionString1);
                            t = typeof(double);
                            break;
                        case Core.ArgumentInfo.ArgumentType.Boolean:
                            val = bool.Parse(info.ExpressionString1);
                            t = typeof(bool);
                            break;
                        case Core.ArgumentInfo.ArgumentType.String:
                            val = info.ExpressionString1;
                            t = typeof(string);
                            break;
                        case Core.ArgumentInfo.ArgumentType.Variable:
                            if (args.Any(x => x.Name == info.ExpressionString1))
                            {
                                return args.Find(x => x.Name == info.ExpressionString1);
                            }
                            else
                            {
                                var sp = info.ExpressionString1.Split(' ');
                                var t2 = Type.GetType(sp[0]);
                                var param = Expression.Parameter(t2, sp[1]);
                                args.Add(param);
                                return param;
                            }
                        case Core.ArgumentInfo.ArgumentType.Method:
                            var ms = Core.MethodParser.MethodParse(info.ExpressionString1).ToArray();
                            Expression expr = null;
                            var skipLoop = false;
                            foreach (var i in ms)
                            {
                                if (skipLoop)
                                {
                                    skipLoop = false;
                                    continue;
                                }
                                if (expr == null)
                                {
                                    if (args.Any(x => x.Name == i.Name))
                                    {
                                        expr = args.Find(x => x.Name == i.Name);
                                    }
                                    else // 公開されたメソッドとの対応を行えるようにする
                                    {
                                        expr = StandardExntensionMethods.GetStandardExpression(i, args);
                                        if (expr == null)
                                        {
                                            if (i.Name == "From")
                                            {
                                                var colx = LinearReference.FromResolver(i);
                                                var source1 = colx.AsQueryable();
                                                var xt = source1.GetType().GenericTypeArguments.First();
                                                var gent = typeof(LinearQueryable<>).MakeGenericType(xt);
                                                var source3 = (IQueryable)Activator.CreateInstance(gent, source1);
                                                expr = Expression.Constant(source3, gent);
                                            }
                                            else if (i.Name == "_$")
                                            {
                                                if (ms.Length >= 2)
                                                {
                                                    skipLoop = true;
                                                    var a3 = i.Args.First().Value;
                                                    var t2 = Type.GetType(a3);
                                                    var i2 = ms.ElementAt(1);
                                                    Expression[] exprs = null;
                                                    if (i2.Args != null)
                                                    {
                                                        exprs = i2.Args.Select<Core.ArgumentInfo, Expression>(_ =>
                                                        {
                                                            if (args.Any(x => x.Name == _.Value))
                                                            {
                                                                return args.Find(x => x.Name == _.Value);
                                                            }
                                                            else
                                                            {
                                                                return Expression.Constant(_.GetValue(), _.GetType2());
                                                            }

                                                        }).ToArray();
                                                    }
                                                    try
                                                    {
                                                        expr = Expression.Call(t2, i2.Name, i2.GetArgumentTypes(), exprs);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        if (ex is InvalidOperationException)
                                                        {
                                                            expr = Expression.Call(t2, i2.Name, null, exprs);
                                                        }
                                                        else
                                                        {
                                                            throw ex;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    throw new ArgumentException("_$ must Call Method.");
                                                }
                                            }
                                            else if (LinearReference.ExpressionCompileCollection.ContainsKey(i.Name))
                                            {
                                                var mtExpr = i.Args.Select(x =>
                                                {
                                                    if (x.Type == Core.ArgumentInfo.ArgumentType.Method)
                                                    {
                                                        var exi = new ExpressionBasicInfo();
                                                        exi.ConstantType = Core.ArgumentInfo.ArgumentType.Method;
                                                        exi.ExpressionType = ExpressionType.Constant;
                                                        exi.ExpressionString1 = x.Value;
                                                        return exi;
                                                    }
                                                    else
                                                    {
                                                        var exi = new ExpressionBasicInfo();
                                                        exi.ConstantType = x.Type;
                                                        exi.ExpressionType = ExpressionType.Constant;
                                                        exi.ExpressionString1 = x.Value;
                                                        return exi;
                                                    }
                                                }).ToDictionary<ExpressionBasicInfo, string, Expression>(x => x.ExpressionString1, x => GenerateExpression(x, args));

                                                expr = LinearReference.ExpressionCompileCollection[i.Name].Invoke(i, args, mtExpr);
                                            }
                                            else if (LinearReference.ExtendExpressionCollection.ContainsKey(i.Name))
                                            {
                                                expr = LinearReference.ExtendExpressionCollection[i.Name];
                                                expr = Expression.Lambda(expr, args);
                                            }

                                            if (expr == null)
                                            {
                                                throw new ArgumentException(string.Format("メソッド名 {0} に対応するメソッドは共通メソッドで定義されていません", i.Name));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    switch (i.Type)
                                    {
                                        case Core.MethodInfo.MethodType.Method:
                                            Expression[] exprs = null;
                                            if (i.Args != null)
                                            {
                                                exprs = i.Args.Select(_ => Expression.Constant(_.GetValue(), _.GetType2())).ToArray();
                                            }
                                            try
                                            {
                                                expr = Expression.Call(expr, i.Name, i.GetArgumentTypes(), exprs);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (ex is InvalidOperationException)
                                                {
                                                    expr = Expression.Call(expr, i.Name, null, exprs);
                                                }
                                                else
                                                {
                                                    throw ex;
                                                }
                                            }
                                            break;
                                        case Core.MethodInfo.MethodType.Property:
                                            var exprs2 = i.Args.Select(_ => Expression.Constant(_.GetValue(), _.GetType2())).ToArray();
                                            expr = Expression.Property(expr, i.Name, exprs2);
                                            break;
                                        case Core.MethodInfo.MethodType.PropertyOrField:
                                            expr = Expression.PropertyOrField(expr, i.Name);
                                            break;
                                    }
                                }

                            }
                            return expr;

                    }
                    return Expression.Constant(val, t);
                case ExpressionType.Add:
                    var addExpr1 = GenerateExpression(info.Expression1, args);
                    var addExpr2 = GenerateExpression(info.Expression2, args);
                    if (info.ConstantType == Core.ArgumentInfo.ArgumentType.String)
                    {
                        var concatMethod = typeof(string).GetRuntimeMethod("Concat", new Type[] { typeof(string), typeof(string) });
                        return Expression.Call(concatMethod, addExpr1, addExpr2);
                    }
                    else
                    {
                        return Expression.Add(addExpr1, addExpr2);
                    }
                case ExpressionType.And:
                    return Expression.AndAlso(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionType.Or:
                    return Expression.OrElse(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionType.Not:
                    return Expression.Not(GenerateExpression(info.Expression1, args));
                default:
                    return Expression.MakeBinary(info.ExpressionType, GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
            }
            // return null;
        }
    }
}
