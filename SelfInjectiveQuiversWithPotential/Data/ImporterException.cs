using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Data
{
    /// <summary>
    /// The exception that is thrown when <see cref="QuiverInPlaneFromMutationAppImporter"/> fails to
    /// import a quiver from a file.
    /// </summary>
    public class ImporterException : Exception
    {
        public ImporterException()
        {
        }

        public ImporterException(string message) : base(message)
        {
        }

        public ImporterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
