using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TagDraco.Core;

namespace TagDraco.GUI
{
    public partial class AlbumUpdateWin : Form
    {
        private Image _image;
        private List<string> _paths;
        private TagWriter _writer;
        private bool writerSuccess = false;
        public ushort success = 0;
        
        public AlbumUpdateWin(TagWriter tagWriter, List<string> paths)
        {
            InitializeComponent();
            _paths = paths;
            _writer = tagWriter;
        }

        private void changePicBtn_Click(object sender, EventArgs e)
        {
            imageBrowser.InitialDirectory = Properties.Settings.Default.lastImagePath;
            if (imageBrowser.ShowDialog() == DialogResult.OK)
            {
                _image = Image.FromFile(imageBrowser.FileName);
                coverBox.Image = _image;

                Properties.Settings.Default.lastImagePath = Path.GetDirectoryName(imageBrowser.FileName);

                Properties.Settings.Default.Save();
            }
            else
            {
                return;
            }
        }
         
        private void updateAlbum_Click(object sender, EventArgs e)
        {
            string albumName = nameBox.Text;
            string albumArtists = authorBox.Text;
            ushort year;
            try
            {
                year = Convert.ToUInt16(yearBox.Text);
            }
            catch
            {
                year = 0;
            }
            

            if (_writer.UpdateAlbum(_paths, albumName, albumArtists, _image, year))
            {
                success = 1;
                Close();
            }
            else
            {
                success = 2;
            }
                
        }
    }
}
