using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Data
{
    /// <summary>
    /// The exception that is thrown when <see cref="QuiverInPlaneToMutationAppExporter"/> fails to
    /// export a quiver to a file.
    /// </summary>
    public class ExporterException : Exception
    {
        public ExporterException()
        {
        }

        public ExporterException(string message) : base(message)
        {
        }

        public ExporterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
