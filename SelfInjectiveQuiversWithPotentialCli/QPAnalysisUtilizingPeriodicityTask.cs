using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Analysis;

namespace SelfInjectiveQuiversWithPotentialCli
{
    /// <summary>
    /// This class represents the task of analyzing a QP utilizing the periodicity of the QP.
    /// </summary>
    public class QPAnalysisUtilizingPeriodicityTask : ITask
    {
        const int DefaultFirstVertex = 1;

        /// <summary>
        /// Defines the types QPs that can be analyzed in this task.
        /// </summary>
        enum QPType
        {
            OddFlower,
            EvenFlowerType1,
            EvenFlowerType2,
            PointedFlower
        }

        /// <summary>
        /// Gets a string representation of the specified value of the <see cref="QPType"/> enum.
        /// </summary>
        /// <param name="qpType">The QP type for which to get a string representation.</param>
        /// <returns>A string representation of <paramref name="qpType"/>.</returns>
        private string GetQPTypeString(QPType qpType)
        {
            switch (qpType)
            {
                case QPType.OddFlower: return "Odd flower";
                case QPType.EvenFlowerType1: return "Even flower, type 1";
                case QPType.EvenFlowerType2: return "Even flower, type 2";
                case QPType.PointedFlower: return "Pointed flower";
            }

            Debug.Fail($"Invalid QP type ({qpType}) specified.");
            return null;
        }

        /// <inheritdoc/>
        public string Description => "Analyze QP utilizing periodicity";

        /// <inheritdoc/>
        public void Do()
        {
            if (!TryGetQP(out var qp, out var periods, out var fixedPoint)) return;

            var analyzer = new QPAnalyzer();
            var settings = new QPAnalysisSettings(
                detectNonCancellativity: true,
                maxPathLength: -1,
                EarlyTerminationCondition.None);

            Console.WriteLine("Analyzing QP ...");
            var results = analyzer.AnalyzeUtilizingPeriodicityConcurrently(qp, periods, settings);

            PrintResults(results);
        }

        private void PrintResults(IQPAnalysisResults<int> results)
        {
            Console.WriteLine($"Main result: {results.MainResult}.");

            if (results.MainResult.HasFlag(QPAnalysisMainResult.SelfInjective))
            {
                Console.WriteLine("The Nakayama permutation is as follows:");
                foreach (var (sourceVertex, targetVertex) in results.NakayamaPermutation)
                {
                    Console.WriteLine($"{sourceVertex} -> {targetVertex}");
                }

                Console.WriteLine($"The order of the Nakayama permutation is {results.NakayamaPermutation.Order}.");
            }
        }

