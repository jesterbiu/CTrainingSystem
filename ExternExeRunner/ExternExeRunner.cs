using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CTrainingSystem
{
    public class ExternExeRunnerMain
    {
        public static void Main()
        {

        }
    }

    public class ExternExeRunner
    {
        // run an external exe designated by filename 
        // which takes args as arguments
        // return the exe's output
        public static string[] Run(string filename, string StartupArgs, string[] RuntimeInputs)
        {
            if (filename == null
                || filename == string.Empty)
            {
                throw new System.ArgumentException("invalid file name!");
            }
            // create an external process
            // exception handle: empty filename, invalid filename?
            Process ExternExe = CreateNewProcess(filename, StartupArgs);

            // run the process using args
            RunProcess(ExternExe, RuntimeInputs);

            // get output
            string[] outputs = GetOutput(ExternExe);

            // exit the process
            ExitProcess(ExternExe);

            System.Console.WriteLine(ExternExe.ExitTime);

            // return outputs
            return outputs.ToArray();
        }

        private static Process CreateNewProcess(string filename, string StartupArgs = null)
        {
            if (filename == null)
            {
                throw new System.ArgumentNullException();
            }
            else
            {
                if (!System.IO.File.Exists(filename))
                {
                    throw new System.Exception("No such file exists!");
                }
            }

            Process NewProcess = new Process
            {
                StartInfo = StartupArgs == null ?
                    new ProcessStartInfo(filename)
                    : new ProcessStartInfo(filename, StartupArgs)
            };

            
            NewProcess.StartInfo.UseShellExecute = false;
            /*NewProcess.StartInfo.CreateNoWindow = true;
            */
            NewProcess.StartInfo.RedirectStandardInput = true;
            NewProcess.StartInfo.RedirectStandardOutput = true;

            return NewProcess;
        }

        private static void RunProcess(Process proc, string[] RuntimeArgs = null)
        {
            // run the extern process
            proc.Start();
            // input
            if (RuntimeArgs != null)
            {
                foreach (string arg in RuntimeArgs)
                {
                    proc.StandardInput.WriteLine(arg);
                }
            }
        }

        private static string[] GetOutput(Process proc)
        {
            // get output
            List<string> outputs = new List<string>();
            while (!proc.StandardOutput.EndOfStream)
            {
                outputs.Add(proc.StandardOutput.ReadLine());
            }
            return outputs.ToArray();
        }

        private static void ExitProcess(Process proc)
        {
            proc.WaitForExit();
        }
    }
}
