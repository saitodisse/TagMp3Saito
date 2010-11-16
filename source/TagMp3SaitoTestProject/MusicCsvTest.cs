using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagMp3Saito;

namespace TagMp3SaitoTestProject
{
    /// <summary>
    /// Summary description for CsvMakerTest
    /// </summary>
    [TestClass]
    public class MusicCsvTest
    {
        [TestMethod]
        public void Save_Csv_File_With_Two_Musics()
        {
            var ml = new MusicLoader();
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            ml.Sources.Add(dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\Silvana Malta-07-Jangada.mp3");
            ml.Sources.Add(dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\04 Ex-amor.mp3");
            ml.LoadPaths();
            MusicList musics = ml.GetMusicList();

            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
                          @"\TagMp3SaitoTestProject\CSV_Files\testCsv.csv";

            //clear
            if (File.Exists(path))
                File.Delete(path);

            //create
            var csv = new MusicCsv(musics);
            csv.SaveCsvFile(path);

            //check
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Load_And_Save_Csv_File_With_Two_Musics()
        {
            string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName +
                          @"\TagMp3SaitoTestProject\CSV_Files\testCsv.txt";

            //load
            var csv = new MusicCsv(path);
            List<Music> musics = csv.Load();
            Assert.AreEqual(2, musics.Count);
        }
    }
}