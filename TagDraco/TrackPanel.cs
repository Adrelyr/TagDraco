using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TagDraco
{
    class TrackPanel : Panel
    {
        private PictureBox cover = new PictureBox();
        private Label details = new Label();

        private const byte COVER_SIZE = 128;

        private const byte COVER_POS_XY = 10;
        private const byte LABELS_X = 148;
        private const byte TITLE_Y = 10;
        private const byte ARTIST_Y = 20;
        private const byte ALBUM_Y = 30;
        private const byte YEAR_Y = 40;
        private const byte GENRE_Y = 50;
        private const byte TRACK_Y = 60;

        private Color DARK_BLAY = Color.FromArgb(20, 20, 24);
        private Color BLAY = Color.FromArgb(47, 49, 60);

        private const char COMA = ',';

        public TrackPanel(string title, string[] artists, string album, uint year, string[] genres, uint track, Image cover, string file)
        {
            this.cover.Image = cover;
            this.cover.Size = new Size(COVER_SIZE, COVER_SIZE);
            this.cover.Location = new Point(COVER_POS_XY, COVER_POS_XY);
            this.cover.BackColor = Color.FromArgb(10, 10, 14); ;

            this.details.Location = new Point(LABELS_X, TITLE_Y);
            this.details.ForeColor = Color.White;
            this.details.AutoSize = true;

            string alArtists = "";
            foreach (String artist in artists)
            {
                alArtists = alArtists + artist + COMA;
            }

            string genre = "";
            foreach (String genre1 in genres)
            {
                genre = genre + genre1 + COMA;
            }  
            
            this.details.Text = "File: "+file+"\n"+title+"\n"+alArtists+"\n"+album+"\n"+year.ToString()+"\n"+genre+"\n"+track.ToString();

            this.Controls.Add(this.cover);
            this.Controls.Add(this.details);

            this.AutoSize = true;
            this.BackColor = DARK_BLAY;
            this.Cursor = Cursors.Hand;
        }
    }
}
