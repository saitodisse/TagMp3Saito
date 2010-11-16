using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagMp3Saito;
using TagMp3Saito.ReflectHelp;

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
            Mp3FieldList pl = MusicCsv.GetDefaultMp3FieldListProperties();
            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
            pl.Save(dirInfo.FullName + @"\TagMp3SaitoTestProject\PropertyListSaved\FieldList.xml");
        }
    }
}