﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TagDraco.GUI;
using TagLib;

namespace TagDraco.Core
{
    public class TagWriter
    {
        const string TEMP_FILE_NAME = "tagDracoTemp.png";

        public TagWriter()
        {
        }

        ~TagWriter() {}

        public bool UpdateFile(string path, Image cover, string title, string performers, string albumartists, string album, uint year, uint track, string genres)
        {
            try {
                TagLib.File file = TagLib.File.Create(path);
                file.Tag.Title = title;
                file.Tag.Performers = performers.Split(',');
                file.Tag.AlbumArtists = albumartists.Split(',');
                file.Tag.Album = album;
                file.Tag.Year = year;
                file.Tag.Track = track;
                file.Tag.Genres = genres.Split(',');
                SaveMetadataToFile(file, cover);
                file.Dispose();
                return true;
            }catch(Exception e)
                { 

                Console.WriteLine("[Writer] - An error occured while trying to save the tags : {0} {1}", e.Message, e.StackTrace);
                MessageBox.Show("An error occured : " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
        }

        public bool UpdateAlbum(List<string> paths, string albumName, string performers, Image Cover, ushort year)
        {
            int index = 0;
            try {
                
                foreach (string path in paths)
                {
                    Console.WriteLine("skdoosh");
                    TagLib.File file = TagLib.File.Create(path);
                    file.Tag.Album = albumName;
                    file.Tag.Performers = performers.Split(',');
                    file.Tag.AlbumArtists = performers.Split(',');
                    file.Tag.Year = year;
                    SaveMetadataToFile(file, Cover);
                    index++;
                    file.Dispose();
                }
                return true;
            }catch(Exception e)
            {
                Console.WriteLine("[Writer] - An error occured while trying to save the tags on {2}\n: {0} {1}", e.Message, e.StackTrace, paths[index]);
                MessageBox.Show("An error occured : " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateTrackNumber(string filePath, uint track)
        {
            TagLib.File file = TagLib.File.Create(filePath);
            try
            {
                file.Tag.Track = track;
                file.Save();
                file.Dispose();
                return true;
            }catch(Exception e)
            {
                MessageBox.Show("An error occured:\n" + e.Message);
                return false;
            }
        }

        private void SaveMetadataToFile(TagLib.File file, Image cover)
        {
            try {
                if (cover!=(null)) { 
                    string fileName = Path.GetTempPath() + TEMP_FILE_NAME;
                    cover.Save(fileName, ImageFormat.Png);
                    Picture picture = new Picture(fileName);
                    file.Tag.Pictures = new Picture[] { picture };
                    System.IO.File.Delete(fileName);
                }
                file.Save();
                Console.WriteLine("[Writer] - Succesfully saved tags for file {0}",file.Name);
                file.Dispose();
            }
            catch(Exception e)
            {
                Console.WriteLine("[Writer] - An error occured while trying to save the tags on {2}\n: {0} {1}",e.Message, e.StackTrace, file.Name);
                MessageBox.Show("An error occured : "+e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
