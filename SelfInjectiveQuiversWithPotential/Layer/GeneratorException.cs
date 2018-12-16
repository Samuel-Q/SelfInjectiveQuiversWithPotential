using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// The exception that is thrown by <see cref="InteractiveLayeredQuiverGenerator"/> when
    /// something goes wrong.
    /// </summary>
    public class GeneratorException : Exception
    {
        public GeneratorException()
        {
        }

        public GeneratorException(string message) : base(message)
        {
        }

        public GeneratorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
