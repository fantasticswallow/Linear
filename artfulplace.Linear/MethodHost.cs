using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear.Core;
using artfulplace.Linear.Linq;
using artfulplace.Linear.Lambda;
using System.Reflection;

namespace artfulplace.Linear
{
    /// <summary>
    /// メソッドを実行するホストオブジェクトを提供します
    /// </summary>
    internal class MethodHost
    {
        internal static IEnumerable<T> Invoke<T>(IQueryable<T> source, string predicate)
        {
            var info = MethodParser.MethodParse(predicate);
            var argType = typeof(T);
            object obj = source;
            foreach (var i in info)
            {
                if (i.Type == Core.MethodInfo.MethodType.Property )
                {
                    var prop = obj.GetType().GetRuntimeProperty(i.Name);
                    if (i.Args.Count() > 0)
                    {
                        obj = prop.GetValue(obj,i.Args.Select(_ => _.GetValue()).ToArray());
                    }
                    else
                    {
                        obj = prop.GetValue(obj);
                    }
                }
                else
                {
                    if (i.Args.IsNotEmpty())
                    {
                        if (!(i.Args.Any(_ => _.Type == ArgumentInfo.ArgumentType.Lambda)))
                        {
                            Type[] argt = null;
                            object[] args = null;
                            argt = i.Args.Select(_ =>
                            {
                                switch (_.Type)
                                {
                                    case ArgumentInfo.ArgumentType.Boolean:
                                        return typeof(bool);
                                    case ArgumentInfo.ArgumentType.String:
                                        return typeof(string);
                                    case ArgumentInfo.ArgumentType.Double:
                                        return typeof(double);
                                    default:
                                        return typeof(int);
                                }   
                            }).ToArray();
                            args = i.Args.Select(_ => _.GetValue()).ToArray();
                            if (obj.GetType().GetTypeInfo().ImplementedInterfaces.Contains(typeof(IQueryable<T>)))
                            {
                                var t1 = new Type[] { typeof(IQueryable<T>) };
                                argt = t1.Union(argt).ToArray();
                                var m1 = typeof(Queryable).GetRuntimeMethods().Where(_ => _.Name == i.Name && _.GetParameters().Count() == argt.Length).ToArray();
                                obj = m1.First().Invoke(obj, args);
                            }
                            else
                            {
                                obj = obj.GetType().GetRuntimeMethod(i.Name, argt).Invoke(obj, args);
                            }
                        }
                        else
                        {
                            var arglen = i.Args.Count2();
                            var args = new object[arglen];
                            var argt = obj.GetType();
                            var ms2 = argt.GetRuntimeMethods();
                            var isExtension = false;
                            // if (argt.GetTypeInfo().)
                            if (argt.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IQueryable<T>)))
                            {
                                ms2 = typeof(Queryable).GetRuntimeMethods();
                                isExtension = true;
                            }
                            var ms = ms2.Where(_ => _.Name == i.Name && (_.GetParameters().Count() == arglen || _.GetParameters().Count() == arglen + 1)).ToArray();
                            var mcount = ms.Count();
                            if (mcount == 1)
                            {
                                i.Args.ForEach((_, idx) =>
                                {
                                    switch (_.Type)
                                    {
                                        case ArgumentInfo.ArgumentType.Boolean:
                                            args[idx] = _.GetValue();
                                            break;
                                        case ArgumentInfo.ArgumentType.String:
                                            args[idx] = _.GetValue();
                                            break;
                                        case ArgumentInfo.ArgumentType.Double:
                                            args[idx] = _.GetValue();
                                            break;
                                        case ArgumentInfo.ArgumentType.Lambda:
                                            var linfo = LambdaParser.Parse(_.Value, _.BracketInfo);
                                            var pt = ms[0].GetParameters()[idx].ParameterType;
                                            if (isExtension)
                                            {
                                                var param = pt.GenericTypeArguments.Skip(1).ToArray();
                                                if (param[0].IsGenericParameter)
                                                {
                                                    param[0] = argType;
                                                }
                                                var expr = ExpressionBuilder.DynamicBuild(linfo, param);
                                                args[idx] = Convert.ChangeType(expr, pt);
                                            }
                                            else
                                            {
                                                var param = pt.GenericTypeArguments;
                                                if (param[0].IsGenericParameter)
                                                {
                                                    param[0] = argType;
                                                }
                                                var expr = ExpressionBuilder.DynamicBuild(linfo, param);
                                                args[idx] = Convert.ChangeType(expr, pt);
                                            }
                                            
                                            break;
                                        default:
                                            args[idx] = _.GetValue();
                                            break;
                                    }
                                });
                                ms.First().Invoke(obj, args);
                            }
                            else if (mcount <= 0)
                            {
                                throw new ArgumentException(string.Format("メソッド名{0}に一致するメソッドが発見できません。メソッド名、または引数が正しくない可能性があります。",i.Name));
                            }
                            else
                            {
                                i.Args.ForEach((_, idx) =>
                                {
                                    switch (_.Type)
                                    {
                                        case ArgumentInfo.ArgumentType.Boolean:
                                            args[idx] = _.GetValue();
                                            break;
                                        case ArgumentInfo.ArgumentType.String:
                                            args[idx] = _.GetValue();
                                            break;
                                        case ArgumentInfo.ArgumentType.Double:
                                            args[idx] = _.GetValue();
                                            break;
                                        case ArgumentInfo.ArgumentType.Lambda:
                                            var linfo = LambdaParser.Parse(_.Value, _.BracketInfo);

                                            var pt = ms[0].GetParameters()[idx].ParameterType;
                                            var param = pt.GenericTypeArguments;
                                            if (param[0].IsGenericParameter)
                                            {
                                                param[0] = argType;
                                            }
                                            var expr = ExpressionBuilder.DynamicBuild(linfo, param);
                                            args[idx] = Convert.ChangeType(expr, pt);
                                            break;
                                        default:
                                            args[idx] = _.GetValue();
                                            break;
                                    }
                                });
                            }
                            
                        }
                    }
                    else
                    {
                        obj = obj.GetType().GetRuntimeMethod(i.Name, null).Invoke(obj, null);
                    }
                }

                var ct = obj.GetType();
                if (ct.IsGenericParameter)
                {
                    argType = ct.GenericTypeArguments.First();
                }
                else
                {
                    argType = ct;
                }
                
            }
            return (IEnumerable<T>)obj;
        }

    }
}
