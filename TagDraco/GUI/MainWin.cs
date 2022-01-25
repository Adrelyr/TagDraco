using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TagDraco.Core;
using TagLib;


namespace TagDraco.GUI
{
    public partial class MainGUI : Form
    {
        string aboutString;
        Reader reader;
        TrackPanel selectedTrackPanel;
        Image albumCover;
        PictureUtils utils = new PictureUtils();

        public MainGUI()
        {
            InitializeComponent();

            sortComboBox.SelectedIndex = 0;
            aboutString = "TagDraco " + Program.VERSION + " developped by Adrelyr.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
            reader = new Reader();
        }

        void Clear(bool clearLists)
        {
            mainPanel.Controls.Clear();
            mainPanel.RowStyles.Clear();
            mainPanel.VerticalScroll.Value = 0;

            titleBox.Text       = string.Empty;
            artistBox.Text      = string.Empty;
            contArtistsBox.Text = string.Empty;
            albumBox.Text       = string.Empty;
            yearBox.Text        = string.Empty;
            trackBox.Text       = string.Empty;
            genreBox.Text       = string.Empty;
            if (coverBox.Image != null)
            {
                coverBox.Image.Dispose();
                coverBox.Image = null;
            }
                
            if(clearLists)
                reader.ClearFiles();
            progressBar.Value  = 0;
        }

        void PopulateMainPanel()
        {
            status.Text = "Displaying...";
            status.Update();

            int fileCount = reader.tags.Count;

            progressBar.Value = 0;
            progressBar.Maximum = fileCount;

            mainPanel.RowStyles.Clear();
            mainPanel.RowCount = fileCount;

            for (int i = 0; i < fileCount; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle());

                TrackPanel trackPanel = new TrackPanel(i + 1, reader.tags[i]);
                trackPanel.TrackPanelClicked += OnTrackPanelClicked;
                trackPanel.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(trackPanel);

                progressBar.Value = i + 1;
            }

            mainPanel.Refresh();
            status.Text = "Waiting.";
        }

        void PopulateDetailBox()
        {
            albumBox.Text       = selectedTrackPanel.Tags.Album;
            artistBox.Text      = selectedTrackPanel.Tags.GetJoinedArtists();
            contArtistsBox.Text = selectedTrackPanel.Tags.GetJoinedContributingArtists();
            titleBox.Text       = selectedTrackPanel.Tags.Title == null ? "" : selectedTrackPanel.Tags.Title;
            yearBox.Text        = selectedTrackPanel.Tags.Year.ToString();
            genreBox.Text       = selectedTrackPanel.Tags.GetJoinedGenres();
            trackBox.Text       = selectedTrackPanel.Tags.Track.ToString();

            TagLib.File f = TagLib.File.Create(selectedTrackPanel.Tags.FilePath);

            coverBox.Image = utils.IPictureToImage(f.Tag.Pictures[0], 256);
            f.Dispose();
        }

        void SortFiles()
        {
            if (sortComboBox.SelectedIndex == 0)
            {
                reader.SortByTrackNumberAsc();
            }
            else if (sortComboBox.SelectedIndex == 1)
            {
                reader.SortByTrackNumberDesc();
            }

            PopulateMainPanel();
        }

