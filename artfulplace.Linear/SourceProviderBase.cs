using Core = artfulplace.Linear.Core;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace artfulplace.Linear
{
    public abstract class SourceProviderBase
    {
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// SourceProvider 内に定義された関数を呼び出し、その結果を取得します
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        /// <remarks>Linear がコレクションを取得するために利用します。通常のコードから呼び出す機会はありません。</remarks>
        public IEnumerable GetSource(Core.MethodInfo mt)
        {
            var t = this.GetType().GetRuntimeMethods();
            var m1 = t.Where(x => x.Name == mt.Name).FirstOrDefault();
            if (m1 != null)
            {
                return (IEnumerable)m1.Invoke(this, mt.Args.Select(x => ProviderArgument.CreateArgument(x)).ToArray());
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private async void taskAwait<T>(Func<Task<T>> ts, Action<T> x)
        {
            var r = await ts();
            x(r);
        }

        /// <summary>
        /// awaitable なメソッドを待機し、その結果を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fr"></param>
        /// <returns></returns>
        /// <remarks>SourceProviderとして実装されるクラス内のメソッド待機用です。それ以外の用途では使用しないでください</remarks>
        protected T taskWait<T>(Func<Task<T>> fr)
        {
            var f = false;
            T trs = default(T);
            Task.Factory.StartNew(() => taskAwait(fr, x => { trs = x; f = true; }));
            System.Threading.SpinWait.SpinUntil(() => f, 40000);
            return trs;
        }
    }

    /// <summary>
    /// SourceProvider　経由で提供される引数のデータを提供します
    /// </summary>
    public class ProviderArgument
    {
        public object Value { get; set; }
        public enum ArgumentType
        {
            String,
            Char,
            Integer,
            Long,
            Double,
            Boolean,
            Undefined
        }
        public ArgumentType Type { get; set; }

        internal static ProviderArgument CreateArgument(Core.ArgumentInfo ai)
        {
            var ri = new ProviderArgument();
            ri.Value = ai.GetValue();
            switch (ai.Type)
            {
                case Core.ArgumentInfo.ArgumentType.Boolean:
                    ri.Type = ArgumentType.Boolean;
                    break;
                case Core.ArgumentInfo.ArgumentType.Char:
                    ri.Type = ArgumentType.Char;
                    break;
                case Core.ArgumentInfo.ArgumentType.Double:
                    ri.Type = ArgumentType.Double;
                    break;
                case Core.ArgumentInfo.ArgumentType.Integer:
                    ri.Type = ArgumentType.Integer;
                    break;
                case Core.ArgumentInfo.ArgumentType.Long:
                    ri.Type = ArgumentType.Long;
                    break;
                case Core.ArgumentInfo.ArgumentType.String:
                    ri.Type = ArgumentType.String;
                    break;
                default:
                    ri.Type = ArgumentType.Undefined;
                    break;
            }
            return ri;
        }

    }
}
