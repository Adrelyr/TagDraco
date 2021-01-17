using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TagDraco.GUI;
using TagLib;

namespace TagDraco.Core
{
    class Writer
    {
        public const string TEMP_FILE_NAME = "tagDracoTemp.png";
        private MainGUI mainGUI;

        public Writer(MainGUI mainGUI)
        {
            this.mainGUI = mainGUI;
        }

        ~Writer() {}

        public TagLib.File UpdateFile(TagLib.File file, Image cover, string title, string performers, string albumartists, string album, uint year, uint track, string genres)
        {
            file.Tag.Title = title;
            file.Tag.Performers = performers.Split(',');
            file.Tag.AlbumArtists = albumartists.Split(',');
            file.Tag.Album = album;
            file.Tag.Year = year;
            file.Tag.Track = track;
            file.Tag.Genres = genres.Split(',');
            return SaveMetadataToFile(file, cover);
        }

        public bool UpdateAlbum(Dictionary<int,TagLib.File> tagFile, string albumName, string performers, string artist, Image Cover, uint year, string genre)
        {
            try { 
                foreach(TagLib.File file in tagFile.Values)
                {
                    file.Tag.Album = albumName;
                    file.Tag.Performers = performers.Split(',');
                    file.Tag.AlbumArtists = artist.Split(',');
                    file.Tag.Genres = genre.Split(',');
                    file.Tag.Year = year;
                    SaveMetadataToFile(file, Cover);
                }
                return true;
            }catch(Exception e)
            {
                Console.WriteLine("[Writer] - An error occured while trying to save the tags : {0} {1}", e.Message, e.StackTrace);
                MessageBox.Show("An error occured : " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private TagLib.File SaveMetadataToFile(TagLib.File file, Image cover)
        {
            try {
                if (!cover.Equals(null)) { 
                    string fileName = Path.GetTempPath() + TEMP_FILE_NAME;
                    cover.Save(fileName, ImageFormat.Png);
                    Picture picture = new Picture(fileName);
                    file.Tag.Pictures = new Picture[] { picture };
                    System.IO.File.Delete(fileName);
                }
                file.Save();
                Console.WriteLine("[Writer] - Succesfully saved tags for file {0}",file.Name);
                MessageBox.Show("Tags successfuly updated.", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return file;
            }
            catch(Exception e)
            {
                Console.WriteLine("[Writer] - An error occured while trying to save the tags : {0} {1}",e.Message, e.StackTrace);
                MessageBox.Show("An error occured : "+e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
