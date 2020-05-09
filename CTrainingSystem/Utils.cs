﻿using System;
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
        public static SortedDictionary<K, T> DeepCopySortedDictionary<K, T>(SortedDictionary<K, T> sd)
            where T : ICloneable
        {
            if (sd == null)
            { 
                return null; 
            }

            SortedDictionary<K, T> newSet = new SortedDictionary<K, T>(sd.Comparer);
            foreach (KeyValuePair<K, T> pair in sd)
            {
                T cloneT = (T)pair.Value.Clone();
                newSet.Add(pair.Key, cloneT);
            }
            return newSet;
        }

        public static void Main()
        {
            
        }
    }

}
