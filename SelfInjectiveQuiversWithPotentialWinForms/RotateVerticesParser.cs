using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    // TODO: Consider placing the TryParserVerticesString (which is also used in RelabelVerticesParser)
    // in a common class.

    /// <summary>
    /// This class is used to parse the user input for the &quot;rotate vertices&quot; action.
    /// </summary>
    /// <remarks>
    /// <para>This class does not pay attention to the quiver in which the relabeling will take
    /// place. That is, this class does not ensure that the vertices are actual vertices in the
    /// quiver.</para>
    /// </remarks>
    public class RotateVerticesParser
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

        public bool TryGetRotationData(string verticesString, out IEnumerable<int> vertices, out string errorMessage)
        {
            vertices = null;
            errorMessage = null;

            if (!TryParseVerticesString(verticesString, out vertices, out string errorMessage2))
            {
                errorMessage = $"Failed to parse the string of vertices.{Environment.NewLine}{errorMessage2}";
                return false;
            }

            return true;
        }
    }
}
