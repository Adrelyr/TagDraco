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
        public Tag GetTagsFromFile(String filePath)
        {
            TagLib.File tagFile = TagLib.File.Create(filePath);
            currentFilePath = filePath;
            return tagFile.Tag;
        }

        public string getCurrentFilePath()
        {
            return currentFilePath;
        }
    }

    
}