        #region EVENTS
        private void OpenFileClicked(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Clear(true);
                reader.CreateTagLibFiles(openFileDialog.FileNames.ToList());
                SortFiles();
                openFileDialog.Dispose();
            }
            else
            {
                return;
            }
        }

        private void OpenFolderClicked(object sender, EventArgs e)
        {
            if(folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                if(folderBrowserDialog.SelectedPath == string.Empty)
                    return;

                string[] allfiles = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.mp3", SearchOption.AllDirectories);

                Clear(true);
                reader.CreateTagLibFiles(allfiles.ToList());
                SortFiles();
                folderBrowserDialog.Dispose();
            }
            else
            {
                return;
            }
        }

        private void OnTrackPanelClicked(object sender, EventArgs e)
        {
            if(selectedTrackPanel != null)
                selectedTrackPanel.BorderStyle = BorderStyle.None;

            TrackPanel trackPanel = (TrackPanel)sender;
            trackPanel.BorderStyle = BorderStyle.FixedSingle;

            selectedTrackPanel = trackPanel;
            PopulateDetailBox();
        }

        private void mainPanel_Scroll(object sender, ScrollEventArgs e)
        {
            //mainPanel.Refresh();
        }

        private void mainPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            int previousScrollPos = mainPanel.VerticalScroll.Value;
            mainPanel.VerticalScroll.Value = 0;
            for (int i = 0; i < mainPanel.Controls.Count; i++)
            {
                mainPanel.Controls[i].Location = new System.Drawing.Point(10, i * 42);
            }
            mainPanel.VerticalScroll.Value = previousScrollPos;
            mainPanel.Refresh();
        }

        private void ClearMenuClicked(object sender, EventArgs e)
        {
            Clear(true);
        }

        private void SortTypeChanged(object sender, EventArgs e)
        {
            SortFiles();
            mainPanel.Controls.Clear();
            PopulateMainPanel();
        }

        private void ChangeCoverClicked(object sender, EventArgs e)
        {
            imageBrowser.ShowDialog();
            if (imageBrowser.FileNames.Count() == 0)
            {
                return;
            }

            albumCover = Image.FromFile(imageBrowser.FileName);
            coverBox.Image = albumCover;

            Properties.Settings.Default.lastImagePath = Path.GetDirectoryName(imageBrowser.FileName);
            imageBrowser.InitialDirectory = Properties.Settings.Default.lastImagePath;
            Properties.Settings.Default.Save();
        }

        private void UpdateTrack(object sender, EventArgs e)
        {
            Writer writer = new Writer();
            if (writer.UpdateFile(selectedTrackPanel.Tags.FilePath,
                albumCover,
                titleBox.Text,
                contArtistsBox.Text,
                artistBox.Text,
                albumBox.Text,
                Convert.ToUInt32(yearBox.Text),
                Convert.ToUInt32(trackBox.Text),
                genreBox.Text))
            MessageBox.Show("Updated Tags!", "Updated Tags", MessageBoxButtons.OK, MessageBoxIcon.Information);

            selectedTrackPanel.Tags.AlbumCover = utils.ResizeImage(albumCover, 24, 24);
            selectedTrackPanel.Tags.Title = titleBox.Text;
            selectedTrackPanel.Tags.ContributingArtists = contArtistsBox.Text.Split(',');
            selectedTrackPanel.Tags.Artists = artistBox.Text.Split(',');
            selectedTrackPanel.Tags.Year = Convert.ToUInt32(yearBox.Text);
            selectedTrackPanel.Tags.Track = Convert.ToUInt32(trackBox.Text);
            selectedTrackPanel.Tags.Genres = genreBox.Text.Split(',');
        } 
        
        private void UpdateAlbum(object sender, EventArgs e)
        {
            Writer writer = new Writer();
            string[] paths = new string[mainPanel.Controls.Count];
            foreach(Control c in mainPanel.Controls)
            {
                TrackPanel panel = c as TrackPanel;
                paths.Append(panel.Tags.FilePath);
                panel.Tags.AlbumCover = utils.ResizeImage(albumCover, 24, 24);
                panel.Tags.ContributingArtists = contArtistsBox.Text.Split(',');
                panel.Tags.Artists = artistBox.Text.Split(',');
                panel.Tags.Year = Convert.ToUInt32(yearBox.Text);
                panel.Tags.Genres = genreBox.Text.Split(',');
            }

            writer.UpdateAlbum(paths,
                albumBox.Text,
                contArtistsBox.Text,
                artistBox.Text,
                albumCover,
                Convert.ToUInt32(yearBox.Text),
                genreBox.Text);
        }

        #endregion

        private void MainGUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                UpdateTrack(null, null);
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                UpdateAlbum(null, null);
            }
        }

       
    }
}
