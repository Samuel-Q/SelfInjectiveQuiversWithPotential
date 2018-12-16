using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// The exception that is thrown when a potential recipe cannot be executed.
    /// </summary>
    public class PotentialRecipeExecutionException : Exception
    {
        public PotentialRecipeExecutionException()
        {
        }

        public PotentialRecipeExecutionException(string message) : base(message)
        {
        }

        public PotentialRecipeExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
