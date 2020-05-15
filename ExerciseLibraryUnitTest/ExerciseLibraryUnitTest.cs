using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CTrainingSystem;
using System.Collections.Generic;

namespace ExerciseLibraryUnitTest
{
    public class ExerciseLibraryUnitTestHelper
    {
        public static AbstractExercise CreateExerciseAlbum(string name)
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

        public static AbstractExercise CreateExerciseSingle(string name)
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

        public static AbstractExercise GetExerciseGivenPath(string path, ExerciseAlbum album)
        {
            if (null == path)
            {
                return null;
            }

            Queue<string> dirs = new Queue<string>();
            string[] path_split = path.Split('/');
            foreach (string p in path_split)
            {
                dirs.Enqueue(p);
            }
            AbstractExercise iAe = album;
            while (dirs.Count > 0)
            {
                string name = dirs.Dequeue();
                if (iAe is ExerciseAlbum a)
                {
                    iAe = a.GetExercise(name);
                }

                // check result
                if (iAe == null)
                {
                    return null;
                }
                else if (iAe is ExerciseSingle single
                    && dirs.Count > 0)
                {
                    return null;
                }
                else
                {
                    continue;
                }
            }

            return iAe;
        }

        public static List<string> BFSPrintAlbums(ExerciseAlbum ea)
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
                    foreach (KeyValuePair<string, AbstractExercise> pair in A.NestedExercises)
                    {
                        q.Enqueue(pair.Value);
                    }
                }
            }
            return prints;
        }

        public static bool IsSameSequence(List<string> la, List<string> lb)
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
    }

    [TestClass]
    public class ExerciseLibraryUnitTest
    {
        /// <summary>
        /// Generate a album has this structure:
        ///    Album1 - Album2 - Album4 - Problem4
        ///                    - Problem3     
        ///           - Album3 - Album5
        ///                    - Problem5
        ///           - Problem1
        ///           - Problem2           
        /// </summary>
        /// <param name="albums"></param>
        /// <param name="expected"></param>
        public void GenerateAlbums(out List<AbstractExercise> albums, out List<string> expected)
        {
           

            // create singles
            List<AbstractExercise> singles = new List<AbstractExercise>();
            string singleHeader = "Problem-";
            string num = "12345";
            for (int i = 0; i < 5; i++)
            {
                AbstractExercise single = ExerciseLibraryUnitTestHelper.CreateExerciseSingle(singleHeader + num[i]);
                singles.Add(single);
            }

            // create albums
            albums = new List<AbstractExercise>();
            string albumHeader = "Album-";
            for (int i = 0; i < 5; i++)
            {
                AbstractExercise album = ExerciseLibraryUnitTestHelper.CreateExerciseAlbum(albumHeader + num[i]);
                albums.Add(album);
            }

            /* 
             *  Act 1
             */
            ((ExerciseAlbum)albums[3]).AddExercise(singles[3]);

            ((ExerciseAlbum)albums[1]).AddExercise(albums[3]);
            ((ExerciseAlbum)albums[1]).AddExercise(singles[2]);

            ((ExerciseAlbum)albums[2]).AddExercise(albums[4]);
            ((ExerciseAlbum)albums[2]).AddExercise(singles[4]);

            ((ExerciseAlbum)albums[0]).AddExercise(albums[2 - 1]);
            ((ExerciseAlbum)albums[0]).AddExercise(albums[3 - 1]);
            ((ExerciseAlbum)albums[0]).AddExercise(singles[1 - 1]);
            ((ExerciseAlbum)albums[0]).AddExercise(singles[2 - 1]);

            // generate print list
            expected = new List<string>();
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
        }

        // test basic ExerciseAlbum and ExerciseSingle functionalities
        // NestedExercises & ExerciseProblem
        [TestMethod]
        public void TestExerciseDataMembers()
        {
            // arrange 1
            List<AbstractExercise> albums = null;
            List<string> expected = null;
            GenerateAlbums(out albums, out expected);

            // act 1
            List<string> printlist = ExerciseLibraryUnitTestHelper.BFSPrintAlbums((ExerciseAlbum)albums[0]);

            // assert 1
            bool isSameSeq = ExerciseLibraryUnitTestHelper.IsSameSequence(expected, printlist);
            Assert.IsTrue(isSameSeq);
        }

        // Test GetExercise()
        [TestMethod]
        public void TestGetExercise()
        {
            // arrange 2
            List<AbstractExercise> albums = null;
            List<string> expected = null;
            GenerateAlbums(out albums, out expected);

            // act 2
            AbstractExercise getexercise
                = ExerciseLibraryUnitTestHelper.GetExerciseGivenPath("Album-2/Album-4/Problem-4", (ExerciseAlbum)albums[0]);

            // assert 2
            Assert.IsTrue(getexercise != null
                && getexercise.Name == "Problem-4");
        }

        // Test DeleteExercise()
        [TestMethod]
        public void TestDeleteExercise()
        {
            // arrange
            List<AbstractExercise> albums = null;
            List<string> expected = null;
            GenerateAlbums(out albums, out expected);

            // act            
            ExerciseAlbum album3 = (ExerciseAlbum)albums[2];
            album3.DeleteExercise("Album-5");
            expected.Remove("Album-5");
                       
            // assert
            List<string> printlist 
                = ExerciseLibraryUnitTestHelper.BFSPrintAlbums((ExerciseAlbum)albums[0]);
            bool isSameSeq = ExerciseLibraryUnitTestHelper.IsSameSequence(expected, printlist);
            Assert.IsTrue(isSameSeq);
        }

        // Test serialization
        [TestMethod]
        public void TestSerialization()
        {
            // arrange
            List<AbstractExercise> albums = null;
            List<string> expectedPrint = null;
            GenerateAlbums(out albums, out expectedPrint);
            ExerciseAlbum originAlbum = (ExerciseAlbum)albums[0];

            // act
            string fileName = "ExerciseLibrarySerializationTest.xml";
            AbstractExercise.WriteExerciseToXml(originAlbum, fileName);
            ExerciseAlbum deserializedAlbum 
                = (ExerciseAlbum)AbstractExercise.ReadExerciseFromXml(fileName);

            // assert
            List<string> actualPrint = ExerciseLibraryUnitTestHelper.BFSPrintAlbums(deserializedAlbum);
            bool isSameSeq = ExerciseLibraryUnitTestHelper.IsSameSequence(expectedPrint, actualPrint);
            Assert.IsTrue(isSameSeq);
        }
    }
}
