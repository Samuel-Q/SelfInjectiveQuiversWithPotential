using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotential.Data
{
    /// <summary>
    /// This class is used to import a quiver in plane from a file in the format of the
    /// &quot;Mutation App&quot;.
    /// </summary>
    public class QuiverInPlaneFromMutationAppImporter
    {
        /// <summary>
        /// Imports the quiver from the file with the specified path.
        /// </summary>
        /// <param name="path">The path of the file from which to import the quiver.</param>
        /// <returns>The quiver that was imported from the file.</returns>
        /// <remarks>
        /// <para><see cref="QuiverInPlane{TVertex}"/> currently does not have any support for
        /// parallel arrows (multiple edges). The Mutation App has support for parallel arrows, so
        /// Mutation App files may contain quivers with parallel arrows. This method throws an
        /// <see cref="ImporterException"/> if the file contains a quiver with parallel arrows.</para>
        /// </remarks>
        /// <exception cref="ImporterException">The quiver was not imported successfully from file.
        /// In particular, this may happen if the quiver stored in the Mutation App file contains
        /// parallel arrows.</exception>
        public QuiverInPlane<int> ImportQuiverInPlane(string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            var lines = ReadMutationAppStringFromFile(path);
            var quiverInPlane = GetQuiverInPlaneFromMutationAppLines(lines);

            return quiverInPlane;
        }

        private IEnumerable<string> ReadMutationAppStringFromFile(string path)
        {
            string errorMessage = "Failed to read data from file.";
            try
            {
                return File.ReadLines(path);
            }
            catch (ArgumentException ex) { throw new ImporterException(errorMessage, ex); }
            catch (PathTooLongException ex) { throw new ImporterException(errorMessage, ex); }
            catch (DirectoryNotFoundException ex) { throw new ImporterException(errorMessage, ex); }
            catch (FileNotFoundException ex) { throw new ImporterException(errorMessage, ex); }
            catch (IOException ex) { throw new ImporterException(errorMessage, ex); }
            catch (UnauthorizedAccessException ex) { throw new ImporterException(errorMessage, ex); }
            catch (NotSupportedException ex) { throw new ImporterException(errorMessage, ex); }
            catch (System.Security.SecurityException ex) { throw new ImporterException(errorMessage, ex); }
        }

        private QuiverInPlane<int> GetQuiverInPlaneFromMutationAppLines(IEnumerable<string> lines)
        {
            Dictionary<string, List<string>> fieldToDataDict;
            const string FailedToParseDataErrorMessage = "Failed to parse the data from the file.";
            try
            {
                fieldToDataDict = GetFieldToDataDictionary(lines);
            }
            catch (ImporterException ex)
            {
                throw new ImporterException(FailedToParseDataErrorMessage, ex);
            }

            int numVertices;
            bool[,] adjacencyMatrix;
            Point[] vertexPositions;
            try
            {
                numVertices = ReadNumberOfPoints(fieldToDataDict);
                adjacencyMatrix = ReadMatrix(fieldToDataDict, numVertices);
                vertexPositions = ReadPoints(fieldToDataDict, numVertices);
            }
            catch (ImporterException ex) { throw new ImporterException(FailedToParseDataErrorMessage, ex); }
            catch (NotSupportedException ex) { throw new ImporterException(FailedToParseDataErrorMessage, ex); }

            var quiverInPlane = GetQuiverInPlaneFromParsedData(numVertices, adjacencyMatrix, vertexPositions);

            return quiverInPlane;
        }

        /// <summary>
        /// Returns a dictionary mapping field names (e.g., &quot;Number of points&quot; or
        /// &quot;Vertex radius&quot;) to field data (e.g., 15 or 9).
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// <para>The field names are without the two leading slashes. The field data are unparsed
        /// collections of lines.</para>
        /// </remarks>
        private Dictionary<string, List<string>> GetFieldToDataDictionary(IEnumerable<string> lines)
        {
            const string FieldNamePrefix = "//";
            var dict = new Dictionary<string, List<string>>();
            string curField = null;
            foreach (var line in lines)
            {
                if (line.StartsWith(FieldNamePrefix))
                {
                    // Read field name
                    curField = line.Substring(FieldNamePrefix.Length);
                    if (dict.ContainsKey(curField))
                    {
                        throw new ImporterException($"Duplicate field {curField}.");
                    }

                    dict[curField] = new List<string>();
                }
                else
                {
                    // Append field data

                    // Silently ignore data before the first field name
                    if (curField is null) continue;

                    dict[curField].Add(line);
                }
            }

            return dict;
        }

        /// <summary>
        /// Reads the number of points (number of vertices) from the field-name-to-data dictionary.
        /// </summary>
        /// <param name="fieldNameToDataDict">The field-name-to-data dictionary.</param>
        /// <returns>The number of points.</returns>
        /// <exception cref="ImporterException">Something goes wrong.</exception>
        private int ReadNumberOfPoints(Dictionary<string, List<string>> fieldNameToDataDict)
        {
            const string NumberOfPointsFieldName = "Number of points";
            if (!fieldNameToDataDict.TryGetValue(NumberOfPointsFieldName, out var unparsedData))
            {
                throw new ImporterException($"'{NumberOfPointsFieldName}' field is not present.");
            }

            if (unparsedData.Count == 0)
            {
                throw new ImporterException($"{NumberOfPointsFieldName} field has no lines of data.");
            }
            else if (unparsedData.Count >= 2)
            {
                throw new ImporterException($"{NumberOfPointsFieldName} field has more than one line of data ({fieldNameToDataDict.Count} lines).");
            }

            var line = unparsedData[0];

            const string ErrorMessage = "Failed to parse number of points.";
            try
            {
                int numVertices = Int32.Parse(line, CultureInfo.InvariantCulture);
                return numVertices;
            }
            catch (FormatException ex) { throw new ImporterException(ErrorMessage, ex); }
            catch (OverflowException ex) { throw new ImporterException(ErrorMessage, ex); }
        }

        /// <summary>
        /// Reads the matrix (adjacency matrix) from the field-name-to-data dictionary.
        /// </summary>
        /// <param name="fieldToDataDict">The field-name-to-data dictionary.</param>
        /// <param name="numVertices">The number of vertices as read earlier.</param>
        /// <returns>The zero-based adjacency matrix.</returns>
        /// <exception cref="ImporterException">Something goes wrong (other than that the stored
        /// adjacency matrix has parallel arrows).</exception>
        /// <exception cref="NotSupportedException">The stored adjacency matrix has parallel arrows.</exception>
        private bool[,] ReadMatrix(Dictionary<string, List<string>> fieldToDataDict, int numVertices)
        {
            const string MatrixFieldName = "Matrix";
            if (!fieldToDataDict.TryGetValue(MatrixFieldName, out var unparsedData))
            {
                throw new ImporterException($"'{MatrixFieldName}' field is not present.");
            }

            if (unparsedData.Count == 0)
            {
                throw new ImporterException($"{MatrixFieldName} field has no lines of data.");
            }

            var matrixSizeLine = unparsedData[0];
            var splitLine = matrixSizeLine.Split(' ');
            int numMatrixSizeEntries = splitLine.Length;
            if (numMatrixSizeEntries != 2)
            {
                throw new ImporterException($"Matrix size line has {numMatrixSizeEntries} entr{(numMatrixSizeEntries == 1 ? "y" : "ies")}. Expected 2 entries.");
            }

            foreach (var matrixSizeEntryString in splitLine)
            {
                const string ErrorMessage = "Failed to parse matrix size entry.";
                try
                {
                    int matrixSizeEntry = Int32.Parse(matrixSizeEntryString, CultureInfo.InvariantCulture);
                    if (matrixSizeEntry != numVertices)
                    {
                        throw new ImporterException($"The matrix size entry {matrixSizeEntry} is incompatible with the number of points {numVertices}.");
                    }
                }
                catch (FormatException ex) { throw new ImporterException(ErrorMessage, ex); }
                catch (OverflowException ex) { throw new ImporterException(ErrorMessage, ex); }
            }
            
            if (unparsedData.Count != numVertices + 1)
            {
                throw new ImporterException($"{MatrixFieldName} field has {unparsedData.Count} lines of data. Expected {numVertices + 1} lines of data.");
            }

            var adjacencyMatrix = new bool[numVertices, numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                ReadMatrixRow(i);
            }

            return adjacencyMatrix;

            void ReadMatrixRow(int i)
            {
                var line = unparsedData[i + 1].TrimEnd(); // These lines have a trailing space for some reason.
                var rowEntries = line.Split(' ');
                if (rowEntries.Length != numVertices)
                {
                    throw new ImporterException($"The row of (zero-based) index {i} in the matrix has {rowEntries.Length} entries. Expected {numVertices} entries.");
                }

                for (int j = 0; j < numVertices; j++)
                {
                    var entryString = rowEntries[j];
                    string errorMessage = $"Failed to parse matrix entry ({i}, {j}) (zero-based).";
                    try
                    {
                        int entry = Int32.Parse(entryString, CultureInfo.InvariantCulture);
                        if (entry <= 0) continue;
                        else if (entry == 1) adjacencyMatrix[i, j] = true;
                        else throw new NotSupportedException($"Parallel arrows are not supported.");
                    }
                    catch (FormatException ex) { throw new ImporterException(errorMessage, ex); }
                    catch (OverflowException ex) { throw new ImporterException(errorMessage, ex); }
                }
            }
        }

        private Point[] ReadPoints(Dictionary<string, List<string>> fieldToDataDict, int numVertices)
        {
            const string PointsFieldName = "Points";
            if (!fieldToDataDict.TryGetValue(PointsFieldName, out var unparsedData))
            {
                throw new ImporterException($"'{PointsFieldName}' field is not present.");
            }

            if (unparsedData.Count != numVertices)
            {
                throw new ImporterException($"{PointsFieldName} field has {unparsedData.Count} line(s) of data. Expected {numVertices} lines of data.");
            }

            var points = new Point[numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                points[i] = ReadPoint(i);
            }

            return points;

            Point ReadPoint(int i)
            {
                var line = unparsedData[i];
                var pointEntries = line.Split(' ');
                const int XCoordinateIndex = 1;
                const int YCoordinateIndex = 2;
                int minimumNumEntries = Math.Max(XCoordinateIndex, YCoordinateIndex) + 1;
                if (pointEntries.Length < minimumNumEntries)
                {
                    throw new ImporterException($"The line for point {i} (zero-based) {pointEntries.Length} entries. At least {minimumNumEntries} entries are needed.");
                }

                int x, y;
                var xEntryString = pointEntries[XCoordinateIndex];
                const string XErrorMessage = "Failed to parse x-coordinate.";
                try
                {
                    double xEntry = Double.Parse(xEntryString, CultureInfo.InvariantCulture);
                    x = Convert.ToInt32(xEntry);
                }
                catch (FormatException ex) { throw new ImporterException(XErrorMessage, ex); }
                catch (OverflowException ex) // Catches exception from both Double.Parse and Convert.ToInt32.
                {
                    throw new ImporterException(XErrorMessage, ex);
                }

                var yEntryString = pointEntries[YCoordinateIndex];
                const string YErrorMessage = "Failed to parse y-coordinate.";
                try
                {
                    double yEntry = Double.Parse(yEntryString, CultureInfo.InvariantCulture);
                    y = Convert.ToInt32(yEntry);
                }
                catch (FormatException ex) { throw new ImporterException(YErrorMessage, ex); }
                catch (OverflowException ex) // Catches exception from both Double.Parse and Convert.ToInt32.
                {
                    throw new ImporterException(YErrorMessage, ex);
                }

                return new Point(x, y);
            }
        }

        private QuiverInPlane<int> GetQuiverInPlaneFromParsedData(int numVertices, bool[,] adjacencyMatrix, Point[] vertexPositions)
        {
            const int FirstVertex = 1;
            var vertices = Enumerable.Range(FirstVertex, numVertices);

            var arrows = adjacencyMatrix.Select((val, indices) => val ? new Arrow<int>(indices.Item1 + FirstVertex, indices.Item2 + FirstVertex) : null)
                                        .Where(val => val != null);

            var vertexPositionDict = vertexPositions.Select((point, i) => new KeyValuePair<int, Point>(i + FirstVertex, point))
                                                    .ToDictionary(p => p.Key, p => p.Value);

            var quiverInPlane = new QuiverInPlane<int>(vertices, arrows, vertexPositionDict);
            return quiverInPlane;
        }
    }
}
