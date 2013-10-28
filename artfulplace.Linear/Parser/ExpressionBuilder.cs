﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using artfulplace.Linear.Linq;

namespace artfulplace.Linear.Lambda
{
    internal class ExpressionBuilder
    {
        internal static Expression<Func<TResult>> Build<TResult>(LambdaInfo info)
        {
            // var args = new List<ParameterExpression>();
            // args.Add(argExpression<TArg>(info.Arguments[0]));
            // var args2 = args.ToList();
            return Expression.Lambda<Func<TResult>>(GenerateExpression(info.Expressions.First(), null),null);
        }

        internal static Expression<Func<TArg, TResult>> Build<TArg, TResult>(LambdaInfo info)
        {
            var args = new List<ParameterExpression>();
            args.Add(argExpression<TArg>(info.Arguments[0]));
            var args2 = args.ToList();
            return Expression.Lambda<Func<TArg, TResult>>(GenerateExpression(info.Expressions.First(), args2), args);
        }
        internal static Expression<Func<T1,T2, TResult>> Build<T1,T2, TResult>(LambdaInfo info)
        {
            var args = new List<ParameterExpression>();
            args.Add(argExpression<T1>(info.Arguments[0]));
            args.Add(argExpression<T2>(info.Arguments[1]));
            var args2 = args.ToList();
            return Expression.Lambda<Func<T1,T2, TResult>>(GenerateExpression(info.Expressions.First(), args2), args);
        }

        internal static Expression<Func<T1, T2, T3, TResult>> Build<T1, T2, T3, TResult>(LambdaInfo info)
        {
            var args = new List<ParameterExpression>();
            args.Add(argExpression<T1>(info.Arguments[0]));
            args.Add(argExpression<T2>(info.Arguments[1]));
            args.Add(argExpression<T3>(info.Arguments[2]));
            var args2 = args.ToList();
            return Expression.Lambda<Func<T1, T2, T3, TResult>>(GenerateExpression(info.Expressions.First(), args2), args);
        }

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

        internal static Expression DynamicBuild(LambdaInfo info,Type[] args)
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
                case ExpressionParser.OperatorKind.Constant:
                    object val = null;
                    switch (info.ConstantType)
                    {
                        case Core.ArgumentInfo.ArgumentType.Integer:
                            val = int.Parse(info.ExpressionString1);
                            break;
                        case Core.ArgumentInfo.ArgumentType.Double:
                            val = double.Parse(info.ExpressionString1);
                            break;
                        case Core.ArgumentInfo.ArgumentType.Boolean:
                            val = bool.Parse(info.ExpressionString1);
                            break;
                        case Core.ArgumentInfo.ArgumentType.String:
                            val = info.ExpressionString1;
                            break;
                        case Core.ArgumentInfo.ArgumentType.Variable:
                            if (args.Any(x => x.Name == info.ExpressionString1))
                            {
                                return args.Find(x => x.Name == info.ExpressionString1);
                            }
                            else
                            {
                                var sp = info.ExpressionString1.Split(' ');
                                var t = Type.GetType(sp[0]);
                                var param = Expression.Parameter(t,sp[1]);
                                args.Add(param);
                                return param;
                            }
                    }
                    return Expression.Constant(val);
                case ExpressionParser.OperatorKind.And:
                    return Expression.AndAlso(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Or:
                    return Expression.OrElse(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Not:
                    return Expression.Not(GenerateExpression(info.Expression1,args));
                case ExpressionParser.OperatorKind.Equals:
                    return Expression.Equal(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.NotEquals:
                    return Expression.NotEqual(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Greater:
                    return Expression.GreaterThan(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.GreaterEquals:
                    return Expression.GreaterThanOrEqual(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Less:
                    return Expression.LessThan(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.LessEquals:
                    return Expression.LessThanOrEqual(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.AddAssign:
                    return Expression.AddAssign(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.SubtractAssign:
                    return Expression.SubtractAssign(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.MultiplicationAssign:
                    return Expression.MultiplyAssign(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.DivisionAssign:
                    return Expression.DivideAssign(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.ModuloAssign:
                    return Expression.ModuloAssign(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Add:
                    return Expression.AddAssign(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Subtract:
                    return Expression.Subtract(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Multiplication:
                    return Expression.Multiply(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Division:
                    return Expression.Divide(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Modulo:
                    return Expression.Modulo(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
                case ExpressionParser.OperatorKind.Basic:
                    return Expression.Assign(GenerateExpression(info.Expression1,args), GenerateExpression(info.Expression2,args));
            }
            return null;
        }
    }
}