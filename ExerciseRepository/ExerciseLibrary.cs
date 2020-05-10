using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CTrainingSystem
{
    public class ExerciseLibrary
    {
        // get
        // insert: create a new one or import one
        //private static List<>
        /*
        private static readonly char SplitNote = '/';
        public AbstractExercise GetExercise(string path)
        {
            // validate input
            if (path == null
                || path.Length == 0)
            {
                return null;
            }
            string[] paths = path.Split(SplitNote);
            Queue<string> directories = new Queue<string>();
            foreach (string s in paths)
            {
                directories.Enqueue(s);
            }
            return Get(directories);
        }

        private AbstractExercise Get(Queue<string> directories)
        {
            string NextDirectoryName = directories.Dequeue();
            if (NestedExercises_.ContainsKey(NextDirectoryName))
            {
                AbstractExercise NextDirectory = NestedExercises_[NextDirectoryName];

                // the final exerise obtained
                if (directories.Count == 0)
                {
                    object TargetExercise = NextDirectory.Clone();
                    return (AbstractExercise)TargetExercise;
                }
                // try to obtain the exercise recursively
                else
                {
                    ExerciseAlbum album = NextDirectory as ExerciseAlbum;
                    if (album != null)
                    {
                        return album.Get(directories);
                    }
                }
            }

            return null;
        }
        */
    }
   
}
