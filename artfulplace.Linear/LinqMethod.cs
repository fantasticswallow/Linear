using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using artfulplace.Linear.Core;
using artfulplace.Linear.Linq;
using artfulplace.Linear.Lambda;

namespace artfulplace.Linear
{
    internal class LinqMethod
    {

        //internal static IQueryable<T> Average<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 0)
        //    {
        //        // IQueryable`1[Decimal] 
        //        return source.Average();
        //    }
        //    else if (args.Count() == 1)
        //    {
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource Int32] 
        //        return source.Average(args[0]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.Average においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        internal static T Aggregate<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`3 [TSource TSource TSource] 
                return source.Aggregate((Expression<Func<T, T, T>>)args[0]);
            }
            //else if (args.Count() == 2)
            //{
            //    // IQueryable`1[TSource] TAccumulate Expression`1[Func`3 [TAccumulate TSource TAccumulate] 
            //    return source.Aggregate(args[0], args[1],);
            //}
            //else if (args.Count() == 3)
            //{
            //    // IQueryable`1[TSource] TAccumulate Expression`1[Func`3 [TAccumulate TSource TAccumulate] Expression`1[Func`2 [TAccumulate TResult] 
            //    return source.Aggregate(args[0], args[1], args[2]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Aggregate においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T FirstOrDefault<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.FirstOrDefault();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.FirstOrDefault((Expression<Func<T,bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.FirstOrDefault においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T Last<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.Last();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.Last((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Last においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T LastOrDefault<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.LastOrDefault();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.LastOrDefault((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.LastOrDefault においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T Single<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.Single();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.Single((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Single においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T SingleOrDefault<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.SingleOrDefault();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.SingleOrDefault((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.SingleOrDefault においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T ElementAt<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Int32 
                return source.ElementAt((int)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.ElementAt においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T ElementAtOrDefault<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Int32 
                return source.ElementAtOrDefault((int)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.ElementAtOrDefault においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<T> DefaultIfEmpty<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.DefaultIfEmpty();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] TSource 
                return source.DefaultIfEmpty((T)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.DefaultIfEmpty においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static bool Contains<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] TSource 
                return source.Contains((T)args[0]);
            }
            //else if (args.Count() == 2)
            //{
            //    // IQueryable`1[TSource] TSource IEqualityComparer`1[TSource] 
            //    return source.Contains(args[0], args[1]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Contains においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<T> Reverse<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.Reverse();
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Reverse においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static bool SequenceEqual<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] IEnumerable`1[TSource] 
                return source.SequenceEqual((IEnumerable<T>)args[0]);
            }
            //else if (args.Count() == 2)
            //{
            //    // IQueryable`1[TSource] IEnumerable`1[TSource] IEqualityComparer`1[TSource] 
            //    return source.SequenceEqual(args[0], args[1]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.SequenceEqual においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static bool Any<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.Any();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.Any((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Any においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static bool All<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.All((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.All においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static int Count<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.Count();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.Count((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Count においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static long LongCount<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.LongCount();
            }
            else  if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.LongCount((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.LongCount においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T Min<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.Min();
            }
            //else if (args.Count() == 1)
            //{
            //    // IQueryable`1[TSource] Expression`1[Func`2 [TSource TResult] 
            //    return source.Min(args[0]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Min においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T Max<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.Max();
            }
            //else if (args.Count() == 1)
            //{
            //    // IQueryable`1[TSource] Expression`1[Func`2 [TSource TResult] 
            //    return source.Max(args[0]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Max においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        //internal static T Sum<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 0)
        //    {
        //        // IQueryable`1[Int32] 
        //        return source.Sum();
        //    }
        //    else if (args.Count() == 1)
        //    {
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource Int32] 
        //        return source.Sum(args[0]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.Sum においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        //internal static IQueryable<T> AsQueryable<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 0)
        //    {
        //        // IEnumerable`1[TElement] 
        //        return source.AsQueryable();
        //        // IEnumerable 
        //        return source.AsQueryable();
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.AsQueryable においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        internal static IQueryable<T> Where<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                if (info[0].LambdaArgCount == 2)
                {
                    // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                    return source.Where((Expression<Func<T,bool>>)args[0]);
                }
                else if (info[0].LambdaArgCount == 3)
                {
                    // IQueryable`1[TSource] Expression`1[Func`3 [TSource Int32 Boolean] 
                    return source.Where((Expression<Func<T,int, bool>>)args[0]);
                }
                else
                {
                    throw new ArgumentException("メソッド名 IQueryable<T>.Where においてエラーが発生しました。引数の型、または数が正しくありません。");
                }
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Where においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        //internal static IQueryable<T> OfType<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 0)
        //    {
        //        // IQueryable 
        //        return source.OfType();
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.OfType においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        //internal static IQueryable<T> Cast<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 0)
        //    {
        //        // IQueryable 
        //        return source.Cast<T>();
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.Cast においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        internal static IQueryable<TResult> Select<T,TResult>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                if (info[0].LambdaArgCount == 1)
                {
                    // IQueryable`1[TSource] Expression`1[Func`2 [TSource TResult] 
                    return source.Select((Expression<Func<T, TResult>>)args[0]);
                }
                else if (info[0].LambdaArgCount == 2)
                {
                    // IQueryable`1[TSource] Expression`1[Func`3 [TSource Int32 TResult] 
                    return source.Select((Expression<Func<T, int, TResult>>)args[0]);
                }
                else
                {
                    throw new ArgumentException("メソッド名 IQueryable<T>.SelectMany においてエラーが発生しました。引数の型、または数が正しくありません。");
                }
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Select においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<TResult> SelectMany<T, TResult>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource IEnumerable`1] 
                if (info[0].LambdaArgCount == 1)
                {
                    return source.SelectMany((Expression<Func<T, IEnumerable<TResult>>>)args[0]);
                }
                else if (info[0].LambdaArgCount == 2)
                {
                    // IQueryable`1[TSource] Expression`1[Func`3 [TSource Int32 IEnumerable`1] 
                    return source.SelectMany((Expression<Func<T,int,IEnumerable<TResult>>>)args[0]);
                }
                else
                {
                    throw new ArgumentException("メソッド名 IQueryable<T>.SelectMany においてエラーが発生しました。引数の型、または数が正しくありません。");
                }
            }
            //else if (args.Count() == 2)
            //{
            //    // IQueryable`1[TSource] Expression`1[Func`3 [TSource Int32 IEnumerable`1] Expression`1[Func`3 [TSource TCollection TResult] 
            //    return source.SelectMany(args[0], args[1]);
            //    // IQueryable`1[TSource] Expression`1[Func`2 [TSource IEnumerable`1] Expression`1[Func`3 [TSource TCollection TResult] 
            //    return source.SelectMany(args[0], args[1]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.SelectMany においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }

        #region join ~ order by
                
        //internal static IQueryable<T> Join<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 4)
        //    {
        //        // IQueryable`1[TOuter] IEnumerable`1[TInner] Expression`1[Func`2 [TOuter TKey] Expression`1[Func`2 [TInner TKey] Expression`1[Func`3 [TOuter TInner TResult] 
        //        return source.Join(args[0], args[1], args[2], args[3]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.Join においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //    if (args.Count() == 5)
        //    {
        //        // IQueryable`1[TOuter] IEnumerable`1[TInner] Expression`1[Func`2 [TOuter TKey] Expression`1[Func`2 [TInner TKey] Expression`1[Func`3 [TOuter TInner TResult] IEqualityComparer`1[TKey] 
        //        return source.Join(args[0], args[1], args[2], args[3], args[4]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.Join においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        //internal static IQueryable<T> GroupJoin<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 4)
        //    {
        //        // IQueryable`1[TOuter] IEnumerable`1[TInner] Expression`1[Func`2 [TOuter TKey] Expression`1[Func`2 [TInner TKey] Expression`1[Func`3 [TOuter IEnumerable`1 TResult] 
        //        return source.GroupJoin(args[0], args[1], args[2], args[3]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.GroupJoin においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //    if (args.Count() == 5)
        //    {
        //        // IQueryable`1[TOuter] IEnumerable`1[TInner] Expression`1[Func`2 [TOuter TKey] Expression`1[Func`2 [TInner TKey] Expression`1[Func`3 [TOuter IEnumerable`1 TResult] IEqualityComparer`1[TKey] 
        //        return source.GroupJoin(args[0], args[1], args[2], args[3], args[4]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.GroupJoin においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        //internal static IQueryable<T> OrderBy<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 1)
        //    {
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] 
        //        return source.OrderBy(args[0]);
        //    }
        //    //else if (args.Count() == 2)
        //    //{
        //    //    // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] IComparer`1[TKey] 
        //    //    return source.OrderBy(args[0], args[1]);
        //    //}
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.OrderBy においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        //internal static IQueryable<T> OrderByDescending<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 1)
        //    {
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] 
        //        return source.OrderByDescending(args[0]);
        //    }
        //    //else if (args.Count() == 2)
        //    //{
        //    //    // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] IComparer`1[TKey] 
        //    //    return source.OrderByDescending(args[0], args[1]);
        //    //}
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.OrderByDescending においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        //internal static IQueryable<T> ThenBy<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 1)
        //    {
        //        // IOrderedQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] 
        //        return source.ThenBy(args[0]);
        //    }
        //    //else if (args.Count() == 2)
        //    //{
        //    //    // IOrderedQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] IComparer`1[TKey] 
        //    //    return source.ThenBy(args[0], args[1]);
        //    //}
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.ThenBy においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        //internal static IQueryable<T> ThenByDescending<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 1)
        //    {
        //        // IOrderedQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] 
        //        return source.ThenByDescending(args[0]);
        //    }
        //    //else if (args.Count() == 2)
        //    //{
        //    //    // IOrderedQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] IComparer`1[TKey] 
        //    //    return source.ThenByDescending(args[0], args[1]);
        //    //}
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.ThenByDescending においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        #endregion

        internal static IQueryable<T> Take<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Int32 
                return source.Take((int)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Take においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<T> TakeWhile<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                if (info[0].LambdaArgCount == 2)
                {
                    // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                    return source.TakeWhile((Expression<Func<T, bool>>)args[0]);
                }
                else if (info[0].LambdaArgCount == 3)
                {
                    // IQueryable`1[TSource] Expression`1[Func`3 [TSource Int32 Boolean] 
                    return source.TakeWhile((Expression<Func<T, int, bool>>)args[0]);
                }
                else
                {
                    throw new ArgumentException("メソッド名 IQueryable<T>.TakeWhile においてエラーが発生しました。引数の型、または数が正しくありません。");
                }
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.SkipWhile においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<T> Skip<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Int32 
                return source.Skip((int)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Skip においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<T> SkipWhile<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                if (info[0].LambdaArgCount == 2)
                {
                    // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                    return source.SkipWhile((Expression<Func<T, bool>>)args[0]);
                }
                else if (info[0].LambdaArgCount == 3)
                {
                    // IQueryable`1[TSource] Expression`1[Func`3 [TSource Int32 Boolean] 
                    return source.SkipWhile((Expression<Func<T, int, bool>>)args[0]);
                }
                else
                {
                    throw new ArgumentException("メソッド名 IQueryable<T>.SkipWhile においてエラーが発生しました。引数の型、または数が正しくありません。");
                }
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.SkipWhile においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }

        #region group by

        //internal static IQueryable<T> GroupBy<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 1)
        //    {
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] 
        //        return source.GroupBy(args[0]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.GroupBy においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //    if (args.Count() == 2)
        //    {
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] Expression`1[Func`2 [TSource TElement] 
        //        return source.GroupBy(args[0], args[1]);
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] Expression`1[Func`3 [TKey IEnumerable`1 TResult] 
        //        return source.GroupBy(args[0], args[1]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.GroupBy においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //    if (args.Count() == 3)
        //    {
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] Expression`1[Func`2 [TSource TElement] IEqualityComparer`1[TKey] 
        //        return source.GroupBy(args[0], args[1], args[2]);
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] Expression`1[Func`2 [TSource TElement] Expression`1[Func`3 [TKey IEnumerable`1 TResult] 
        //        return source.GroupBy(args[0], args[1], args[2]);
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] Expression`1[Func`3 [TKey IEnumerable`1 TResult] IEqualityComparer`1[TKey] 
        //        return source.GroupBy(args[0], args[1], args[2]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.GroupBy においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //    if (args.Count() == 4)
        //    {
        //        // IQueryable`1[TSource] Expression`1[Func`2 [TSource TKey] Expression`1[Func`2 [TSource TElement] Expression`1[Func`3 [TKey IEnumerable`1 TResult] IEqualityComparer`1[TKey] 
        //        return source.GroupBy(args[0], args[1], args[2], args[3]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.GroupBy においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}

        #endregion
        
        internal static IQueryable<T> Distinct<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.Distinct();
            }
            //else if (args.Count() == 1)
            //{
            //    // IQueryable`1[TSource] IEqualityComparer`1[TSource] 
            //    return source.Distinct(args[0]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Distinct においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<T> Concat<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] IEnumerable`1[TSource] 
                return source.Concat((IEnumerable<T>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Concat においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        //internal static IQueryable<T> Zip<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        //{
        //    if (args.Count() == 2)
        //    {
        //        // IQueryable`1[TFirst] IEnumerable`1[TSecond] Expression`1[Func`3 [TFirst TSecond TResult] 
        //        return source.Zip(args[0], args[1]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("メソッド名 IQueryable<T>.Zip においてエラーが発生しました。引数の型、または数が正しくありません。");
        //    }
        //}
        internal static IQueryable<T> Union<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] IEnumerable`1[TSource] 
                return source.Union((IEnumerable<T>)args[0]);
            }
            //else if (args.Count() == 2)
            //{
            //    // IQueryable`1[TSource] IEnumerable`1[TSource] IEqualityComparer`1[TSource] 
            //    return source.Union(args[0], args[1]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Union においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<T> Intersect<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] IEnumerable`1[TSource] 
                return source.Intersect((IEnumerable<T>)args[0]);
            }
            //else if (args.Count() == 2)
            //{
            //    // IQueryable`1[TSource] IEnumerable`1[TSource] IEqualityComparer`1[TSource] 
            //    return source.Intersect(args[0], args[1]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Intersect においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static IQueryable<T> Except<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 1)
            {
                // IQueryable`1[TSource] IEnumerable`1[TSource] 
                return source.Except((IEnumerable<T>)args[0]);
            }
            //else if (args.Count() == 2)
            //{
            //    // IQueryable`1[TSource] IEnumerable`1[TSource] IEqualityComparer`1[TSource] 
            //    return source.Except(args[0], args[1]);
            //}
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.Except においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }
        internal static T First<T>(IQueryable<T> source, object[] args, MethodTypeInfo[] info)
        {
            if (args.Count() == 0)
            {
                // IQueryable`1[TSource] 
                return source.First();
            }
            else if (args.Count() == 1)
            {
                // IQueryable`1[TSource] Expression`1[Func`2 [TSource Boolean] 
                return source.First((Expression<Func<T, bool>>)args[0]);
            }
            else
            {
                throw new ArgumentException("メソッド名 IQueryable<T>.First においてエラーが発生しました。引数の型、または数が正しくありません。");
            }
        }

        internal class MethodTypeInfo
        {
            internal Core.ArgumentInfo.ArgumentType ArgType { get; set; }
            internal int LambdaArgCount { get; set; }
            internal Type LambdaResult { get; set; }
        }

        
    }
}