        /// <summary>
        /// Prompts the user for a QP.
        /// </summary>
        /// <param name="qp">Output parameter for the QP.</param>
        /// <param name="periods">Output parameter for the periods of the QP.</param>
        /// <param name="fixedPoint">Output parameter for the fixed-point of the QP (or
        /// <see langword="null"/> if there is none).</param>
        /// <returns><see langword="true"/> if the user specified a QP;
        /// <see langword="false"/> otherwise.</returns>
        private bool TryGetQP(
            out QuiverWithPotential<int> qp,
            out IEnumerable<IEnumerable<int>> periods,
            out int? fixedPoint)
        {
            if (!TryGetQPType(out var qpType) || !TryGetNumPeriods(qpType, out int numPeriods))
            {
                qp = null;
                periods = null;
                fixedPoint = null;
                return false;
            }

            switch (qpType)
            {
                case QPType.OddFlower:
                    qp = UsefulQPs.GetOddFlowerQP(numPeriods, DefaultFirstVertex);
                    periods = UsefulQPs.GetPeriodsOfOddFlowerQP(numPeriods, DefaultFirstVertex);
                    fixedPoint = null;
                    break;
                case QPType.EvenFlowerType1:
                    qp = UsefulQPs.GetEvenFlowerType1QP(numPeriods, DefaultFirstVertex);
                    periods = UsefulQPs.GetPeriodsOfEvenFlowerType1QP(numPeriods, DefaultFirstVertex);
                    fixedPoint = null;
                    break;
                case QPType.EvenFlowerType2:
                    qp = UsefulQPs.GetEvenFlowerType2QP(numPeriods, DefaultFirstVertex);
                    periods = UsefulQPs.GetPeriodsOfEvenFlowerType2QP(numPeriods, DefaultFirstVertex);
                    fixedPoint = null;
                    break;
                case QPType.PointedFlower:
                    qp = UsefulQPs.GetPointedFlowerQP(numPeriods, DefaultFirstVertex);
                    periods = UsefulQPs.GetPeriodsOfPointedFlowerQPWithoutFixedPoint(numPeriods, DefaultFirstVertex);
                    throw new NotImplementedException();
                    break;
                default:
                    Debug.Fail($"Invalid QP type ({qpType}) specified.");
                    qp = null;
                    periods = null;
                    fixedPoint = null;
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Prompts the user for a QP type.
        /// </summary>
        /// <param name="qpType">Output parameter for the QP type.</param>
        /// <returns><see langword="true"/> if the user specified a QP type.
        /// <see langword="false"/> if the user specified the cancel option.</returns>
        private bool TryGetQPType(out QPType qpType)
        {
            var qpTypes = Utility.GetEnumValues<QPType>().ToArray();

            while (true)
            {
                PrintQPTypes(qpTypes);
                Console.Write("QP type: ");
                string qpTypeString = Console.ReadLine();

                if (!int.TryParse(qpTypeString, out int oneBasedQPTypeIndex))
                {
                    Console.WriteLine($"Failed to parse '{qpTypeString}' as an integer.");
                    continue;
                }

                if (oneBasedQPTypeIndex < 0 || oneBasedQPTypeIndex > qpTypes.Length)
                {
                    Console.WriteLine($"{oneBasedQPTypeIndex} is not a valid QP type.");
                    continue;
                }

                if (oneBasedQPTypeIndex == 0)
                {
                    qpType = default;
                    return false;
                }

                qpType = (QPType)(oneBasedQPTypeIndex - 1);
                return true;
            }
        }

        /// <summary>
        /// Prompts the user for the number of periods.
        /// </summary>
        /// <param name="qpType">The QP type.</param>
        /// <param name="numPeriods">The number of periods for the QP.</param>
        /// <returns><see langword="true"/> if the user specified a number of periods (which is
        /// currently the only option); <see langword="false"/> otherwise.</returns>
        private bool TryGetNumPeriods(QPType qpType, out int numPeriods)
        {
            while (true)
            {
                Console.Write("Number of periods: ");
                string numPeriodsString = Console.ReadLine();

                if (!int.TryParse(numPeriodsString, out numPeriods))
                {
                    Console.WriteLine($"Failed to parse '{numPeriodsString}' as an integer.");
                    continue;
                }

                if (!NumPeriodsIsValid(qpType, numPeriods))
                {
                    PrintNumPeriodsDescription(qpType);
                    continue;
                }

                return true;
            }
        }

        /// <summary>
        /// Prints the conditions that the number of periods needs to satisfy for the specified QP
        /// type.
        /// </summary>
        /// <param name="qpType">The type of the QP.</param>
        private void PrintNumPeriodsDescription(QPType qpType)
        {
            string numPeriodsDescription;
            switch (qpType)
            {
                case QPType.OddFlower: numPeriodsDescription = UsefulQPs.OddFlowerParameterValidityDescription; break;
                case QPType.EvenFlowerType1: numPeriodsDescription = UsefulQPs.EvenFlowerType1ParameterValidityDescription; break;
                case QPType.EvenFlowerType2: numPeriodsDescription = UsefulQPs.EvenFlowerType2ParameterValidityDescription; break;
                case QPType.PointedFlower: numPeriodsDescription = UsefulQPs.PointedFlowerParameterValidityDescription; break;
                default:
                    Debug.Fail($"Invalid QP type ({qpType}) specified.");
                    numPeriodsDescription = null;
                    break;
            }

            Console.WriteLine(numPeriodsDescription);
        }

        /// <summary>
        /// Indicates whether the specified number of periods is valid for the specified QP type.
        /// </summary>
        /// <param name="qpType">The QP type.</param>
        /// <param name="numPeriods">The number of periods.</param>
        /// <returns><see langword="true"/> if <paramref name="numPeriods"/> is valid for QPs of
        /// type <paramref name="qpType"/>; <see langword="false"/> otherwise.</returns>
        private bool NumPeriodsIsValid(QPType qpType, int numPeriods)
        {
            switch (qpType)
            {
                case QPType.OddFlower: return UsefulQPs.OddFlowerParameterIsValid(numPeriods);
                case QPType.EvenFlowerType1: return UsefulQPs.EvenFlowerType1ParameterIsValid(numPeriods);
                case QPType.EvenFlowerType2: return UsefulQPs.EvenFlowerType2ParameterIsValid(numPeriods);
                case QPType.PointedFlower: return UsefulQPs.PointedFlowerParameterIsValid(numPeriods);
            }

            Debug.Fail($"Invalid QP type ({qpType}) specified.");
            return false;
        }

        /// <summary>
        /// Prints the QP types and their one-based indices, including a cancel option.
        /// </summary>
        /// <param name="qpTypes">The QP types to print.</param>
        private void PrintQPTypes(QPType[] qpTypes)
        {
            Console.WriteLine("Tasks:");

            foreach (var (qpType, index) in qpTypes.EnumerateWithIndex())
            {
                int oneBasedIndex = index + 1;
                Console.WriteLine($"{oneBasedIndex} - {GetQPTypeString(qpType)}");
            }

            string cancelDescription = "Cancel";
            Console.WriteLine($"{0} - {cancelDescription}");
        }
    }
}
