using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TagDraco.Core;
using TagLib;

namespace TagDraco.GUI
{
    public partial class MainGUI : Form
    {
        const short IMG_SIZE = 256;
        const char COMA = ';';
        const string VERSION = "1.2.18";
        const string ABOUT_STRING = "TagDraco " + VERSION + " developped by Dreregon.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
        private Dictionary<int,Reader> tagMap = new Dictionary<int, Reader>();
        
        private short selectedIndex = 0;

        private readonly Color DARK_BLAY = Color.FromArgb(20, 20, 24);
        private readonly Color BLAY = Color.FromArgb(47, 49, 60);
        private PictureUtils utils = new PictureUtils();

        public MainGUI()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear(true);
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "") return;
            if (openFileDialog1.FileNames.Length == 0) return;
            
            int index = 0;
            progressBar1.Maximum = openFileDialog1.FileNames.Length * 2;
            foreach (String fileName in openFileDialog1.FileNames) { 
                tagMap.Add(index, new Reader(fileName));
                Console.WriteLine("[Main] - Loaded tags from file {0} at index {1}", fileName, index);
                progressBar1.Value += 1;
                status.Text = "Reading file " +index.ToString()+" out of "+openFileDialog1.FileNames.Length.ToString() + "...";
                status.Update();
                index++;
            }
            LoadMetaData(0);
        }

        private void saveMetadataBtnPressed(object sender, EventArgs e)
        {
            if (tagMap.Count == 0) return;
            tagMap[selectedIndex].GetFileTags().Title = titleBox.Text;
            tagMap[selectedIndex].GetFileTags().Performers = contArtistsBox.Text.Split(COMA);
            tagMap[selectedIndex].GetFileTags().AlbumArtists = artistBox.Text.Split(COMA);
            tagMap[selectedIndex].GetFileTags().Album = albumBox.Text;
            tagMap[selectedIndex].GetFileTags().Year = uint.Parse(yearBox.Text);
            tagMap[selectedIndex].GetFileTags().Track = uint.Parse(trackBox.Text);
            tagMap[selectedIndex].GetFileTags().Genres = genreBox.Text.Split(COMA);
            Writer writer = new Writer(tagMap[selectedIndex]);
            writer.UpdateTags(tagMap[selectedIndex].GetFile(), pictureBox1.Image);
            tagMap[selectedIndex].GetTagsFromFile(tagMap[selectedIndex].GetCurrentFilePath());
            ClearTrackPanels();
            LoadMetaData(selectedIndex);
        }

        private void changePicBtnPressed(object sender, EventArgs e)
        {
            imageBrowser.ShowDialog();
            if (imageBrowser.FileName==("")) return;

            Bitmap newBitmap = new Bitmap(imageBrowser.FileName);
            pictureBox1.Image = utils.ResizeImage(newBitmap, new System.Drawing.Size(256, 256));
            newBitmap.Dispose();
        }
        void LoadMetaData(int index)
        { 
            ClearTrackPanels();
            GC.Collect();
            Clear(false);
            int panelYPos=10;
            status.Text = "Displaying " + tagMap.Count.ToString() + " values...";
            status.Refresh();
            status.Update();
            Thread.Sleep(10);

            foreach (Reader read in tagMap.Values)
            {
                IPicture p = null;
                Image finalCover = null;
                if (read.GetFileTags().Pictures.Length != 0)
                {
                    p = read.GetFileTags().Pictures[0];
                    finalCover = utils.IPictureToImage(p, 24);
                }
                //cover.Dispose();
                TrackPanel trackPanel = new TrackPanel(read.GetFile().Name, finalCover, read.GetFileTags().Title);
                trackPanel.Location = new System.Drawing.Point(10, panelYPos);
                trackPanel.Padding = new Padding(10);
                trackPanel.Size = new Size(panel1.Width - 20, 32);

                this.panel1.Controls.Add(trackPanel);
                trackPanel.MouseClick += new MouseEventHandler(this.onTrackPanelClick);
                trackPanel.label.MouseClick += new MouseEventHandler(this.onTrackPanelClick);
                panelYPos += 42;
                //finalCover.Dispose();
                if(progressBar1.Value == progressBar1.Maximum)
                {
                    progressBar1.Value = progressBar1.Maximum/2;
                }
                
                    progressBar1.Value += 1;
            }
            loadMetadataIntoDetailsBox(tagMap[index].GetFileTags());
            status.Text = "Done.";
        }

        void onTrackPanelClick(object sender, MouseEventArgs e)
        {
            Control panel = (Control)sender;
            Clear(false);
            if (sender is Label)
            {
                selectedIndex = (short)panel1.Controls.IndexOf(panel.Parent);
            }
            else
            {
                selectedIndex = (short)panel1.Controls.IndexOf(panel);
            }
            loadMetadataIntoDetailsBox(tagMap[selectedIndex].GetFileTags());
        }

        private void loadMetadataIntoDetailsBox(Tag tag)
        {
            titleBox.Text = tag.Title;
            albumBox.Text = tag.Album;
            artistBox.Text = tag.JoinedAlbumArtists;
            titleBox.Text = tag.Title;
            yearBox.Text = tag.Year.ToString();
            genreBox.Text = tag.JoinedGenres;
            trackBox.Text = tag.Track.ToString();
            contArtistsBox.Text = tag.JoinedPerformers;

            try
            {
                if (tag.Pictures.Length==0)
                {
                    pictureBox1.Image = null;
                    return;
                } 
                pictureBox1.Image = utils.IPictureToImage(tag.Pictures[0], IMG_SIZE);
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }


        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear(true);
            ClearTrackPanels();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ABOUT_STRING, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void ClearTrackPanels()
        {
            panel1.Controls.Clear();
        }

        void Clear(bool clearDictionary)
        {
            GC.Collect();
            if (clearDictionary)
            {
                tagMap.Clear();
                progressBar1.Value = 0;
            }
            titleBox.Text = "";
            albumBox.Text = "";
            artistBox.Text = "";
            trackBox.Text = "";
            genreBox.Text = "";
            yearBox.Text = "";
            contArtistsBox.Text = "";
            pictureBox1.Image = null;
            status.Text = "Waiting.";
            
        }

        private void TagDraco_Load(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            imageBrowser.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        private void updateAlbum_Click(object sender, EventArgs e)
        {
            if (tagMap.Count == 0) return;
            foreach(Reader reader in tagMap.Values)
            {
                reader.GetFileTags().Title = titleBox.Text;
                reader.GetFileTags().Performers = contArtistsBox.Text.Split(COMA);
                reader.GetFileTags().AlbumArtists = artistBox.Text.Split(COMA);
                reader.GetFileTags().Album = albumBox.Text;
                reader.GetFileTags().Year = uint.Parse(yearBox.Text);
                reader.GetFileTags().Genres = genreBox.Text.Split(COMA);
                Writer writer = new Writer(reader);
                writer.UpdateTags(reader.GetFile(), pictureBox1.Image);
            }
            
            tagMap[selectedIndex].GetTagsFromFile(tagMap[selectedIndex].GetCurrentFilePath());
            ClearTrackPanels();
            LoadMetaData(selectedIndex);
        }
    }
}
