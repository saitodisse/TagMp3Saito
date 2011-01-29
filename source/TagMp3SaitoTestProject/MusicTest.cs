using System.Collections.Generic;
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
        [TestMethod]
        public void Load_Id3V1_Mp3_File()
        {
            DirectoryInfo dirInfo = TestHelpers.GetTestDirectoryInfo();
            string path = dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\04 Ex-amor.mp3";

            var mus = new MusicFile(path, TestHelpers.GetMp3FieldsSelecionados());
            Assert.AreEqual(path, mus.FullPath);
        }

        //[TestMethod]
        //public void Load_Id3V2_Mp3_File()
        //{
        //    DirectoryInfo dirInfo = TestHelpers.GetTestDirectoryInfo();
        //    string path = dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\Silvana Malta-07-Jangada.mp3";

        //    var mus = new MusicFile(path, TestHelpers.GetMp3FieldsSelecionados());
        //    Assert.AreEqual(path, mus.FullPath);
        //}

        /// <summary>
        /// ID3v2 - Update fields
        /// </summary>
        [TestMethod]
        public void Update_Some_Information()
        {
            DirectoryInfo dirInfo = TestHelpers.GetTestDirectoryInfo();
            string path = dirInfo.FullName + @"\TagMp3SaitoTestProject\Mp3_To_Test\04 Ex-amor.mp3";
            var mus = new MusicFile(path, TestHelpers.GetMp3FieldsSelecionados());
        }

    }
}