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
        Reader reader = new Reader();
        Bitmap cover;
        const char COMA = ',';
        const string TEMP_FILE_NAME = "tagDracoTemp.png";
        public TagDraco()
        {
            InitializeComponent();
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            Tag fileTags = reader.GetTagsFromFile(openFileDialog1.FileName);

            albumBox.Text = fileTags.Album;
            foreach(String s in fileTags.AlbumArtists)
            {
                artistBox.Text = artistBox.Text + s + ",";
            }
            titleBox.Text = fileTags.Title;
            yearBox.Text = fileTags.Year.ToString();
            genreBox.Text = fileTags.FirstGenre;
            trackBox.Text = fileTags.Track.ToString();

            IPicture p = fileTags.Pictures[0];
            MemoryStream ms = new MemoryStream(p.Data.Data);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            Bitmap b = BitmapImage2Bitmap(bitmap);
            pictureBox1.Image = ResizeImage(b, 256,256);
            cover = b;
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (!CheckFile()) return;

            TagLib.File tagFile = TagLib.File.Create(reader.getCurrentFilePath());
            Tag tags = tagFile.Tag;

            tags.Title = titleBox.Text;
            tags.Album = albumBox.Text;
            tags.AlbumArtists = artistBox.Text.Split(COMA);
            tags.Track = (uint)int.Parse(trackBox.Text);
            tags.Genres = genreBox.Text.Split(COMA);
            tags.Year = (uint)int.Parse(yearBox.Text);
            string fileName = System.IO.Path.GetTempPath() + TEMP_FILE_NAME;
            pictureBox1.Image.Save(Path.GetTempPath() + TEMP_FILE_NAME, ImageFormat.Png);
            Picture[] picture = new Picture[1];
            picture[0] = new Picture(fileName);
            tags.Pictures = picture;

            try
            {
                tagFile.Save();
            }
            catch
            {
                MessageBox.Show("An error occured while saving the data to the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void changePicBtnPressed(object sender, EventArgs e)
        {
            if (!CheckFile()) return;
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
    }
}
