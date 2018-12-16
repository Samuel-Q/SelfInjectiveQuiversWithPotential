using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Recipes;

namespace SelfInjectiveQuiversWithPotentialTests
{
    public class TestUtility
    {
        #region Triforce
        public static Quiver<int> GetTriforceQuiver()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5, 6 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(1, 3),
                new Arrow<int>(3, 2),
                new Arrow<int>(2, 1),
                new Arrow<int>(2, 5),
                new Arrow<int>(5, 4),
                new Arrow<int>(4, 2),
                new Arrow<int>(3, 6),
                new Arrow<int>(6, 5),
                new Arrow<int>(5, 3),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetTriforcePotential()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Path<int>(1, new Arrow<int>[] { new Arrow<int>(1, 3), new Arrow<int>(3, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(2, new Arrow<int>[] { new Arrow<int>(2, 5), new Arrow<int>(5, 4), new Arrow<int>(4, 2) })),
                new DetachedCycle<int>(new Path<int>(3, new Arrow<int>[] { new Arrow<int>(3, 6), new Arrow<int>(6, 5), new Arrow<int>(5, 3) })),
                new DetachedCycle<int>(new Path<int>(2, new Arrow<int>[] { new Arrow<int>(2, 5), new Arrow<int>(5, 3), new Arrow<int>(3, 2) })),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], 1 },
                { cycles[1], 1 },
                { cycles[2], 1 },
                { cycles[3], -1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetTriforceQP()
        {
            var quiver = GetTriforceQuiver();
            var potential = GetTriforcePotential();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Tetraforce
        public static Quiver<int> GetTetraforceQuiver()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(1, 2),
                new Arrow<int>(2, 5),
                new Arrow<int>(5, 4),
                new Arrow<int>(4, 1),
                new Arrow<int>(5, 6),
                new Arrow<int>(6, 3),
                new Arrow<int>(3, 2),
                new Arrow<int>(6, 9),
                new Arrow<int>(9, 8),
                new Arrow<int>(8, 5),
                new Arrow<int>(4, 7),
                new Arrow<int>(7, 8),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetTetraforcePotential()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Path<int>(1, new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 5), new Arrow<int>(5, 4), new Arrow<int>(4, 1) })),
                new DetachedCycle<int>(new Path<int>(2, new Arrow<int>[] { new Arrow<int>(2, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 3), new Arrow<int>(3, 2) })),
                new DetachedCycle<int>(new Path<int>(5, new Arrow<int>[] { new Arrow<int>(5, 6), new Arrow<int>(6, 9), new Arrow<int>(9, 8), new Arrow<int>(8, 5) })),
                new DetachedCycle<int>(new Path<int>(4, new Arrow<int>[] { new Arrow<int>(4, 7), new Arrow<int>(7, 8), new Arrow<int>(8, 5), new Arrow<int>(5, 4) })),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], 1 },
                { cycles[1], -1 },
                { cycles[2], 1 },
                { cycles[3], -1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetTetraforceQP()
        {
            var quiver = GetTetraforceQuiver();
            var potential = GetTetraforcePotential();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Cycle
        public static Quiver<int> GetCycleQuiver(int n)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n));

            var vertices = Enumerable.Range(1, n);
            var arrows = Enumerable.Range(1, n - 1).Select(k => new Arrow<int>(k, k + 1)).AppendElement(new Arrow<int>(n, 1));
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetCyclePotential(int n)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n));

            var arrows = Enumerable.Range(1, n-1).Select(k => new Arrow<int>(k, k+1)).AppendElement(new Arrow<int>(n, 1));
            var cycle = new DetachedCycle<int>(new Path<int>(1, arrows));
            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycle, 1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetCycleQP(int n)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n));

            var quiver = GetCycleQuiver(n);
            var potential = GetCyclePotential(n);
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region CobMinus5
        public static Quiver<int> GetCobMinus5Quiver()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(1, 2),
                new Arrow<int>(2, 3),
                new Arrow<int>(3, 4),
                new Arrow<int>(4, 5),
                new Arrow<int>(5, 1),

                new Arrow<int>(2, 7),
                new Arrow<int>(7, 6),
                new Arrow<int>(6, 1),

                new Arrow<int>(3, 9),
                new Arrow<int>(9, 8),
                new Arrow<int>(8, 2),

                new Arrow<int>(4, 11),
                new Arrow<int>(11, 10),
                new Arrow<int>(10, 3),

                new Arrow<int>(5, 13),
                new Arrow<int>(13, 12),
                new Arrow<int>(12, 4),

                new Arrow<int>(1, 15),
                new Arrow<int>(15, 14),
                new Arrow<int>(14, 5),

                new Arrow<int>(7, 8),
                new Arrow<int>(9, 10),
                new Arrow<int>(11, 12),
                new Arrow<int>(13, 14),
                new Arrow<int>(15, 6),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetCobMinus5Potential()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 5), new Arrow<int>(5, 1) }),

                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 7), new Arrow<int>(7, 6), new Arrow<int>(6, 1) }),
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(2, 3), new Arrow<int>(3, 9), new Arrow<int>(9, 8), new Arrow<int>(8, 2) }),
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(3, 4), new Arrow<int>(4, 11), new Arrow<int>(11, 10), new Arrow<int>(10, 3) }),
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(4, 5), new Arrow<int>(5, 13), new Arrow<int>(13, 12), new Arrow<int>(12, 4) }),
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(1, 15), new Arrow<int>(15, 14), new Arrow<int>(14, 5), new Arrow<int>(5, 1) }),

                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(6, 1), new Arrow<int>(1, 15), new Arrow<int>(15, 6) }),
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(8, 2), new Arrow<int>(2, 7), new Arrow<int>(7, 8) }),
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(10, 3), new Arrow<int>(3, 9), new Arrow<int>(9, 10) }),
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(12, 4), new Arrow<int>(4, 11), new Arrow<int>(11, 12) }),
                new DetachedCycle<int>(new Arrow<int>[] { new Arrow<int>(14, 5), new Arrow<int>(5, 13), new Arrow<int>(13, 14) }),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], 1 },

                { cycles[1], -1 },
                { cycles[2], -1 },
                { cycles[3], -1 },
                { cycles[4], -1 },
                { cycles[5], -1 },


                { cycles[6], 1 },
                { cycles[7], 1 },
                { cycles[8], 1 },
                { cycles[9], 1 },
                { cycles[10], 1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetCobMinus5QP()
        {
            var quiver = GetCobMinus5Quiver();
            var potential = GetCobMinus5Potential();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region CobMinus7
        public static QuiverWithPotential<int> GetCobMinus7QP()
        {
            var instructions = new IPotentialRecipeInstruction[]
            {
                new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(0, 1, 4), 7),
                new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(2, 2, 3), 7),
                new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(0, 1, 4), 7),
                new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(2, 3, 4), 7),
            };
            var recipe = new PotentialRecipe(instructions);
            var executor = new RecipeExecutor();
            return executor.ExecuteRecipe(recipe, 7);
        }
        #endregion

        #region Unnamed QP 2
        public static Quiver<int> GetUnnamedQuiver2()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(1, 4),
                new Arrow<int>(4, 3),
                new Arrow<int>(3, 2),
                new Arrow<int>(2, 1),
                new Arrow<int>(1, 5),
                new Arrow<int>(5, 2),
                new Arrow<int>(2, 6),
                new Arrow<int>(6, 3),
                new Arrow<int>(3, 7),
                new Arrow<int>(7, 4),
                new Arrow<int>(4, 8),
                new Arrow<int>(8, 1),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetUnnamedPotential2()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Path<int>(1, new Arrow<int>[] { new Arrow<int>(1, 4), new Arrow<int>(4, 3), new Arrow<int>(3, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(1, new Arrow<int>[] { new Arrow<int>(1, 5), new Arrow<int>(5, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(2, new Arrow<int>[] { new Arrow<int>(2, 6), new Arrow<int>(6, 3), new Arrow<int>(3, 2) })),
                new DetachedCycle<int>(new Path<int>(3, new Arrow<int>[] { new Arrow<int>(3, 7), new Arrow<int>(7, 4), new Arrow<int>(4, 3) })),
                new DetachedCycle<int>(new Path<int>(4, new Arrow<int>[] { new Arrow<int>(4, 8), new Arrow<int>(8, 1), new Arrow<int>(1, 4) })),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], -1 },
                { cycles[1], 1 },
                { cycles[2], 1 },
                { cycles[3], 1 },
                { cycles[4], 1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetUnnamedQP2()
        {
            var quiver = GetUnnamedQuiver2();
            var potential = GetUnnamedPotential2();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Unnamed QP 3
        public static Quiver<int> GetUnnamedQuiver3()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(1, 4),
                new Arrow<int>(4, 3),
                new Arrow<int>(3, 2),
                new Arrow<int>(2, 1),
                new Arrow<int>(1, 5),
                new Arrow<int>(5, 6),
                new Arrow<int>(6, 2),
                new Arrow<int>(2, 7),
                new Arrow<int>(7, 8),
                new Arrow<int>(8, 3),
                new Arrow<int>(3, 9),
                new Arrow<int>(9, 10),
                new Arrow<int>(10, 4),
                new Arrow<int>(4, 11),
                new Arrow<int>(11, 12),
                new Arrow<int>(12, 1),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetUnnamedPotential3()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(1, 4), new Arrow<int>(4, 3), new Arrow<int>(3, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(1, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(2, 7), new Arrow<int>(7, 8), new Arrow<int>(8, 3), new Arrow<int>(3, 2) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(3, 9), new Arrow<int>(9, 10), new Arrow<int>(10, 4), new Arrow<int>(4, 3) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(4, 11), new Arrow<int>(11, 12), new Arrow<int>(12, 1), new Arrow<int>(1, 4) })),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], -1 },
                { cycles[1], 1 },
                { cycles[2], 1 },
                { cycles[3], 1 },
                { cycles[4], 1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetUnnamedQP3()
        {
            var quiver = GetUnnamedQuiver3();
            var potential = GetUnnamedPotential3();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Unnamed QP 4
        public static Quiver<int> GetUnnamedQuiver4()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(1, 4),
                new Arrow<int>(4, 3),
                new Arrow<int>(3, 2),
                new Arrow<int>(2, 1),
                new Arrow<int>(1, 5),
                new Arrow<int>(5, 6),
                new Arrow<int>(6, 2),
                new Arrow<int>(2, 7),
                new Arrow<int>(7, 8),
                new Arrow<int>(8, 3),
                new Arrow<int>(3, 9),
                new Arrow<int>(9, 10),
                new Arrow<int>(10, 4),
                new Arrow<int>(4, 11),
                new Arrow<int>(11, 12),
                new Arrow<int>(12, 1),
                new Arrow<int>(5, 12),
                new Arrow<int>(7, 6),
                new Arrow<int>(9, 8),
                new Arrow<int>(11, 10),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetUnnamedPotential4()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(1, 4), new Arrow<int>(4, 3), new Arrow<int>(3, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(1, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(2, 7), new Arrow<int>(7, 8), new Arrow<int>(8, 3), new Arrow<int>(3, 2) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(3, 9), new Arrow<int>(9, 10), new Arrow<int>(10, 4), new Arrow<int>(4, 3) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(4, 11), new Arrow<int>(11, 12), new Arrow<int>(12, 1), new Arrow<int>(1, 4) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(1, 5), new Arrow<int>(5, 12), new Arrow<int>(12, 1) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(2, 7), new Arrow<int>(7, 6), new Arrow<int>(6, 2) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(3, 9), new Arrow<int>(9, 8), new Arrow<int>(8, 3) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(4, 11), new Arrow<int>(11, 10), new Arrow<int>(10, 4) })),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], -1 },
                { cycles[1], 1 },
                { cycles[2], 1 },
                { cycles[3], 1 },
                { cycles[4], 1 },
                { cycles[5], -1 },
                { cycles[6], -1 },
                { cycles[7], -1 },
                { cycles[8], -1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetUnnamedQP4()
        {
            var quiver = GetUnnamedQuiver4();
            var potential = GetUnnamedPotential4();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Unnamed QP 6
        // Brute-forcer found this to be self-injective
        public static Quiver<int> GetUnnamedQuiver6()
        {
            var vertices = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(0,1),
                new Arrow<int>(1,2),
                new Arrow<int>(2,3),
                new Arrow<int>(3,4),
                new Arrow<int>(4,0),
                new Arrow<int>(1,5),
                new Arrow<int>(5,6),
                new Arrow<int>(6,2),
                new Arrow<int>(2,0),
                new Arrow<int>(0,2),
                new Arrow<int>(2,1),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetUnnamedPotential6()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 0) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 1), new Arrow<int>(1, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 2), new Arrow<int>(2, 0) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 2), new Arrow<int>(2, 0)})),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 0) })),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], 1 },
                { cycles[1], -1 },
                { cycles[2], 1 },
                { cycles[3], -1 },
                { cycles[4], -1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetUnnamedQP6()
        {
            var quiver = GetUnnamedQuiver6();
            var potential = GetUnnamedPotential6();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Unnamed QP 7
        // Brute-forcer found this to be self-injective
        public static Quiver<int> GetUnnamedQuiver7()
        {
            var vertices = new int[] { 0, 1, 2, 3, 4, 5 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(0,1),
                new Arrow<int>(1,2),
                new Arrow<int>(2,3),
                new Arrow<int>(3,4),
                new Arrow<int>(4,0),
                new Arrow<int>(0,2),
                new Arrow<int>(2,0),
                new Arrow<int>(4,3),
                new Arrow<int>(3,5),
                new Arrow<int>(5,4),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetUnnamedPotential7()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 0) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 5), new Arrow<int>(5, 4), new Arrow<int>(4, 0) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 2), new Arrow<int>(2, 0)})),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(3, 4), new Arrow<int>(4, 3) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 0) })),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], 1 },
                { cycles[1], -1 },
                { cycles[2], 1 },
                { cycles[3], -1 },
                { cycles[4], -1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetUnnamedQP7()
        {
            var quiver = GetUnnamedQuiver7();
            var potential = GetUnnamedPotential7();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Unnamed QP 8
        // Brute-forcer found this to be self-injective way back
        public static Quiver<int> GetUnnamedQuiver8()
        {
            var vertices = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(0,1),
                new Arrow<int>(1,2),
                new Arrow<int>(2,3),
                new Arrow<int>(3,4),
                new Arrow<int>(4,0),
                new Arrow<int>(1,5),
                new Arrow<int>(5,6),
                new Arrow<int>(6,2),
                new Arrow<int>(2,1),
            };
            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetUnnamedPotential8()
        {
            var cycles = new List<DetachedCycle<int>>
            {
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 0) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(1, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 2), new Arrow<int>(2, 1) })),
                new DetachedCycle<int>(new Path<int>(new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 1)})),
            };

            var cycleDict = new Dictionary<DetachedCycle<int>, int>()
            {
                { cycles[0], 1 },
                { cycles[1], 1 },
                { cycles[2], -1 },
            };

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetUnnamedQP8()
        {
            var quiver = GetUnnamedQuiver8();
            var potential = GetUnnamedPotential8();
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Unnamed QP family 1
        // The family of n-gons with glued on triangles (parameterized by n >= 2 or so)
        // Later insight: These are Martin's "double cycles"!
        public static Quiver<int> GetUnnamedFamily1Quiver(int n)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n));

            var vertices = Enumerable.Range(0, 2*n);
            var arrows = new List<Arrow<int>>();
            foreach (var k in Enumerable.Range(0, n))
            {
                arrows.Add(new Arrow<int>(k, (k - 1).Modulo(n))); // n-gon arrow (internal arrow)
                arrows.Add(new Arrow<int>(k, k + n)); // arrow from n-gon to triangle (external arrow going out)
                arrows.Add(new Arrow<int>(k + n, (k + 1) % n)); // arrow from triangle to n-gon (external arrow going in)
            }

            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetUnnamedFamily1Potential(int n)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n));

            var ngonArrows = Enumerable.Range(0, n).Select(k => n-k).Select(k => new Arrow<int>(k % n, k-1));
            var ngonCycle = new DetachedCycle<int>(new Path<int>(ngonArrows));
            var cycleDict = new Dictionary<DetachedCycle<int>, int>() { { ngonCycle, -1 } };
            foreach (var k in Enumerable.Range(0, n))
            {
                var triangleArrows = new Arrow<int>[] { new Arrow<int>(k, k + n), new Arrow<int>(k + n, (k + 1) % n), new Arrow<int>((k + 1) % n, k) };
                var triangleCycle = new DetachedCycle<int>(new Path<int>(triangleArrows));
                cycleDict[triangleCycle] = 1;
            }

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetUnnamedFamily1QP(int n)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n)); // n >= 2 surely works (not sure about n <= 1, but they are too small to matter anyway?)

            var quiver = GetUnnamedFamily1Quiver(n);
            var potential = GetUnnamedFamily1Potential(n);
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion

        #region Unnamed QP family 2
        // The family of n-gons with glued on m-gons (parameterized by n >= 2, m >= 3 or so)
        // These have been tested for n in [3, 22] and m in [4, 23] without coming up with anything
        public static Quiver<int> GetUnnamedFamily2Quiver(int n, int m)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n));
            if (m <= 2) throw new ArgumentOutOfRangeException(nameof(m)); // Not sure about smaller m; they might work

            var vertices = Enumerable.Range(0, n + n*(m-2)); // n for the n-gon and m-2 for each of its vertices
            var arrows = new List<Arrow<int>>();
            foreach (var k in Enumerable.Range(0, n))
            {
                arrows.Add(new Arrow<int>(k, (k - 1).Modulo(n))); // n-gon arrow (internal arrow)
                foreach (var j in Enumerable.Range(0, m-2))
                {
                    arrows.Add(new Arrow<int>(k + j*n, k + (j+1)*n)); // non-last m-gon arrows (note the numbering)
                }

                arrows.Add(new Arrow<int>(k + (m-2)*n, (k+1) % n)); // last m-gon arrow, back to the n-gon
            }

            return new Quiver<int>(vertices, arrows);
        }

        public static Potential<int> GetUnnamedFamily2Potential(int n, int m)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n));
            if (m <= 2) throw new ArgumentOutOfRangeException(nameof(m)); // Not sure about smaller m; they might work

            var ngonArrows = Enumerable.Range(0, n).Select(k => n - k).Select(k => new Arrow<int>(k % n, k - 1));
            var ngonCycle = new DetachedCycle<int>(new Path<int>(ngonArrows));
            var cycleDict = new Dictionary<DetachedCycle<int>, int>() { { ngonCycle, -1 } };
            foreach (var k in Enumerable.Range(0, n))
            {
                var mgonArrows = Enumerable.Range(0, m - 2).Select(j => new Arrow<int>(k + j * n, k + (j + 1) * n))
                                                        .AppendElement(new Arrow<int>(k + (m-2)*n, (k+1) % n))
                                                        .AppendElement(new Arrow<int>((k+1) % n, k));
                var mgonCycle = new DetachedCycle<int>(new Path<int>(mgonArrows));
                cycleDict[mgonCycle] = 1;
            }

            return new Potential<int>(cycleDict);
        }

        public static QuiverWithPotential<int> GetUnnamedFamily2QP(int n, int m)
        {
            if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n)); // n >= 2 surely works (not sure about n <= 1, but they are too small to matter anyway?)
            if (m <= 2) throw new ArgumentOutOfRangeException(nameof(m)); // Not sure about smaller m; they might work

            var quiver = GetUnnamedFamily2Quiver(n, m);
            var potential = GetUnnamedFamily2Potential(n, m);
            return new QuiverWithPotential<int>(quiver, potential);
        }
        #endregion
    }
}
