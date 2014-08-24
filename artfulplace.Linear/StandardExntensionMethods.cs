using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear.RegexExtension;
using artfulplace.Linear.Core;
using System.Reflection;
using System.Text.RegularExpressions;

namespace artfulplace.Linear
{
    internal class StandardExntensionMethods
    {
        internal static Expression GetStandardExpression(Core.MethodInfo info,List<ParameterExpression> args)
        {
            Expression[] exprs = null;
            switch (info.Name)
            {
                case "IsRegexAnd":
                case "IsRegexNot":
                case "IsRegexXor":
                    if (info.Args != null)
                    {
                        exprs = info.Args.Select(_ =>
                        {
                            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                            {
                                return (Expression)args.Find(x => x.Name == _.Value);
                            }
                            else if (_.Type == ArgumentInfo.ArgumentType.Method)
                            {
                                return stringMethodToExpression(_.Value, args);
                            }
                            else
                            {
                                return Expression.Constant(_.GetValue(), _.GetType2());
                            }
                        }).ToArray();
                    }
                    return Expression.Call(typeof(MultiRegex), info.Name, null, exprs);
                case "StringFormat":
                    if (info.Args != null)
                    {
                        exprs = info.Args.Select(_ =>
                        {
                            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                            {
                                return (Expression)args.Find(x => x.Name == _.Value);
                            }
                            else if (_.Type == ArgumentInfo.ArgumentType.Method)
                            {
                                return stringMethodToExpression(_.Value, args);
                            }
                            else
                            {
                                return Expression.Constant(_.GetValue(), _.GetType2());
                            }
                        }).ToArray();
                    }
                    return Expression.Call(typeof(string), "Format", null, exprs);
                case "RegexIsMatch":
                case "RegexReplace":
                case "RegexSplit":
                    if (info.Args != null)
                    {
                        exprs = info.Args.Select(_ =>
                        {
                            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                            {
                                return (Expression)args.Find(x => x.Name == _.Value);
                            }
                            else if (_.Type == ArgumentInfo.ArgumentType.Method)
                            {
                                return stringMethodToExpression(_.Value, args);
                            }
                            else
                            {
                                return Expression.Constant(_.GetValue(), _.GetType2());
                            }
                        }).ToArray();
                    }
                    return Expression.Call(typeof(Regex), info.Name.Replace("Regex",""), null, exprs);
                case "CreateTime":
                    if (info.Args != null)
                    {
                        exprs = info.Args.Select(_ =>
                        {
                            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                            {
                                return (Expression)args.Find(x => x.Name == _.Value);
                            }
                            else if (_.Type == ArgumentInfo.ArgumentType.Method)
                            {
                                return stringMethodToExpression(_.Value, args);
                            }
                            else
                            {
                                return Expression.Constant(_.GetValue(), _.GetType2());
                            }
                        }).ToArray();
                    }
                    var cons = typeof(DateTime).GetTypeInfo().DeclaredConstructors.Where(_ => _.GetParameters().Length == exprs.Length).First();
                    return Expression.New(cons, exprs);
                case "CreateTimeSpan":
                    if (info.Args != null)
                    {
                        exprs = info.Args.Select(_ =>
                        {
                            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                            {
                                return (Expression)args.Find(x => x.Name == _.Value);
                            }
                            else if (_.Type == ArgumentInfo.ArgumentType.Method)
                            {
                                return stringMethodToExpression(_.Value, args);
                            }
                            else
                            {
                                return Expression.Constant(_.GetValue(), _.GetType2());
                            }
                        }).ToArray();
                    }
                    var cons2 = typeof(TimeSpan).GetTypeInfo().DeclaredConstructors.Where(_ => _.GetParameters().Length == exprs.Length).First();
                    return Expression.New(cons2, exprs);
                case "TypeConvert":
                    var tcArg1 = info.Args.ElementAt(0);
                    var tcArg2 = info.Args.ElementAt(1);
                    Expression tcExpr;
                    if (tcArg1.Type == ArgumentInfo.ArgumentType.Variable)
                    {
                        tcExpr = (Expression)args.Find(x => x.Name == tcArg1.Value);
                    }
                    else if (tcArg1.Type == ArgumentInfo.ArgumentType.Method)
                    {
                        tcExpr = stringMethodToExpression(tcArg1.Value, args);
                    }
                    else
                    {
                        tcExpr = Expression.Constant(tcArg1.GetValue(), tcArg1.GetType2());
                    }
                    var tcT = Type.GetType(tcArg2.Value);
                    return Expression.Convert(tcExpr, tcT);
                case "$":
                    if (info.Args.Count() > 1)
                    {
                        exprs = info.Args.Skip(1).Select(_ =>
                        {
                            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                            {
                                return (Expression)args.Find(x => x.Name == _.Value);
                            }
                            else if (_.Type == ArgumentInfo.ArgumentType.Method)
                            {
                                return stringMethodToExpression(_.Value, args);
                            }
                            else
                            {
                                return Expression.Constant(_.GetValue(), _.GetType2());
                            }
                        }).ToArray();
                    }
                    var a3 = info.Args.First();
                    var t4 = Type.GetType(a3.Value);
                    var cons3 = t4.GetTypeInfo().DeclaredConstructors.Where(_ => _.GetParameters().Length == exprs.Length).First();
                    return Expression.New(cons3, exprs);
                    
                //case "PropChange":
                //    var propChangeArgs = new Type[2];
                //    if (info.Args != null)
                //    {
                //        exprs = info.Args.Select(_ =>
                //        {
                //            if (_.Type == ArgumentInfo.ArgumentType.Variable)
                //            {
                //                // return (Expression)args.Find(x => x.Name == _.Value);
                //                var argExpr = (Expression)args.Find(x => x.Name == _.Value);
                //                propChangeArgs[0] =argExpr.Type;
                //                return argExpr;
                //            }
                //            else if (_.Type == ArgumentInfo.ArgumentType.Method)
                //            {
                //                return stringMethodToExpression(_.Value, args);
                //            }
                //            else if (_.Type == ArgumentInfo.ArgumentType.Lambda)
                //            {
                //                var propLinfo = LambdaParser.Parse(_.Value, _.BracketInfo);
                //                propChangeArgs[1] = propLinfo.Arguments[0].GetType2();
                //                return Lambda.ExpressionBuilder.DynamicBuild(propLinfo, propLinfo.Arguments.Select(x => x.GetType2()).ToArray());
                //            }
                //            else
                //            {
                //                return Expression.Constant(_.GetValue(), _.GetType2());
                //            }
                //        }).ToArray();
                //    }
                //    return Expression.Call(typeof(StandardExntensionMethods), "PropChange", propChangeArgs, exprs);

            }
            return null;
        }

