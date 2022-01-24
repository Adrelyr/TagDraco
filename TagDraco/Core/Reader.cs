using System;
using TagLib;
using TagDraco.GUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

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
            foreach (string path in filePaths)
            {
                File file = File.Create(path);
                tags.Add(new Tags(file.Tag.Album, file.Tag.Title, file.Tag.AlbumArtists, file.Tag.Performers, file.Tag.Year, file.Tag.Track, file.Tag.Genres, pictureUtils.IPictureToImage(file.Tag.Pictures[0], 512), path));
                file.Dispose();
            }
            filePaths.Clear();
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
            tags.Clear(); 
        }

        ~Reader(){}
    }
}
