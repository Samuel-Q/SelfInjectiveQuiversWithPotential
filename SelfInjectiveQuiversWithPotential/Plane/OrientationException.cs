using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    public class OrientationException : Exception
    {
        public OrientationException()
        {
        }

        public OrientationException(string message) : base(message)
        {
        }

        public OrientationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
