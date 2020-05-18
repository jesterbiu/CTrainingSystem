using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrainingSystem
{
    /// <summary>
    /// Test the input program given arguments and constraints
    /// and output results
    /// </summary>
    public class ProgramTest
    {       
        public bool Test(
            string programName, 
            string startupArgs, 
            string[] runtimeInputs,
            string[] expected)
        {
            if (programName == null)
            {
                throw new System.ArgumentNullException();
            }

            // run the program with args and get output
            // constraints
            string[] outputs 
                = ExternExeRunner.Run(programName, startupArgs, runtimeInputs);

            //

            return AssertIsEqual(expected, outputs);
        }

        // assert that expected is the same string sequence as the actual
        // return true if they are the same sequence
        private bool AssertIsEqual(string[] expected, string[] actual)
        {
            // check null args
            if (expected == null || actual == null)
            {
                throw new System.ArgumentNullException();
            }

            // compare each pair of strings in expected and actual
            if (expected.Length == actual.Length)
            {
                for (int i = 0; i < expected.Length; i++)
                {
                    if (!string.Equals(expected[i], actual[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            // they are not the same sequence 
            // if they have different lengths 
            return false;
        }

        
        private object constraint; // default constraint
        public void SetConstraint()
        {

        }
    }
}
