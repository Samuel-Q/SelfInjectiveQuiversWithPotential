using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential.Layer;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class LayerQuiverSpecificationTestFixture
    {
        public static LayerType CreateLayerType(params int[] values)
        {
            return new LayerType(values);
        }

        public static Composition CreateComposition(params int[] terms)
        {
            return new Composition(terms);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException()
        {
            var validLayerType = CreateLayerType(10);
            var validCompositions = new Composition[0];
            Assert.That(() => new LayerQuiverSpecification(null, validCompositions), Throws.ArgumentNullException);
            Assert.That(() => new LayerQuiverSpecification(validLayerType, null), Throws.ArgumentNullException);
        }

        public static IEnumerable<TestCaseData> Constructor_ThrowsArgumentException_OnWrongNumberOfCompositions_TestCaseSource()
        {
            var layerType = CreateLayerType(3);
            IEnumerable<Composition> compositions = new Composition[] { CreateComposition() };
            yield return new TestCaseData(layerType, compositions);

            compositions = new Composition[] { CreateComposition(), CreateComposition() };
            yield return new TestCaseData(layerType, compositions);

            layerType = CreateLayerType(3, 2, 5);
            compositions = new Composition[] { };
            yield return new TestCaseData(layerType, compositions);

            compositions = new Composition[] { CreateComposition() };
            yield return new TestCaseData(layerType, compositions);

            compositions = Enumerable.Repeat(CreateComposition(), 3);
            yield return new TestCaseData(layerType, compositions);

            compositions = Enumerable.Repeat(CreateComposition(), 4);
            yield return new TestCaseData(layerType, compositions);
        }

        [TestCaseSource(nameof(Constructor_ThrowsArgumentException_OnWrongNumberOfCompositions_TestCaseSource))]
        public void Constructor_ThrowsArgumentException_OnWrongNumberOfCompositions(LayerType layerType, IEnumerable<Composition> compositions)
        {
            Assert.That(() => new LayerQuiverSpecification(layerType, compositions), Throws.ArgumentException);
        }

        public static IEnumerable<TestCaseData> Constructor_Works_TestCaseSource()
        {
            var layerType = CreateLayerType(3);
            IEnumerable<Composition> compositions = new Composition[0];
            yield return new TestCaseData(layerType, compositions);

            layerType = CreateLayerType(3, 2, 5);
            compositions = new Composition[] { CreateComposition(3, 1), CreateComposition(3, 2) };
            yield return new TestCaseData(layerType, compositions);

            compositions = new Composition[] { CreateComposition(1, 3), CreateComposition(4, 1) };
            yield return new TestCaseData(layerType, compositions);
        }

        [TestCaseSource(nameof(Constructor_Works_TestCaseSource))]
        public void Constructor_Works(LayerType layerType, IEnumerable<Composition> compositions)
        {
            var specification = new LayerQuiverSpecification(layerType, compositions);
            Assert.That(specification.LayerType, Is.EqualTo(layerType));
            Assert.That(specification.Compositions, Is.EqualTo(compositions));
        }
    }
}
