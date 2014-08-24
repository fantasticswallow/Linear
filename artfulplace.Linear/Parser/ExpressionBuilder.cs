using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using artfulplace.Linear.Linq;

namespace artfulplace.Linear.Lambda
{
    internal class ExpressionBuilder
    {
        //internal static Expression<Func<TResult>> Build<TResult>(LambdaInfo info)
        //{
        //    // var args = new List<ParameterExpression>();
        //    // args.Add(argExpression<TArg>(info.Arguments[0]));
        //    // var args2 = args.ToList();
        //    return Expression.Lambda<Func<TResult>>(GenerateExpression(info.Expressions.First(), null),null);
        //}

        //internal static Expression<Func<TArg, TResult>> Build<TArg, TResult>(LambdaInfo info)
        //{
        //    var args = new List<ParameterExpression>();
        //    args.Add(argExpression<TArg>(info.Arguments[0]));
        //    var args2 = args.ToList();
        //    return Expression.Lambda<Func<TArg, TResult>>(GenerateExpression(info.Expressions.First(), args2), args);
        //}
        //internal static Expression<Func<T1,T2, TResult>> Build<T1,T2, TResult>(LambdaInfo info)
        //{
        //    var args = new List<ParameterExpression>();
        //    args.Add(argExpression<T1>(info.Arguments[0]));
        //    args.Add(argExpression<T2>(info.Arguments[1]));
        //    var args2 = args.ToList();
        //    return Expression.Lambda<Func<T1,T2, TResult>>(GenerateExpression(info.Expressions.First(), args2), args);
        //}

        //internal static Expression<Func<T1, T2, T3, TResult>> Build<T1, T2, T3, TResult>(LambdaInfo info)
        //{
        //    var args = new List<ParameterExpression>();
        //    args.Add(argExpression<T1>(info.Arguments[0]));
        //    args.Add(argExpression<T2>(info.Arguments[1]));
        //    args.Add(argExpression<T3>(info.Arguments[2]));
        //    var args2 = args.ToList();
        //    return Expression.Lambda<Func<T1, T2, T3, TResult>>(GenerateExpression(info.Expressions.First(), args2), args);
        //}

        private static ParameterExpression argExpression<TArg>(LambdaArgumentInfo info)
        {
            if (string.IsNullOrEmpty(info.Type))
            {
                return Expression.Parameter(typeof(TArg), info.Name);
            }
            else
            {
                return Expression.Parameter(Type.GetType(info.Type), info.Name);
            }
        }

        /// <summary>
        /// 式の情報を利用した引数を生成します
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static ParameterExpression argExpressionDynamic (LambdaArgumentInfo info)
        {
            return Expression.Parameter(Type.GetType(info.Type), info.Name);
        }

        private static ParameterExpression argExpressionDynamic(LambdaArgumentInfo info,Type t)
        {
            return Expression.Parameter(t, info.Name);
        }

        internal static LambdaExpression DynamicBuild(LambdaInfo info,Type[] args)
        {
            var args2 = new List<ParameterExpression>();
            info.Arguments.ForEach((x, idx) => args2.Add(argExpressionDynamic(x,args[idx])));
            var args3 = args2.ToList();
            var expr = GenerateExpression(info.Expressions.First(), args3);
            return Expression.Lambda(expr, args2);
        }

        internal static Expression DynamicBuild<TArg>(LambdaInfo info)
        {
            var args = new List<ParameterExpression>();
            args.Add(argExpression<TArg>(info.Arguments[0]));
            var argLen = info.Arguments.Count();
            if (argLen > 1)
            {
                for (var i = 1; i < argLen; i++)
                {
                    args.Add(argExpressionDynamic(info.Arguments[i]));
                }
            }
            var args2 = args.ToList();
            var expr = GenerateExpression(info.Expressions.First(), args2);
            return Expression.Lambda(expr, args);
        }

        
        private static Expression GenerateExpression(ExpressionBasicInfo info, List<ParameterExpression> args)
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
                            foreach (var i in ms)
                            {
                                if (expr == null)
                                {
                                    if (args.Any(x => x.Name == i.Name))
                                    {
                                        expr = args.Find(x => x.Name == i.Name);
                                    }
                                    else // 公開されたメソッドとの対応を行えるようにする
                                    {
                                        
                                        
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
        }
    }
}
