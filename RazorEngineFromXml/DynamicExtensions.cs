using System;
using System.Collections.Generic;

namespace RazorEngineFromXml
{
    public static class DynamicExtensions
    {
        public static IEnumerable<dynamic> Select(this object source, Func<dynamic, dynamic> map)
        {
            foreach (dynamic item in source as dynamic)
            {
                yield return map(item);
            }
        }
        public static IEnumerable<dynamic> Where(this object source, Func<dynamic, dynamic> predicate)
        {
            foreach (dynamic item in source as dynamic)
            {
                if (predicate(item))
                    yield return item;
            }
        }
    }
}
