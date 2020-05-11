using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CTrainingSystem
{    
    /* 
     * 题库和题目类的抽象基类
     * abstract base class for ExerciseAlbum and ExerciseSingle
     */
    public abstract class AbstractExercise : ICloneable
    {
        // constructor
        public AbstractExercise() { IsValid = true; }
        public AbstractExercise(string name)
        {
            IsValid = true;
            Name = name;
        }

        // clone
        public virtual object Clone()
        {
            TestValid();
            return null; 
        }

        // set IsValid as false
        // invalidate this AbstractExercise
        protected void InvalidateThis()
        {
            IsValid = false;
        }

        // delete this AbstractExercise
        protected static void DeleteThis(AbstractExercise ae)
        {
            ae.DeleteThisExercise();
        }

        protected virtual void DeleteThisExercise()
        {
            // do nothing
        }

        // test if this AbstractExercise object is still valid to access
        // throw an exception if it is invalid
        protected void TestValid()
        {
            if (false == IsValid)
            {
                throw new System.Exception("This exercise has been deleted!");
            }
        }

        // fields
        private string Name_;
        private bool IsValid_;
        public string Name
        {
            get
            {
                TestValid();
                return Name_;
            }
            protected set
            {
                Name_ = value;
            }
        }
        public bool IsValid
        {
            get
            {
                return IsValid_;
            }
            protected set
            {
                IsValid_ = value;
            }
        }
    }


    /*  题库：内含题库和题目 
     *  a collection of ExerciseAlbums and ExerciseSingles
     */
    public class ExerciseAlbum : AbstractExercise
    {
        // constructor
        public ExerciseAlbum(string name) :
            base(name)
        { }
        public ExerciseAlbum(ExerciseAlbum othAlbum) :
            base(othAlbum.Name)
        {
            NestedExercises =
                Utils.DeepCopySortedDictionary<string, AbstractExercise>(othAlbum.NestedExercises);
        }

        // clone       
        public override object Clone()
        {
            TestValid();
            return new ExerciseAlbum(this);
        }

        // equals and gethashcode
        public override int GetHashCode()
        {
            TestValid();
            return Name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            TestValid();
            return Equals(obj as ExerciseAlbum);
        }
        public bool Equals(ExerciseAlbum oth)
        {
            TestValid();
            if (null == oth)
            {
                return false;
            }
            else
            {
                return this.GetHashCode() == oth.GetHashCode();
            }
        }

        // get an exercise in this album
        public AbstractExercise GetExercise(string name)
        {
            TestValid();
            if (name != null
                && NestedExercises_.ContainsKey(name))
            {
                return NestedExercises_[name];
            }

            return null;
        }       

        // add
        // return true if success and vice versa
        public bool AddExercise(AbstractExercise exercise)
        {
            TestValid();

            // validate input
            if (exercise == null)
            {
                return false;
            }

            // check duplicate exercise by name  
            // throw an exception if a duplicate exists 
            if (NestedExercises_.ContainsKey(exercise.Name))
            {
                string ErrorInfo = "This album already has a(n) " + exercise.Name + "!";
                throw new System.Exception(ErrorInfo);
            }

            // insert to the backing field
            //AbstractExercise newExercise = (AbstractExercise)exercise.Clone();
            NestedExercises_.Add(exercise.Name, exercise);

            return true;
        }

        // delete an exercise in this album
        public bool DeleteExercise(string name)
        {
            TestValid();

            if (name != null && NestedExercises_.ContainsKey(name))
            {
                AbstractExercise ae = NestedExercises_[name];
                AbstractExercise.DeleteThis(ae);
                NestedExercises_.Remove(name);
                return true;
            }
            
            return false;           
        }

        protected override void DeleteThisExercise()
        {
            foreach (KeyValuePair<string, AbstractExercise> pair in NestedExercises_)
            {
                AbstractExercise.DeleteThis((AbstractExercise)pair.Value);
            }

            InvalidateThis();
        }

        // fields
        protected SortedDictionary<string, AbstractExercise> NestedExercises_
            = new SortedDictionary<string, AbstractExercise>();
        public SortedDictionary<string, AbstractExercise> NestedExercises
        {
            get
            {
                TestValid();
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
    public class ExerciseSingle : AbstractExercise
    {
        // constructor
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
            TestValid();
            return new ExerciseSingle(this);
        }

        // equals and gethashcode
        public override int GetHashCode()
        {
            TestValid();
            return Problem.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TestValid();
            return Equals(obj as ExerciseSingle);
        }

        public bool Equals(ExerciseSingle oth)
        {
            TestValid();
            if (null == oth)
            {
                return false;
            }
            else
            {
                return this.GetHashCode() == oth.GetHashCode();
            }
        }

        // delete
        protected override void DeleteThisExercise()
        {
            InvalidateThis();
        }

        // fields
        ExerciseProblem Problem_;
        public ExerciseProblem Problem
        {
            get
            {
                TestValid();
                return (ExerciseProblem)Problem_.Clone();
            }
            private set
            {
                Problem_ = value;
            }
        }
    }


}
