using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Extensions.Linq
{
    public static class MoreLinq
    {
        public static T MaxByOrDefault<T>(this IEnumerable<T> enumerable, Func<T, IComparable> selector)
        {
            if(enumerable.Any() == false)
                return default;

            T result = enumerable.First();
            var resultComparable = selector(result);

            foreach(var value in enumerable)
            {
                var currentComparable = selector(value);
                if(currentComparable.CompareTo(resultComparable) > 0)
                {
                    result = value;
                    resultComparable = currentComparable;
                }
            }

            return result;
        }

        public static T MinByOrDefault<T>(this IEnumerable<T> enumerable, Func<T, IComparable> selector)
        {
            if(enumerable.Any() == false)
                return default;

            T result = enumerable.First();
            var resultComparable = selector(result);

            foreach(var value in enumerable)
            {
                var currentComparable = selector(value);
                if(currentComparable.CompareTo(resultComparable) < 0)
                {
                    result = value;
                    resultComparable = currentComparable;
                }
            }

            return result;
        }

    }
}
