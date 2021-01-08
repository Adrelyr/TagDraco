using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TagLib;

namespace TagDraco.Core
{
    class Writer
    {
        public const string TEMP_FILE_NAME = "tagDracoTemp.png";
        private TagLib.File file;

        public Writer(Reader reader)
        {
            file = reader.GetFile();
        }

        ~Writer() {}

        public bool UpdateTags(TagLib.File file, Image cover)
        {
            return SaveMetadataToFile(file, cover);
        }

        private bool SaveMetadataToFile(TagLib.File file, Image cover)
        {
            try {
                if (!cover.Equals(null)) { 
                    string fileName = Path.GetTempPath() + TEMP_FILE_NAME;
                    cover.Save(fileName, ImageFormat.Png);
                    Picture picture = new Picture(fileName);
                    file.Tag.Pictures =new Picture[] { picture };
                    System.IO.File.Delete(fileName);
                }
                file.Save();
                Console.WriteLine("[Writer] - Succesfully saved tags for file {0}",file.Name);
                MessageBox.Show("Tags successfuly updated.", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("[Writer] - An error occured while trying to save the tags : {0} {1}",e.Message, e.StackTrace);
                MessageBox.Show("An error occured : "+e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
