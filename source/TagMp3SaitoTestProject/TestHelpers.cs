using System;
using System.Collections.Generic;
using System.IO;
using TagMp3Saito;

namespace TagMp3SaitoTestProject
{
    public static class TestHelpers
    {
        public static DirectoryInfo GetTestDirectoryInfo()
        {
            return new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.Parent.Parent;
        }

        public static List<Mp3Field> GetMp3FieldsSelecionados()
        {
            var selecionados = new List<Mp3Field> { new Mp3Field { TagName = "TIT2", Active = true, FrameTypeString = "TextFrame" } };
            return selecionados;
        }
    }
}