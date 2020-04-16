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
        const string VERSION = "a1.2.0";
        const string ABOUT_STRING = "TagDraco " + VERSION + " developped by Dreregon.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
        Reader reader = new Reader();
        Writer writer = new Writer();
        Tag fileTags;
        Bitmap cover;
        public static Dictionary<int, Tag> metaData = new Dictionary<int, Tag>();
       
        public TagDraco()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metaData.Clear();
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == null) return;
            LoadMetaData(-1);
        }

        private void saveMetadataBtnPressed(object sender, EventArgs e)
        {
            if (CheckFile()) return;

            int index = 0;
            ListView.SelectedIndexCollection indexes =
                this.listView.SelectedIndices;
            foreach (int index1 in indexes)
            {
                index = index1;
            }

            if (!writer.SaveMetadataToFile(TagLib.File.Create(listView.SelectedItems[0].SubItems[listView.SelectedItems[0].SubItems.Count-1].Text), titleBox.Text, albumBox.Text, artistBox.Text.Split(COMA),
                    int.Parse(trackBox.Text), int.Parse(yearBox.Text), genreBox.Text.Split(COMA), contArtistsBox.Text.Split(COMA), pictureBox1.Image, index))
            {
                MessageBox.Show("An error occured while saving the data to the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadMetaData(index);
        }

        private void changePicBtnPressed(object sender, EventArgs e)
        {
            if (CheckFile()) return;
            imageBrowser.ShowDialog();
            try
            {
                cover = new Bitmap(imageBrowser.FileName);
            } catch {
                return;
            }  
            pictureBox1.Image = Utils.ResizeImage(cover, new System.Drawing.Size(256,256));
        }

        bool CheckFile()
        {
            return reader.getCurrentFilePath() == null;
        }

        void LoadMetaData(int index2)
        {
            metaData = new Dictionary<int, Tag>();
            listView.Items.Clear();
            Clear();
            GC.Collect();
            int index = 0;
            foreach (String s in openFileDialog1.FileNames)
            {
                Console.WriteLine("Loading file " + s);
                fileTags = reader.GetTagsFromFile(s);
                if (fileTags == null) return;

                metaData.Add(index, fileTags);
                ListViewItem item = new ListViewItem(fileTags.Title);

                item.SubItems.Add(fileTags.Album);

                string alArtists = "";
                foreach (String s1 in fileTags.AlbumArtists)
                {
                    alArtists = alArtists + s1 + COMA;
                }
                item.SubItems.Add(alArtists);

                string perf = "";
                foreach (String s2 in fileTags.Performers)
                {
                    perf = perf + s2 + COMA;
                }
                item.SubItems.Add(perf);

                item.SubItems.Add(""+fileTags.Track);

                string genres = "";
                foreach (String s3 in fileTags.Genres)
                {
                    genres = genres + s3 + COMA;
                }
                item.SubItems.Add(genres);

                item.SubItems.Add(""+fileTags.Year);

                item.SubItems.Add(s);

                listView.Items.Add(item);

                Console.WriteLine("Tags saved in index " + index);
                index++;
                
            }

            listView.Focus();
            if (index2 == -1) { 
                //loadMetadataIntoDetailsBox(metaData[0]);
                listView.Items[0].Focused = true;
                listView.Items[0].Selected = true;
                listView.Items[0].EnsureVisible();
            }
            else { 
                //loadMetadataIntoDetailsBox(metaData[index2]);
                listView.Items[index2].Focused = true;
                listView.Items[index2].Selected = true;
                listView.Items[index2].EnsureVisible();
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
            metaData.Clear();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ABOUT_STRING, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void Clear()
        {
            if(cover != null) cover.Dispose();
            titleBox.Text = "";
            albumBox.Text = "";
            artistBox.Text = "";
            trackBox.Text = "";
            genreBox.Text = "";
            yearBox.Text = "";
            contArtistsBox.Text = "";
            pictureBox1.Image = null;
            reader.SetFile(null);
        }

        private void TagDraco_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes =
            this.listView.SelectedIndices;
            foreach (int index in indexes)
            {
                Clear();
                GC.Collect();
                loadMetadataIntoDetailsBox(metaData[index]);
            }
        }

        private void loadMetadataIntoDetailsBox(Tag tag)
        {
            albumBox.Text = tag.Album;
            foreach (String s in tag.AlbumArtists)
            {
                artistBox.Text = artistBox.Text + s + COMA;
            }
            titleBox.Text = tag.Title;
            yearBox.Text = tag.Year.ToString();
            genreBox.Text = tag.FirstGenre;
            trackBox.Text = tag.Track.ToString();
            foreach (String s in tag.Performers)
            {
                contArtistsBox.Text = contArtistsBox.Text + s + COMA;
            }

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
                pictureBox1.Image = Utils.ResizeImage(b, new System.Drawing.Size(256,256));
                cover = b;
            }
            catch
            {
                cover = null;
            }
        }
    }
}
