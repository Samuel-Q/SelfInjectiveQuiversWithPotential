using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents an immutable undirected graph (with loops but without
    /// multiple/parallel edges).
    /// </summary>
    public class ImmutableUndirectedGraph<TVertex> : IReadOnlyUndirectedGraph<TVertex> where TVertex : IEquatable<TVertex>
    {
        public ISet<TVertex> Vertices { get; }

        public IEnumerable<Edge<TVertex>> Edges {
            get
            {
                var edgesWithDuplicates = AdjacencyLists.SelectMany(pair => pair.Value.Select(endpoint2 => new Edge<TVertex>(pair.Key, endpoint2)));
                return new HashSet<Edge<TVertex>>(edgesWithDuplicates);
            }
        }

        public IReadOnlyDictionary<TVertex, ISet<TVertex>> AdjacencyLists { get; }

        public ImmutableUndirectedGraph(IEnumerable<TVertex> vertices, IEnumerable<Edge<TVertex>> edges)
        {
            if (vertices is null) throw new ArgumentNullException(nameof(vertices));
            if (edges is null) throw new ArgumentNullException(nameof(edges));

            Vertices = new HashSet<TVertex>(vertices);
            if (Vertices.Count != vertices.Count()) throw new ArgumentException($"The vertex collection contains duplicates.");

            AdjacencyLists = CreateAdjacencyLists(vertices, edges);
        }

        private IReadOnlyDictionary<TVertex, ISet<TVertex>> CreateAdjacencyLists(IEnumerable<TVertex> vertices, IEnumerable<Edge<TVertex>> edges)
        {
            var dict = new Dictionary<TVertex, ISet<TVertex>>();
            foreach (var vertex in vertices) dict[vertex] = new HashSet<TVertex>();

            foreach (var edge in edges)
            {
                var (vertex1, vertex2) = edge;

                if (!dict.ContainsKey(vertex1)) throw new ArgumentException($"The edge {edge} has an endpoint {vertex1} not in the collection of vertices.", nameof(edges));
                if (!dict.ContainsKey(vertex2)) throw new ArgumentException($"The edge {edge} has an endpoint {vertex2} not in the collection of vertices.", nameof(edges));

                if (dict[vertex1].Contains(vertex2)) throw new ArgumentException($"The edge {edge} appears several times in the collection of edges.", nameof(edges));

                dict[vertex1].Add(vertex2);
                dict[vertex2].Add(vertex1);
            }

            return dict;
        }

        public bool HasEdge(TVertex endpoint1, TVertex endpoint2)
        {
            return AdjacencyLists[endpoint1].Contains(endpoint2);
        }
    }
}
