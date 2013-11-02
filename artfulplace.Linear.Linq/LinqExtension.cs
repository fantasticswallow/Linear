using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace artfulplace.Linear.Linq
{
    public static class LinqExtension
    {
        /// <summary>
        /// 指定した位置から指定した数だけ要素を取得します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="idx">起点となる位置</param>
        /// <param name="count">取得する数</param>
        /// <returns></returns>
        public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int idx, int count)
        {
            var len = source.Count2();
            if (len < idx + count)
            {
                count = len;
            }
            int i;
            for (i = 0; i < count; i++)
            {
                yield return source.ElementAt(idx + i);
            }
        }

        /// <summary>
        /// 指定した条件に一致する最初の要素から指定した数だけ要素を取得します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">最初の要素の条件</param>
        /// <param name="count">取得する数</param>
        /// <returns></returns>
        public static IEnumerable<T> Take<T>(this IEnumerable<T> source, Func<T,bool> predicate, int count)
        {
            var obj = source.Find(predicate);
            if (obj == null)
            {
                yield break;
            }
            var idx = source.ElementOf(obj);
            
            int i;
            for (i = 0; i < count; i++)
            {
                yield return source.ElementAt(idx + i);
            }
        }

        /// <summary>
        /// 要素を評価してから要素数を取得します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>含まれる要素の数</returns>
        public static int Count2<T>(this IEnumerable<T> source)
        {
            return source.ToArray().Length;
        }

        /// <summary>
        /// 条件に一致する要素を取得します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">要素の条件</param>
        /// <returns></returns>
        public static T Find<T>(this IEnumerable<T> source, Func<T,bool> predicate)
        {
            foreach(var x in source)
            {
                if (predicate(x))
                {
                    return x;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 特定の要素の位置を取得します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target">位置を取得する要素</param>
        /// <returns></returns>
        public static int ElementOf<T> (this IEnumerable<T> source,T target)
        {
            var i = 0;
            foreach (var x in source)
            {
                if (x.Equals(target))
                {
                    return i;
                }
                i += 1;
            }
            return -1;
        }

        /// <summary>
        /// 特定の要素の位置を指定した等価評価式を使って取得します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target">位置を取得する要素</param>
        /// <param name="equals">等価かどうかを比較する式</param>
        /// <returns></returns>
        public static int ElementOf<T>(this IEnumerable<T> source, T target, Func<T,T,bool> equals)
        {
            var i = 0;
            foreach (var x in source)
            {
                if (equals(x,target))
                {
                    return i;
                }
                i += 1;
            }
            return -1;
        }

        /// <summary>
        /// すべての要素に対して処理を行います
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="dealing">要素に対して行う処理</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> dealing)
        {
            if (source.IsNotEmpty())
            {
                foreach (var x in source)
                {
                    dealing(x);
                }
            }
        }

        /// <summary>
        /// すべての要素に対してカウンタ変数を利用した処理を行います
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="dealing">要素に対して行う処理</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T,int> dealing)
        {
            int idx = 0;
            if (source.IsNotEmpty())
            {
                foreach (var x in source)
                {
                    dealing(x, idx);
                    idx += 1;
                }
            }
        }

        /// <summary>
        /// 指定したコレクションに一つ以上要素が含まれているかどうか調べます
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Count() > 0;
        }

        /// <summary>
        /// 一致する要素の位置をすべて返します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target">調べる要素</param>
        /// <returns></returns>
        public static IEnumerable<int> IndexMany<T>(this IEnumerable<T> source,T target)
        {
            var indexer = new List<int>();
            source.ForEach((x,idx) => {
                if (x.Equals(target))
                {
                    indexer.Add(idx);
                }
            });
            return indexer;
        }

        /// <summary>
        /// 一致する要素の位置をすべて返します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target">調べる要素</param>
        /// <param name="equals">等価比較を行う式</param>
        /// <returns></returns>
        public static IEnumerable<int> IndexMany<T>(this IEnumerable<T> source, T target,Func<T,T,bool> equals)
        {
            var indexer = new List<int>();
            source.ForEach((x, idx) =>
            {
                if (equals(x,target))
                {
                    indexer.Add(idx);
                }
            });
            return indexer;
        }
    }
}
