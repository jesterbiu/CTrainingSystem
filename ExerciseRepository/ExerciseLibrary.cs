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

    }

    /* 
     * abstract class for ExerciseAlbum and ExerciseSingle
     */
    public abstract class AbstractExercise : ICloneable
    {
        // ctor
        public AbstractExercise() { }
        public AbstractExercise(string name)
        {
            Name = name;
        }

        // clone
        public virtual object Clone()
        { return null; }

        // fields
        private string Name_;
        public string Name
        {
            get
            {
                return Name_;
            }
            protected set
            {
                Name_ = value;
            }
        }
    }
       

    /*  题库：内含题库和题目 
     *  a collection of ExerciseAlbums and ExerciseSingles
     */
    public class ExerciseAlbum: AbstractExercise 
    {
        // constructor
        public ExerciseAlbum(string name):
            base(name)
        { }
        public ExerciseAlbum(ExerciseAlbum othAlbum):
            base(othAlbum.Name)
        {
            NestedExercises = 
                Utils.DeepCopySortedDictionary<string, AbstractExercise>(othAlbum.NestedExercises);
        }

        // clone       
        public override object Clone()
        {
            return new ExerciseAlbum(this);
        }

        // equals and gethashcode
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }       
        public override bool Equals(object obj)
        {
            return Equals(obj as ExerciseAlbum);
        }
        public bool Equals(ExerciseAlbum oth)
        {
            if (null == oth)
            {
                return false;
            }
            else
            {
                return this.GetHashCode() == oth.GetHashCode();
            }
        }

        // get an exercise given a path
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

        // insert
        public void AddExercise(AbstractExercise exercise)
        {
            // check duplicate exercise by name            
            if (NestedExercises_.ContainsKey(exercise.Name))
            {
                string ErrorInfo = "This album already has a(n) " + exercise.Name + "!";               
                throw new System.Exception(ErrorInfo);
            }

            // insert to the backing field
            AbstractExercise newExercise = (AbstractExercise)exercise.Clone();            
            NestedExercises_.Add(newExercise.Name, newExercise);
        }

        // remove
        public void DeleteExercise(string path)
        {

        }

        // fields
        private SortedDictionary<string, AbstractExercise> NestedExercises_ 
            = new SortedDictionary<string, AbstractExercise>();
        public SortedDictionary<string, AbstractExercise> NestedExercises
        {
            get
            {
                return Utils.DeepCopySortedDictionary<string, AbstractExercise>(NestedExercises_);
            }
            private set 
            { 
                NestedExercises_ = value; 
            }
        }       
    }

    /*  题目：内含一个题目，即ExerciseProblem
     *  holds a single ExerciseProblem
     */
    public class ExerciseSingle: AbstractExercise
    {
        // ctor
        public ExerciseSingle(ExerciseProblem problem) :
            base(problem.Name)
        {
            this.Problem = (ExerciseProblem)problem.Clone();
        }
        public ExerciseSingle(ExerciseSingle othSingle)
        {
            if (othSingle != this)
            {
                Name = othSingle.Name;
                this.Problem = (ExerciseProblem)othSingle.Problem.Clone();
            }
        }

        // clone
        public override object Clone()
        {
            return new ExerciseSingle(this);
        }

        // equals and gethashcode
        public override int GetHashCode()
        {
            return Problem.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExerciseSingle);
        }

        public bool Equals(ExerciseSingle oth)
        {
            if (null == oth)
            {
                return false;
            }
            else
            {
                return this.GetHashCode() == oth.GetHashCode();
            }
        }

        // fields
        ExerciseProblem Problem_;
        public ExerciseProblem Problem
        {
            get
            {
                return (ExerciseProblem)Problem_.Clone();
            }
            private set
            {
                Problem_ = value;
            }
        }
    }
}
