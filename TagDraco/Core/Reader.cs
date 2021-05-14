using System;
using TagLib;
using TagDraco.GUI;
using System.Collections.Generic;

namespace TagDraco.Core
{
    class Reader
    {
        private MainGUI mainWin;
        public Dictionary<int, string> sortedFilePaths { get; }
        private List<File> files;

        public Reader(MainGUI mainGUI)
        {
            mainWin = mainGUI;
            sortedFilePaths = new Dictionary<int, string>();
        }

        public void CreateTagLibFiles(List<string> filePaths){
           files = filePaths.ConvertAll(filePath => TagLib.File.Create(filePath));
        }

        public void SortByTrackNumber()
        {
            mainWin.InitStatus(files.Count);
            files.Sort(delegate (TagLib.File x, TagLib.File y)
            {
                if (x.Tag.Track < y.Tag.Track)
                    return -1;
                else if (x.Tag.Track == y.Tag.Track)
                    return 0;
                else
                    return 1;
            });

            int index = 0;
            foreach (TagLib.File file in files)
            {
                sortedFilePaths.Add(index, file.Name);
                mainWin.UpdateStatus("Loading Files", index, files.Count);
                index++;
            }
            files.Clear();
        }

        public TagLib.Tag GetTagsFromPath(string path)
        {
            return File.Create(path).Tag;
        }

        public void ClearFiles()
        {
            sortedFilePaths.Clear();
        }

        public bool IsEmpty => sortedFilePaths.Count == 0;

        ~Reader(){}
    }
}