        private static Expression stringMethodToExpression(string target,List<ParameterExpression> args)
        {
            var ms = Core.MethodParser.MethodParse(target).ToArray();
            Expression expr = null;
            foreach (var i in ms)
            {
                if (expr == null)
                {
                    if (args.Any(x => x.Name == i.Name))
                    {
                        expr = args.Find(x => x.Name == i.Name);
                    }
                    else
                    {
                        throw new ArgumentException("メソッドの引数はラムダ式のパラメータから始めなくてはいけません。");
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


        /// <summary>
        /// Change object's property value. To use Select and collection type will not be changed.
        /// </summary>
        /// <typeparam name="T">Target object's Type</typeparam>
        /// <typeparam name="TVal">Target property's Type</typeparam>
        /// <param name="obj">To Apply SetValue object</param>
        /// <param name="name">Property Name</param>
        /// <param name="pred">Expression of Transform value</param>
        /// <returns>This code affects side effect for target object.  If you use this code, your collection must evaluate after Linear evaluated.</returns>
        public static T PropChange<T, TVal> (T obj, string name, Func<TVal,TVal> pred)
        {
            try
            {
                var prop = obj.GetType().GetRuntimeProperty(name);
                if (prop != null)
                {
                    prop.SetValue(obj, pred((TVal)prop.GetValue(obj)));
                }
            }
            catch (Exception)
            {
            }
            return obj;
        }

    }
}
