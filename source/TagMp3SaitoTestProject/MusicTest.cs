using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagMp3Saito;

namespace TagMp3SaitoTestProject
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MusicTest
    {
        /// <summary>
        /// ID3v1 - Load a tiny music from Test Project folder
        /// </summary>
        [TestMethod]
        public void Load_Id3V2_Mp3_File()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            string path = dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\Silvana Malta-07-Jangada.mp3";

            var mus = new Music(path);
            Assert.AreEqual(path, mus.FullPath);
            Assert.AreEqual("Silvana Malta", mus.Artist);
            Assert.AreEqual("Silvana Malta - Artist Album", mus.Accompaniment_ArtistAlbum);
            Assert.AreEqual("07-Jangada", mus.Title);
            Assert.AreEqual("Be Bossa", mus.Album);

            Assert.AreEqual("7", mus.TrackNumber);

            Assert.AreEqual("2007", mus.Year);
        }

        /// <summary>
        /// ID3v2 - Load a tiny music from Test Project folder
        /// </summary>
        [TestMethod]
        public void Load_Id3V1_Mp3_File()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            string path = dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\04 Ex-amor.mp3";

            var mus = new Music(path);
            Assert.AreEqual(path, mus.FullPath);
            Assert.AreEqual("Flávia Bittencourt", mus.Artist);
            Assert.AreEqual("Ex-amor", mus.Title);
            Assert.AreEqual("Sentido", mus.Album);

            // The trackNumber can is alpha-numeric at ID3v2
            int number = 0;
            if (int.TryParse(mus.TrackNumber, out number))
                Assert.AreEqual(4, int.Parse(mus.TrackNumber));

            Assert.AreEqual("2005", mus.Year);
        }

        /// <summary>
        /// ID3v2 - Update fields
        /// </summary>
        [TestMethod]
        public void Update_Some_Information()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            string path = dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\04 Ex-amor.mp3";
            var mus = new Music(path);

            //change
            mus.Artist = "ChangedArtist";
            Assert.AreEqual("ChangedArtist", mus.Artist);
            Assert.AreNotEqual("ChangedArtist2", mus.Artist);
            //come back
            mus.Artist = "Flávia Bittencourt";
            Assert.AreEqual("Flávia Bittencourt", mus.Artist);
        }
    }
}