using CTrainingSystem;
using System.Diagnostics;
namespace SystemTest
{   
    public class CompilerCallerManualTest
    {
        static readonly string ConfigFilePath =
           @"..\..\..\ProgramTestInput\Configs\CompilerPath.txt";

        /*
        public static void Main(string[] args)
        {
            WriteConfig();
            CompilerTest1();
        }
        */

        static void WriteConfig()
        {
            // WriteConfig()
            string CompilerPath = @"C:\MinGW\bin\gcc.exe";
            System.IO.File.WriteAllText(ConfigFilePath, CompilerPath);
        }

        static void CompilerTest1()
        {          
            // arrange
            string SourceFiles =
                @"..\..\..\ProgramTestInput\ExerciseCommits\add.h"
                + " "
                + @"..\..\..\ProgramTestInput\ExerciseCommits\mul.c";

            CompilerCaller cc = new CompilerCaller(ConfigFilePath);
            cc.CallCompiler("mul", SourceFiles);

            // act
            string[] Output = 
                ExternExeRunner.Run(@"..\..\..\ProgramTestOutput\mul.exe", null, null);
            
            // assert
            foreach (string op in Output)
            {
                System.Console.WriteLine(op);
            }

            System.Console.ReadKey();
        }
    }
}