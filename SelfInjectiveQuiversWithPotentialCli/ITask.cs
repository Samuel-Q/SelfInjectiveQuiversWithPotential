using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialCli
{
    /// <summary>
    /// Classes implementing this interface represent a &quot;task&quot; to be done from the
    /// command-line, e.g., having a user specify a QP and then analyzing the QP for
    /// self-injectivity.
    /// </summary>
    /// <remarks>
    /// <para>This interface has nothing to do with the <see cref="System.Threading.Tasks"/>
    /// namespace.</para>
    /// </remarks>
    interface ITask
    {
        /// <summary>
        /// Does the task.
        /// </summary>
        void Do();

        /// <summary>
        /// Gets a string describing the task.
        /// </summary>
        string Description { get; }
    }
}
