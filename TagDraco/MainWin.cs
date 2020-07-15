using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using TagLib;
using System.Windows.Media.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TagDraco
{
    public partial class TagDraco : Form
    {
        const char COMA = ',';
        const string VERSION = "1.0.0";
        const string ABOUT_STRING = "TagDraco " + VERSION + " developped by Dreregon.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
        private Dictionary<int,Reader> tagMap = new Dictionary<int, Reader>();

        private short selectedIndex = 0;

        private Color DARK_BLAY = Color.FromArgb(20, 20, 24);
        private Color BLAY = Color.FromArgb(47, 49, 60);

        public TagDraco()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "") return;
            if (openFileDialog1.FileNames.Length == 0) return;
            int index = 0;
            foreach(String fileName in openFileDialog1.FileNames) { 
                tagMap.Add(index, new Reader(fileName));
                Console.WriteLine("[Main] - Loaded tags from file {0} at index {1}", fileName, index);
                index++;
            }
            LoadMetaData(0);
        }

        private void saveMetadataBtnPressed(object sender, EventArgs e)
        {
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
            pictureBox1.Image = Utils.ResizeImage(newBitmap, new System.Drawing.Size(256, 256));
            newBitmap.Dispose();

        }
        void LoadMetaData(int index)
        { 
            ClearTrackPanels();
            GC.Collect();
            Clear(false);
           
            int panelYPos = 10;
            foreach(Reader read in tagMap.Values)
            {
                IPicture p = null;
                Image finalCover = null;
                if (read.GetFileTags().Pictures.Length != 0) { 
                    p = read.GetFileTags().Pictures[0];
                    MemoryStream ms = new MemoryStream(p.Data.Data);
                    ms.Seek(0, SeekOrigin.Begin);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    Bitmap cover = Utils.BitmapImage2Bitmap(bitmap);
                    finalCover = Utils.ResizeImage(cover, new System.Drawing.Size(128, 128));
                }
                //cover.Dispose();
                TrackPanel trackPanel = new TrackPanel(
                        read.GetFileTags().Title,
                        read.GetFileTags().Performers,
                        read.GetFileTags().Album,
                        read.GetFileTags().Year,
                        read.GetFileTags().Genres,
                        read.GetFileTags().Track,
                        finalCover,
                        read.GetFile().Name
                    );
                trackPanel.Location = new System.Drawing.Point(10, panelYPos);
                trackPanel.Click += new EventHandler(onTrackPanelClicked);
                trackPanel.MouseHover += new EventHandler(onTrackPanelHovered);
                trackPanel.MouseLeave += new EventHandler(onTrackPanelExited);

                this.panel1.Controls.Add(trackPanel);
                panelYPos += 148;
                //finalCover.Dispose();
            }
            loadMetadataIntoDetailsBox(tagMap[index].GetFileTags());
        }

        private void loadMetadataIntoDetailsBox(Tag tag)
        {
            titleBox.Text = tag.Title;
            albumBox.Text = tag.Album;

            foreach (String s in tag.AlbumArtists)
                artistBox.Text = artistBox.Text + s + COMA;

            titleBox.Text = tag.Title;
            yearBox.Text = tag.Year.ToString();

            foreach (String s in tag.Genres)
                genreBox.Text = genreBox.Text + s + COMA;

            trackBox.Text = tag.Track.ToString();

            foreach (String s in tag.Performers)
                contArtistsBox.Text = contArtistsBox.Text + s + COMA;

            try
            {
                IPicture p = tag.Pictures[0];
                MemoryStream ms = new MemoryStream(p.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                Bitmap b = Utils.BitmapImage2Bitmap(bitmap);
                pictureBox1.Image = Utils.ResizeImage(b, new System.Drawing.Size(256, 256));
                //b.Dispose();
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }

        private void onTrackPanelClicked(object sender, EventArgs e)
        {
            TrackPanel p = (TrackPanel)sender;
            p.BackColor = BLAY;
            Clear(false);
            selectedIndex = (short)panel1.Controls.IndexOf(p);
            loadMetadataIntoDetailsBox(tagMap[panel1.Controls.IndexOf(p)].GetFileTags());
        }

        private void onTrackPanelHovered(object sender, EventArgs e)
        {
            TrackPanel p = (TrackPanel)sender;
            p.BackColor = BLAY;
        }

        private void onTrackPanelExited(object sender, EventArgs e)
        {
            TrackPanel p = (TrackPanel)sender;
            p.BackColor = DARK_BLAY;
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

        void Clear(bool clearDictionary)
        {
            GC.Collect();
            if(clearDictionary) tagMap.Clear();
            titleBox.Text = "";
            albumBox.Text = "";
            artistBox.Text = "";
            trackBox.Text = "";
            genreBox.Text = "";
            yearBox.Text = "";
            contArtistsBox.Text = "";
            pictureBox1.Image = null;
        }

        void ClearTrackPanels()
        {
            panel1.Controls.Clear();
        }

        private void TagDraco_Load(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            imageBrowser.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }
    }
}
