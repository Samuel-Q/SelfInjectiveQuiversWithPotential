using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotential
{
    public static class Extensions
    {
        /// <remarks>For tuple deconstruction.</remarks>
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
        {
            key = tuple.Key;
            value = tuple.Value;
        }

        public static ISet<T> ToSet<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is null) throw new ArgumentNullException(nameof(enumerable));
            return new HashSet<T>(enumerable);
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
            return dictionary.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Gets the first element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to get the first element from.</param>
        /// <param name="element">Output parameter for the first element of the input sequence.</param>
        /// <returns><see langword="true"/> if <paramref name="source"/> contains at least one element;
        /// <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        public static bool TryFirst<TSource>(this IEnumerable<TSource> source, out TSource element)
        {
            return source.TryFirst(elem => true, out element);
        }

        /// <summary>
        /// Gets the first element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TSource">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to get an element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="element">Output parameter for the first element of the input sequence
        /// that satisfies the condition.</param>
        /// <returns><see langword="true"/> if <paramref name="source"/> contains at least one
        /// element that satisfies the condition; <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>,
        /// or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static bool TryFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource element)
        {
            return source.TryFirst(predicate, out element, out _);
        }

        /// <summary>
        /// Gets the first element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TSource">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to get an element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="element">Output parameter for the first element of the input sequence
        /// that satisfies the condition.</param>
        /// <param name="index">Output parameter for the zero-based index of the first element of
        /// the input sequence that satisfies the condition.</param>
        /// <returns><see langword="true"/> if <paramref name="source"/> contains at least one
        /// element that satisfies the condition; <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>,
        /// or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static bool TryFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource element, out int index)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            index = 0;
            foreach (var elem in source)
            {
                if (predicate(elem))
                {
                    element = elem;
                    return true;
                }

                ++index;
            }

            element = default; // Unnecessary, but okay
            return false;
        }

        /// <summary>
        /// Gets the only element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to get a single element from.</param>
        /// <param name="element">Output parameter for the single element of the input sequence.</param>
        /// <returns><see langword="true"/> if <paramref name="source"/> contains a single element;
        /// <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        public static bool TrySingle<TSource>(this IEnumerable<TSource> source, out TSource element)
        {
            return source.TrySingle(elem => true, out element);
        }

        /// <summary>
        /// Gets the only element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TSource">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to get a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="element">Output parameter for the single element of the input sequence
        /// that satisfies the condition.</param>
        /// <returns><see langword="true"/> if <paramref name="source"/> contains a single element
        /// that satisfies the condition; <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>,
        /// or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static bool TrySingle<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource element)
        {
            return source.TrySingle(predicate, out element, out _);
        }

        /// <summary>
        /// Gets the only element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TSource">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to get a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="element">Output parameter for the single element of the input sequence
        /// that satisfies the condition.</param>
        /// <param name="index">Output parameter for the zero-based index of the single element of
        /// the input sequence that satisfies the condition.</param>
        /// <returns><see langword="true"/> if <paramref name="source"/> contains a single element
        /// that satisfies the condition; <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>,
        /// or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static bool TrySingle<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource element, out int index)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            int numMatchingElements = 0;
            int tempIndex = index = 0;
            element = default; // Needed to appease the compiler.
            foreach (var elem in source)
            {
                if (predicate(elem))
                {
                    if (++numMatchingElements > 1)
                    {
                        element = default;
                        return false;
                    }

                    element = elem;
                    index = tempIndex;
                }

                ++tempIndex;
            }

            if (numMatchingElements == 0)
            {
                element = default;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indicates whether two enumerables are equal up to order.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerables.</typeparam>
        /// <param name="enumerable1">The first enumerable.</param>
        /// <param name="enumerable2">The second enumerable.</param>
        /// <returns><see langword="true"/> if <paramref name="enumerable1"/> contains the same
        /// elements with the same multiplicity as <paramref name="enumerable2"/>;
        /// <see langword="false"/> otherwise.</returns>
        public static bool EqualUpToOrder<T>(this IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
        {
            if (enumerable1 == null) throw new ArgumentNullException(nameof(enumerable1));
            if (enumerable2 == null) throw new ArgumentNullException(nameof(enumerable2));

            // This implementation could probably be optimized
            var list = new List<T>(enumerable2);
            foreach (var elem in enumerable1)
            {
                var index = list.FindIndex(x => x.Equals(elem));
                if (index == -1) return false;
                list.RemoveAt(index);
            }

            return (list.Count == 0);
        }

        /// <summary>
        /// Indicates whether all items of the specified <see cref="IEnumerable{T}"/> are equal.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="this">The collection.</param>
        /// <returns>A boolean value indicating whether all items of <paramref name="this"/> are equal.</returns>
        public static bool AllAreEqual<T>(this IEnumerable<T> @this)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.Count() == 0) return true;
            var first = @this.First();
            return @this.All(x => x.Equals(first));
        }

        public static bool None<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.All(x => !predicate(x));
        }

        public static IEnumerable<T> Cons<T>(this T element, IEnumerable<T> collection)
        {
            return new T[] { element }.Concat(collection);
        }

        // As of .NET Framework 4.7.1 (?), there is an extension method "Append" for this
        public static IEnumerable<T> AppendElement<T>(this IEnumerable<T> enumerable, T element)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            return enumerable.Concat(new T[] { element });
        }

        public static T Last<T>(this IReadOnlyList<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (list.Count == 0) throw new InvalidOperationException("The list is empty.");
            return list[list.Count - 1];
        }

        public static void RemoveLastElement<T>(this List<T> list)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (list.Count == 0) throw new InvalidOperationException("The list is empty.");
            list.RemoveAt(list.Count - 1);
        }

        public static IEnumerable<(T, int)> EnumerateWithIndex<T>(this IEnumerable<T> @this)
        {
            return @this.Select((x, i) => (element: x, index: i));
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> collection, int count)
        {
            var countToTake = Math.Max(collection.Count() - count, 0);
            return collection.Take(countToTake);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        /// <remarks>This extension method provides an efficient implementation of
        /// <see cref="Enumerable.Reverse{TSource}(IEnumerable{TSource})"/> for lists.</remarks>
        public static IEnumerable<T> Reversed<T>(this IReadOnlyList<T> @this)
        {
            for (int i = @this.Count-1; i >= 0; --i)
            {
                yield return @this[i];
            }
        }

        /// <summary>
        /// Indicates whether the collection has a duplicate element.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="this"/>.</typeparam>
        /// <param name="this">The collection.</param>
        /// <returns><see langword="true"/> if the collection contains a duplicate; <see langword="false"/> otherwise.</returns>
        public static bool HasDuplicate<T>(this IEnumerable<T> @this)
        {
            return @this.TryGetDuplicate(out _);
        }

        /// <summary>
        /// Gets a duplicate element of a <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="this"/>.</typeparam>
        /// <param name="this">The collection.</param>
        /// <param name="duplicate">Output parameter for the duplicate element, if it was found.</param>
        /// <returns><see langword="true"/> if the collection contains a duplicate; <see langword="false"/> otherwise.</returns>
        public static bool TryGetDuplicate<T>(this IEnumerable<T> @this, out T duplicate)
        {
            var copy = new HashSet<T>();
            foreach (var element in @this)
            {
                if (copy.Contains(element))
                {
                    duplicate = element;
                    return true;
                }

                copy.Add(element);
            }

            duplicate = default;
            return false;
        }

        public static IEnumerable<T> WithoutDuplicates<T>(this IEnumerable<T> @this)
        {
            return new HashSet<T>(@this);
        }

        /// <summary>
        /// Sorts the specified <see cref="IEnumerable{T}"/> using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to sort.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> with the elements of <paramref name="this"/>
        /// sorted in ascending order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The default comparer
        /// <see cref="Comparer{T}.Default"/> cannot find an implementation of the
        /// <see cref="IComparable{T}"/> generic interface or the <see cref="IComparable"/>
        /// interface for type <typeparamref name="T"/>.</exception>
        public static IEnumerable<T> Sorted<T>(this IEnumerable<T> @this)
        {
            return @this.Sorted(Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts the specified <see cref="IEnumerable{T}"/> using the specified
        /// <see cref="Comparison{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to sort.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> with the elements of <paramref name="this"/>
        /// sorted in ascending order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is
        /// <see langword="null"/>, or <paramref name="comparison"/> is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<T> Sorted<T>(this IEnumerable<T> @this, Comparison<T> comparison)
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));
            if (comparison is null) throw new ArgumentNullException(nameof(comparison));

            var list = @this.ToList();
            list.Sort(comparison);
            return list;
        }

        /// <summary>
        /// Sorts the specified <see cref="IEnumerable{T}"/> using the specified comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to sort.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> with the elements of <paramref name="this"/>
        /// sorted in ascending order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> is
        /// <see langword="null"/>, or <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<T> Sorted<T>(this IEnumerable<T> @this, IComparer<T> comparer)
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            var list = @this.ToList();
            list.Sort(comparer);
            return list;
        }

        // Example: Input {1, 2, 3, 4} gives {(1,2), (2,3), (3,4)} as output
        public static IEnumerable<(T, T)> AdjacentPairs<T>(this IEnumerable<T> collection)
        {
            bool firstIteration = true;
            T prev = default;
            foreach (var cur in collection)
            {
                if (firstIteration) firstIteration = false;
                else
                {
                    yield return (prev, cur);
                }

                prev = cur;
            }
        }

        public static IReadOnlyDictionary<TSourceValue, TSourceKey> Inverse<TSourceKey, TSourceValue>(this IReadOnlyDictionary<TSourceKey, TSourceValue> dictionary)
        {
            return dictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        /// <summary>
        /// Determines whether the dictionary with keys of an enumeration type has precisely the
        /// enumeration members as keys.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary whose keys to investigate.</param>
        /// <returns><see langword="true"/> if <c><paramref name="dictionary"/>.Keys</c> is equal
        /// to the collection of enumeration members of <typeparamref name="TKey"/> as unordered
        /// collections (possibly with repetition); <see langword="false"/> otherwise.</returns>
        /// <remarks>Because <c><paramref name="dictionary"/>.Keys</c> cannot contain duplicates,
        /// the return value is <see langword="false"/> if the enumeration has several members with
        /// the same value.</remarks>
        public static bool HasAllEnumMembersAsKeys<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary) where TKey : Enum
        {
            return dictionary.Keys.EqualUpToOrder(Enum.GetValues(typeof(TKey)).Cast<TKey>());
        }

        public static bool IsInEnum<TEnum>(this TEnum value) where TEnum : Enum
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }

        /// <summary>
        /// Projects the specified two-dimensional array into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">An array of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>An array whose elements are the result of invoking the transform function on
        /// each element of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is
        /// <see langword="null"/>, or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static TResult[,] Select<TSource, TResult>(this TSource[,] source, Func<TSource, TResult> selector)
        {
            TResult WrappedSelector(TSource val, (int i, int j) indices) => selector(val);
            return source.Select(WrappedSelector);
        }

        /// <summary>
        /// Projects the specified two-dimensional array into a new form by incorporating the
        /// element's indices.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">An array of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element;
        /// the second parameter of the function represents the indices of the source element.</param>
        /// <returns>An array whose elements are the result of invoking the transform function on
        /// each element of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is
        /// <see langword="null"/>, or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static TResult[,] Select<TSource, TResult>(this TSource[,] source, Func<TSource, (int, int), TResult> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            var lengths = new int[]
            {
                source.GetUpperBound(0) - source.GetLowerBound(0) + 1,
                source.GetUpperBound(1) - source.GetLowerBound(1) + 1
            };
            var lowerBounds = new int[]
            {
                source.GetLowerBound(0),
                source.GetLowerBound(1)
            };
            var upperBounds = new int[]
            {
                source.GetUpperBound(0),
                source.GetUpperBound(1)
            };
            var result = (TResult[,])Array.CreateInstance(typeof(TResult), lengths, lowerBounds);
            for (int i = lowerBounds[0]; i <= upperBounds[0]; i++)
            {
                for (int j = lowerBounds[1]; j <= upperBounds[1]; j++)
                {
                    result[i, j] = selector(source[i, j], (i, j));
                }
            }

            return result;
        }

        /// <summary>
        /// Filters a two-dimensional array of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An array to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{TSource}"/> that contains elements from the input
        /// array that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is
        /// <see langword="null"/>, or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TSource> Where<TSource>(this TSource[,] source, Func<TSource, bool> predicate)
        {
            bool WrappedPredicate(TSource val, (int i, int j) indices) => predicate(val);
            return source.Where(WrappedPredicate);
        }

        /// <summary>
        /// Filters a two-dimensional array of values based on a predicate. Each element's indices
        /// is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An array to filter.</param>
        /// <param name="predicate">A function to test each element for a condition;
        /// the second parameter of the function represents the indices of the source element.</param>
        /// <returns>An <see cref="IEnumerable{TSource}"/> that contains elements from the input
        /// array that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is
        /// <see langword="null"/>, or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static IEnumerable<TSource> Where<TSource>(this TSource[,] source, Func<TSource, (int, int), bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            for (int i = source.GetLowerBound(0); i <= source.GetUpperBound(0); i++)
            {
                for (int j = source.GetLowerBound(1); j <= source.GetUpperBound(1); j++)
                {
                    var val = source[i, j];
                    if (predicate(val, (i, j)))
                    {
                        yield return val;
                    }
                }
            }
        }

        public static bool IsEven(this int a)
        {
            return a % 2 == 0;
        }

        public static bool IsOdd(this int a)
        {
            return a % 2 == 1;
        }

        /// <summary>
        /// Computes the least non-negative residue.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>a mod b.</returns>
        /// <remarks>The % operator returns the <em>remainder</em>, which may be different from the
        /// least non-negative residue when the dividend is negative, when the dividend is divided
        /// by the divisor.</remarks>
        public static int Modulo(this int a, int b)
        {
            var result = a % b;
            if (result < 0) result += b;
            return result;
        }

        /// <summary>
        /// Determines if an integer is a multiple of another.
        /// </summary>
        /// <param name="a">The tentative multiple.</param>
        /// <param name="b">The tentative divisor.</param>
        /// <returns>A boolean value indicating whether <paramref name="a"/> is a multiple of
        /// <paramref name="b"/>.</returns>
        public static bool IsMultipleOf(this int a, int b)
        {
            if (b == 0) return a == 0;
            else return a % b == 0;
        }

        public static int CapFromBelowBy(this int a, int lowerBound)
        {
            return a < lowerBound ? lowerBound : a;
        }

        public static int CapFromAboveBy(this int a, int upperBound)
        {
            return a > upperBound ? upperBound : a;
        }

        public static bool NextBool(this Random random)
        {
            return random.Next(2) == 1;
        }

        public static bool TryExtractTrailingCycle<TVertex>(this Path<TVertex> @this, out Path<TVertex> closedPath) where TVertex : IEquatable<TVertex>, IComparable<TVertex>
            => TryExtractTrailingCycle(@this, out closedPath, out _);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="this"></param>
        /// <param name="closedPath"></param>
        /// <param name="startIndex">Output parameter for the index of the first vertex/arrow in the cycle.</param>
        /// <returns></returns>
        public static bool TryExtractTrailingCycle<TVertex>(this Path<TVertex> @this, out Path<TVertex> closedPath, out int startIndex) where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var vertices = @this.Vertices.ToList();
            var lastVertex = vertices.Last();
            int indexOfSecondToLastOccurrence = vertices.Take(vertices.Count - 1).ToList().FindLastIndex(v => v.Equals(lastVertex));
            if (indexOfSecondToLastOccurrence == -1)
            {
                closedPath = null;
                startIndex = -1;
                return false;
            }

            // The conversion from vertex index to arrow index ends up being painless
            startIndex = indexOfSecondToLastOccurrence;

            var subpath = @this.ExtractSubpath(startIndex, @this.Length - startIndex);
            closedPath = subpath;
            return true;
        }

        /// <summary>
        /// Gets the inner exceptions of the specified exception.
        /// </summary>
        /// <param name="ex">The exception whose inner exceptions to get.</param>
        /// <returns>An <see cref="IEnumerable{Exception}"/> containing the inner exceptions.</returns>
        /// <remarks><paramref name="ex"/> itself is also contained in the returned <see cref="IEnumerable{Exception}"/>.</remarks>
        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            if (ex is null) throw new ArgumentNullException(nameof(ex));

            while (ex != null)
            {
                yield return ex;
                ex = ex.InnerException;
            }
        }
    }
}
