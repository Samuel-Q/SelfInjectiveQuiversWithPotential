using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SelfInjectiveQuiversWithPotentialTests
{
    static class MyCollectionAssert
    {
        public static void AllItemsAreEqual<T>(IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext()) return; // No elements
            var first = enumerator.Current;
            int curIndex = 0;

            while (enumerator.MoveNext())
            {
                ++curIndex;
                Assert.AreEqual(first, enumerator.Current, $"Items at indices 0 and {curIndex} differ: <{first}>, <{enumerator.Current}>");
            }
        }
    }
}
