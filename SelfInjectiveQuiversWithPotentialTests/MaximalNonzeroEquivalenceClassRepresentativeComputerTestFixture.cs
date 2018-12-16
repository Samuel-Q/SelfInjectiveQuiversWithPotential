using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential.Analysis;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class MaximalNonzeroEquivalenceClassRepresentativeComputerTestFixture : IMaximalNonzeroEquivalenceClassRepresentativeComputerBaseTestFixture
    {
        public override IMaximalNonzeroEquivalenceClassRepresentativeComputer Computer => new MaximalNonzeroEquivalenceClassRepresentativeComputer();
    }
}
