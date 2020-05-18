using CTrainingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInterface
{
    class Program
    {
        private static readonly string rootAlbumPath = "rootAlbum.xml";
        private static Stack<ExerciseAlbum> previousAlbums = new Stack<ExerciseAlbum>();
        private static Dictionary<string, Delegate> handlers = new Dictionary<string, Delegate>();
        static void Main(string[] args)
        {
            // generate some albums
            GenerateTestRootAlbum();

            // load album from xml file
            CTrainingSystem.CTrainingSystem.LoadRootExerciseAlbum(rootAlbumPath);

            while (true)
            {
                // "dir": print directory
                // ".\name": open a exercise single
                // "cd"
                //      "cd ..": return to the previous album
                //      "cd xx": enter an album nested in the current album

            }
        }

        // generate a root test album and serailize it
        static void GenerateTestRootAlbum()
        {
            ExerciseAlbum album = new ExerciseAlbum("Root Album");

            // add: print list reversely
            string filepath = @"..\..\..\ProgramTestInput\ExerciseTexts\PrintListReversely.txt";
            ExerciseProblem problem =
                ExerciseProblem.GetExerciseProblem(filepath);
            ExerciseSingle single = new ExerciseSingle(problem);
            album.AddExercise(single);

            // add: quicksort
            filepath = @"..\..\..\ProgramTestInput\ExerciseTexts\quicksort.txt";
            problem =
                ExerciseProblem.GetExerciseProblem(filepath);
            single = new ExerciseSingle(problem);
            album.AddExercise(single);

            // serialize the root album
            AbstractExercise.WriteExerciseToXml(album, rootAlbumPath);
        }

        // initialize handlers
        delegate AbstractExercise getExercise(ExerciseAlbum album, string name);
        static void InitializeHandlers()
        {
            
        }
    }
}
