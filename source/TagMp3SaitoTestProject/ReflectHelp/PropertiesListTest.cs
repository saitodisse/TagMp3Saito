using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagMp3Saito;

namespace TagMp3SaitoTestProject
{
    /// <summary>
    /// Summary description for ExtratorM3uTest
    /// </summary>
    [TestClass]
    public class PropertiesListTest
    {
        [TestMethod]
        public void Save_Property_List()
        {
            List<Mp3Field> mp3Fields = MusicCsv.GetDefaultMp3Fields();
            DirectoryInfo dirInfo = TestHelpers.GetTestDirectoryInfo();
        }
    }
}