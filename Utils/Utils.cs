using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrainingSystem
{
    public static class Utils
    {
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
                string DuplicateS = new string(s.ToCharArray());
                Duplicate.Add(DuplicateS);
            }
            return Duplicate;
        }

    }
}
