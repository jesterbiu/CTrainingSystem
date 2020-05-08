using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrainingSystem
{
    public static class Utils
    {
        // allocate a new string which contains a copy of string oth
        public static string DeepCopyString(string oth)
        {
            if (oth == null)
            {
                return null;
            }
            return new string(oth.ToCharArray());
        }

        // return a deep copy of a List<string>
        public static List<string> CopyStringList(List<string> list)
        {
            if (list == null)
            {
                return null;
            }

            List<string> Duplicate = new List<string>();
            foreach (string s in list)
            {                
                Duplicate.Add(DeepCopyString(s));
            }
            return Duplicate;
        }

        // return a deep copy of a List<T> object
        // require T implements ICloneable
        public static List<T> DeepCopyList<T>(List<T> list) 
            where T : ICloneable
        {
            if (null == list)
            {
                return null;
            }

            List<T> CopyList = new List<T>();
            foreach (T t in list)
            {
                T CopyT = (T)t.Clone();
                CopyList.Add(CopyT);
            }

            return CopyList;
        }

        // return a deep copy of a SortedSet<T> object
        // require T implements ICloneable
        public static SortedSet<T> DeepCopySortedSet<T>(SortedSet<T> set)
            where T : ICloneable
        {
            if (set == null)
            { 
                return null; 
            }

            SortedSet<T> newSet = new SortedSet<T>(set.Comparer);
            foreach (T element in set)
            {
                T newElement = (T)element.Clone();
                newSet.Add(newElement);
            }
            return newSet;
        }

        public static void Main()
        {
            
        }
    }

}
