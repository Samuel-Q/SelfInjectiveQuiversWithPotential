using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    public class QuiverWithPotential<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        public Quiver<TVertex> Quiver { get; private set; }

        public Potential<TVertex> Potential { get; private set; }

        public QuiverWithPotential(Potential<TVertex> potential)
        {
            if (potential == null) throw new ArgumentNullException(nameof(potential));

            var vertices = new HashSet<TVertex>(potential.Cycles.SelectMany(c => c.CanonicalPath.Vertices));
            var arrows = new HashSet<Arrow<TVertex>>(potential.Cycles.SelectMany(c => c.CanonicalPath.Arrows));
            var quiver = new Quiver<TVertex>(vertices, arrows);
            Quiver = quiver;
            Potential = potential;
        }

        public QuiverWithPotential(Quiver<TVertex> quiver, Potential<TVertex> potential)
        {
            if (quiver == null) throw new ArgumentNullException(nameof(quiver));
            if (potential == null) throw new ArgumentNullException(nameof(potential));

            foreach (var cycle in potential.Cycles)
            {
                if (!quiver.Vertices.Contains(cycle.CanonicalPath.StartingPoint))
                    throw new ArgumentException($"The starting point {cycle.CanonicalPath.StartingPoint} of the canonical path of one of the cycles in the potential is not a vertex in the quiver.");

                foreach (var arrow in cycle.CanonicalPath.Arrows.Skip(1))
                {
                    if (!quiver.Vertices.Contains(arrow.Source))
                        throw new ArgumentException(String.Format("The vertex {0} is present in one of the cycles in the potential but is not a vertex in the quiver.", arrow.Source));
                }
            }

            Quiver = quiver;
            Potential = potential;
        }

        public bool Equals(QuiverWithPotential<TVertex> otherQP)
        {
            if (otherQP is null) return false;
            return Quiver.Equals(otherQP.Quiver) && Potential.Equals(otherQP.Potential);
        }

        public override bool Equals(object obj)
        {
            if (obj is QuiverWithPotential<TVertex> otherQP) return Equals(otherQP);
            else return false;
        }
    }
}
