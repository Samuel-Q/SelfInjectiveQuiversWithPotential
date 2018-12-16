using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class ImmutableUndirectedGraphTestFixture
    {
        public ImmutableUndirectedGraph<TVertex> CreateGraph<TVertex>(IEnumerable<TVertex> vertices, IEnumerable<Edge<TVertex>> edges)
            where TVertex : IEquatable<TVertex>
        {
            return new ImmutableUndirectedGraph<TVertex>(vertices, edges);
        }

        // Regression test
        [Test]
        public void Edges_ReturnsOneEdgePerEdge()
        {
            var vertices = new int[] { 1, 2 };
            var edges = new Edge<int>[] { new Edge<int>(1, 2) };
            var graph = CreateGraph(vertices, edges);
            Assert.That(graph.Edges, Is.EqualTo(edges));
        }
    }
}
