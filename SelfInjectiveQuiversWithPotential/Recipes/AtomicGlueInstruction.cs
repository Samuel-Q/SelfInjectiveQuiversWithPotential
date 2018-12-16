using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// This class represents an instruction to glue a single cycle onto the potential.
    /// </summary>
    /// <remarks>The arrows of the boundary that are in the resulting cycle are said to be
    /// &quot;consumed&quot; by the instruction.</remarks>
    [DebuggerDisplay("(Index, Count, CycleLength) = ({Index}, {Count}, {CycleLength})")]
    public class AtomicGlueInstruction : IPotentialRecipeInstruction
    {
        /// <summary>
        /// Gets the zero-based starting index of the range of arrows in the boundary to consume.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the number of arrows in the boundary to consume.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the length in arrows of the cycle resulting from executing this instruction (not the
        /// length of the path glued on!).
        /// </summary>
        public int CycleLength { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomicGlueInstruction"/> class.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of arrows in the boundary to consume.</param>
        /// <param name="count">The number of arrows in the boundary to consume.</param>
        /// <param name="cycleLength">The length in arrows of the resulting cycle.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1</exception>
        /// <exception cref="ArgumentException"><paramref name="cycleLength"/> is weakly less than
        /// <paramref name="count"/>.</exception>
        public AtomicGlueInstruction(int index, int count, int cycleLength)
        {
            // Require at least one arrow to be consumed (at least for now; non-unique orientation otherwise)
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
            if (cycleLength <= count) throw new ArgumentException("The resulting cycle must be longer than the path to consume.");

            Index = index;
            Count = count;
            CycleLength = cycleLength;
        }

        /// <inheritdoc/>
        public RecipeExecutorState Execute(RecipeExecutorState state)
        {
            InternalExecute(state, out var newState, true);
            return newState;
        }

        /// <inheritdoc/>
        public bool TryExecute(RecipeExecutorState stateBefore, out RecipeExecutorState stateAfter)
        {
            return InternalExecute(stateBefore, out stateAfter, false);
        }

        /// <remarks>Like <see cref="TryExecute(RecipeExecutorState, out RecipeExecutorState)"/>
        /// but with a boolean parameter for throwing.</remarks>
        private bool InternalExecute(RecipeExecutorState state, out RecipeExecutorState stateAfter, bool shouldThrow)
        {
            var oldPathTuples = state.Boundary.GetRange(Index, Count);
            var oldPathArrows = oldPathTuples.Select(x => x.Arrow);
            var oldPathOrientations = oldPathTuples.Select(x => x.Orientation);
            if (!oldPathOrientations.AllAreEqual())
            {
                if (shouldThrow) throw new PotentialRecipeExecutionException("The path in the boundary has arrows of different orientation.");

                stateAfter = null;
                return false;
            }
            var oldPathOrientation = oldPathTuples.First().Orientation;
            if (oldPathOrientation == BoundaryArrowOrientation.Left) oldPathArrows = oldPathArrows.Reverse();
            var oldPath = new Path<int>(oldPathArrows);
            
            var newPathLength = CycleLength - Count;
            var newPath = Utility.MakePath(oldPath.EndingPoint, oldPath.StartingPoint, newPathLength, state.NextVertex);
            if (newPathLength == 1)
            {
                var newArrow = newPath.Arrows.Single();
                // Bad cancellation
                if (state.PotentiallyProblematicArrows.Contains(newArrow))
                {
                    if (shouldThrow) throw new PotentialRecipeExecutionException("The arrow of the singleton new path is already present in the potential.");

                    stateAfter = null;
                    return false;
                }

                // 2-cycle (which is bad)
                if (state.PotentiallyProblematicArrows.Contains(new Arrow<int>(newArrow.Target, newArrow.Source)))
                {
                    if (shouldThrow) throw new PotentialRecipeExecutionException("The anti-parallel of the arrow of the singleton new path is present in the potential.");

                    stateAfter = null;
                    return false;
                }
            }

            var newPathOrientation = oldPathOrientation.Reverse();
            IEnumerable<Arrow<int>> newPathArrows = newPath.Arrows;
            if (newPathOrientation == BoundaryArrowOrientation.Left) newPathArrows = newPathArrows.Reverse();
            var newPathTuples = newPathArrows.Select(a => (a, newPathOrientation));

            var newCycle = new SimpleCycle<int>(oldPath.AppendPath(newPath));
            int coefficient = newPathOrientation == BoundaryArrowOrientation.Right ? +1 : -1;

            var newBoundary = new CircularList<(Arrow<int>, BoundaryArrowOrientation)>(state.Boundary);
            newBoundary.ReplaceRange(Index, Count, newPathTuples);

            var newPotentiallyProblematicArrows = new HashSet<Arrow<int>>(state.PotentiallyProblematicArrows);
            if (Count == 1) newPotentiallyProblematicArrows.Add(oldPath.Arrows.Single());

            stateAfter = new RecipeExecutorState
            {
                Potential = state.Potential.AddCycle(newCycle, coefficient),
                NextVertex = state.NextVertex + (newPath.LengthInVertices - 2),
                Boundary = newBoundary,
                PotentiallyProblematicArrows = newPotentiallyProblematicArrows
            };

            return true;
        }
    }
}
