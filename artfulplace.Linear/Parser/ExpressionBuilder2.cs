﻿using System;
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
            info.Arguments.ForEach((x, idx) => args2.Add(argExpressionDynamic(x, args[idx])));
            var args3 = args2.ToList();
            var expr = GenerateExpression(info.Expressions.First(), args3);
            return Expression.Lambda(expr, args2);
        }

        private Expression GenerateExpression(ExpressionBasicInfo info, List<ParameterExpression> args)
        {
            switch (info.ExpressionType)
            {
                case ExpressionParser.OperatorKind.Constant:
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
                                        expr = StandardExntensionMethods.GetStandardExpression(i, args);
                                        if (expr == null)
                                        {
                                            if (i.Name == "From")
                                            {
                                                var colx = LinearReference.FromResolver(i);
                                                var source1 = colx.AsQueryable();
                                                var xt = source1.GetType().GenericTypeArguments.First();
                                                var gent = typeof(LinearQueryable<>).MakeGenericType(xt);
                                                var source3 = (IQueryable)Activator.CreateInstance(gent,source1);
                                                expr = Expression.Constant(source3, gent);
                                            }
                                            else if (LinearReference.ExpressionCompileCollection.ContainsKey(i.Name))
                                            {
                                                expr = LinearReference.ExpressionCompileCollection[i.Name].Invoke(i, args);
                                            }
                                            else if (LinearReference.ExtendExpressionCollection.ContainsKey(i.Name))
                                            {
                                                expr = LinearReference.ExtendExpressionCollection[i.Name];
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
                                            expr = Expression.Call(expr, i.Name, i.GetArgumentTypes(), exprs);
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
                case ExpressionParser.OperatorKind.And:
                    return Expression.AndAlso(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Or:
                    return Expression.OrElse(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Not:
                    return Expression.Not(GenerateExpression(info.Expression1, args));
                case ExpressionParser.OperatorKind.Equals:
                    return Expression.Equal(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.NotEquals:
                    return Expression.NotEqual(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Greater:
                    return Expression.GreaterThan(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.GreaterEquals:
                    return Expression.GreaterThanOrEqual(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Less:
                    return Expression.LessThan(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.LessEquals:
                    return Expression.LessThanOrEqual(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.AddAssign:
                    return Expression.AddAssign(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.SubtractAssign:
                    return Expression.SubtractAssign(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.MultiplicationAssign:
                    return Expression.MultiplyAssign(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.DivisionAssign:
                    return Expression.DivideAssign(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.ModuloAssign:
                    return Expression.ModuloAssign(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Add:
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
                case ExpressionParser.OperatorKind.Subtract:
                    return Expression.Subtract(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Multiplication:
                    return Expression.Multiply(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Division:
                    return Expression.Divide(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Modulo:
                    return Expression.Modulo(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
                case ExpressionParser.OperatorKind.Basic:
                    return Expression.Assign(GenerateExpression(info.Expression1, args), GenerateExpression(info.Expression2, args));
            }
            return null;
        }
    }
}