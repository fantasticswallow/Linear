using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using artfulplace.Linear.Linq;
using artfulplace.Linear.Core;

namespace artfulplace.Linear
{
    internal class SourceReference
    {
        public SourceReference(string name,Assembly asm)
        {
            TargetName = name;
            EntryAssembly = asm;
        }

        public string TargetName { get; set; }

        public IEnumerable Result(bool safeMode)
        {
            var namecol = MethodParser.MethodParse(this.TargetName);
            var ty2 = EntryAssembly.DefinedTypes.Find(_ => _.Name == namecol.First().Name);
            var ty = ty2.AsType();
            object resObj = null;
            for (var i = 1; i < namecol.Count(); i++)
            {
                var mi = namecol.ElementAt(i);
                switch (mi.Type)
                {
                    case Core.MethodInfo.MethodType.Method:
                        if (safeMode)
                        {
                            throw new InvalidOperationException("セーフモードではSourceReferenceでメソッドを実行することはできません。");
                        }
                        var argts = mi.GetArgumentTypes();
                        if (argts == null)
                        {
                            argts = new Type[0];
                        }
                        var m = ty.GetRuntimeMethod(mi.Name, argts);
                        if (m != null)
                        {
                            ty = m.ReturnType;
                            if (ty == null)
                            {
                                throw new ArgumentException(string.Format("型 {0} のメソッド {1} は値を返しません。", ty.Name, mi.Name));
                            }
                            if (mi.Args != null)
                            {
                                resObj = m.Invoke(resObj, mi.Args.Select(_ => _.GetValue()).ToArray());
                            }
                            else
                            {
                                resObj = m.Invoke(resObj, null);
                            }
                            
                        }
                        else
                        {
                            throw new ArgumentException(String.Format("SourceReference target is not valid. Please Check don't miss or Class Member is valid. : target = \"{0}\"", mi.Name));
                        }
                        break;
                    case Core.MethodInfo.MethodType.Property:
                        var pi = ty.GetRuntimeProperty(mi.Name);
                        if (pi == null)
                        {
                            throw new ArgumentException(string.Format("型 {0} においてプロパティ {1} は存在しません。", ty.Name, mi.Name));
                        }
                        ty = pi.PropertyType;
                        var args = mi.Args.Select(_ => _.GetValue()).ToArray();
                        resObj = pi.GetValue(resObj, args);
                        break;
                    case Core.MethodInfo.MethodType.PropertyOrField:
                        var pi2 = ty.GetRuntimeProperty(mi.Name);
                        if (pi2 == null)
                        {
                            var fi = ty.GetRuntimeField(mi.Name);
                            if (fi == null)
                            {
                                throw new ArgumentException(string.Format("型 {0} においてプロパティ、またはフィールド {1} は存在しません。", ty.Name, mi.Name));
                            }
                            ty = fi.FieldType;
                            resObj = fi.GetValue(resObj);
                        }
                        else
                        {
                            ty = pi2.PropertyType;
                            var args2 = mi.Args.Select(_ => _.GetValue()).ToArray();
                            resObj = pi2.GetValue(resObj, args2);
                        }
                        break;
                }

               
            }
            return (IEnumerable)resObj;
        }

        public Assembly EntryAssembly { get; set; }
    }
}
