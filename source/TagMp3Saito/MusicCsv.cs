using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TagMp3Saito.ReflectHelp;

namespace TagMp3Saito
{
    public class MusicCsv
    {
        private MusicList _musics = new MusicList();

        public MusicCsv(MusicList musics)
        {
            _musics = musics;
        }

        public MusicCsv(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SaveCsvFile(string path)
        {
            var sb = new StringBuilder();

            if (_musics == null)
                throw new Exception("no songs loaded to save the CSV");

            // ** <th> **
            //var mp3FieldList = GetDefaultMp3FieldListProperties();
            Mp3FieldList mp3FieldList = GetXmlConfigMp3FieldListProperties();
            for (int i = 0; i < mp3FieldList.Count; i++)
            {
                Mp3Field pair = mp3FieldList[i];
                sb.Append("=\"");
                sb.Append(pair.PropName);
                sb.Append("\"");

                // does not include TAB for the last one
                if (i < mp3FieldList.Count - 1)
                    sb.Append("\t");
            }
            sb.AppendLine();

            // ** <td> **
            foreach (Music mus in _musics)
            {
                sb.Append(Mus2CsvLine(mus, mp3FieldList, "=\"", "\"", "\t", EncoderForCsv));
                sb.AppendLine();
            }

            ////Encode special characters
            var fileWriter = new StreamWriter(path, false, Encoding.Unicode);
            fileWriter.Write(sb.ToString());
            fileWriter.Flush();
            fileWriter.Close();
        }

        private string Mus2CsvLine(Music mus, Mp3FieldList mp3FieldList, string startWith, string endWith,
                                   string separator, Func<string, string> encoderForCsv)
        {
            var sb = new StringBuilder();
            foreach (Mp3Field field in mp3FieldList)
            {
                sb.Append(startWith);
                object value = mus.GetType().GetProperties().Where(p => p.Name == field.PropName).Single().GetValue(
                    mus, null);
                if (value != null)
                {
                    if (field.PropName == "CommentOne")
                        sb.Append(EncoderForCsv(value.ToString()));
                    else
                        sb.Append(value);
                }
                sb.Append(endWith);
                sb.Append(separator);
            }
            return sb.ToString();
        }

        private static string EncoderForCsv(string original)
        {
            var sb = new StringBuilder(original);
            sb.Replace("\r", "\\r");
            sb.Replace("\n", "\\n");
            sb.Replace("\t", "\\t");
            sb.Replace("\"", "\"\"");
            return sb.ToString();
        }

        private static string DecoderFromCsv(string changed)
        {
            var sb = new StringBuilder(changed);
            sb.Replace("\\r", "\r");
            sb.Replace("\\n", "\n");
            sb.Replace("\\t", "\t");
            sb.Replace("\"\"", "\"");
            return sb.ToString();
        }

        /// <summary>
        /// The first line contains the properties
        /// </summary>
        /// <returns></returns>
        public MusicList Load()
        {
            if (!File.Exists(Path))
                return null;

            _musics = new MusicList();
            var sr = new StreamReader(Path);
            string row;
            string[] columns;

            // See column Order with the first line
            row = sr.ReadLine();
            Mp3FieldList mp3FieldList = GetColumns(row, "\t");

            // Get "FullPath"
            if (mp3FieldList.Where(p => p.PropName == "FullPath").Count() == 0)
                throw new Exception("The FullPath column was not found.");
            int fullPathIndex = mp3FieldList.Where(p => p.PropName == "FullPath").Single().Index;

            _musics.FieldList = mp3FieldList;

            Music mus;
            while ((row = sr.ReadLine()) != null)
            {
                //Clean the crap at the start and the end of each column
                row = Regex.Replace(row, "(^|\\t)(=\")", "$1");
                row = Regex.Replace(row, "(\")($|\\t)", "$2");

                columns = row.Split('\t');

                // Load Mp3 File
                mus = new Music();

                mus.GetType().GetProperties().Where(p => p.Name == "FullPath").Single().SetValue(mus,
                                                                                                 columns[fullPathIndex],
                                                                                                 null);
                mus.LoadId3Tags();

                // Set each Property
                for (int i = 0; i < mp3FieldList.Count; i++)
                {
                    if (mp3FieldList[i].PropName == "CommentOne")
                        columns[i] = DecoderFromCsv(columns[i]);

                    PropertyInfo[] props = mus.GetType().GetProperties();
                    IEnumerable<PropertyInfo> prop1 = props.Where(p => p.Name == mp3FieldList[i].PropName);
                    PropertyInfo prop11 = prop1.SingleOrDefault();
                    prop11.SetValue(mus, columns[i], null);
                }

                _musics.Add(mus);
            }

            sr.Close();

            return _musics;
        }

        private static Mp3FieldList GetColumns(string row, string sep)
        {
            var listaCol = new Mp3FieldList();
            var mus = new Music();
            for (int i = 0; i < row.Split(sep.ToCharArray()[0]).Length; i++)
            {
                string col = row.Split(sep.ToCharArray()[0])[i].Replace("=", string.Empty).Replace("\"", string.Empty);
                listaCol.Add(new Mp3Field(true, i, col));
            }
            return listaCol;
        }

        public static Mp3FieldList GetDefaultMp3FieldListProperties()
        {
            var mus = new Music();
            PropertyInfo[] pi = mus.GetType().GetProperties();
            var pl = new Mp3FieldList();

            string[] proptList = {
                                     "Artist",
                                     "DiscNumber",
                                     "Album",
                                     "TrackNumber",
                                     "Title",
                                     "Genre",
                                     "Year",
                                     "OriginalArtist",
                                     "Accompaniment_ArtistAlbum",
                                     "CommentOne",
                                     "Subtitle",
                                     "FullPath"
                                 };

            for (int i = 0; i < proptList.Length; i++)
            {
                string s = proptList[i];
                pl.Add(new Mp3Field(true, i, s));
            }
            return pl;
        }

        public static Mp3FieldList GetXmlConfigMp3FieldListProperties()
        {
            var mus = new Music();
            PropertyInfo[] pi = mus.GetType().GetProperties();
            var pl = new Mp3FieldList();

            string path = new DirectoryInfo(Environment.CurrentDirectory).FullName + @"\columnsConfig.xml";
            if (!File.Exists(path))
                return GetDefaultMp3FieldListProperties();

            XDocument doc = XDocument.Load(path);

            foreach (XElement ele in doc.Elements("TagMp3Saito-Column-Config").Descendants())
            {
                int count = 0;
                if (Convert.ToBoolean(ele.Attribute("show").Value))
                {
                    pl.Add(new Mp3Field(true, count, ele.Name.ToString()));
                    count++;
                }
            }
            return pl;
        }
    }
}