using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace empty_project.Utilities
{
    public static class EnumerableWithIndex
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }
}
