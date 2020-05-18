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

        // consturctor
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

        public readonly string gccPathEmpty = "Gcc path is empty!";
        public readonly string gccNotFound = "Gcc not found!";
        public readonly string noSourceFiles = "No source files!";
        // compile the source files and named the output .exe as ExerciseName
        public bool Compile(string ExerciseName, string sourceFiles, out string[] errors)
        {
            errors = new string[1];
            try
            {
                if (Compiler == null || Compiler.Length == 0)
                {
                    throw new System.Exception(gccPathEmpty);
                }
                if (!System.IO.File.Exists(CompilerPath))
                {
                    throw new System.Exception(gccNotFound);
                }
            }
            catch (System.Exception expt)
            {
                errors[0] = expt.Message;
                return false;
            }
          
            // validate source files
            if (sourceFiles == null || sourceFiles.Length <= 0)
            {
                errors[0] = noSourceFiles;
                return false;
            }

            // set arguments
            string CompileCommand = FormCompileCommand(sourceFiles, ExerciseName);

            // compile and get compiler's output messages
            errors = ExternExeRunner.Run(Compiler, CompileCommand, null);

            // check if the compilation succeed
            foreach (string e in errors)
            {
                if (e.IndexOf("error") != -1)
                {
                    return false;
                }
            }
            return true;
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