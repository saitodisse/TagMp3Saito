using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagMp3Saito;

namespace TagMp3SaitoTestProject
{
    /// <summary>
    /// Summary description for ExtratorM3uTest
    /// </summary>
    [TestClass]
    public class M3uLoaderTest
    {
        [TestMethod]
        public void Extract_All_mp3_Itens()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            string path = dirInfo.FullName + @"\TagMp3SaitoTestProject\m3u_to_test\m3uLista.m3u";
            Assert.AreEqual(167, M3uPathExtractor.ExtractPaths(path).Length);
        }
    }
}