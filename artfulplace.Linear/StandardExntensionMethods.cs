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

    }
}
