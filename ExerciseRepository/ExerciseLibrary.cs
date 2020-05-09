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
                return Utils.DeepCopyString(Name_);
            }
            private set
            {
                Name_ = value;
            }
        }
    }

    /* 题库比较器：返回两个AbstractExercise子类对象的顺序
    // same type:       return Compare(e1.Name, e2.Name)
    // differen types:  ExerciseAlbum preceeds ExerciseSingle
    */
    public class ExerciseComparer : IComparer<AbstractExercise>
    {
        public int Compare(AbstractExercise e1, AbstractExercise e2)
        {
            if (e1.GetType() == e2.GetType())
            {
                return e1.Name.CompareTo(e2.Name);
            }
            else
            {
                Type e1Type = e1.GetType();
                Type e2Type = e2.GetType();
                return e1Type.Name.CompareTo(e2Type.Name);
            }
        }
    }

    /*  题库：内含题库和题目 
     *  a collection of ExerciseAlbums and ExerciseSingles
     */
    public class ExerciseAlbum: AbstractExercise 
    {
        // ctor
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

        // insert
        public void AddExercise(AbstractExercise exercise)
        {
            // check duplicate exercise by name            
            if (NestedExercises.ContainsKey(exercise.Name))
            {
                string ErrorInfo = "This album already has a(n) " + exercise.Name + "!";               
                throw new System.Exception(ErrorInfo);
            }

            // insert to the backing field
            AbstractExercise newExercise = (AbstractExercise)exercise.Clone();            
            NestedExercises_.Add(newExercise.Name, newExercise);
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
        public ExerciseSingle(ExerciseSingle othSingle) :
            base(othSingle.Name)
        {
            Problem = othSingle.Problem;
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
