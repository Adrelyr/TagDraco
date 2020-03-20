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
        const string VERSION = "a1.0.1";
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
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == null) return;
            LoadMetaData();
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                return cover = new System.Drawing.Bitmap(outStream);
            }
        }

        private static Image ResizeImage(Image imgToResize, System.Drawing.Size size)
        {

            //Get the image current width
            int sourceWidth = imgToResize.Width;
            //Get the image current height
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired siz
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }


        private void saveMetadataBtnPressed(object sender, EventArgs e)
        {
            if (CheckFile()) return;

            int index = 0;
            ListView.SelectedIndexCollection indexes =
                this.listView1.SelectedIndices;
            foreach (int index1 in indexes)
            {
                index = index1;
            }

            if (!writer.SaveMetadataToFile(TagLib.File.Create(listView1.SelectedItems[0].Text), titleBox.Text, albumBox.Text, artistBox.Text.Split(COMA),
                    int.Parse(trackBox.Text), int.Parse(yearBox.Text), genreBox.Text.Split(COMA), contArtistsBox.Text.Split(COMA), pictureBox1.Image, index))
            {
                MessageBox.Show("An error occured while saving the data to the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            pictureBox1.Image = ResizeImage(cover, new System.Drawing.Size(256,256));
        }

        bool CheckFile()
        {
            return reader.getCurrentFilePath() == null;
        }

        void LoadMetaData()
        {
            metaData = new Dictionary<int, Tag>();
            listView1.Items.Clear();
            Clear();
            GC.Collect();
            int index = 0;
            foreach (String s in openFileDialog1.FileNames)
            {
                Console.WriteLine("Loading file " + s);
                fileTags = reader.GetTagsFromFile(s);
                if (fileTags == null) return;
    
                listView1.Items.Add(new ListViewItem(s));

                metaData.Add(index, fileTags);
                Console.WriteLine("Tags saved in index " + index);
                index++;
                
            }
            loadMetadataIntoDetailsBox(metaData[0]);
            //listView1.
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
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
            this.listView1.SelectedIndices;
            foreach (int index in indexes)
            {
                Clear();
                GC.Collect();
                Console.WriteLine(index);
                Console.WriteLine(metaData[index].Title);
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

                Bitmap b = BitmapImage2Bitmap(bitmap);
                pictureBox1.Image = ResizeImage(b, new System.Drawing.Size(256,256));
                cover = b;
            }
            catch
            {
                cover = null;
            }
        }
    }
}
