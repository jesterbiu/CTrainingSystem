using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CTrainingSystem
{
    public class CTrainingSystem
    {
        #region Load, get, add, delete exercises
        // load root album
        //private static string rootAlbum = "RootAlbum.xml";
        public static ExerciseAlbum LoadRootExerciseAlbum(string rootAlbumPath)
        {
            return (ExerciseAlbum)AbstractExercise.ReadExerciseFromXml(rootAlbumPath);
        }
        
        // get and return the exercise named exerciseName
        // nested in workDirectory        
        public static AbstractExercise GetExercise(ExerciseAlbum workDirectory, string exerciseName)
        {
            // validate input arguments
            if (workDirectory == null 
                || exerciseName == null
                || exerciseName == string.Empty)
            {
                throw new System.ArgumentException();
            }

            // get exercise 
            AbstractExercise exercise = workDirectory.GetExercise(exerciseName);

            // validate the result
            if (exercise == null)
            {
                throw new System.Exception(
                    "this album does not contain such exercise album or exercise single!");
            }
            return exercise;
        }

        // get nested exercise
        public static SortedDictionary<string, AbstractExercise> GetNestedExercise(ExerciseAlbum workDirectory)
        {
            return workDirectory.NestedExercises;
        }

        // add exercise


        // delete exercise

        #endregion


        #region Compile source files, and test the output program
        // compile and test
        // input source files given by a single string and a ExerciseSingle
        // compile the source files, test the program if compilation succeed
        // return the result and error infos
        // gccPath = @"C:\MinGW\bin\gcc.exe";     
        private static readonly string configFilePath =
           @"..\..\..\ProgramTestInput\Configs\CompilerPath.txt";
        private static readonly string outputProgramsPath = @"..\..\..\ProgramTestOutput\";
        private static readonly string dotExe = ".exe";
        public static bool CompileAndTest(string sourceFiles, ExerciseSingle exercise, out string[] errors)
        {
            // validate inputs
            if (sourceFiles == null
                || sourceFiles.Length == 0
                || exercise == null)
            {
                throw new System.ArgumentException();
            }

            // compile and generate a executable file (.exe)
            bool compileSuccess = Compile(exercise.Name, sourceFiles, out errors);
            if (!compileSuccess)
            {
                return false;
            }

            // run the generated file and test it
            var testResult = RunAndTest(exercise);

            return testResult.Item1;            
        }

        // compile the source files using gcc that the user provided
        // output .exe file to designated path
        // return error
        private static bool Compile(string exerciseName, string sourceFiles, out string[] errors)
        {
            // compile
            CompilerCaller compiler = new CompilerCaller(configFilePath);
            bool compileSuccess = compiler.Compile(exerciseName, sourceFiles, out errors);

            return compileSuccess;
        }

        // run the program given exercise's test inputs
        // and return the result that consists of 
        // Item1(pass or not) and Item2 (program's output)
        private static Tuple<bool, string[]> RunAndTest(ExerciseSingle exercise)
        {
            // run the generated file with exercise test inputs
            string programPath = outputProgramsPath + exercise.Name + dotExe;
            string[] runtimeArgs = exercise.Problem.TestInputs.ToArray();
            string[] actualOutputs = ExternExeRunner.Run(programPath, null, runtimeArgs);

            // check outputs by asserting 
            // that if the exepectedOutputs is the same as actualOutputs
            string[] expectedOutputs = exercise.Problem.TestOutputs.ToArray();
            if (!Utils.IsSameStringArray(actualOutputs, expectedOutputs))
            {
                return new Tuple<bool, string[]>(false, expectedOutputs);
            }
            else
            { 
                return new Tuple<bool, string[]>(true, expectedOutputs); 
            }
        }
        #endregion

    }

}
