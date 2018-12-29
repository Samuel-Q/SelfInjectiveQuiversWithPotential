using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialCli
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        /// <summary>
        /// Runs the program.
        /// </summary>
        private static void Run()
        {
            var tasks = new ITask[]
            {
                new QPAnalysisUtilizingPeriodicityTask()
            };

            while (TryGetTaskIndex(tasks, out int taskIndex))
            {
                var task = tasks[taskIndex];
                task.Do();
            }
        }

        /// <summary>
        /// Prompts the user for a task index.
        /// </summary>
        /// <param name="numTasks">The number of tasks.</param>
        /// <param name="taskIndex">Output parameter for the zero-based task index obtained from
        /// the user.</param>
        /// <returns><see langword="true"/> if the user (eventually) entered a task index.
        /// <see langword="false"/> if the user specified the exit option.</returns>
        private static bool TryGetTaskIndex(ITask[] tasks, out int taskIndex)
        {
            while (true)
            {
                PrintTasks(tasks);
                Console.Write("Task: ");
                string taskIndexString = Console.ReadLine();

                if (!int.TryParse(taskIndexString, out int oneBasedTaskIndex))
                {
                    Console.WriteLine($"Failed to parse '{taskIndexString}' as an integer.");
                    continue;
                }

                if (oneBasedTaskIndex < 0 || oneBasedTaskIndex > tasks.Length)
                {
                    Console.WriteLine($"{oneBasedTaskIndex} is not a valid task.");
                    continue;
                }

                taskIndex = oneBasedTaskIndex - 1;
                return (oneBasedTaskIndex != 0);
            }
        }

        /// <summary>
        /// Prints the tasks and their one-based indices, including an exit option.
        /// </summary>
        /// <param name="tasks">The tasks to print.</param>
        private static void PrintTasks(ITask[] tasks)
        {
            Console.WriteLine("Tasks:");

            foreach (var (task, index) in tasks.EnumerateWithIndex())
            {
                int oneBasedIndex = index + 1;
                Console.WriteLine($"{oneBasedIndex} - {task.Description}");
            }

            string exitDescription = "Exit";
            Console.WriteLine($"{0} - {exitDescription}");
        }
    }
}
