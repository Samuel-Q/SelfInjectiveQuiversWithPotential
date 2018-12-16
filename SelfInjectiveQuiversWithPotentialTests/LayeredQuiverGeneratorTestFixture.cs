using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Layer;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class LayeredQuiverGeneratorTestFixture
    {
        private ExhaustiveCompositionGenerator exhaustiveGenerator;

        [OneTimeSetUp]
        public void SetUp()
        {
            exhaustiveGenerator = CreateExhaustiveGenerator();
        }

        LayeredQuiverGenerator CreateGenerator()
        {
            return new LayeredQuiverGenerator(firstVertex: 1);
        }

        LayerType CreateLayerType(params int[] parameterValues)
        {
            return new LayerType(parameterValues);
        }

        Potential<int> CreatePotential(DetachedCycle<int> cycle, int coefficient)
        {
            return new Potential<int>(cycle, coefficient);
        }

        DetachedCycle<int> CreateDetachedCycle(params int[] vertices)
        {
            return new DetachedCycle<int>(vertices);
        }

        ExhaustiveCompositionGenerator CreateExhaustiveGenerator()
        {
            return new ExhaustiveCompositionGenerator();
        }

        [Test]
        public void GenerateForFixedLayerType_LayerTypeInt32_Works_ForSingleLayer()
        {
            var generator = CreateGenerator();
            var layerType = CreateLayerType(3);
            var outputs = generator.GenerateForFixedLayerType(layerType, exhaustiveGenerator).ToList();

            Assert.That(outputs, Has.Count.EqualTo(1));
            var qp = outputs.Single().QP;

            // TODO: Maybe assert things for the quiver in plane

            var expectedVertices = new int[] { 1, 2, 3 };
            var expectedArrows = new Arrow<int>[]
            {
                new Arrow<int>(1, 2),
                new Arrow<int>(2, 3),
                new Arrow<int>(3, 1)
            };

            Assert.That(qp.Quiver.Vertices, Is.EquivalentTo(expectedVertices));
            Assert.That(qp.Quiver.Arrows, Is.EquivalentTo(expectedArrows));

            var expectedPotential = CreatePotential(CreateDetachedCycle(1, 2, 3, 1), +1);
            Assert.That(qp.Potential, Is.EqualTo(expectedPotential));
        }

        [Test]
        public void GenerateForFixedLayerType_LayerTypeInt32_Works_ForSeveralLayers()
        {
            var generator = CreateGenerator();
            var layerType = CreateLayerType(3, 1, 3);
            var qps = generator.GenerateForFixedLayerType(layerType, exhaustiveGenerator).ToList();

            Assert.That(qps, Has.Count.EqualTo(6));
        }
    }
}
