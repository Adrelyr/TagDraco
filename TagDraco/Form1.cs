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

        private void BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                cover = new System.Drawing.Bitmap(outStream);
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void saveMetadataBtnPressed(object sender, EventArgs e)
        {
            if (CheckFile()) return;

            if(!writer.SaveMetadataToFile(reader.GetFile(), titleBox.Text, albumBox.Text, artistBox.Text.Split(COMA),
                    int.Parse(trackBox.Text), int.Parse(yearBox.Text), genreBox.Text.Split(COMA), contArtistsBox.Text.Split(COMA), pictureBox1.Image))
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
            pictureBox1.Image = ResizeImage(cover, 256, 256);
        }

        bool CheckFile()
        {
            return reader.getCurrentFilePath() == null;
        }

        void LoadMetaData()
        {
            Clear();
            GC.Collect();
            fileTags = reader.GetTagsFromFile(openFileDialog1.FileName);

            albumBox.Text = fileTags.Album;
            foreach (String s in fileTags.AlbumArtists)
            {
                artistBox.Text = artistBox.Text + s + COMA;
            }
            titleBox.Text = fileTags.Title;
            yearBox.Text = fileTags.Year.ToString();
            genreBox.Text = fileTags.FirstGenre;
            trackBox.Text = fileTags.Track.ToString();
            foreach (String s in fileTags.Performers)
            {
                contArtistsBox.Text = contArtistsBox.Text + s + COMA;
            }

            try { 
            IPicture p = fileTags.Pictures[0];
            MemoryStream ms = new MemoryStream(p.Data.Data);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            Bitmap b = BitmapImage2Bitmap(bitmap);
            pictureBox1.Image = ResizeImage(b, 256, 256);
            cover = b;
            }
            catch
            {
                cover = null;
            }
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
    }
}
