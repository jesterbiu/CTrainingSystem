using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTrainingSystem;

namespace SystemTest
{
    class ExerciseProblemManualTest
    {
        public static void Main(string[] args)
        {
            ExerciseProblemTest1();
        }

        static void ExerciseProblemTest1()
        {
            // arrange
            //测试前把这个地址改为实际的问题文档的地址
            string FilePath =
                @"..\..\..\ProgramTestInput\ExerciseTexts\PrintListReversely.txt";
            ExerciseProblem problem = ExerciseProblem.GetExerciseProblem(FilePath);
            PrintExerciseProblem(problem);
            System.Console.ReadKey();
        }

        static void PrintListOfList(List<string> Lists, bool NewLine = false)
        {
            foreach (string s in Lists)
            {
                System.Console.Write(s);
                if (NewLine)
                {
                    System.Console.WriteLine();
                }
            }
        }

        static void PrintExerciseProblem(ExerciseProblem problem)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Name:\n{0}", problem.Name);
            System.Console.WriteLine("Description:\n{0}", problem.Description);
            System.Console.WriteLine("Counts:\n{0}\n", problem.TestInputs.Count);
            System.Console.WriteLine("TestInput:");
            PrintListOfList(problem.TestInputs, true);
            System.Console.WriteLine("\nTestOutput:");
            PrintListOfList(problem.TestOutputs, true);

            System.Console.ReadKey();
        }

        
    }
}
