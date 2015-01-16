using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using artfulplace.Linear;


namespace TestConsole
{
    public class TestProvider : SourceProviderBase
    {
        public override string Name
        {
            get { return "tapi"; }
        }

        public IEnumerable<int> GetCount(ProviderArgument a1)
        {
            
            var i = (int)a1.Value;
            return Enumerable.Range(1, i);
        }

        public IEnumerable<double> GetPow(ProviderArgument a1, ProviderArgument a2)
        {

            var i = (int)a1.Value;
            var d = (double)a2.Value;
            return Enumerable.Range(1, i).Select(x => Math.Pow(x,d));
        }
    }
}
