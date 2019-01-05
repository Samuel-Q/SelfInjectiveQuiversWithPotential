using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Defines the types of cancellativity of a bound quiver.
    /// </summary>
    /// <remarks>
    /// <para>This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise
    /// combination of its members.</para>
    /// </remarks>
    [Flags]
    public enum CancellativityTypes
    {
        /// <summary>
        /// The empty value.
        /// </summary>
        None = 0,

        /// <summary>
        /// (Strong) cancellativity.
        /// </summary>
        Cancellativity = 0x01,

        /// <summary>
        /// Weak cancellativity.
        /// </summary>
        WeakCancellativity = 0x02
    }
}
