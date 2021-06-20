using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TagLib;

namespace TagDraco.Core
{
    class PictureUtils
    {
        public Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            MemoryStream outStream = new MemoryStream();
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapImage));
            enc.Save(outStream);
            Bitmap bmp = new Bitmap(outStream);
            outStream.Flush();
            outStream.Close();
            return bmp;
        }

        public Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            float nPercentW = size.Width / (float)sourceWidth;
            float nPercentH = size.Height / (float)sourceHeight;
            float nPercent;
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }

        public Image IPictureToImage(IPicture p, int size)
        {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                MemoryStream ms = new MemoryStream(p.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);         
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                ms.Flush();
                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Writer] - An error occured while trying to save the tags : {0} {1}", e.Message, e.StackTrace);
                MessageBox.Show("An error occured : " + e.Message+"\nThe program will continue, but the image won't be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            
            Bitmap b = BitmapImage2Bitmap(bitmap);
            
            return ResizeImage(b, new Size(size, size));
        }
    }
}
