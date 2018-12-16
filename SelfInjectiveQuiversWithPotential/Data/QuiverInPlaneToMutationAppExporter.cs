using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;
using static System.FormattableString; // Use the Invariant method to output culture-invariant numbers

namespace SelfInjectiveQuiversWithPotential.Data
{
    /// <summary>
    /// This class is used to export a quiver in plane as a file in the format of the
    /// &quot;Mutation App&quot;.
    /// </summary>
    public class QuiverInPlaneToMutationAppExporter
    {
        /// <summary>
        /// Exports the specified quiver to the file with the specified path.
        /// </summary>
        /// <param name="path">The path of the file to which to export the quiver.</param>
        /// <param name="quiverInPlane">The quiver to export.</param>
        /// <remarks>
        /// <para>The labels of the vertices are not stored explicitly in the Mutation App file
        /// format, so the labels are in general not preserved.</para>
        /// </remarks>
        /// <exception cref="ExporterException">The file was not exported successfully.</exception>
        public void ExportQuiverInPlane(string path, QuiverInPlane<int> quiverInPlane)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            if (quiverInPlane is null) throw new ArgumentNullException(nameof(quiverInPlane));

            string data = GetMutationAppStringForQuiverInPlane(quiverInPlane);

            string errorMessage = "Failed to export quiver to file.";
            try
            {
                File.WriteAllText(path, data);
            }
            catch (ArgumentNullException ex) { throw new ExporterException(errorMessage, ex); }
            catch (ArgumentException ex) { throw new ExporterException(errorMessage, ex); }
            catch (PathTooLongException ex) { throw new ExporterException(errorMessage, ex); }
            catch (DirectoryNotFoundException ex) { throw new ExporterException(errorMessage, ex); }
            catch (IOException ex) { throw new ExporterException(errorMessage, ex); }
            catch (UnauthorizedAccessException ex) { throw new ExporterException(errorMessage, ex); }
            catch (NotSupportedException ex) { throw new ExporterException(errorMessage, ex); }
            catch (System.Security.SecurityException ex) { throw new ExporterException(errorMessage, ex); }
        }

        /// <summary>
        /// Gets the string contents of a Mutation App file corresponding to the specified quiver
        /// in plane.
        /// </summary>
        /// <param name="quiverInPlane"></param>
        /// <returns></returns>
        private string GetMutationAppStringForQuiverInPlane(QuiverInPlane<int> quiverInPlane)
        {
            var builder = new StringBuilder();
            var vertices = quiverInPlane.Vertices.Sorted().ToList();

            builder.AppendLine("//Number of points");
            builder.AppendLine($"{vertices.Count}");

            builder.AppendLine("//Vertex radius");
            const int VertexRadius = 9;
            builder.AppendLine($"{VertexRadius}");

            builder.AppendLine("//Labels shown");
            const bool LabelsShown = true;
            builder.AppendLine($"{(LabelsShown ? 1 : 0)}");

            builder.AppendLine("//Matrix");
            builder.AppendLine($"{vertices.Count} {vertices.Count}");
            foreach (var sourceVertex in vertices.Sorted())
            {
                var adjacencyValues = vertices.Sorted().Select(targetVertex => GetAdjacencyValue(sourceVertex, targetVertex));
                var adjacencyValuesString = String.Join(" ", adjacencyValues);
                builder.AppendLine(adjacencyValuesString);
            }

            int GetAdjacencyValue(int source, int target)
            {
                if (quiverInPlane.AdjacencyLists[source].Contains(target)) return 1;
                else if (quiverInPlane.AdjacencyLists[target].Contains(source)) return -1;
                else return 0;
            }

            builder.AppendLine("//Growth factor");
            const double GrowthFactor = 0.2;
            builder.AppendLine(Invariant($"{GrowthFactor}"));

            builder.AppendLine("//Arrow label size");
            const double ArrowLabelSize = 12.0;
            builder.AppendLine(Invariant($"{ArrowLabelSize:F1}"));

            builder.AppendLine("//Traffic lights");
            const bool TrafficLights = false;
            builder.AppendLine($"{(TrafficLights ? 1 : 0)}");

            builder.AppendLine("//Points");
            foreach (var vertex in vertices.Sorted())
            {
                const int ThisVertexRadius = 9;
                double x = quiverInPlane.GetVertexPosition(vertex).X;
                double y = quiverInPlane.GetVertexPosition(vertex).Y;
                const bool Frozen = false;

                builder.AppendLine(Invariant($"{ThisVertexRadius} {x:F1} {y:F1} {(Frozen ? 1 : 0)}"));
            }

            builder.AppendLine("//Historycounter");
            const int HistoryCounter = -1;
            builder.AppendLine($"{HistoryCounter}");

            builder.AppendLine("//History");
            var history = new int[] { };
            var historyString = String.Join(" ", history);
            builder.AppendLine(historyString);

            builder.AppendLine("//Cluster is null");

            return builder.ToString();
        }
    }
}
