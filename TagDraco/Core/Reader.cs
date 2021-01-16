using System;
using TagLib;

namespace TagDraco.Core
{
    class Reader
    {
        private string currentFilePath;
        public File tagFile { get; set; }
        public Tag tag => tagFile.Tag; 

        public Reader(string path)
        {
            GetTagsFromFile(path);
        }

        ~Reader(){}

        public void GetTagsFromFile(String filePath)
        {
            try { 
                tagFile = TagLib.File.Create(filePath);
            }catch(Exception e){
                Console.WriteLine("[Reader] - An error occured : {0}", e.Message);
            }
        }
    }
}
