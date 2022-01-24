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
            titleBox.Text       = string.Empty;
            artistBox.Text      = string.Empty;
            contArtistsBox.Text = string.Empty;
            albumBox.Text       = string.Empty;
            yearBox.Text        = string.Empty;
            trackBox.Text       = string.Empty;
            genreBox.Text       = string.Empty;
            if (coverBox.Image != null)
                coverBox.Image.Dispose();
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
            albumBox.Text       = selectedTrackPanel.tags.Album;
            artistBox.Text      = selectedTrackPanel.tags.GetJoinedArtists();
            contArtistsBox.Text = selectedTrackPanel.tags.GetJoinedContributingArtists();
            titleBox.Text       = selectedTrackPanel.tags.Title == null ? "" : selectedTrackPanel.tags.Title;
            yearBox.Text        = selectedTrackPanel.tags.Year.ToString();
            genreBox.Text       = selectedTrackPanel.tags.GetJoinedGenres();
            trackBox.Text       = selectedTrackPanel.tags.Track.ToString();
            coverBox.Image      = selectedTrackPanel.tags.AlbumCover;
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
        private void OpenFileClicked(object sender, System.EventArgs e)
        {
            Clear(true);
            openFileDialog.ShowDialog(this);
            int fileCount = openFileDialog.FileNames.Count();
            if (fileCount == 0)
            {
                return;
            }
            if(fileCount >= 1000)
            {
                MessageBox.Show(null, "You cannot load more than 99 files!\n(Until I find a way to not make this thing eat so much ram)", "Too much files!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            reader.CreateTagLibFiles(openFileDialog.FileNames.ToList());
            SortFiles();
            openFileDialog.Dispose();
        }

        private void OpenFolderClicked(object sender, EventArgs e)
        {
            Clear(true);
            folderBrowserDialog.ShowDialog(this);
            if (folderBrowserDialog.SelectedPath == string.Empty)
                return;

            string[] allfiles = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.mp3", SearchOption.AllDirectories);
            if(allfiles.Count() >= 1000)
            {
                MessageBox.Show(null, "You cannot load more than 99 files!\n(Until I find a way to not make this thing eat so much ram)", "Too much files!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            reader.CreateTagLibFiles(allfiles.ToList());

            

            SortFiles();
            folderBrowserDialog.Dispose();
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

        #endregion

        
    }
}
