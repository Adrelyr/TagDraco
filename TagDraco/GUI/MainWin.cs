using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TagDraco.Core;
using System.Linq;
using TagLib;
using System.IO;

namespace TagDraco.GUI
{
    public partial class MainGUI : Form
    {
        const short IMG_SIZE = 256;
        const char COMA = ';';
        static string VERSION;
        static string ABOUT_STRING;
        const int TRACKPANEL_Y_OFFSET = 42;

        MouseEventHandler mouseClick; 

        private short selectedIndex = 0;

        private readonly Color DARK_BLAY = Color.FromArgb(20, 20, 24);
        private readonly Color BLAY = Color.FromArgb(47, 49, 60);


        private PictureUtils utils = new PictureUtils();

        private Reader tagReader;

        public MainGUI()
        {
            InitializeComponent();
            tagReader = new Reader(this);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                VERSION = "debug";
            }
            else
            {
                VERSION = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            ABOUT_STRING = "TagDraco " + VERSION + " developped by Adrelyr.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
            
            mouseClick = new MouseEventHandler(onTrackPanelClick);
        }

        public void InitStatus(int total)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = total;
        }

        public void UpdateStatus(string message, int progress, int total)
        {
            progressBar1.Value=progress;
            status.Text = message + " : "+progress+"/"+total;
            status.Update();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear(true);
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "") return;
            if (openFileDialog1.FileNames.Length == 0) return;
            Properties.Settings.Default.lastMusicPath = Path.GetDirectoryName(openFileDialog1.FileName);
            Properties.Settings.Default.Save();
            openFileDialog1.InitialDirectory = Properties.Settings.Default.lastMusicPath;
            List<string> files = openFileDialog1.FileNames.ToList();
            tagReader.CreateTagLibFiles(files);
            tagReader.SortByTrackNumber();

            LoadMetaData(0);
        }

        private void saveMetadataBtnPressed(object sender, EventArgs e)
        {
            if (tagReader.IsEmpty) return;
            Writer writer = new Writer();

            string title = titleBox.Text;
            string performers = contArtistsBox.Text;
            string albumArtists = artistBox.Text;
            string album = albumBox.Text;
            uint year = uint.Parse(yearBox.Text);
            uint track = uint.Parse(trackBox.Text);
            string genres = genreBox.Text;
            
            writer.UpdateFile(tagReader.sortedFilePaths[selectedIndex], pictureBox1.Image, title, performers, albumArtists
                ,album, year, track, genres);
            
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
            Properties.Settings.Default.lastImagePath = Path.GetDirectoryName(imageBrowser.FileName);
            imageBrowser.InitialDirectory = Properties.Settings.Default.lastImagePath;
            Properties.Settings.Default.Save();
        }

        void LoadMetaData(int index)
        { 
            ClearTrackPanels();
            Clear(false);
            int panelYPos=10;
            int totalValues = tagReader.sortedFilePaths.Count;
            Thread.Sleep(10);
            InitStatus(totalValues);

            Dictionary<uint, Image> hashCodes = new Dictionary<uint, Image>();

            foreach (string path in tagReader.sortedFilePaths.Values)
            {
                IPicture iPicture = null;
                Image finalCover = null;
                if (tagReader.GetTagsFromPath(path).Pictures.Length != 0)
                {
                    iPicture = tagReader.GetTagsFromPath(path).Pictures[0];
                    uint hash = iPicture.Data.Checksum;
                    if (!hashCodes.ContainsKey(hash))
                        hashCodes.Add(hash, utils.IPictureToImage(iPicture, 24));
                    finalCover = hashCodes[hash];
                }
                TrackPanel trackPanel = new TrackPanel(path, finalCover, tagReader.GetTagsFromPath(path).Title)
                {
                    Location = new Point(10, panelYPos),
                    Size = new Size(mainPanel.ClientSize.Width - 20, 32)
                };
                mainPanel.Controls.Add(trackPanel);
                
                trackPanel.MouseClick += mouseClick;
                trackPanel.Label.MouseClick += mouseClick;
                panelYPos += TRACKPANEL_Y_OFFSET;

                UpdateStatus("Displaying Files", (panelYPos / 42), totalValues);
            }
            loadMetadataIntoDetailsBox(tagReader.GetTagsFromPath(tagReader.sortedFilePaths[index]));
            status.Text = "Done.";
        }

        void onTrackPanelClick(object sender, MouseEventArgs e)
        {
            GC.Collect();
            Control panel = (Control)sender;
            Clear(false);
            if (sender is Label)
            {
                selectedIndex = (short)mainPanel.Controls.IndexOf(panel.Parent);
            }
            else
            {
                selectedIndex = (short)mainPanel.Controls.IndexOf(panel);
            }
            loadMetadataIntoDetailsBox(tagReader.GetTagsFromPath(tagReader.sortedFilePaths[selectedIndex]));
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
            foreach(Control control in mainPanel.Controls)
            {
                control.Dispose();
            }
            mainPanel.Controls.Clear();
            
        }

        void Clear(bool clearDictionary)
        {
            if (clearDictionary)
            {
                tagReader.ClearFiles();
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
            if (Properties.Settings.Default.lastMusicPath.Equals("none"))
            {
                Properties.Settings.Default.lastMusicPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }

            if (Properties.Settings.Default.lastImagePath.Equals("none"))
            {
                Properties.Settings.Default.lastImagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            openFileDialog1.InitialDirectory = Properties.Settings.Default.lastMusicPath;
            imageBrowser.InitialDirectory = Properties.Settings.Default.lastImagePath;
            Properties.Settings.Default.Save();
        }

        private void updateAlbum_Click(object sender, EventArgs e)
        {
            if(tagReader.IsEmpty) return;
            string performers = contArtistsBox.Text;
            string albumArtists = artistBox.Text;
            string album = albumBox.Text;
            uint year = uint.Parse(yearBox.Text);
            string genres = genreBox.Text;
            Image cover = pictureBox1.Image;

            Writer tagWriter = new Writer();

            tagWriter.UpdateAlbum(tagReader.sortedFilePaths, album, performers, albumArtists, cover, year, genres);

            ClearTrackPanels();
            LoadMetaData(selectedIndex);
        }
    }
}
