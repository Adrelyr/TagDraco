using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TagDraco.Core
{
    class TrackPanel : Panel
    {
        private PictureBox cover = new PictureBox();

        private Label titleLabel = new Label();
        private Label albumLabel = new Label();
        private Label genreLabel = new Label();
        private Label yearLabel = new Label();
        private Label artistsLabel = new Label();
        private Label trackLabel = new Label();
        private Label composersLabel = new Label();

        private TextBox titleBox=new TextBox();
        private TextBox albumBox = new TextBox();
        private TextBox genreBox = new TextBox();
        private TextBox yearBox = new TextBox();
        private TextBox artistsBox = new TextBox();
        private TextBox trackBox = new TextBox();
        private TextBox composersBox = new TextBox();

        private List<TextBox> textBoxes = new List<TextBox>();
        private List<Label> labels = new List<Label>();

        private const byte COVER_SIZE = 128;

        private const byte COVER_POS_XY = 10;
        private const byte LABELS_X = 148;
        private const short TEXTBOX_X = 256;
        private const byte TITLE_Y = 10;
        private const byte ARTIST_Y = 20;
        private const byte ALBUM_Y = 30;
        private const byte YEAR_Y = 40;
        private const byte GENRE_Y = 50;
        private const byte TRACK_Y = 60;

        private Color DARK_BLAY = Color.FromArgb(20, 20, 24);
        private Color BLAY = Color.FromArgb(47, 49, 60);
        private const char COMA = ',';


        public TrackPanel(Reader reader, Image cover, string file)
        {
            this.cover.Image = cover;
            this.cover.Size = new Size(COVER_SIZE, COVER_SIZE);
            this.cover.Location = new Point(COVER_POS_XY, COVER_POS_XY);
            this.cover.BackColor = Color.FromArgb(10, 10, 14);
            this.cover.Name = "cover";

            textBoxes.Add(titleBox);
            textBoxes.Add(albumBox);
            textBoxes.Add(genreBox);
            textBoxes.Add(yearBox);
            textBoxes.Add(artistsBox);
            textBoxes.Add(trackBox);
            textBoxes.Add(composersBox);

            labels.Add(titleLabel);
            labels.Add(albumLabel);
            labels.Add(genreLabel);
            labels.Add(yearLabel);
            labels.Add(artistsLabel);
            labels.Add(trackLabel);
            labels.Add(composersLabel);

            titleLabel.Text = "Title:";
            albumLabel.Text = "Album:";
            genreLabel.Text = "Genres:";
            yearLabel.Text = "Year:";
            artistsLabel.Text = "Artists:";
            trackLabel.Text = "Track:";
            composersLabel.Text = "Album Artists";

            byte labelLocation = 10;
            foreach(Label label in labels)
            {
                label.ForeColor = Color.White;
                label.Location = new Point(LABELS_X, labelLocation);
                labelLocation += 30;
            }

            byte location = 10;
            foreach (TextBox box in textBoxes)
            {
                box.Size = new Size(256, 20);
                box.Location = new Point(TEXTBOX_X, location);
                box.BackColor = DARK_BLAY;
                box.ForeColor = Color.White;
                box.BorderStyle = BorderStyle.FixedSingle;
                location += 30;
            }
               

            string alArtists = "";
            foreach (String artist in reader.GetFileTags().Performers)
            {
                alArtists += artist + COMA;
            }

            string genre = "";
            foreach (String genre1 in reader.GetFileTags().Genres)
            {
                genre += genre1 + COMA;
            }

            string composers = "";
            foreach (String composer1 in reader.GetFileTags().AlbumArtists)
            {
                composers += composer1 + COMA;
            }

            titleBox.Text = reader.GetFileTags().Title;
            albumBox.Text = reader.GetFileTags().Album;
            genreBox.Text = genre;
            yearBox.Text = reader.GetFileTags().Year.ToString();
            artistsBox.Text = alArtists;
            trackBox.Text = reader.GetFileTags().Track.ToString();
            composersBox.Text = composers;

            Controls.Add(titleLabel);
            Controls.Add(albumLabel);
            Controls.Add(genreLabel);
            Controls.Add(yearLabel);
            Controls.Add(artistsLabel);
            Controls.Add(trackLabel);
            Controls.Add(composersLabel);

            Controls.Add(titleBox);
            Controls.Add(albumBox);
            Controls.Add(genreBox);
            Controls.Add(yearBox);
            Controls.Add(artistsBox);
            Controls.Add(trackBox);
            Controls.Add(composersBox);

            Controls.Add(this.cover);
            
            AutoSize = true;
            BackColor = BLAY;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
