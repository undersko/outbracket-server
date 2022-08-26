using System;
using System.Collections.Generic;
using System.Linq;

namespace Outbracket.Common.Extensions
{
    public static class ArrayExtensions
    {
        public static IEnumerable<TResult> ToEnumerable<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source == null ? Enumerable.Empty<TResult>() : source.Select(selector);
        }
        
        public static TResult[] ToNotNullArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source == null ? Array.Empty<TResult>() : source.Select(selector).Where(x => x != null).ToArray();
        }
    }
}