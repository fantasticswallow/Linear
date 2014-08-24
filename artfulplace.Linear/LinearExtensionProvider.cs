using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artfulplace.Linear
{
    public static class LinearExtensionProvider
    {
        public static IEnumerable<TResult> ToLinear<T,TResult>(this IEnumerable<T> source, string target)
        {
            return Linear.ToLinear<T, TResult>(source, target);
        }

        public static IEnumerable<T> ToLinear<T>(this IEnumerable<T> source, string target)
        {
            return Linear.ToLinear<T, T>(source, target);
        }

        public static IEnumerable<TResult> ToLinear<T, TResult>(this IEnumerable<T> source, string target,Action<Exception> onerror)
        {
            try
            {
                return Linear.ToLinear<T, TResult>(source, target);
            }
            catch (Exception ex)
            {
                onerror(ex);
                return (IEnumerable<TResult>)source;
            }
        }

        public static IEnumerable<T> ToLinear<T>(this IEnumerable<T> source, string target, Action<Exception> onerror)
        {
            try
            {
                return Linear.ToLinear<T, T>(source, target);
            }
            catch (Exception ex)
            {
                onerror(ex);
                return source;
            }
        }
    }
}
