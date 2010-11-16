using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagMp3Saito;

namespace TagMp3SaitoTestProject
{
    /// <summary>
    /// Summary description for MusicLoaderTest
    /// </summary>
    [TestClass]
    public class MusicLoaderTest
    {
        [TestMethod]
        public void Load_Files()
        {
            var ml = new MusicLoader();
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            ml.Sources.Add(dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\Silvana Malta-07-Jangada.mp3");
            ml.Sources.Add(dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\04 Ex-amor.mp3");
            Assert.AreEqual(2, ml.LoadPaths().Length);
        }

        [TestMethod]
        public void Load_Folder()
        {
            var ml = new MusicLoader();
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            ml.Sources.Add(dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test");
            Assert.AreEqual(2, ml.LoadPaths().Length);
        }

        [TestMethod]
        public void Load_M3u_File()
        {
            var ml = new MusicLoader();
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            ml.Sources.Add(dirInfo.FullName + @"\TagMp3SaitoTestProject\m3u_to_test\m3uLista.m3u");
            Assert.AreEqual(167, ml.LoadPaths().Length);
        }
    }
}