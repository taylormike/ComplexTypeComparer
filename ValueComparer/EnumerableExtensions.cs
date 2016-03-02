using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueComparison
{
    public static class EnumerableExtensions
    {
        internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach(T item in source)
            {
                action(item);
            }
        }

        // returns each paris as a KeyValuePair<,>
        internal static IEnumerable<KeyValuePair<TLeft, TRight>> Zip<TLeft, TRight>(
            this IEnumerable<TLeft> left, IEnumerable<TRight> right)
        {
            return Zip(left, right, (x, y) => new KeyValuePair<TLeft, TRight>(x, y));
        }

        // accepts a projection from the caller for each pair
        internal static IEnumerable<TResult> Zip<TLeft, TRight, TResult>(
            this IEnumerable<TLeft> left, IEnumerable<TRight> right,
            Func<TLeft, TRight, TResult> selector)
        {
            using (IEnumerator<TLeft> leftE = left.GetEnumerator())
            using (IEnumerator<TRight> rightE = right.GetEnumerator())
            {
                while (leftE.MoveNext() && rightE.MoveNext())
                {
                    yield return selector(leftE.Current, rightE.Current);
                }
            }
        }
    }
}
