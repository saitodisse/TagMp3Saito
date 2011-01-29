using System;
using System.Collections.Generic;
using System.IO;
using Id3Tag;
using Id3Tag.HighLevel;

namespace TagMp3Saito
{
    [Serializable]
    public class MusicList : List<MusicFile>
    {
    }


    [Serializable]
    public class MusicFile
    {
        public MusicFile()
        {
        }

        public MusicFile(string filePath, List<Mp3Field> mp3FieldsSelecionados)
        {
            FullPath = filePath;
            LoadId3Tags(mp3FieldsSelecionados);
        }

        protected Id3V1Tag TagV1
        {
            get;
            set;
        }
        protected TagContainer TagV2
        {
            get;
            set;
        }

        public List<Mp3Field> Mp3Fields
        {
            get;
            set;
        }

        public string FullPath
        {
            get;
            set;
        }

        public void LoadId3Tags(List<Mp3Field> mp3FieldsSelecionados)
        {
            Mp3Fields = new List<Mp3Field>();

            var manager = Id3TagFactory.CreateId3TagManager();

            //Scanning for Tags
            var state = manager.GetTagsStatus(FullPath);

            if (state.Id3V1TagFound)
                TagV1 = manager.ReadV1Tag(FullPath); //Reading Tags

            if (state.Id3V2TagFound)
                TagV2 = manager.ReadV2Tag(FullPath); //Reading Tags

            if (TagV2 == null)
                return;

            foreach (var mp3FieldsSelecionado in mp3FieldsSelecionados)
            {
                if (mp3FieldsSelecionado.FrameTypeString == "TextFrame" && TagV2.SearchFrame(mp3FieldsSelecionado.TagName) != null)
                {
                    mp3FieldsSelecionado.Valor = FrameUtilities.ConvertToText(TagV2.SearchFrame(mp3FieldsSelecionado.TagName)).Content;
                    Mp3Fields.Add(mp3FieldsSelecionado);
                }
            }
        }

        public void Save(List<Mp3Field> fieldList)
        {
            var mp3File = new FileInfo(FullPath);
            if (mp3File.IsReadOnly)
                mp3File.IsReadOnly = false;

            var manager = Id3TagFactory.CreateId3TagManager();
            manager.WriteV1Tag(mp3File.FullName, TagV1);
            manager.WriteV2Tag(mp3File.FullName, TagV2);

            var tagContainer = Id3TagFactory.CreateId3TagManager().ReadV2Tag(mp3File.FullName);
            //tagContainer.SearchFrame("TIT2")

        }
    }
}