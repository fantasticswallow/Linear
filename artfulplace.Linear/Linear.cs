using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace artfulplace.Linear
{
    /// <summary>
    /// Linear Collection Object
    /// </summary>
    class Linear<T>
    {

        public Dictionary<string, IEnumerable<T>> MemoriedCollection { get; set; }
        
        public IEnumerable<T> From(string name)
        {
            var name2 = name.ToLower();
            if (MemoriedCollection.ContainsKey(name2))
            {
                return MemoriedCollection[name2];
            }
            else
            {
                throw new ArgumentException("Linear Compile Error : From Database can't find " + name2);
            }
        }

        public IEnumerable<T> From(SourceReference<T> refer)
        {
            return refer.Result();
        }


    }
}
