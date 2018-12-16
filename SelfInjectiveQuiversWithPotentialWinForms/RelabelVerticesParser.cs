using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    // TODO: Consider placing the TryParserVerticesString (which is also used in RotateVerticesParser)
    // in a common class.

    /// <summary>
    /// This class is used to parse the strings of whitespace-separated vertices as relabeling maps
    /// (i.e., an injective map of some subset of the integers to some subset of the integers).
    /// </summary>
    /// <remarks>
    /// <para>This class does not pay attention to the quiver in which the relabeling will take
    /// place. That is, this class does not ensure that the old vertices are actual vertices in the
    /// quiver and that the new vertices do not clash with some of the unrelabeled vertices in the
    /// quiver.</para>
    /// </remarks>
    public class RelabelVerticesParser
    {
        private bool TryParseVerticesString(string verticesString, out IEnumerable<int> vertices, out string errorMessage)
        {
            // Empty separator array for splitting on all whitespace characters.
            var splitString = verticesString.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            try
            {
                vertices = splitString.Select(str => Int32.Parse(str)).ToList();
                errorMessage = null;
                return true;
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                vertices = null;
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Creates the relabeling map defined by the specified strings of vertices.
        /// </summary>
        /// <param name="oldVerticesString">A whitespace-separated string of the vertices to
        /// relabel.</param>
        /// <param name="newVerticesString">A whitespace-separated string of the new labels for the
        /// vertices.</param>
        /// <param name="relabelingMap">Output parameter for the relabeling map, which is set to
        /// <see langword="null"/> if the relabeling map is not created successfully.</param>
        /// <param name="errorMessage">Output parameter for an error message in case the
        /// relabeling map is not created successfully.</param>
        /// <returns><see langword="true"/> if the relabeling map is successfully created;
        /// <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="oldVerticesString"/> is
        /// <see langword="null"/>, or <paramref name="newVerticesString"/> is
        /// <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>The relabeling map cannot be successfully created in any of the following cases:
        /// <list type="bullet">
        /// <item><description><paramref name="oldVerticesString"/> or
        /// <paramref name="newVerticesString"/> is not a whitespace-separated string of integers.
        /// </description></item>
        /// <item><description><paramref name="oldVerticesString"/> or
        /// <paramref name="newVerticesString"/> contains an integer less than
        /// <see cref="int.MinValue"/> or greater than <see cref="int.MaxValue"/>.</description></item>
        /// <item><description><paramref name="oldVerticesString"/> and
        /// <paramref name="newVerticesString"/> contain different numbers of integers.</description></item>
        /// <item><description><paramref name="oldVerticesString"/> or
        /// <paramref name="newVerticesString"/> contains a duplicate integer.</description></item>
        /// </list>
        /// </para>
        /// <para>More than one whitespace character may separate two vertices in the strings of
        /// vertices.</para>
        /// </remarks>
        public bool TryCreateRelabelingMap(
            string oldVerticesString,
            string newVerticesString,
            out IReadOnlyDictionary<int, int> relabelingMap,
            out string errorMessage)
        {
            if (oldVerticesString is null) throw new ArgumentNullException(nameof(oldVerticesString));
            if (newVerticesString is null) throw new ArgumentNullException(nameof(newVerticesString));

            relabelingMap = null;
            errorMessage = null;

            if (!TryParseVerticesString(oldVerticesString, out var oldVertices, out string errorMessage2))
            {
                errorMessage = "Failed to parse the string of old vertices." + Environment.NewLine + errorMessage2;
                return false;
            }

            if (!TryParseVerticesString(newVerticesString, out var newVertices, out errorMessage2))
            {
                errorMessage = "Failed to parse the string of new vertices." + Environment.NewLine + errorMessage2;
                return false;
            }

            if (oldVertices.Count() != newVertices.Count())
            {
                errorMessage = $"The numbers of old vertices ({oldVertices.Count()}) and new vertices ({newVertices.Count()}) are not equal.";
                return false;
            }

            if (oldVertices.TryGetDuplicate(out var duplicate))
            {
                errorMessage = $"The string of old vertices has a duplicate {duplicate}.";
                return false;
            }

            if (newVertices.TryGetDuplicate(out duplicate))
            {
                errorMessage = $"The string of new vertices has a duplicate {duplicate}.";
                return false;
            }

            relabelingMap = oldVertices.Zip(newVertices, (oldVertex, newVertex) => new KeyValuePair<int, int>(oldVertex, newVertex))
                                       .ToDictionary(pair => pair.Key, pair => pair.Value);
            return true;
        }
    }
}
