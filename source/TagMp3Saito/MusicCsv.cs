using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Id3Tag;
using Id3Tag.HighLevel;
using Id3Tag.HighLevel.Id3Frame;
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

        public string Path
        {
            get;
            set;
        }

        public void SaveCsvFile(string path)
        {
            var sb = new StringBuilder();

            if (_musics == null)
                throw new Exception("no songs loaded to save the CSV");

            // ** <th> **
            var fieldJSON = GetSelectedMp3FieldsFromJSON();
            foreach (Mp3Field pair in fieldJSON)
            {
                sb.Append("=\"");
                sb.Append(pair);
                sb.Append("\"");
                sb.Append("\t");
            }

            sb.Append("=\"");
            sb.Append("** FullPath **");
            sb.Append("\"");

            sb.AppendLine();

            // ** <td> **
            foreach (MusicFile mus in _musics)
            {
                sb.Append(Mus2CsvLine(mus, "=\"", "\"", "\t", EncoderForCsv));
                sb.AppendLine();
            }

            var fileWriter = new StreamWriter(path, false, Encoding.Unicode);
            fileWriter.Write(sb.ToString());
            fileWriter.Flush();
            fileWriter.Close();
        }

        private string Mus2CsvLine(MusicFile mus, string startWith, string endWith,
                                   string separator, Func<string, string> encoderForCsv)
        {
            var sb = new StringBuilder();
            foreach (Mp3Field fieldJSON in GetSelectedMp3FieldsFromJSON())
            {
                sb.Append(startWith);

                // para campos de observação
                if (fieldJSON.TagName == "XXXXXXX")
                    sb.Append(EncoderForCsv(fieldJSON.Valor.ToString()));

                // se o campo do JSON exitir na lista da musica
                else if (mus.Mp3Fields.Any(f => f.TagName == fieldJSON.TagName))
                    sb.Append(mus.Mp3Fields.FirstOrDefault(f => f.TagName == fieldJSON.TagName).Valor);

                else
                    sb.Append("");

                sb.Append(endWith);
                sb.Append(separator);
            }

            sb.Append(startWith);
            sb.Append(mus.FullPath);
            sb.Append(endWith);

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

        public void LoadAndSave()
        {
            if (!File.Exists(Path))
                return;

            //_musics = new MusicList();
            var sr = new StreamReader(Path);
            string row;
            string[] columns;

            // See column Order with the first line
            row = sr.ReadLine();
            List<Mp3Field> mp3Fields = GetFirstLineColumns(row, "\t");

            int fullPathIndex = mp3Fields.Count;

            MusicFile mus;
            while ((row = sr.ReadLine()) != null)
            {
                //Clean the crap at the start and the end of each column
                row = Regex.Replace(row, "(^|\\t)(=\")", "$1");
                row = Regex.Replace(row, "(\")($|\\t)", "$2");

                columns = row.Split('\t');

                // Load Mp3 File
                mus = new MusicFile();

                // Call constructor passing FullPath and the default fields configuration
                var OLD_FILE = columns[fullPathIndex];
                mus.GetType().GetProperties().Where(p => p.Name == "FullPath").Single().SetValue(mus,
                                                                                                 OLD_FILE,
                                                                                                 null);
                //????
                //mus.LoadId3Tags(mp3Fields);

                if (!File.Exists(OLD_FILE))
                    continue;

                // Set each Property
                var id3TagManager = new Id3TagManager();

                var tagsStatus = id3TagManager.GetTagsStatus(OLD_FILE);
                if (tagsStatus.Id3V1TagFound)
                {
                    id3TagManager.RemoveV1Tag(OLD_FILE);
                }
                if (tagsStatus.Id3V2TagFound)
                {
                    id3TagManager.RemoveV2Tag(OLD_FILE);
                }

                //id3TagManager.WriteV2Tag(columns[fullPathIndex], CreateTagContainer(mp3Fields, columns));


                //Save in a new file
                var NEW_FILE = OLD_FILE + ".aux";

                Id3TagFactory.CreateId3TagManager().WriteV2Tag(
                    OLD_FILE,
                    NEW_FILE, 
                    CreateTagContainer(mp3Fields, columns));

                
                
                var oldFile = new FileInfo(OLD_FILE);
                var newFile = new FileInfo(NEW_FILE);
                
                //Delete OldFile
                oldFile.Delete();
                //and Rename New to Old
                newFile.MoveTo(OLD_FILE);

                //Id3TagFactory.CreateId3TagManager().WriteV2Tag(columns[fullPathIndex], tagContainer);

                //if (mp3Fields[i].Description == "CommentOne")
                //    columns[i] = DecoderFromCsv(columns[i]);


                //_musics.Add(mus);
            }

            sr.Close();

            //return _musics;
        }

        private TagContainer CreateTagContainer(List<Mp3Field> mp3Fields, string[] columns)
        {
            TagContainer container = Id3TagFactory.CreateId3Tag(TagVersion.Id3V23);
            //if (data.Version == TagVersion.Id3V23)
            //{
            //
            //  Configure the ID3v2.3 header
            //
            TagDescriptorV3 extendedHeaderV23 = container.GetId3V23Descriptor();
            // Configure the tag header.
            extendedHeaderV23.SetHeaderOptions(false, false, false);
            //}
            //else
            //{
            //
            //  Configure the ID3v2.4 header
            //
            //TagDescriptorV4 extendedHeaderV24 = container.GetId3V24Descriptor();
            //extendedHeaderV24.SetHeaderOptions(false, false, false, true);
            //}

            for (int i = 0; i < mp3Fields.Count; i++)
            {
                container.Add(new TextFrame(mp3Fields[i].TagName, columns[i], Encoding.Default));
            }

            return container;
        }


        private static List<Mp3Field> GetFirstLineColumns(string row, string sep)
        {
            var listaCol = new List<Mp3Field>();

            var defaultMp3Fields = GetDefaultMp3Fields();

            var mus = new MusicFile();
            for (int i = 0; i < row.Split(sep.ToCharArray()[0]).Length; i++)
            {
                string col = row.Split(sep.ToCharArray()[0])[i].Replace("=", string.Empty).Replace("\"", string.Empty);

                Regex regex = new Regex(@"^\[(.*?)\].*$");
                string tagName = regex.Match(col).Groups[1].Value;



                //find field from default list
                var defaultMp3Field = defaultMp3Fields.SingleOrDefault(ff => ff.TagName == tagName);


                if (defaultMp3Field == null && col == "** FullPath **")
                    break;

                if (defaultMp3Field == null && col != "** FullPath **")
                    throw new Exception("** FullPath ** Column not found at first line, tagName founded:" + tagName);


                //get fields from first line and deafult fields
                var mp3Field = new Mp3Field
                                   {
                                       Index = i,
                                       Active = true,
                                       Description = defaultMp3Field.Description,
                                       DetailedDescription = defaultMp3Field.DetailedDescription,
                                       FrameTypeString = defaultMp3Field.FrameTypeString,
                                       TagName = defaultMp3Field.TagName,
                                       IdV1_Compatible = defaultMp3Field.IdV1_Compatible,
                                   };

                listaCol.Add(mp3Field);
            }
            return listaCol;
        }

        public static List<Mp3Field> GetDefaultMp3Fields()
        {
            var mp3Fields = new List<Mp3Field>();

            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "AudioEncryptionFrame(AENC)",
            //    DetailedDescription = "Audio Encryption Frame ",
            //    FrameTypeString = "AudioEncryptionFrame",
            //    IdV1_Compatible = false,
            //    Index = 1,
            //    TagName = "AENC ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Comments(COMM)",
            //    DetailedDescription = "Comments ",
            //    FrameTypeString = "CommentFrame",
            //    IdV1_Compatible = false,
            //    Index = 2,
            //    TagName = "COMM ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "MusicCDIdentifierFrame(MCDI)",
            //    DetailedDescription = "Music CD Identifier Frame ",
            //    FrameTypeString = "MusicCDIdentifierFrame",
            //    IdV1_Compatible = false,
            //    Index = 3,
            //    TagName = "MCDI ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "PictureFrame(APIC)",
            //    DetailedDescription = "Picture Frame ",
            //    FrameTypeString = "PictureFrame",
            //    IdV1_Compatible = false,
            //    Index = 4,
            //    TagName = "APIC ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Playcounter(PCNT)",
            //    DetailedDescription = "Play counter ",
            //    FrameTypeString = "PlayCounterFrame",
            //    IdV1_Compatible = false,
            //    Index = 5,
            //    TagName = "PCNT ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Popularimeter(POPM)",
            //    DetailedDescription = "Popularimeter ",
            //    FrameTypeString = "PopularimeterFrame",
            //    IdV1_Compatible = false,
            //    Index = 6,
            //    TagName = "POPM ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "PrivateFrame(PRIV)",
            //    DetailedDescription = "Private Frame ",
            //    FrameTypeString = "PrivateFrame",
            //    IdV1_Compatible = false,
            //    Index = 7,
            //    TagName = "PRIV ",
            //    Valor = null
            //});
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Album_Movie_Showtitle(TALB)",
                DetailedDescription = "Album/Movie/Show title",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 8,
                TagName = "TALB",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "BPM(TBPM)",
                DetailedDescription = "BPM (beats per minute)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 9,
                TagName = "TBPM",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Composer(TCOM)",
                DetailedDescription = "Composer",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 10,
                TagName = "TCOM",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Contenttype(TCON)",
                DetailedDescription = "Content type",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 11,
                TagName = "TCON",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Copyrightmessage(TCOP)",
                DetailedDescription = "Copyright message",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 12,
                TagName = "TCOP",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Date(TDAT)",
                DetailedDescription = "Date (replaced by TDRC in v2.4)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 13,
                TagName = "TDAT",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Encodingtime(TDEN)",
                DetailedDescription = "Encoding time",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 14,
                TagName = "TDEN",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Playlistdelay(TDLY)",
                DetailedDescription = "Playlist delay",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 15,
                TagName = "TDLY",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Originalreleasetime(TDOR)",
                DetailedDescription = "Original release time",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 16,
                TagName = "TDOR",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Recordingtime(TDRC)",
                DetailedDescription = "Recording time",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 17,
                TagName = "TDRC",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Releasetime(TDRL)",
                DetailedDescription = "Release time",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 18,
                TagName = "TDRL",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Taggingtime(TDTG)",
                DetailedDescription = "Tagging time",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 19,
                TagName = "TDTG",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Encodedby(TENC)",
                DetailedDescription = "Encoded by",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 20,
                TagName = "TENC",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Lyricist_Textwriter(TEXT)",
                DetailedDescription = "Lyricist/Text writer",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 21,
                TagName = "TEXT",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Filetype(TFLT)",
                DetailedDescription = "File type",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 22,
                TagName = "TFLT",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Time(TIME)",
                DetailedDescription = "Time (replaced by TDRC in v2.4)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 23,
                TagName = "TIME",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Involvedpeoplelist(TIPL)",
                DetailedDescription = "Involved people list",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 24,
                TagName = "TIPL",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Contentgroupdescription(TIT1)",
                DetailedDescription = "Content group description",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 25,
                TagName = "TIT1",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Title_songname_contentdescription(TIT2)",
                DetailedDescription = "Title/songname/content description",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 26,
                TagName = "TIT2",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Subtitle_Descriptionrefinement(TIT3)",
                DetailedDescription = "Subtitle/Description refinement",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 27,
                TagName = "TIT3",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Initialkey(TKEY)",
                DetailedDescription = "Initial key",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 28,
                TagName = "TKEY",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Language(TLAN)",
                DetailedDescription = "Language(s)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 29,
                TagName = "TLAN",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Length(TLEN)",
                DetailedDescription = "Length",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 30,
                TagName = "TLEN",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Musiciancreditslist(TMCL)",
                DetailedDescription = "Musician credits list",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 31,
                TagName = "TMCL",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Mediatype(TMED)",
                DetailedDescription = "Media type",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 32,
                TagName = "TMED",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Mood(TMOO)",
                DetailedDescription = "Mood",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 33,
                TagName = "TMOO",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Originalalbum_movie_showtitle(TOAL)",
                DetailedDescription = "Original album/movie/show title",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 34,
                TagName = "TOAL",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Originalfilename(TOFN)",
                DetailedDescription = "Original filename",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 35,
                TagName = "TOFN",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Originallyricist_textwriter(TOLY)",
                DetailedDescription = "Original lyricist(s)/text writer(s)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 36,
                TagName = "TOLY",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Originalartist_performer(TOPE)",
                DetailedDescription = "Original artist(s)/performer(s)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 37,
                TagName = "TOPE",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Originalreleaseyear(TORY)",
                DetailedDescription = "Original release year (replaced by TDOR in v2.4)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 38,
                TagName = "TORY",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Fileowner_licensee(TOWN)",
                DetailedDescription = "File owner/licensee",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 39,
                TagName = "TOWN",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Leadperformer_Soloist(TPE1)",
                DetailedDescription = "Lead performer(s)/Soloist(s)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 40,
                TagName = "TPE1",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Band_orchestra_accompaniment(TPE2)",
                DetailedDescription = "Band/orchestra/accompaniment",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 41,
                TagName = "TPE2",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Conductor_performerrefinement(TPE3)",
                DetailedDescription = "Conductor/performer refinement",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 42,
                TagName = "TPE3",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Interpreted_remixed(TPE4)",
                DetailedDescription = "Interpreted, remixed, or otherwise modified by",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 43,
                TagName = "TPE4",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Partofaset(TPOS)",
                DetailedDescription = "Part of a set",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 44,
                TagName = "TPOS",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Producednotice(TPRO)",
                DetailedDescription = "Produced notice",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 45,
                TagName = "TPRO",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Publisher(TPUB)",
                DetailedDescription = "Publisher",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 46,
                TagName = "TPUB",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Tracknumber_Positioninset(TRCK)",
                DetailedDescription = "Track number/Position in set",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 47,
                TagName = "TRCK",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Recordingdates(TRDA)",
                DetailedDescription = "Recording dates (replaced by TDRC in v2.4)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 48,
                TagName = "TRDA",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Internetradiostationname(TRSN)",
                DetailedDescription = "Internet radio station name",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 49,
                TagName = "TRSN",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Internetradiostationowner(TRSO)",
                DetailedDescription = "Internet radio station owner",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 50,
                TagName = "TRSO",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Size(TSIZ)",
                DetailedDescription = "Size (deprecated in v2.4)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 51,
                TagName = "TSIZ",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Albumsortorder(TSOA)",
                DetailedDescription = "Album sort order",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 52,
                TagName = "TSOA",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Performersortorder(TSOP)",
                DetailedDescription = "Performer sort order",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 53,
                TagName = "TSOP",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Titlesortorder(TSOT)",
                DetailedDescription = "Title sort order",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 54,
                TagName = "TSOT",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "ISRC(TSRC)",
                DetailedDescription = "ISRC (international standard recording code)",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 55,
                TagName = "TSRC",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Software_HardEnc(TSSE)",
                DetailedDescription = "Software/Hardware and settings used for encoding",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 56,
                TagName = "TSSE",
                Valor = null
            });
            mp3Fields.Add(new Mp3Field
            {
                Active = true,
                Description = "Setsubtitle(TSST)",
                DetailedDescription = "Set subtitle",
                FrameTypeString = "TextFrame",
                IdV1_Compatible = false,
                Index = 57,
                TagName = "TSST",
                Valor = null
            });
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Uniquefileidentifier(UFID)",
            //    DetailedDescription = "Unique file identifier ",
            //    FrameTypeString = "UniqueFileIdentifierFrame",
            //    IdV1_Compatible = false,
            //    Index = 58,
            //    TagName = "UFID ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Unsychronizedlyric_texttranscription(USLT)",
            //    DetailedDescription = "Unsychronized lyric/text transcription ",
            //    FrameTypeString = "UnsynchronisedLyricFrame",
            //    IdV1_Compatible = false,
            //    Index = 59,
            //    TagName = "USLT ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Commercialinformation(WCOM)",
            //    DetailedDescription = "Commercial information",
            //    FrameTypeString = "UrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 60,
            //    TagName = "WCOM",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Copyright_Legalinformation(WCOP)",
            //    DetailedDescription = "Copyright/Legal information",
            //    FrameTypeString = "UrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 61,
            //    TagName = "WCOP",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Officialaudiofilewebpage(WOAF)",
            //    DetailedDescription = "Official audio file webpage",
            //    FrameTypeString = "UrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 62,
            //    TagName = "WOAF",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Officialartist_performerwebpage(WOAR)",
            //    DetailedDescription = "Official artist/performer webpage",
            //    FrameTypeString = "UrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 63,
            //    TagName = "WOAR",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Officialaudiosourcewebpage(WOAS)",
            //    DetailedDescription = "Official audio source webpage",
            //    FrameTypeString = "UrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 64,
            //    TagName = "WOAS",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Officialinternetradiostationhomepage(WORS)",
            //    DetailedDescription = "Official internet radio station homepage",
            //    FrameTypeString = "UrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 65,
            //    TagName = "WORS",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Payment(WPAY)",
            //    DetailedDescription = "Payment",
            //    FrameTypeString = "UrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 66,
            //    TagName = "WPAY",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "Publishersofficialwebpage(WPUB)",
            //    DetailedDescription = "Publishers official webpage",
            //    FrameTypeString = "UrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 67,
            //    TagName = "WPUB",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "UserdefinedText(TUserDefinedTextFrame	TXXX 	User defined Text 	UserdefinedText(TXXX))",
            //    DetailedDescription = "User defined Text ",
            //    FrameTypeString = "UserDefinedTextFrame",
            //    IdV1_Compatible = false,
            //    Index = 68,
            //    TagName = "TUserDefinedTextFrame	TXXX 	User defined Text 	UserdefinedText(TXXX) ",
            //    Valor = null
            //});
            //mp3Fields.Add(new Mp3Field
            //{
            //    Active = true,
            //    Description = "UserURLs(WUserDefinedUrlLinkFrame	WXXX 	User URLs 	UserURLs(WXXX))",
            //    DetailedDescription = "User URLs ",
            //    FrameTypeString = "UserDefinedUrlLinkFrame",
            //    IdV1_Compatible = false,
            //    Index = 69,
            //    TagName = "WUserDefinedUrlLinkFrame	WXXX 	User URLs 	UserURLs(WXXX) ",
            //    Valor = null
            //});

            return mp3Fields;
        }

        public static List<Mp3Field> GetSelectedMp3FieldsFromJSON()
        {
            var pl = new List<Mp3Field>();

            string path = new DirectoryInfo(Environment.CurrentDirectory).FullName + @"\columnsConfig.json";
            if (!File.Exists(path))
                return GetDefaultMp3Fields();

            var mp3Fields = Mp3FieldsHelper.GetMp3FieldsFromJSON(null);

            foreach (var ele in mp3Fields)
            {
                if (Convert.ToBoolean(ele.Active))
                    pl.Add(ele);
            }
            return pl;
        }
    }
}