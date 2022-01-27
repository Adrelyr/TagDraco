using TagLib;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Windows.Forms;

namespace TagDraco.Core
{
    class Reader
    {
        public List<Tags> tags { get; private set; } = new List<Tags>();
        private PictureUtils pictureUtils = new PictureUtils();

        public Reader()
        {
            
        }

        public void CreateTagLibFiles(List<string> filePaths){
            tags.Capacity = filePaths.Count;
            bool didSomeFilesFail = false;
            List<string> failedFiles = null;
            foreach (string path in filePaths)
            {
                File file=null;
                try
                {
                    file = File.Create(path);
                }
                catch (Exception ex)
                {
                    didSomeFilesFail = true;
                    failedFiles.Add(path);
                    continue;
                }
                
                Tags fileTags = new Tags();
                fileTags.Title = file.Tag.Title;
                fileTags.Year = file.Tag.Year;
                fileTags.Album = file.Tag.Album;
                fileTags.Genres = file.Tag.Genres;
                fileTags.Track = file.Tag.Track;
                fileTags.AlbumArtists = file.Tag.AlbumArtists;
                fileTags.TrackArtists = file.Tag.Performers;
                if (file.Tag.Pictures.Length != 0)
                {
                    fileTags.AlbumCover = pictureUtils.IPictureToImage(file.Tag.Pictures[0], 24);
                }
                fileTags.FilePath = path;

                tags.Add(fileTags);
                file.Dispose();
            }
            filePaths.Clear();
            if (didSomeFilesFail)
            {
                string allFailedFiles= string.Empty;
                foreach (string fail in failedFiles)
                {
                    allFailedFiles += fail + "\n";
                }
                MessageBox.Show("Some files failed to load :\n" + allFailedFiles);
            }
        }

        public void SortByTrackNumberAsc()
        {
            tags.Sort(delegate (Tags x, Tags y)
            {
                if (x.Track < y.Track)
                    return -1;
                else if (x.Track == y.Track)
                    return 0;
                else
                    return 1;
            });
        }

        public void SortByTrackNumberDesc()
        {
            tags.Sort(delegate (Tags x, Tags y)
            {
                if (x.Track < y.Track)
                    return 1;
                else if (x.Track == y.Track)
                    return 0;
                else
                    return -1;
            });
        }

        public File GetTagFileFromPath(string path)
        {
            return File.Create(path);
        }

        public void ClearFiles()
        {
            foreach(Tags tag in tags) tag.Dispose();
            tags.Clear(); 
        }

        ~Reader(){}
    }
}
