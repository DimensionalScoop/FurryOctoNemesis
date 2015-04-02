using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoopFramework.Extensions
{
    public static class ListExtension
    {
        public static int GetOrderIndependentHashCode<T>(this IEnumerable<T> source)
        {
            int hash = 0;
            foreach (T element in source)
            {
                hash = hash ^ EqualityComparer<T>.Default.GetHashCode(element);
            }
            return hash;
        }
    }
}
