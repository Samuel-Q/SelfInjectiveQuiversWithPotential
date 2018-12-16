using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class QuiverInPlaneTestFixture
    {
        public QuiverInPlane<TVertex> CreateQuiverInPlane<TVertex>(
            IEnumerable<TVertex> vertices,
            IEnumerable<Arrow<TVertex>> arrows,
            IDictionary<TVertex, Point> vertexPositions)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            return new QuiverInPlane<TVertex>(vertices, arrows, vertexPositions);
        }

        [Test]
        public void IsPlane_ReturnsFalse_OnAntiParallelArrows()
        {
            var vertices = new int[] { 1, 2 };
            var arrows = new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 1) };
            var vertexPositions = new Dictionary<int, Point>
            {
                { 1, new Point(0, 0) },
                { 2, new Point(1, 0) },
            };

            var quiverInPlane = CreateQuiverInPlane(vertices, arrows, vertexPositions);
            Assert.That(quiverInPlane.IsPlane(), Is.False);
        }
    }
}
