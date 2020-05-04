using System.Collections.Generic;
using System.Text;

namespace CTrainingSystem
{
    public class CompilerCaller
    {
        // PATH of compiler        
        private static string Compiler = null;
        private static string CompilerOuputHeader = "-o ";
        private static string Exe = ".exe";

        // ctor
        public CompilerCaller(string ConfigPath)
        {
            // get compiler path from CompilerPath.txt
            string[] Texts = System.IO.File.ReadAllLines(@ConfigPath);
            Compiler = Texts[0];
        }

        public void CallCompiler(string ExerciseName)
        {
            try
            {
                if (Compiler.Length == 0)
                {
                    throw new System.Exception("No compiler found!");
                }
            }
            catch (System.Exception expt)
            {
                System.Console.WriteLine(expt.Message);
                return;
            }

            // wait for user to enter files
            // ctrl + z to end entering
            System.Console.WriteLine("Enter files to compile, a file per line:");
            List<string> Files = new List<string>();
            string File;
            while ((File = System.Console.ReadLine()) != null)
            {
                File = "\"" + File + "\" ";
                Files.Add(File);
            }
            string FileString = string.Concat(Files);

            // set arguments
            string CompilerOutputOption = CompilerOuputHeader + ExerciseName + Exe;
            string CompileCommand = FileString + CompilerOutputOption;

            // output messages
            string[] CompilerOutputs = ExternExeRunner.Run(Compiler, CompileCommand, null);
            foreach (string output in CompilerOutputs)
            {
                System.Console.WriteLine(output);
            }

        }


    }
}