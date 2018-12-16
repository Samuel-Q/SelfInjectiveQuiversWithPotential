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
    public class LayerTypeTestFixture
    {
        public LayerType CreateLayerType(params int[] values)
        {
            return new LayerType(values);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException()
        {
            Assert.That(() => new LayerType(null), Throws.ArgumentNullException);
        }

        [TestCase()]
        [TestCase(3, 3)]
        [TestCase(10, 10, 10, 10)]
        [TestCase(10, 10, 10, 10, 10, 10)]
        public void Constructor_ThrowsArgumentException_OnEvenNumberOfValues(params int[] values)
        {
            Assert.That(() => new LayerType(values), Throws.ArgumentException);
        }

        [TestCase(Int32.MinValue)]
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(2)]
        [TestCase(Int32.MinValue, 2, 3)]
        [TestCase(-10, 2, 5)]
        [TestCase(-1, 2, 5)]
        [TestCase(0, 2, 5)]
        [TestCase(2, 2, 5)]
        [TestCase(Int32.MinValue, 2, 5)]
        [TestCase(-10, 2, 5)]
        [TestCase(-1, 2, 5)]
        [TestCase(0, 2, 5)]
        [TestCase(2, 2, 5)]
        public void Constructor_ThrowsArgumentException_OnFirstLayerSizeLessThan3AndNotEqualTo1(params int[] values)
        {
            Assert.That(() => new LayerType(values), Throws.ArgumentException);
        }

        [TestCase(3, 2, Int32.MinValue)]
        [TestCase(3, 2, -10)]
        [TestCase(3, 2, -1)]
        [TestCase(3, 2, 0)]
        [TestCase(3, 2, 1)]
        [TestCase(3, 2, 2)]
        [TestCase(10, 2, Int32.MinValue)]
        [TestCase(10, 2, -10)]
        [TestCase(10, 2, -1)]
        [TestCase(10, 2, 0)]
        [TestCase(10, 2, 1)]
        [TestCase(10, 2, 2)]
        public void Constructor_ThrowsArgumentException_OnNonFirstLayerSizeLessThan3(params int[] values)
        {
            Assert.That(() => new LayerType(values), Throws.ArgumentException);
        }

        [TestCase(25, Int32.MinValue, 25)]
        [TestCase(25, -10, 25)]
        [TestCase(25, -1, 25)]
        [TestCase(25, 0, 25)]
        [TestCase(25, 15, 25, Int32.MinValue, 25)]
        [TestCase(25, 15, 25, -10, 25)]
        [TestCase(25, 15, 25, -1, 25)]
        [TestCase(25, 15, 25, 0, 25)]
        public void Constructor_ThrowsArgumentException_OnNonPositiveVerticalArrowPairCount(params int[] values)
        {
            Assert.That(() => new LayerType(values), Throws.ArgumentException);
        }

        [TestCase(new int[] { 1 }, 1)]
        [TestCase(new int[] { 3 }, 1)]
        [TestCase(new int[] { 25 }, 1)]
        [TestCase(new int[] { 3, 10, 3 }, 2)]
        [TestCase(new int[] { 1, 10, 3 }, 2)]
        [TestCase(new int[] { 1, 10, 25 }, 2)]
        [TestCase(new int[] { 3, 10, 25 }, 2)]
        [TestCase(new int[] { 25, 10, 3 }, 2)]
        [TestCase(new int[] { 3, 10, 4, 10, 5 }, 3)]
        [TestCase(new int[] { 1, 10, 3, 10, 4, 10, 5 }, 4)]
        public void Constructor_SetsNumLayersCorrectly(int[] values, int numLayers)
        {
            var layerType = CreateLayerType(values);
            Assert.That(layerType.NumLayers, Is.EqualTo(numLayers));
        }

        [TestCase(new int[] { 3 }, new int[] { 3 })]
        [TestCase(new int[] { 25 }, new int[] { 25 })]
        [TestCase(new int[] { 3, 10, 3 }, new int[] { 3, 3 })]
        [TestCase(new int[] { 3, 10, 25 }, new int[] { 3, 25 })]
        [TestCase(new int[] { 25, 10, 3 }, new int[] { 25, 3 })]
        [TestCase(new int[] { 3, 10, 4, 10, 5 }, new int[] { 3, 4, 5 })]
        public void Constructor_SetsLayerSizesCorrectly(int[] values, params int[] layerSizes)
        {
            var layerType = CreateLayerType(values);
            Assert.That(layerType.LayerSizes, Is.EqualTo(layerSizes));
        }

        [TestCase(new int[] { 3 }, new int[] { })]
        [TestCase(new int[] { 25 }, new int[] { })]
        [TestCase(new int[] { 3, 1, 3 }, new int[] { 1 })]
        [TestCase(new int[] { 3, 10, 25 }, new int[] { 10 })]
        [TestCase(new int[] { 25, 10, 3 }, new int[] { 10 })]
        [TestCase(new int[] { 3, 10, 4, 20, 5 }, new int[] { 10, 20 })]
        public void Constructor_SetsVerticalArrowPairCountsCorrectly(int[] values, params int[] verticalArrowPairCounts)
        {
            var layerType = CreateLayerType(values);
            Assert.That(layerType.VerticalArrowPairCounts, Is.EqualTo(verticalArrowPairCounts));
        }
    }
}
