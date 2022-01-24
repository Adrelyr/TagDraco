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
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return b;
        }

        public Image IPictureToImage(IPicture p, int size)
        {
            try
            {
                MemoryStream ms = new MemoryStream(p.Data.Data);
                
                Image img = ResizeImage(Image.FromStream(ms), new Size(size, size));
                ms.Dispose();
                return img;
            }
            catch (Exception e)
            {
                Console.WriteLine("[PictureUtils] - An error occured while trying to convert the image : {0} {1}", e.Message, e.StackTrace);
                //MessageBox.Show("An error occured : " + e.Message+"\nThe program will continue, but the image won't be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
