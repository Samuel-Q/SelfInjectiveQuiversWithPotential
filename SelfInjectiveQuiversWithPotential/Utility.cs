using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    public static class Utility
    {
        #region General
        /// <summary>
        /// Returns an <see cref="IEnumerable{IReadOnlyList{T}}"/> of all &quot;permutations with repetition&quot; of
        /// elements from a specified list.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="collection">The <see cref="IReadOnlyList{T}"/> from which to pick the
        /// combinations.</param>
        /// <param name="numElements">The number of elements in each combination.</param>
        /// <returns>An <see cref="IEnumerable{IReadOnlyList{T}}"/> of all &quot;permutations with repetition&quot; of
        /// elements from <paramref name="collection"/>.</returns>
        /// <remarks>
        /// <para><paramref name="collection"/> is assumed to contain no duplicates.</para>
        /// </remarks>
        /// Would be nice to implement this for a potentially infinite IEnumerable, but it would
        /// make the implementation more complicated (and slower).
        /// Would be good to specify what happens for negative k (numElements). Should probably
        /// return an empty list (which I think is done as of this writing).
        public static IEnumerable<IReadOnlyList<T>> EnumeratePermutationsWithRepetition<T>(IReadOnlyList<T> collection, int numElements)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var comb in EnumeratePermutationsWithRepetitionHelper(collection, 0, numElements, new List<T>())) yield return comb;
        }

        // The startIndex argument is superfluous, but it is kept to show the similarity with the helper for combinations
        // (the implementations differ only by what is passed as the start index).
        // This implementation (foreach yield returning in each iteration) seems crazy
        private static IEnumerable<IReadOnlyList<T>> EnumeratePermutationsWithRepetitionHelper<T>(IReadOnlyList<T> collection, int startIndex, int numElements, List<T> acc)
        {
            if (numElements <= 0)
            {
                yield return new List<T>(acc);
                yield break;
            }

            for (int index = startIndex; index < collection.Count; index++)
            {
                var item = collection[index];
                acc.Add(item);
                foreach (var comb in EnumeratePermutationsWithRepetitionHelper(collection, 0, numElements - 1, acc)) yield return comb;
                acc.RemoveAt(acc.Count - 1);
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{IReadOnlyList{T}}"/> of all combinations (with repetition) of
        /// elements from a specified list.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="collection">The <see cref="IReadOnlyList{T}"/> from which to pick the
        /// combinations.</param>
        /// <param name="numElements">The number of elements in each combination.</param>
        /// <returns>An <see cref="IEnumerable{IReadOnlyList{T}}"/> of all combinations (with repetition) of
        /// elements from <paramref name="collection"/>.</returns>
        /// <remarks>
        /// <para><paramref name="collection"/> is assumed to contain no duplicates.</para>
        /// <para>The combinations (each represented as an <see cref="IReadOnlyList{T}"/>) in the
        /// returned <see cref="IEnumerable{IReadOnlyList{T}}"/> are inherently ordered but may each
        /// be viewed as a representative of the equivalence class of ordered arrangements that is
        /// the unordered arrangement (combination). The returned <see cref="IEnumerable{IReadOnlyList{T}}"/>
        /// contains precisely one representative for each equivalence class (combination).
        /// </para></remarks>
        /// Would be nice to implement this for a potentially infinite IEnumerable, but it would
        /// make the implementation more complicated (and slower).
        /// Would be good to specify what happens for negative k (numElements). Should probably
        /// return an empty list (which I think is done as of this writing).
        public static IEnumerable<IReadOnlyList<T>> EnumerateCombinationsWithRepetition<T>(IReadOnlyList<T> collection, int numElements)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var comb in EnumerateCombinationsWithRepetitionHelper(collection, 0, numElements, new List<T>())) yield return comb;
        }

        // This implementation (foreach yield returning in each iteration) seems crazy
        private static IEnumerable<IReadOnlyList<T>> EnumerateCombinationsWithRepetitionHelper<T>(IReadOnlyList<T> collection, int startIndex, int numElements, List<T> acc)
        {
            if (numElements <= 0)
            {
                yield return new List<T>(acc);
                yield break;
            }

            for (int index = startIndex; index < collection.Count; index++)
            {
                var item = collection[index];
                acc.Add(item);
                foreach (var comb in EnumerateCombinationsWithRepetitionHelper(collection, index, numElements - 1, acc)) yield return comb;
                acc.RemoveAt(acc.Count - 1);
            }
        }

        /// <summary>
        /// <see cref="Enumerable.Range(int, int)"/> that allows negative count.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<int> SafeRange(int start, int count)
        {
            return Enumerable.Range(start, count >= 0 ? count : 0);
        }

        public static IEnumerable<int> InfiniteRange(int start)
        {
            return InfiniteRange(start, step: 1);
        }

        public static IEnumerable<int> InfiniteRange(int start, int step)
        {
            int val = start;
            while (true)
            {
                yield return val;
                val += step;
            }
        }

        /// <summary>
        /// Generates a sequence that contain a sequence of values repeated.
        /// </summary>
        /// <typeparam name="TResult">The type of the values in the sequence to be repeated.</typeparam>
        /// <param name="sequence">The sequence to be repeated.</param>
        /// <param name="count">The number of times to repeat the value in the generated sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the sequence repeated.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
        public static IEnumerable<TResult> RepeatMany<TResult>(IEnumerable<TResult> sequence, int count)
        {
            if (sequence is null) throw new ArgumentNullException(nameof(sequence));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return Enumerable.Repeat(sequence, count).SelectMany(seq => seq);
        }

        /// <summary>
        /// Generates a sequence that contain a sequence of values repeated.
        /// </summary>
        /// <typeparam name="TResult">The type of the values in the sequence to be repeated.</typeparam>
        /// <param name="count">The number of times to repeat the value in the generated sequence.</param>
        /// <param name="sequence">The sequence to be repeated.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the sequence repeated.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
        public static IEnumerable<TResult> RepeatMany<TResult>(int count, params TResult[] sequence)
        {
            if (sequence is null) throw new ArgumentNullException(nameof(sequence));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            return Enumerable.Repeat(sequence, count).SelectMany(seq => seq);
        }

        public static IEnumerable<(T1, T2)> CartesianProduct<T1, T2>(IEnumerable<T1> collection1, IEnumerable<T2> collection2)
        {
            return from elem1 in collection1
                   from elem2 in collection2
                   select (elem1, elem2);
        }

        public static IEnumerable<TEnum> GetEnumValues<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
        #endregion

        #region Application-specific
        private static IEnumerable<Arrow<int>> GetPathArrows(int startVertex, int arrowCount)
        {
            var arrows = Enumerable.Range(startVertex, arrowCount).Select(n => new Arrow<int>(n, n + 1));
            return arrows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstVertex"></param>
        /// <param name="lastVertex">The last vertex of the path (inclusive).</param>
        /// <returns></returns>
        private static IEnumerable<Arrow<int>> GetPathArrowsFromTo(int firstVertex, int lastVertex)
        {
            int arrowCount = lastVertex - firstVertex; // This makes a loop if firstVertex == lastVertex, which is probably the reasonable behavior
            return GetPathArrows(firstVertex, arrowCount);
        }

        public static Path<int> MakePath(int startVertex, int pathLength)
        {
            var path = new Path<int>(startVertex, GetPathArrows(startVertex, pathLength));
            return path;
        }

        public static Path<int> MakePathFromTo(int firstVertex, int lastVertex)
        {
            var arrows = GetPathArrowsFromTo(firstVertex, lastVertex);
            var path = new Path<int>(firstVertex, arrows);
            return path;
        }

        /// <summary>
        /// Makes a path from one vertex to another using the specified intermediary vertices.
        /// </summary>
        /// <param name="firstVertex"></param>
        /// <param name="lastVertex"></param>
        /// <param name="pathLength"></param>
        /// <param name="nextVertex"></param>
        /// <remarks>The &quot;interior&quot; vertices of the path are
        /// <paramref="nextVertex"/>, <paramref="nextVertex"/>+1, ..., <paramref="nextVertex"/>+<paramref name="pathLength"/>-1.</remarks>
        /// <returns></returns>
        public static Path<int> MakePath(int firstVertex, int lastVertex, int pathLength, int nextVertex)
        {
            if (pathLength <= 0) throw new ArgumentOutOfRangeException(nameof(pathLength)); // If firstVertex == lastVertex, pathLength == 0 might make sense though

            if (pathLength == 1)
            {
                var arrowsList = new List<Arrow<int>> { new Arrow<int>(firstVertex, lastVertex) };
                return new Path<int>(firstVertex, arrowsList);
            }

            var arrows = new List<Arrow<int>>
            {
                new Arrow<int>(firstVertex, nextVertex)
            };

            int lastNewVertex = nextVertex + pathLength - 2;
            arrows.AddRange(GetPathArrowsFromTo(nextVertex, lastNewVertex));
            arrows.Add(new Arrow<int>(lastNewVertex, lastVertex));

            var path = new Path<int>(firstVertex, arrows);
            return path;
        }

        public static SimpleCycle<int> MakeCycle(int startVertex, int numVertices)
        {
            int lastVertex = startVertex + numVertices - 1;
            var arrows = Enumerable.Range(startVertex, numVertices).Select(x => (x != lastVertex) ? new Arrow<int>(x, x + 1) : new Arrow<int>(lastVertex, startVertex));
            var cycle = new SimpleCycle<int>(arrows);
            return cycle;
        }

        public static bool PathsAreVertexDisjoint<TVertex>(Path<TVertex> path1, Path<TVertex> path2) where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            return path1.Vertices.Intersect(path2.Vertices).Count() == 0;
        }
        #endregion

        #region Math
        public static int Power(int @base, int exponent)
        {
            if (exponent < 0) throw new ArgumentOutOfRangeException(nameof(exponent));
            int val = 1;
            while (exponent-- > 0) val *= @base;
            return val;
        }

        public static int Factorial(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            int val = 1;
            while (n > 0) val *= (n--);
            return val;
        }

        public static int BinomialCoefficient(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }

        public static int TriangularNumber(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            return n * (n + 1) / 2;
        }

        // Code duplication but no other headache from having two implementations
        public static int GetOrderOfPermutation<T>(IReadOnlyDictionary<T, T> permutation)
        {
            // TODO: Make sure that the permutation really is a permutation
            // Stupid but easy implementation
            var identity = permutation.Keys.ToDictionary(x => x);
            var function = new Dictionary<T, T>(permutation.ToDictionary(p => p.Key, p => p.Value));
            int order = 1;
            while (!function.SequenceEqual(identity))
            {
                foreach (var point in function.Keys.ToList()) function[point] = permutation[function[point]];
                order++;
            }

            return order;
        }

        public static int GetOrderOfPermutation<T>(IDictionary<T, T> permutation)
        {
            // TODO: Make sure that the permutation really is a permutation
            // Stupid but easy implementation
            var identity = permutation.Keys.ToDictionary(x => x);
            var function = new Dictionary<T, T>(permutation);
            int order = 1;
            while (!function.SequenceEqual(identity))
            {
                foreach (var point in function.Keys.ToList()) function[point] = permutation[function[point]];
                order++;
            }

            return order;
        }
        #endregion
    }
}
