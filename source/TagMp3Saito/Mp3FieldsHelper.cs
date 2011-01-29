using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TagMp3Saito
{
    public static class Mp3FieldsHelper
    {
        public static List<Mp3Field> GetMp3FieldsFromJSON(string path)
        {
            if (path == null)
                path = GetDefaultPath();

            var streamReader = new StreamReader(path, Encoding.Unicode);
            var mp3FieldsJSON = streamReader.ReadToEnd();
            streamReader.Close();

            return JSONSerializer.JSonDeserialize<List<Mp3Field>>(mp3FieldsJSON);
        }

        public static void SaveJSON(List<Mp3Field> mp3Fields, string path = null)
        {
            var objetoSerializado = JSONSerializer.JSonSerialize(mp3Fields);

            if (path == null)
            {
                path = GetDefaultPath();
            }

            var fileWriter = new StreamWriter(path, false, Encoding.Unicode);
            fileWriter.Write(objetoSerializado);
            fileWriter.Flush();
            fileWriter.Close();
        }


        public static string GetDefaultPath()
        {
            return Environment.CurrentDirectory + @"\columnsConfig.json";
        }

    }
}
