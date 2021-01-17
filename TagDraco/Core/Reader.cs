using System;
using TagLib;
using TagDraco.GUI;
using System.Collections.Generic;

namespace TagDraco.Core
{
    class Reader
    {
        private MainGUI mainWin;
        public Dictionary<int, TagLib.File> tagFiles { get; }
        private List<File> files;

        public Reader(MainGUI mainGUI)
        {
            mainWin = mainGUI;
            tagFiles = new Dictionary<int, File>();
        }

        public void CreateTagLibFiles(List<string> filePaths){
           files = filePaths.ConvertAll(filePath => TagLib.File.Create(filePath));
        }

        public void UpdateFile(string path, int index)
        {

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
                tagFiles.Add(index, file);
                mainWin.UpdateStatus("Loading Files", index, files.Count);
                index++;
            }
        }

        public void ClearFiles()
        {
            tagFiles.Clear();
        }

        public bool IsEmpty => tagFiles.Count == 0;

        ~Reader(){}
    }
}
