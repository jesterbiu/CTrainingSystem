using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CTrainingSystem;
using System.Collections.Generic;

namespace ExerciseLibraryUnitTest
{
    [TestClass]
    public class ExerciseLibraryUnitTest
    {
        AbstractExercise CreateExerciseAlbum(string name)
        {
            if (name == null)
            {
                return null;
            }
            else
            {
                ExerciseAlbum album = new ExerciseAlbum(name);                
                return album;
            }
        }

        AbstractExercise CreateExerciseSingle(string name)
        {
            if (name == null)
            {
                return null;
            }
            else
            {
                ExerciseProblem p = new ExerciseProblem(name, null, null, null);
                return new ExerciseSingle(p);
            }
        }

        List<string> BFSPrintAlbums(ExerciseAlbum ea)
        {
            List<string> prints = new List<string>();
            Queue<AbstractExercise> q = new Queue<AbstractExercise>();
            q.Enqueue(ea);
            while (q.Count > 0)
            {
                AbstractExercise E = q.Dequeue();
                prints.Add(E.Name);

                if (E is ExerciseAlbum A)
                {
                    foreach (AbstractExercise abe in A.NestedExercises)
                    {
                        q.Enqueue(abe);
                    }
                }       
            }
            return prints;
        }

        bool IsSameSequence(List<string> la, List<string> lb)           
        {
            if (la == null
                || lb == null
                || la.Count != lb.Count)
            {
                return false;
            }

            for (int i = 0; i < la.Count; i++)
            {
                if (la[i] != lb[i])
                {
                    return false;
                }
            }
            return true;
        }

        [TestMethod]
        public void Test1()
        {
            /* 
             *  Arrange:
             *  Album1-
             *          Album2-                               
             *                  Album4-
             *                          Problem4
             *                  Problem3
             *          Album3-
             *                  Album5 - (nothing)
             *                  Problem5
             *          Problem1
             *          Problem2
             *  
             *  
             *  
             */

            // create singles
            List<AbstractExercise> singles = new List<AbstractExercise>();
            string singleHeader = "Problem-";
            string num = "12345";
            for (int i = 0; i < 5; i++)
            {
                AbstractExercise single = CreateExerciseSingle(singleHeader + num[i]);
                singles.Add(single);
            }
            
            // create albums
            List<AbstractExercise> albums = new List<AbstractExercise>();
            string albumHeader = "Album-";            
            for (int i = 0; i < 5; i++)
            {                
                AbstractExercise album = CreateExerciseAlbum(albumHeader + num[i]);
                albums.Add(album);
            }

            /* 
             *  Act
             */
            ((ExerciseAlbum)albums[3]).AddExercise(singles[3]);

            ((ExerciseAlbum)albums[1]).AddExercise(albums[3]);
            ((ExerciseAlbum)albums[1]).AddExercise(singles[2]);

            ((ExerciseAlbum)albums[2]).AddExercise(albums[4]);
            ((ExerciseAlbum)albums[2]).AddExercise(singles[4]);

            ((ExerciseAlbum)albums[0]).AddExercise(albums[2-1]);
            ((ExerciseAlbum)albums[0]).AddExercise(albums[3-1]);
            ((ExerciseAlbum)albums[0]).AddExercise(singles[1 - 1]);
            ((ExerciseAlbum)albums[0]).AddExercise(singles[2-1]);

            List<string> printlist = BFSPrintAlbums((ExerciseAlbum)albums[0]);

            /*
             *  Assert
             */
            List<string> expected = new List<string>();
            //expected.Add("Problem-");
            //expected.Add("Album-");
            expected.Add("Album-1");
            expected.Add("Album-2");
            expected.Add("Album-3");
            expected.Add("Problem-1");
            expected.Add("Problem-2");

            expected.Add("Album-4");
            expected.Add("Problem-3");

            expected.Add("Album-5");
            expected.Add("Problem-5");

            expected.Add("Problem-4");

            Assert.IsTrue(IsSameSequence(expected, printlist));




        }
    }
}
