using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    using DataStructures;
    using Microsoft.Z3;

    /// <summary>
    /// A class for checking whether two quivers with potential are isomorphic.
    /// </summary>
    /// <remarks>The Z3 automatic theorem prover is used to check for isomorphisms.</remarks>
    public class QPIsomorphismChecker
    {
        // TODO: Output the isomorphism as well!
        public bool AreIsomorphic<TVertex1, TVertex2>(QuiverWithPotential<TVertex1> qp1, QuiverWithPotential<TVertex2> qp2)
            where TVertex1 : IEquatable<TVertex1>, IComparable<TVertex1>
            where TVertex2 : IEquatable<TVertex2>, IComparable<TVertex2>
        {
            if (qp1 == null) throw new ArgumentNullException(nameof(qp1));
            if (qp2 == null) throw new ArgumentNullException(nameof(qp2));

            if (qp1.Quiver.Vertices.Count != qp2.Quiver.Vertices.Count) return false;

            var positiveCycleGroups1 = GetCyclesGroupedByLength(qp1, +1);
            var negativeCycleGroups1 = GetCyclesGroupedByLength(qp1, -1);
            var positiveCycleGroups2 = GetCyclesGroupedByLength(qp2, +1);
            var negativeCycleGroups2 = GetCyclesGroupedByLength(qp2, -1);

            // Check that the number of cycles of a fixed length and sign in qp1 and qp2 are equal, for every length and sign
            var posTuples1 = positiveCycleGroups1.Select(group => (group.First().Length, group.Count()));
            var posTuples2 = positiveCycleGroups2.Select(group => (group.First().Length, group.Count()));
            if (!posTuples1.EqualUpToOrder(posTuples2)) return false;

            var negTuples1 = negativeCycleGroups1.Select(group => (group.First().Length, group.Count()));
            var negTuples2 = negativeCycleGroups2.Select(group => (group.First().Length, group.Count()));
            if (!negTuples1.EqualUpToOrder(negTuples2)) return false;

            GetVertexMaps(out var vertexMap1, out var vertexMap2, out var abstractVertices);
            var z3 = new Context();
            var goal = z3.MkGoal(models: true, unsatCores: true, proofs: false);

            string[] vertexNames = abstractVertices.Select(k => $"V{k}").ToArray();

            var vertexSort = z3.MkEnumSort("Vertex", vertexNames);
            var z3Vertices = vertexSort.Consts;
            var vertexProduct = from v1 in abstractVertices
                                from v2 in abstractVertices
                                select (v1, v2);

            // Define uninterpreted adjacency functions and provide assertions for their every value.
            // This is the only way to "define" a function in the API it seems.
            // The alternative, to muddy up the assertions on f, does not seem appetizing.
            var a1 = z3.MkFuncDecl("a1", new Sort[] { vertexSort, vertexSort }, z3.MkBoolSort());
            foreach (var (v1, v2) in vertexProduct)
            {
                var z3Vertex1 = z3Vertices[v1];
                var z3Vertex2 = z3Vertices[v2];
                bool arrowInQp1 = qp1.Quiver.AdjacencyLists[vertexMap1[v1]].Contains(vertexMap1[v2]);
                goal.Assert(z3.MkEq(a1.Apply(z3Vertex1, z3Vertex2), z3.MkBool(arrowInQp1)));
            }

            var a2 = z3.MkFuncDecl("a2", new Sort[] { vertexSort, vertexSort }, z3.MkBoolSort());
            foreach (var (v1, v2) in vertexProduct)
            {
                var z3Vertex1 = z3Vertices[v1];
                var z3Vertex2 = z3Vertices[v2];
                bool arrowInQp2 = qp2.Quiver.AdjacencyLists[vertexMap2[v1]].Contains(vertexMap2[v2]);
                goal.Assert(z3.MkEq(a2.Apply(z3Vertex1, z3Vertex2), z3.MkBool(arrowInQp2)));
            }

            var f = z3.MkFuncDecl("f", vertexSort, vertexSort);

            goal.Assert(z3.MkDistinct(vertexSort.Consts.Select(v => f.Apply(v)).ToArray()));

            foreach (var (v1,v2) in vertexProduct)
            {
                var z3Vertex1 = z3Vertices[v1];
                var z3Vertex2 = z3Vertices[v2];
                bool arrowInQp1 = qp1.Quiver.AdjacencyLists[vertexMap1[v1]].Contains(vertexMap1[v2]);
                goal.Assert(z3.MkEq(a1.Apply(z3Vertex1, z3Vertex2), a2.Apply(f.Apply(z3Vertex1), f.Apply(z3Vertex2))));
            }

            // Define uninterpreted constants for the cycle vertices
            // Store the constants in a dictionary indexed by cycle length and values a nested list indexed by cycleIndex (for cycles of that length and sign) and vertexIndex (in cycle)
            var positiveCycleLengthToCycleConsts1 = GetCycleConsts(1, +1, positiveCycleGroups1);
            var negativeCycleLengthToCycleConsts1 = GetCycleConsts(1, -1, negativeCycleGroups1);
            var positiveCycleLengthToCycleConsts2 = GetCycleConsts(2, +1, positiveCycleGroups2);
            var negativeCycleLengthToCycleConsts2 = GetCycleConsts(2, -1, negativeCycleGroups2);

            AssertThatFMapsEveryCycleToAnotherCycleOfTheSameSign(positiveCycleLengthToCycleConsts1, positiveCycleLengthToCycleConsts2);
            AssertThatFMapsEveryCycleToAnotherCycleOfTheSameSign(negativeCycleLengthToCycleConsts1, negativeCycleLengthToCycleConsts2);

            var solver = z3.MkSolver();
            var status = solver.Check();
            switch (status)
            {
                case Status.SATISFIABLE: return true;
                case Status.UNSATISFIABLE: return false;
                case Status.UNKNOWN: throw new NotImplementedException();
                default: throw new NotImplementedException();
            }

            void AssertThatFMapsEveryCycleToAnotherCycleOfTheSameSign(
                Dictionary<int, List<CircularList<Expr>>> cycleLengthToCycleConsts1,
                Dictionary<int, List<CircularList<Expr>>> cycleLengthToCycleConsts2)
            {
                foreach (var cycleLength in cycleLengthToCycleConsts1.Keys)
                {
                    var cycles1 = cycleLengthToCycleConsts1[cycleLength];
                    var cycles2 = cycleLengthToCycleConsts2[cycleLength];
                    foreach (var cycle in cycles1)
                    {
                        var expr = GetBooleanExpressionForCycleBeingMappedToOneOfManyCycles(cycle, cycles2);
                        goal.Assert(expr);
                    }
                }
            }

            BoolExpr GetBooleanExpressionForCycleBeingMappedToOneOfManyCycles(
                CircularList<Expr> cycle1,
                IEnumerable<CircularList<Expr>> cycles)
            {
                return z3.MkOr(cycles.Select(cycle2 => GetBooleanExpressionForCycleBeingMappedToCycle(cycle1, cycle2)));
            }

            BoolExpr GetBooleanExpressionForCycleBeingMappedToCycle(
                CircularList<Expr> cycle1,
                CircularList<Expr> cycle2)
            {
                return z3.MkOr(Enumerable.Range(0, cycle1.Count).Select(_ =>
                {
                    cycle2.RotateLeft(1);
                    return GetBooleanExpressionForPathBeingMappedToPath(cycle1, cycle2);
                }));
            }

            BoolExpr GetBooleanExpressionForPathBeingMappedToPath(
                IEnumerable<Expr> path1,
                IEnumerable<Expr> path2)
            {
                return z3.MkAnd(path1.Zip(path2, (v1, v2) => z3.MkEq(f.Apply(v1), v2)));
            }


            Dictionary<int, List<CircularList<Expr>>> GetCycleConsts<TVertex>(
                int qpNum,
                int sign,
                IEnumerable<IGrouping<int, DetachedCycle<TVertex>>> cycleGroups)
                where TVertex : IEquatable<TVertex>, IComparable<TVertex>
            {
                string signString = sign > 0 ? "P" : "N";

                var output = new Dictionary<int, List<CircularList<Expr>>>();
                foreach (var cycleGroup in cycleGroups)
                {
                    var cycleGroupLength = cycleGroup.First().Length;
                    output[cycleGroupLength] = new List<CircularList<Expr>>();

                    foreach (var (cycle, cycleIndex) in cycleGroup.EnumerateWithIndex())
                    {
                        output[cycleGroupLength].Add(new CircularList<Expr>());
                        foreach (var (vertex, vertexIndex) in cycle.CanonicalPath.Vertices.EnumerateWithIndex())
                        {
                            var constant = z3.MkConst($"q{qpNum}{signString}L{cycleGroupLength}C{cycleIndex}V{vertexIndex}", vertexSort);
                            output[cycleGroupLength][cycleIndex].Add(constant);
                        }
                    }
                }

                return output;
            }

            IEnumerable<IGrouping<int, DetachedCycle<TVertex>>> GetCyclesGroupedByLength<TVertex>(QuiverWithPotential<TVertex> qp, int sign)
                where TVertex : IEquatable<TVertex>, IComparable<TVertex>
            {
                // Can't deconstruct the argument as (pair, coeff), because it matches the (pair, index) overload of Where and Select
                return qp.Potential.LinearCombinationOfCycles.ElementToCoefficientDictionary.Where(pair => pair.Value == sign)
                                                                                            .Select(pair => pair.Key)
                                                                                            .GroupBy(c => c.Length);
            }

            // Would be better to do this with only one QP and call the function twice imo
            void GetVertexMaps(out Dictionary<int, TVertex1> _vertexMap1, out Dictionary<int, TVertex2> _vertexMap2, out int[] _abstractVertices)
            {
                _vertexMap1 = new Dictionary<int, TVertex1>();
                int vertexNumber = 0;
                foreach (var vertex in qp1.Quiver.Vertices)
                {
                    _vertexMap1[vertexNumber++] = vertex;
                }

                _vertexMap2 = new Dictionary<int, TVertex2>();
                vertexNumber = 0;
                foreach (var vertex in qp2.Quiver.Vertices)
                {
                    _vertexMap2[vertexNumber++] = vertex;
                }

                int _vertexCount = vertexNumber;
                _abstractVertices = Enumerable.Range(0, _vertexCount).ToArray();
            }
        }
    }
}
