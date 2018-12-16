using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    /// <summary>
    /// This class is used to extract a quiver with potential from a plane quiver.
    /// </summary>
    public class QPExtractor
    {
        public QPExtractionResult TryExtractQP<TVertex>(QuiverInPlane<TVertex> quiverInPlane, out QuiverWithPotential<TVertex> qp)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (quiverInPlane is null) throw new ArgumentNullException(nameof(quiverInPlane));

            // Not entirely sure that loops are a catastrophy
            if (quiverInPlane.HasLoops())
            {
                qp = null;
                return QPExtractionResult.QuiverHasLoops;
            }

            if (quiverInPlane.HasAntiParallelArrows())
            {
                qp = null;
                return QPExtractionResult.QuiverHasAntiParallelArrows;
            }

            // Algorithm:
            // 1. Perform a "face search" on the underlying graph, the output of which is a
            // collection of faces, each described by its bounding cycle (of non-directed edges)
            // 2. For every face, try to get its orientation in the directed quiver:
            //      If the bounding cycle or its reverse is a path in the directed quiver,
            //      determine the orientation (counterclockwise or clockwise).
            //      Else, return "QuiverHasFaceWithInconsistentOrientation"
            // 3. Construct the potential with positive cycles the cycles from step (2) with 
            //    clockwise orientation and with negative cycles the cycles from step (2) with
            //    counterclockwise orientation
            // 4. Output the QP and return Success.

            var underlyingGraph = quiverInPlane.GetUnderlyingGraph();
            var faceFinder = new FaceFinder();

            // Remember that this outputs counterclockwise bounding cycles.
            if (!faceFinder.TryFindFaces(underlyingGraph, out var boundingCyclesAsVertexCollections))
            {
                qp = null;
                return QPExtractionResult.QuiverIsNotPlane;
            }

            var boundingCycles = boundingCyclesAsVertexCollections.Select(cycleAsVertexInPlaneCollection => cycleAsVertexInPlaneCollection.Select(vertexInPlane => vertexInPlane.Vertex))
                                                                  .Select(cycleAsVertexCollection => new DetachedCycle<TVertex>(cycleAsVertexCollection));

            var counterclockwiseFaceCycles = new List<DetachedCycle<TVertex>>();
            var clockwiseFaceCycles = new List<DetachedCycle<TVertex>>();
            foreach (var cycle in boundingCycles)
            {
                // Counterclockwise bounding cycle in the directed quiver (and so on)
                if (cycle.CanonicalPath.Arrows.All(arrow => quiverInPlane.ContainsArrow(arrow))) counterclockwiseFaceCycles.Add(cycle);
                else if (cycle.Reverse().CanonicalPath.Arrows.All(arrow => quiverInPlane.ContainsArrow(arrow))) clockwiseFaceCycles.Add(cycle.Reverse());
                else
                {
                    qp = null;
                    return QPExtractionResult.QuiverHasFaceWithInconsistentOrientation;
                }
            }

            var counterclockwiseDict = counterclockwiseFaceCycles.ToDictionary(cycle => cycle, cycle => -1);
            var clockwiseDict = clockwiseFaceCycles.ToDictionary(cycle => cycle, cycle => +1);
            var dict = counterclockwiseDict.Concat(clockwiseDict).ToDictionary(p => p.Key, p => p.Value);
            var potential = new Potential<TVertex>(dict);

            qp = new QuiverWithPotential<TVertex>(quiverInPlane.GetUnderlyingQuiver(), potential);
            return QPExtractionResult.Success;

            //// Algorithm:
            //// Order the arrows from a vertex cyclically by the angle, say with a dictionary of cyclic lists:
            ////     vertex -> [target1, target2, ...]
            ////
            //// Because clockwise gets positive sign, it might be easier to order things cyclically by -angle
            ////
            //// Keep track of signed arrows "consumed" (the sign corresponding to turning left or right)
            ////
            //// While there is an arrow with a non-consumed sign (a non-consumed signed arrow, say):
            ////     Do a 'left/right (depending on the non-consumed sign) face search' for the arrow
            ////
            //// How a face search is done is documented elsewhere for now.
            ////
            //// This produces a collection of positively oriented faces (represented by the bounding cycle)
            //// and a collection of negatively oriented faces (represented by the bounding cycle)
            ////
            //// Take the bounding cycles of the positively oriented faces as the cycles in the potential with positive sign
            ////  and the bounding cycles of the negatively oriented faces as the cycles in the potential with negative sign


            //var counterclockwiseFaceCycles = SearchForFaces(Orientation.Counterclockwise);
            //var clockwiseFaceCycles = SearchForFaces(Orientation.Clockwise);
            //var leftDict = counterclockwiseFaceCycles.ToDictionary(cycle => cycle, cycle => -1);
            //var rightDict = clockwiseFaceCycles.ToDictionary(cycle => cycle, cycle => +1);
            //var dict = counterclockwiseDict.Concat(clockwiseDict).ToDictionary(p => p.Key, p => p.Value);
            //var potential = new Potential<TVertex>(dict);

            //qp = new QuiverWithPotential<TVertex>(quiverInPlane.GetUnderlyingQuiver(), potential);
            //return QPExtractorResult.Success;

            //IEnumerable<DetachedCycle<TVertex>> SearchForFaces(Orientation searchOrientation)
            //{
            //    var cycles = new HashSet<DetachedCycle<TVertex>>();
            //    var remainingArrows = new HashSet<Arrow<TVertex>>(quiverInPlane.GetArrows());
            //    var remainingArrowsStack = new Stack<Arrow<TVertex>>(quiverInPlane.GetArrows());
            //    while (remainingArrows.Count > 0)
            //    {
            //        Arrow<TVertex> startArrow;
            //        while (!remainingArrows.Contains(startArrow = remainingArrowsStack.Pop())) ;

            //        var path = new Path<TVertex>(startArrow.Source, startArrow.Target);

            //        var prevVertex = startArrow.Source;
            //        var curVertex = startArrow.Target;

            //        // Start the search for this start arrow!
            //        while (true)
            //        {
            //            // Terminate the search if we have found a cycle
            //            if (path.TryExtractTrailingCycle(out var closedPath, out int startIndex))
            //            {
            //                var cycleOrientation = quiverInPlane.GetOrientationOfCycle(closedPath);
            //                // If the orientations agree, then the cycle is a bounding cycle for a (bounded) face
            //                // Else, it bounds the unbounded face (which we do not care about)
            //                if (cycleOrientation == searchOrientation)
            //                {
            //                    var detachedCycle = new DetachedCycle<TVertex>(closedPath);

            //                    // When restarting with a boundary arrow, we might get the same cycle again
            //                    if (!cycles.Contains(detachedCycle)) cycles.Add(new DetachedCycle<TVertex>(closedPath));
            //                }

            //                foreach (var arrow in path.Arrows) remainingArrows.Remove(arrow);
            //                break;
            //            }

            //            // Get next successor (next in the angle sense)
            //            // If no successor, terminate the search (all arrows are direction-boundary arrows)
            //            // Else, update path, prevVertex and curVertex and do next iteration

            //            var successors = quiverInPlane.AdjacencyLists[curVertex];
            //            if (successors.Count == 0)
            //            {
            //                foreach (var arrow in path.Arrows) remainingArrows.Remove(arrow);
            //                break;
            //            }

            //            var baseVertex = curVertex;
            //            var basePos = quiverInPlane.GetVertexPosition(baseVertex);
            //            var successorsAndPredecessor = successors.AppendElement(prevVertex);

            //            // Sort the vertices by angle so that the vertex following the predecessor vertex (prevVertex) is
            //            // the vertex corresponding to a "maximal" turn.
            //            var successorsAndPredecessorSortedByAngle = searchOrientation == Orientation.Clockwise ?
            //                successorsAndPredecessor.OrderBy(vertex => quiverInPlane.GetVertexPosition(vertex), new AngleBasedPointComparer(basePos)) :
            //                successorsAndPredecessor.OrderByDescending(vertex => quiverInPlane.GetVertexPosition(vertex), new AngleBasedPointComparer(basePos));

            //            var successorsAndPredecessorSortedByAngleList = new CircularList<TVertex>(successorsAndPredecessorSortedByAngle);
            //            int predecessorIndex = successorsAndPredecessorSortedByAngleList.IndexOf(prevVertex);
            //            successorsAndPredecessorSortedByAngleList.RotateLeft(predecessorIndex);

            //            var nextVertex = successorsAndPredecessorSortedByAngleList.Skip(1).First();

            //            path = path.AppendVertex(nextVertex);
            //            prevVertex = curVertex;
            //            curVertex = nextVertex;
            //        }
            //    }

            //    return cycles;
            //}

            //var successorsSortedByAngle = quiverInPlane.Vertices.Select(baseVertex =>
            //{
            //    var basePos = quiverInPlane.GetVertexPosition(baseVertex);

            //    var sortedSuccessors = quiverInPlane.AdjacencyLists[baseVertex].OrderBy(vertex => quiverInPlane.GetVertexPosition(vertex), new AngleBasedPointComparer(basePos));

            //    var sortedVertexList = new CircularList<TVertex>(sortedSuccessors);
            //    return new KeyValuePair<TVertex, CircularList<TVertex>>(baseVertex, sortedVertexList);
            //}).ToDictionary(p => p.Key, p => p.Value);

            //var arrows = new HashSet<Arrow<TVertex>>(quiverInPlane.GetArrows());

            ////TODO: Continue here
            //throw new NotImplementedException();






            //// Old idea
            //// Algorithm:
            //// Order the arrows to/from a vertex cyclically by the angle, say with a dictionary of cyclic lists of tuples:
            ////     vertex -> [(outgoing/incoming, target/source), ...]
            ////
            //// Because clockwise gets positive sign, it might be easier to order things cyclically by -angle
            ////
            //// Keep track of arrows "consumed" and their signs
            ////
            //// For every vertex:
            ////     Begin with the first outgoing arrow
            ////     Check if the next arrow is incoming
            ////     If not, 
            ////     Try to "turn right" (take the previous arrow) 
            ////     

            ////var neighborsSortedByAngle = quiverInPlane.Vertices.Select(baseVertex =>
            ////{
            ////    var basePos = quiverInPlane.GetVertexPosition(baseVertex);
            ////    var outgoingArrowTargetVertices = quiverInPlane.AdjacencyLists[baseVertex];
            ////    var incomingArrowSourceVertices = quiverInPlane.Vertices.Where(sourceVertex => quiverInPlane.AdjacencyLists[sourceVertex].Contains(baseVertex));
            ////    var undirectedlyAdjacentVertices = incomingArrowSourceVertices.Concat(outgoingArrowTargetVertices);
            ////    var sortedVertices = undirectedlyAdjacentVertices.OrderBy(vertex => quiverInPlane.GetVertexPosition(vertex), new AngleBasedPointComparer(basePos));
            ////    var sortedVertexList = new CircularList<TVertex>(sortedVertices);
            ////    return new KeyValuePair<TVertex, CircularList<TVertex>>(baseVertex, sortedVertexList);
            ////}).ToDictionary(p => p.Key, p => p.Value);

            ////var arrows = new HashSet<Arrow<TVertex>>(quiverInPlane.GetArrows());

            ////TODO: Continue here
            ////throw new NotImplementedException();
        }
    }
}
