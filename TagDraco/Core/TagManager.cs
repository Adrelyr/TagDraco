using TagLib;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Windows.Forms;

namespace TagDraco.Core
{
    class TagManager
    {
        public List<Tag> Tags { get; private set; } = new List<Tag>();
        private PictureUtils pictureUtils = new PictureUtils();

        public TagManager()
        {
            
        }

        /// <summary>
        /// Creates a list of Tag for each of the tags contained in filePaths<br></br>
        /// It creates files from the Taglib library and retrieves the information used by the tools<br></br>
        /// and puts them in the custom Tag class I made. This is for memory usage, since the Taglib class<br></br>
        /// is quite heavy.
        /// </summary>
        /// <param name="filePaths"></param>
        /// 
        public void RetrieveTagsFromFiles(List<string> filePaths){
            Tags.Capacity = filePaths.Count;
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
                
                Tag fileTags = new Tag();
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

                Tags.Add(fileTags);
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
            Tags.Sort(delegate (Tag x, Tag y)
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
            Tags.Sort(delegate (Tag x, Tag y)
            {
                if (x.Track < y.Track)
                    return 1;
                else if (x.Track == y.Track)
                    return 0;
                else
                    return -1;
            });
        }

        public void SortAlphabetically()
        {
            Tags.Sort(delegate (Tag x, Tag y)
            {
                return string.Compare(x.Title, y.Title, true);
            });
        }

        public void SortAlphabeticallyReverse()
        {
            Tags.Sort(delegate (Tag x, Tag y)
            {
                return -string.Compare(x.Title, y.Title, true);
            });
        }

        public Tag GetTagsAtIndex(int index)
        {
            return Tags[index];
        }

        public File GetTagFileFromPath(string path)
        {
            return File.Create(path);
        }

        public void ClearFiles()
        {
            foreach(Tag tag in Tags) tag.Dispose();
            Tags.Clear(); 
        }

        ~TagManager(){}
    }
}
