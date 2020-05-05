using System.Collections.Generic;
using System.Text;

namespace CTrainingSystem
{
    public class CompilerCaller
    {      

        // PATH of compiler        
        private string Compiler;
        public string CompilerPath
        {
            get => Compiler;
            set => Compiler = value;
        }        

        // ctor
        public CompilerCaller(string ConfigPath)
        {
            // get compiler path from CompilerPath.txt
            string Text = null;
            try
            {
                Text = System.IO.File.ReadAllText(@ConfigPath);
            }
            catch (System.Exception FileException)
            {
                System.Console.WriteLine(FileException.Message);
                CompilerPath = null;
            }
            CompilerPath = Text;
        }

        public void CallCompiler(string ExerciseName, string pSource = null)
        {
            try
            {
                if (Compiler == null || Compiler.Length == 0)
                {
                    throw new System.Exception("No compiler found!");
                }
            }
            catch (System.Exception expt)
            {
                System.Console.WriteLine(expt.Message);
                return;
            }

            // read source files' paths
            string SourceFiles = pSource;
            if (SourceFiles == null)
            {
                SourceFiles = GetSourceFiles();
            }
            // validate source files
            if (SourceFiles == null || SourceFiles.Length <= 0)
            {
                System.Console.WriteLine("No source files!");
                return;
            }

            // set arguments
            string CompileCommand = FormCompileCommand(SourceFiles, ExerciseName);

            // output messages
            string[] CompilerOutputs = ExternExeRunner.Run(Compiler, CompileCommand, null);
            foreach (string output in CompilerOutputs)
            {
                System.Console.WriteLine(output);
            }
        }

        // get user's input of paths of source files
        // ctrl + z to end entering
        private static string GetSourceFiles()
        {
            // user tips
            System.Console.WriteLine("Enter files to compile, a file per line:");

            // read input
            List<string> SourceFiles = new List<string>();
            string File;
            while ((File = System.Console.ReadLine()) != null)
            {
                File = "\"" + File + "\" ";
                SourceFiles.Add(File);
            }

            // return multiple file paths in a single string
            string SourceFilesString = string.Concat(SourceFiles);
            return SourceFilesString;
        }


        private static readonly string CompilerOuputHeader = @" -o ..\..\..\ProgramTestOutput\";
        private static readonly string Exe = ".exe";           
        private static string FormCompileCommand(string SourceFiles, string ExerciseName, string OtherOptions = null)
        {         
            if (SourceFiles == null)
            {
                return null;
            }

            // command format: 
            // [compile options] [source file 1, source file 2, ... ] [output options]
            // form compile command
            StringBuilder CompileCommand = new StringBuilder();

            // add other options if there is any
            if (OtherOptions != null)
            {
                CompileCommand.Append(OtherOptions);
            }

            // add source files
            CompileCommand.Append(SourceFiles);

            // add output options
            StringBuilder CompilerOutputOption = 
                new StringBuilder(CompilerOuputHeader + ExerciseName + Exe);
            CompileCommand.Append(CompilerOutputOption);

            return CompileCommand.ToString();
        }


    }
}