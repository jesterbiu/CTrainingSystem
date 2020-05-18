using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CTrainingSystem
{

    /// <summary>
    /// 题库和题目类的抽象基类
    /// * abstract base class for ExerciseAlbum and ExerciseSingle
    /// </summary>
    #region AbstractExercise

    [DataContract]
    [KnownType(typeof(ExerciseAlbum))]
    [KnownType(typeof(ExerciseSingle))]
    public abstract class AbstractExercise : ICloneable
    {
        #region Constructors

        // constructor
        public AbstractExercise() { IsValid = true; }
        public AbstractExercise(string name)
        {
            IsValid = true;
            Name = name;
        }

        #endregion

        #region Overridden object methods 

        // Clone interface (Overriden object.Clone())
        // implementation varies between types
        public virtual object Clone()
        {
            TestValid();
            return null; 
        }

        // Note: Testing the Equality of 2 instances should have the same result using either Equals() or GetHashCode()

        // GetHashCode (overriden object.GetHashCode())
        // Every instance (which is of types inherited AbstractExercise)
        // inside an ExerciseAlbum must have a UNIQUE name.
        // Consider two strings equal if their GetHashCode() are equal.
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }        

        // Equals (overridden object.Equals())
        // test equality using overload version Equals()
        public override bool Equals(object obj)
        {
            TestValid();
            return Equals(obj as AbstractExercise);
        }

        // Equals (overload object.Equals())
        // use the overriden GetHashCode() to test equality.        
        public bool Equals(AbstractExercise oth)
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

        #endregion

        #region Member methods

        // set IsValid as false
        // invalidate the actual object whose type inherites AbstractExercise
        protected void InvalidateThis()
        {
            IsValid = false;
        }

        // to call the correct version of DeleteThisExercise at run-time
        // based on the actual type of the object ae
        protected static void DeleteThis(AbstractExercise ae)
        {
            ae.DeleteThisExercise();
        }

        // delete interface
        // implementation varies between types
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

        // serialize the object exercise into XML file by fileName
        public static void WriteExerciseToXml(AbstractExercise exercise, string fileName)
        {
            if (exercise == null
                || fileName == null)
            {
                throw new System.ArgumentNullException();
            }

            // open the XML file in FileStream
            // overwrite the file if it exists, otherwise create a new one
            FileStream xmlFile = new FileStream(fileName, FileMode.Create);

            // create a text XML writer
            // associated it with the opened XML file stream above
            XmlDictionaryWriter xmlWriter = XmlDictionaryWriter.CreateTextWriter(xmlFile);

            // create a serializer for the AbstractExercise type
            DataContractSerializer serializer =
                new DataContractSerializer(typeof(AbstractExercise));

            // serialize the exercise object
            serializer.WriteObject(xmlWriter, exercise);

            // close streams
            xmlWriter.Close();
            xmlFile.Close();
        }

        // deserialize an object from the given XML file
        public static AbstractExercise ReadExerciseFromXml(string fileName)
        {
            if (fileName == null)
            {
                throw new System.ArgumentNullException();
            }

            // open XML file in FileStream
            FileStream xmlFile = new FileStream(fileName, FileMode.Open);

            // crete a text XML reader
            // associated it with the opened XML file above
            XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(xmlFile, new XmlDictionaryReaderQuotas());

            // create a serializer for the AbstratExercise type
            DataContractSerializer serizalizer = new DataContractSerializer(typeof(AbstractExercise));

            // Deserialize the object
            AbstractExercise exercise = (AbstractExercise)serizalizer.ReadObject(reader, true);

            // close streams
            reader.Close();
            xmlFile.Close();

            return exercise;
        }


        #endregion

        #region Fields & properties

        // backing fields for property
        [DataMember]
        private string Name_;
        [DataMember]
        private bool IsValid_;
        
        // properties
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
        
        #endregion
    }

    #endregion

    /// <summary>
    /// 题库：内含题库和题目 
    /// a collection of ExerciseAlbums and ExerciseSingles
    /// </summary>
    #region ExerciseAlbum
    
    [DataContract]
    public class ExerciseAlbum : AbstractExercise
    {
        #region Constructors        
        
        // no default constructor thus ExerciseAlbum.Name != String.Empty

        // constructor: construct a new empty ExerciseAlbum instance 
        // only given its name
        public ExerciseAlbum(string name) :
            base(name)
        { }

        // constructor: copy constructor
        public ExerciseAlbum(ExerciseAlbum othAlbum) :
            base(othAlbum.Name)
        {
            NestedExercises =
                Utils.DeepCopySortedDictionary<string, AbstractExercise>(othAlbum.NestedExercises);
        }

        #endregion

        #region Overriden object method
        // clone this ExerciseAlbum object
        public override object Clone()
        {
            TestValid();
            return new ExerciseAlbum(this);
        }
        #endregion

        #region Member methods
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
            if (exercise == null || exercise.Name == string.Empty)
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

        #endregion

        #region Field & property
        // fields
        [DataMember]
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
        #endregion
    }
    
    #endregion

    /// <summary>
    /// 题目：内含一个题目，即ExerciseProblem
    /// holds a single ExerciseProblem
    /// </summary>  
    #region ExerciseSingle
    
    [DataContract]
    public class ExerciseSingle : AbstractExercise
    {
        #region Constructor
        // constructors

        // constructor: construct with a ExerciseProblem
        public ExerciseSingle(ExerciseProblem problem) :
            base(problem.Name)
        {
            this.Problem = (ExerciseProblem)problem.Clone();
        }
           
        // copy constuctor
        public ExerciseSingle(ExerciseSingle othSingle)
        {
            if (othSingle != this)
            {
                Name = othSingle.Name;
                this.Problem = (ExerciseProblem)othSingle.Problem.Clone();
            }
        }
        #endregion

        #region Overriden object method
        // clone
        public override object Clone()
        {
            TestValid();
            return new ExerciseSingle(this);
        }
        #endregion

        #region Member method
        // delete
        protected override void DeleteThisExercise()
        {
            InvalidateThis();
        }
        #endregion

        #region Field & property
        // fields
        [DataMember]
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
        #endregion
    }
    #endregion

}
