using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace TagDraco
{
    class Reader
    {
        private string currentFilePath;
        private TagLib.File tagFile;
        public Tag GetTagsFromFile(String filePath)
        {
            tagFile = TagLib.File.Create(filePath);
            currentFilePath = filePath;
            return tagFile.Tag;
        }

        public string getCurrentFilePath()
        {
            return currentFilePath;
        }

        public TagLib.File GetFile()
        {
            return tagFile;
        }

        public void SetFile(TagLib.File newFile)
        {
            tagFile = newFile;
        }

    }
}
