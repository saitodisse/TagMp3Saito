using System.Collections.Generic;
using System.IO;

namespace TagMp3Saito
{
    public class MusicLoader
    {
        #region Delegates

        public delegate void GetNumbetDelegate(int actual);

        #endregion

        private readonly MusicList musics = new MusicList();

        private List<string> _paths;

        public MusicLoader()
        {
            //TODO: this is not a good pratice. See dependency injection.
            Sources = new List<string>();
        }

        public List<string> Sources { get; set; }

        public event GetNumbetDelegate LoadingMusicNumber;

        public string[] LoadPaths()
        {
            _paths = new List<string>();

            foreach (string path in Sources)
            {
                if (File.Exists(path))
                {
                    var fi = new FileInfo(path);
                    switch (fi.Extension)
                    {
                        case ".mp3":
                            _paths.Add(path);
                            break;
                        case ".m3u":
                            _paths.AddRange(M3uPathExtractor.ExtractPaths(path));
                            break;
                    }
                }
                else if (Directory.Exists(path))
                {
                    foreach (string match in SearchRecursive(path, "*.mp3"))
                        _paths.Add(match);
                }
            }

            return _paths.ToArray();
        }

        public void Clear()
        {
            musics.Clear();
            Sources.Clear();
        }


        /// <summary>
        /// Taked from: http://stackoverflow.com/questions/437728/recursive-file-search-in-net
        /// </summary>
        /// <param name="root"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        private static IEnumerable<string> SearchRecursive(string root, string searchPattern)
        {
            var dirs = new Queue<string>();
            dirs.Enqueue(root);
            while (dirs.Count > 0)
            {
                string dir = dirs.Dequeue();

                // files
                string[] paths = null;
                try
                {
                    paths = Directory.GetFiles(dir, searchPattern);
                }
                catch
                {
                } // swallow

                if (paths != null && paths.Length > 0)
                {
                    foreach (string file in paths)
                    {
                        yield return file;
                    }
                }

                // sub-directories
                paths = null;
                try
                {
                    paths = Directory.GetDirectories(dir);
                }
                catch
                {
                } // swallow

                if (paths != null && paths.Length > 0)
                {
                    foreach (string subDir in paths)
                    {
                        dirs.Enqueue(subDir);
                    }
                }
            }
        }

        public MusicList GetMusicList()
        {
            int _countMusics = 0;

            foreach (string path in _paths)
            {
                if (LoadingMusicNumber != null)
                    //raise event
                    LoadingMusicNumber(_countMusics++);

                if (!musics.Exists(m => m.FullPath == path))
                    musics.Add(new Music(path));
            }
            return musics;
        }
    }
}