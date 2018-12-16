using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// Classes implementing this interface represent an undirected graph implemented using
    /// adjacency lists.
    /// </summary>
    /// <remarks>
    /// <para>It might seem a bit peculiar to require the vertex collection to be a set but not
    /// the edge collection. This was done mostly for practical reasons: All implementations of the
    /// interface will likely have the vertices as a set, and C# does not support the necessary
    /// return type covariance it seems
    /// (<see href="https://stackoverflow.com/questions/10796650/why-cant-an-interface-implementation-return-a-more-specific-type"/>).</para></remarks>
    public interface IReadOnlyUndirectedGraph<TVertex> where TVertex : IEquatable<TVertex>
    {
        /// <summary>
        /// Gets the vertices of the undirected graph.
        /// </summary>
        ISet<TVertex> Vertices { get; }

        /// <summary>
        /// Gets the edges of the undirected graph.
        /// </summary>
        IEnumerable<Edge<TVertex>> Edges { get; }

        IReadOnlyDictionary<TVertex, ISet<TVertex>> AdjacencyLists { get; }

        bool HasEdge(TVertex endpoint1, TVertex endpoint2);
    }
}
