using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TagLib;

namespace TagDraco.Core
{
    class PictureUtils
    {
        public Image ResizeImage(Image imgToResize, int sizeW, int sizeH)
        {
            if (imgToResize == null) return null;
            float nPercentW = sizeW / (float)imgToResize.Width;
            float nPercentH = sizeH / (float)imgToResize.Height;
            float nPercent;
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            int destWidth = (int)(imgToResize.Width * nPercent);
            int destHeight = (int)(imgToResize.Height * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            //imgToResize.Dispose();
            return b;
        }

        public Bitmap ResizeImage2(Image image, int width, int height)
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

        public Image IPictureToImage(IPicture p, int size)
        {
            try
            {
                MemoryStream ms = new MemoryStream(p.Data.Data);
                
                Image img = ResizeImage(Image.FromStream(ms), size, size);
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
