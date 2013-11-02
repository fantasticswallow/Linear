using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear.Linq;
using System.Reflection;
using artfulplace.Linear.Lambda;

namespace artfulplace.Linear.Core
{
    /// <summary>
    /// メソッド分割後のメソッドをExpressionに変換します
    /// </summary>
    internal class MethodBuilder
    {
        internal static Expression MethodBuild(Expression source, MethodInfo info)
        {
            var t2 = new List<Type>();
            var iArgs = info.Args.Count() + 1;
            var ms2 = typeof(Queryable).GetRuntimeMethods().Where(x => (x.Name == info.Name) && (x.GetParameters().Length == iArgs)).ToArray();
            var msGenArgs = ms2[0].GetGenericArguments().Length;
            t2.Add(source.Type.GenericTypeArguments.First());

            var exprs = info.Args.Select((_, idx) =>
                {
                    switch (_.Type)
                    {
                        case ArgumentInfo.ArgumentType.Boolean:
                            return Expression.Constant(_.GetValue(), typeof(bool));
                        case ArgumentInfo.ArgumentType.String:
                            return Expression.Constant(_.GetValue(), typeof(string));
                        case ArgumentInfo.ArgumentType.Double:
                            return Expression.Constant(_.GetValue(), typeof(double));
                        case ArgumentInfo.ArgumentType.Long:
                            return Expression.Constant(_.GetValue(), typeof(long));
                        case ArgumentInfo.ArgumentType.Lambda:
                            var linfo = LambdaParser.Parse(_.Value, _.BracketInfo);
                            var ms = typeof(Queryable).GetRuntimeMethods().Where(x => (x.Name == info.Name)).ToArray();
                            
                            ms = ms.Where(x => 
                            {
                                var xps = x.GetParameters();
                                return (xps.Length == info.Args.Count() + 1) && (xps[idx + 1].ParameterType.GenericTypeArguments[0].GenericTypeArguments.Count() == linfo.Arguments.Count() + 1);
                            }).ToArray();

                            var pt = ms[0].GetParameters()[idx].ParameterType;
                            var param = pt.GenericTypeArguments;
                            if (param[0].IsGenericParameter)
                            {
                                param[0] = t2[0];
                            }
                            var lexpr = ExpressionBuilder.DynamicBuild(linfo, param);
                            if (t2.Count < msGenArgs)
                            {
                                t2.Add(lexpr.ReturnType);
                            }
                            return (Expression)lexpr;
                        default:
                            return Expression.Constant(_.GetValue(), typeof(int));
                    }
                });

            var exprList = new List<Expression>();
            exprList.Add(source);
            exprList.AddRange(exprs);

            return Expression.Call(typeof(Queryable), info.Name, t2.ToArray(),exprList.ToArray());
            
            //var source3 = new LinearQueryable<int>(source);
            //var info = MethodParser.MethodParse(target);
            //var li = info.First().Args.First();
            //var linfo = LambdaParser.Parse(li.Value, li.BracketInfo);
            //var expr = ExpressionBuilder.DynamicBuild(linfo, new Type[] { typeof(int), typeof(bool) });

            //// EnumerableQueryだとIE<>が評価されてしまうのでLinearQueryableで評価させることでIQ<>を入れる
            //var sourceExpr = Expression.Constant(source3, source3.GetType());
            //var resExpr = Expression.Call(typeof(Queryable), "Where", new Type[] { typeof(int) }, sourceExpr, expr);
            //var source2 = new LinearQueryable<int>(source, resExpr);
            //var res = (IEnumerable<int>)(source.Provider.Execute<IQueryable<int>>(resExpr));

        }

        internal static Expression MethodBuild(Expression source, MethodInfo info,Type sourceType)
        {
            var t2 = new List<Type>();
            var iArgs = info.Args.Count() + 1;
            var ms2 = typeof(Queryable).GetRuntimeMethods().Where(x => (x.Name == info.Name) && (x.GetParameters().Length == iArgs)).ToArray();
            var msGenArgs = ms2[0].GetGenericArguments().Length;
            t2.Add(sourceType);

            var exprs = info.Args.Select((_, idx) =>
                {
                    switch (_.Type)
                    {
                        case ArgumentInfo.ArgumentType.Boolean:
                            return Expression.Constant(_.GetValue(), typeof(bool));
                        case ArgumentInfo.ArgumentType.String:
                            return Expression.Constant(_.GetValue(), typeof(string));
                        case ArgumentInfo.ArgumentType.Double:
                            return Expression.Constant(_.GetValue(), typeof(double));
                        case ArgumentInfo.ArgumentType.Long:
                            return Expression.Constant(_.GetValue(), typeof(long));
                        case ArgumentInfo.ArgumentType.Lambda:
                            var linfo = LambdaParser.Parse(_.Value, _.BracketInfo);
                            var ms = typeof(Queryable).GetRuntimeMethods().Where(x => (x.Name == info.Name)).ToArray();
                            
                            ms = ms.Where(x => 
                            {
                                var xps = x.GetParameters();
                                return (xps.Length == info.Args.Count() + 1) && (xps[idx + 1].ParameterType.GenericTypeArguments[0].GenericTypeArguments.Count() == linfo.Arguments.Count() + 1);
                            }).ToArray();

                            var pt = ms[0].GetParameters()[idx].ParameterType;
                            var param = pt.GenericTypeArguments;
                            if (param[0].IsGenericParameter)
                            {
                                param[0] = t2[0];
                            }
                            var lexpr = ExpressionBuilder.DynamicBuild(linfo, param);
                            if (t2.Count < msGenArgs)
                            {
                                t2.Add(lexpr.ReturnType);
                            }
                            return (Expression)lexpr;
                        default:
                            return Expression.Constant(_.GetValue(), typeof(int));
                    }
                });

            var exprList = new List<Expression>();
            exprList.Add(source);
            exprList.AddRange(exprs);

            return Expression.Call(typeof(Queryable), info.Name, t2.ToArray(),exprList.ToArray());
            
            //var source3 = new LinearQueryable<int>(source);
            //var info = MethodParser.MethodParse(target);
            //var li = info.First().Args.First();
            //var linfo = LambdaParser.Parse(li.Value, li.BracketInfo);
            //var expr = ExpressionBuilder.DynamicBuild(linfo, new Type[] { typeof(int), typeof(bool) });

            //// EnumerableQueryだとIE<>が評価されてしまうのでLinearQueryableで評価させることでIQ<>を入れる
            //var sourceExpr = Expression.Constant(source3, source3.GetType());
            //var resExpr = Expression.Call(typeof(Queryable), "Where", new Type[] { typeof(int) }, sourceExpr, expr);
            //var source2 = new LinearQueryable<int>(source, resExpr);
            //var res = (IEnumerable<int>)(source.Provider.Execute<IQueryable<int>>(resExpr));

        }
    }
}
