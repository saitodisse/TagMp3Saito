//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace TagMp3Saito.ReflectHelp
//{
//    [Serializable]
//    public class Mp3Field
//    {
//        public Mp3Field()
//        {
//        }

//        public Mp3Field(bool active, int index, string propertyName)
//        {
//            Active = active;
//            Index = index;
//            Description = propertyName;
//        }

//        public int Index { get; set; }
//        public bool Active { get; set; }
//        public string Description { get; set; }

//        public override string ToString()
//        {
//            return String.Format("{0}", Description);
//        }
//    }

//    [Serializable]
//    public class List<Mp3Field> : List<Mp3Field>
//    {
//        public void Save(string path)
//        {
//            string serialized = Serializer.SerializeObject(this);
//            var fileWriter = new StreamWriter(path);
//            fileWriter.Write(serialized);
//            fileWriter.Flush();
//            fileWriter.Close();
//        }

//        public static List<Mp3Field> Load(string path)
//        {
//            var streamReader = new StreamReader(path);
//            string serialized = streamReader.ReadToEnd();
//            streamReader.Close();
//            return Serializer.DeserializeObject<List<Mp3Field>>(serialized);
//        }
//    }
//}