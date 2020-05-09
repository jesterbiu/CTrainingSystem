//serialization
namespace CTrainingSystem
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Xml.Schema;

    // contain methods to properly read exercise problems from text files
    public class ReadTextProblem
    {             
        // find the first string in Texts that equals to Identifier
        // return its index SKIPPING the idnetifier; return -1 if fail
        public static int GetIdentifierIndex(string[] Texts, string Identifier)
        {           
            int Index = -1;
            for (int i = 0; i != Texts.Length; i++)
            {
                string Lower = Texts[i].ToLower();
                if (Lower.Contains(Identifier))
                    //(Lower.Equals(Identifier))
                {
                    Index = i;
                    break;
                }
            }
            return Index + 1;
        }

        // Concatenates strings of the range [IndexBeg, IndexEnd) of Texts
        // ignores empty strings and compensates changing line with a space
        static string StringConcat(string[] Texts, int Beg, int End)
        {
            System.Text.StringBuilder strbuild = new System.Text.StringBuilder();
            while (Beg < Texts.Length && Beg < End)
            {
                if (Texts[Beg].Length > 0)
                {
                    strbuild.Append(Texts[Beg]);                                                  
                }
                // compensate the loss of \n
                strbuild.Append('\n');
                Beg++;
            }
            return strbuild.ToString();
        }

        // read name or description from a string
        // ignores the identifier ("name:", "description:", etc)
        public static string ReadText(string[] Texts, int Beg, int End)
        {                       
            if (Texts.Length <= 0)
            {
                return null;
            }              
            if (Beg == -1 || End == -1)
            {
                return null;
            }

            // assign
            string str = StringConcat(Texts, Beg, End);           
            return str;
        }

        
        // convert a string, which seperates a sequence of numerics by space, 
        // to a list of numerics whose types could be int or double.
        // return null if fail
        static List<object> StringToNumerics(string str, bool IsInteger = true)
        {
            if (str.Length <= 0)
            {
                return null;
            }
            string[] Numbers = str.Split(' ');
            List<object> Results = new List<object>();
            foreach (string s in Numbers)
            {                
                try
                {
                    object temp;
                    if (IsInteger)
                    {
                        temp = System.Convert.ToInt32(s);
                    }
                    else
                    { 
                        temp = System.Convert.ToDouble(s); 
                    }
                    Results.Add(temp);
                }
                catch (System.OverflowException)
                {
                    if (IsInteger)
                    { 
                        System.Console.WriteLine("{0} is outside the range of the Int32 type.", str); 
                    }
                    else
                    {
                        System.Console.WriteLine("{0} is outside the range of the Double type.", str);
                    }
                    return null;
                }
                catch (System.FormatException)
                {
                    System.Console.WriteLine("The {0} value '{1}' is not in a recognizable format.",
                                      str.GetType().Name, str);
                    return null;
                }
            }
            return Results;
        }
        

        // read the counts from designated position: Texts[Index]    
        public static int ReadCounts(string[] Texts, int Index)
        {
            int Counts = -1;
            List<object> List = StringToNumerics(Texts[Index], true);
            if (List.Count > 0)
            {
                Counts = System.Convert.ToInt32(List[0]);
            }
            return Counts;
        }

        // read test data from a string array in [Beg, End)
        // in which each string contains a sequence of numerics
        // return List<List<object>> if succeed; null if fail
        public static List<string> ReadTestData(string[] Texts, int Beg, int End)
        {            
            if (Texts == null || Texts.Length <= 0)
            {
                return null;
            }
            
            List<string> TestDatas = new List<string>();
            while (Beg < Texts.Length && Beg < End)
            {
                if (Texts[Beg].Length > 0)
                { 
                    TestDatas.Add(Texts[Beg]); 
                }
                Beg++;
            }// end of while

            return TestDatas;
        }

        

    }

    public class ExerciseProblem : ICloneable
    {
        static readonly string NameIdentifier = "name:";
        static readonly string DescriptionIdentifier = "description:";
        static readonly string CountsIdentifier = "counts:";
        static readonly string InputIdentifier = "input:";
        static readonly string OutputIdentifier = "output:";

        // factory construct method
        public static ExerciseProblem GetExerciseProblem(string FilePath)
        {
            // input from file
            string[] Texts = null;
            try
            { 
                Texts = System.IO.File.ReadAllLines(FilePath); 
            }
            catch (System.Exception FileException)
            {
                System.Console.WriteLine(FileException.Message);
                return null;
            }

            if (null == Texts || Texts.Length <= 0)
            {
                return null;
            }

            // locate data
            int NameIndex = ReadTextProblem.GetIdentifierIndex(Texts, NameIdentifier);
            int DescriptionIndex = ReadTextProblem.GetIdentifierIndex(Texts, DescriptionIdentifier);
            int CountsIndex = ReadTextProblem.GetIdentifierIndex(Texts, CountsIdentifier);
            int InputIndex = ReadTextProblem.GetIdentifierIndex(Texts, InputIdentifier);
            int OutputIndex = ReadTextProblem.GetIdentifierIndex(Texts, OutputIdentifier);
            if (NameIndex == -1
                || DescriptionIndex == -1
                || CountsIndex == -1
                || InputIndex == -1
                || OutputIndex == -1)
            {
                return null;
            }

            // read input from postitions
            string Name = ReadTextProblem.ReadText(Texts, NameIndex, DescriptionIndex - 1);
            string Description = ReadTextProblem.ReadText(Texts, DescriptionIndex, CountsIndex - 1);
            int Counts = ReadTextProblem.ReadCounts(Texts, CountsIndex);
            List<string> TestInputs = ReadTextProblem.ReadTestData(Texts, InputIndex, InputIndex + Counts);
            List<string> TestOutputs = ReadTextProblem.ReadTestData(Texts, OutputIndex, OutputIndex + Counts);
            if (TestInputs == null
                || TestOutputs == null
                || Counts < 1)
            {
                return null;
            }

            // build an ExerciseProblem
            ExerciseProblem problem = new ExerciseProblem(Name, Description, TestInputs, TestOutputs);

            return problem;
        }

        // clone
        public object Clone()
        {
            return new ExerciseProblem(this.Name, this.Description, 
                this.TestInputs, this.TestOutputs);
        }       

        // constructor
        public ExerciseProblem(string pName, string pDescription,
            List<string> pTestInputs, List<string> pTestOutputs)
        {
            try
            {
                Name = pName;
                Description = pDescription;

                // note: cannot deep copy source objects if they are of reference types
                TestInputs = Utils.CopyStringList(pTestInputs);//const char* str = "..."; -> string str = "...";
                TestOutputs = Utils.CopyStringList(pTestOutputs);                
            }
            catch (System.Exception expt)
            {
                System.Console.WriteLine(expt.Message);
            }
        }

        // fields
        /*
        private readonly string Name_;
        private readonly string Description_;
        private readonly List<string> TestInputs_;
        private readonly List<string> TestOutputs_;
        */

        // properties and backing fields
        private string Name_ = "";
        private string Description_ = "";
        private List<string> TestInputs_ = new List<string>();
        private List<string> TestOutputs_ = new List<string>();

        public string Name
        {
            get 
            {
                return Name_; 
            }
            private set 
            { 
                Name_ = value; 
            }
        }
        
        public string Description
        {
            get
            {
                return Description_;
            }
            private set
            {
                Description_ = value;
            }
        }
       
        public List<string> TestInputs
        {
            get
            {
                return Utils.CopyStringList(TestInputs_);
            }
            private set
            {
                TestInputs_ = value;
            }
        }
        
        public List<string> TestOutputs
        {
            get
            {
                return Utils.CopyStringList(TestOutputs_);
            }
            private set
            {
                TestOutputs_ = value;
            }
        }

    }
    
}