using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace artfulplace.Linear.Linq
{
    public static class RangeExtension
    {
        /// <summary>
        /// 起点と終点の範囲内の整数のシーケンスを返します
        /// </summary>
        /// <param name="startIndex">起点となる数</param>
        /// <param name="lastIndex">終点となる数</param>
        /// <returns></returns>
        public static IEnumerable<int> Range2(int startIndex, int lastIndex)
        {
            int i;
            if (startIndex > lastIndex)
            {
                for (i = startIndex ; i >= lastIndex; i--)
                {
                    yield return i;
                }
            }
            else
            {
                for (i = startIndex; i <= lastIndex; i++)
                {
                    yield return i;
                }
            }
        }
    }
}
