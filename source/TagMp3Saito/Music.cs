using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using TagMp3Saito.ReflectHelp;

namespace TagMp3Saito
{
    [Serializable]
    public class MusicList : List<Music>
    {
        public Mp3FieldList FieldList { get; set; }
    } ;

    [Serializable]
    public class Music
    {
        private IID3v1 IdTag1;
        private IID3v2 IdTag2;

        public Music()
        {
        }

        public Music(string caminho)
        {
            FullPath = caminho;
            LoadId3Tags();
        }

        #region Props

        public string FullPath { get; set; }

        public string Album
        {
            get
            {
                string ret = string.Empty;
                if (IdTag2 != null)
                {
                    if (!string.IsNullOrEmpty(IdTag2.Album))
                        ret = IdTag2.Album;
                }
                if (IdTag1 != null)
                {
                    if (string.IsNullOrEmpty(ret) && !string.IsNullOrEmpty(IdTag1.Album))
                        ret = IdTag1.Album;
                }
                return ret;
            }
            set
            {
                IdTag1.Album = value;
                IdTag2.Album = value;
            }
        }

        public string Artist
        {
            get
            {
                string ret = string.Empty;
                if (IdTag2 != null)
                {
                    if (!string.IsNullOrEmpty(IdTag2.Artist))
                        ret = IdTag2.Artist;
                }
                if (IdTag1 != null)
                {
                    if (string.IsNullOrEmpty(ret) && !string.IsNullOrEmpty(IdTag1.Artist))
                        ret = IdTag1.Artist;
                }
                return ret;
            }
            set
            {
                IdTag1.Artist = value;
                IdTag2.Artist = value;
            }
        }

        public string Title
        {
            get
            {
                string ret = string.Empty;
                if (IdTag2 != null)
                {
                    if (!string.IsNullOrEmpty(IdTag2.Title))
                        ret = IdTag2.Title;
                }
                if (IdTag1 != null)
                {
                    if (string.IsNullOrEmpty(ret) && !string.IsNullOrEmpty(IdTag1.Title))
                        ret = IdTag1.Title;
                }
                return ret;
            }
            set
            {
                IdTag1.Title = value;
                IdTag2.Title = value;
            }
        }

        public string TrackNumber
        {
            get
            {
                string ret = string.Empty;
                if (IdTag2 != null)
                {
                    if (!string.IsNullOrEmpty(IdTag2.TrackNumber))
                        ret = IdTag2.TrackNumber;
                }
                if (IdTag1 != null)
                {
                    if (string.IsNullOrEmpty(ret))
                        ret = IdTag1.TrackNumber.ToString();
                }
                return ret;
            }
            set
            {
                // try parse else dont save
                int number = 0;
                int.TryParse(value, out number);
                if (number != 0)
                    IdTag1.TrackNumber = number;

                IdTag2.TrackNumber = value;
            }
        }

        public string Year
        {
            get
            {
                string ret = string.Empty;
                if (IdTag2 != null)
                {
                    if (!string.IsNullOrEmpty(IdTag2.Year))
                        ret = IdTag2.Year;
                }
                if (IdTag1 != null)
                {
                    if (string.IsNullOrEmpty(ret) && !string.IsNullOrEmpty(IdTag1.Year))
                        ret = IdTag1.Year;
                }
                return ret;
            }
            set
            {
                IdTag1.Year = value;
                IdTag2.Year = value;
            }
        }


        public string Accompaniment_ArtistAlbum
        {
            get
            {
                if (IdTag2 != null)
                    return IdTag2.Accompaniment;
                else
                    return null;
            }
            set { IdTag2.Accompaniment = value; }
        }

        public string DiscNumber
        {
            get
            {
                if (IdTag2 != null)
                    return IdTag2.DiscNumber;
                else
                    return null;
            }
            set { IdTag2.DiscNumber = value; }
        }

        public string Genre
        {
            get
            {
                if (IdTag2 != null)
                    return IdTag2.Genre;
                else
                    return null;
            }
            set { IdTag2.Genre = value; }
        }

        public string OriginalArtist
        {
            get
            {
                if (IdTag2 != null)
                    return IdTag2.OriginalArtist;
                else
                    return null;
            }
            set { IdTag2.OriginalArtist = value; }
        }

        public string CommentOne
        {
            get
            {
                if (IdTag2 != null)
                {
                    if (IdTag2.CommentsList.Count > 0)
                        return IdTag2.CommentsList[0].Value;
                    else
                        return string.Empty;
                }
                return null;
            }
            set
            {
                if (!ID3v2Helper.DoesTagExist(FullPath))
                    IdTag2 = ID3v2Helper.CreateID3v2(FullPath);

                if (IdTag2.CommentsList.Count == 0)
                    IdTag2.CommentsList.AddNew();

                IdTag2.CommentsList[0].Value = value;
            }
        }

        public string Subtitle
        {
            get
            {
                if (IdTag2 != null)
                    return IdTag2.Subtitle;
                else
                    return null;
            }
            set
            {
                IdTag2.Subtitle = value;
            }
        }

        #endregion

        public void LoadId3Tags()
        {
            IdTag1 = ID3v1Helper.CreateID3v1(FullPath);
            IdTag2 = ID3v2Helper.CreateID3v2(FullPath);
        }

        public void Save(Mp3FieldList fieldList)
        {
            if (IdTag1 != null)
            {
                if (fieldList.Any(f => f.PropName == "Artist")) IdTag1.Artist = Artist;
                if (fieldList.Any(f => f.PropName == "Album")) IdTag1.Album = Album;
                if (fieldList.Any(f => f.PropName == "Title")) IdTag1.Title = Title;
                if (fieldList.Any(f => f.PropName == "Year")) IdTag1.Year = Year;

                int number;
                if (fieldList.Any(f => f.PropName == "TrackNumber") && int.TryParse(TrackNumber, out number))
                    IdTag1.TrackNumber = int.Parse(TrackNumber);

                var mp3File = new FileInfo(FullPath);
                if (mp3File.IsReadOnly)
                    mp3File.IsReadOnly = false;

                IdTag1.Save(mp3File.FullName);
            }
            if (IdTag2 != null)
            {
                if (fieldList.Any(f => f.PropName == "Artist")) IdTag2.Artist = Artist;
                if (fieldList.Any(f => f.PropName == "Album")) IdTag2.Album = Album;
                if (fieldList.Any(f => f.PropName == "Title")) IdTag2.Title = Title;
                if (fieldList.Any(f => f.PropName == "Year")) IdTag2.Year = Year;
                if (fieldList.Any(f => f.PropName == "TrackNumber")) IdTag2.TrackNumber = TrackNumber;
                if (fieldList.Any(f => f.PropName == "DiscNumber")) IdTag2.DiscNumber = DiscNumber;
                if (fieldList.Any(f => f.PropName == "Genre")) IdTag2.Genre = Genre;
                if (fieldList.Any(f => f.PropName == "Accompaniment_ArtistAlbum"))
                    IdTag2.Accompaniment = Accompaniment_ArtistAlbum;
                if (fieldList.Any(f => f.PropName == "OriginalArtist")) IdTag2.OriginalArtist = OriginalArtist;

                if (fieldList.Any(f => f.PropName == "CommentOne"))
                {
                    if (IdTag2.CommentsList.Count == 0)
                        IdTag2.CommentsList.AddNew();
                    IdTag2.CommentsList[0].Value = CommentOne;
                }
                if (fieldList.Any(f => f.PropName == "Subtitle")) IdTag2.Subtitle = Subtitle;

                IdTag2.Save(FullPath);
            }
        }
    }
}