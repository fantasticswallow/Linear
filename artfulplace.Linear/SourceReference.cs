using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using artfulplace.Linear.Linq;

namespace artfulplace.Linear
{
    public class SourceReference
    {
        public SourceReference(string name)
        {
            TargetName = name;
        }

        public string TargetName { get; set; }

        public IEnumerable Result()
        {
            var namecol = TargetName.Split('.');
            var ty = EntryAssembly.GetType(namecol[0]);
            int i;
            for (i = 1; i < namecol.Length; i++)
            {
                var m = ty.GetRuntimeMethods();
                if (m.IsNotEmpty())
                {
                    
                }
                else
                {
                    throw new ArgumentException(String.Format("SourceReference target is not valid. Please Check don't miss or Class Member is valid. : target = \"{0}\"", namecol[i]));
                }
            }
            return null;
        }

        public Assembly EntryAssembly { get; set; }
    }
}
