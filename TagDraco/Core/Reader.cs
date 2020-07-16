using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace TagDraco.Core
{
    class Reader
    {
        private string currentFilePath;
        private File tagFile;

        public Reader(String path)
        {
            GetTagsFromFile(path);
        }

        ~Reader(){}

        public void GetTagsFromFile(String filePath)
        {
            try { 
                tagFile = TagLib.File.Create(filePath);
                currentFilePath = filePath;
                Console.WriteLine("[Reader] - Retrieved tags in file {0}", filePath);
            }catch(Exception e){
                Console.WriteLine("[Reader] - An error occured : {0}", e.Message);
            }
        }

        public string GetCurrentFilePath()
        {
            return currentFilePath;
        }

        public Tag GetFileTags()
        {
            return tagFile.Tag;
        }

        public File GetFile()
        {
            return tagFile;
        }
    }
}
