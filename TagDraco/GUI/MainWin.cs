using System;
using System.Collections.Generic;
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
        TagManager tagManager;
        TrackPanel selectedTrackPanel;
        Image albumCover;
        PictureUtils utils = new PictureUtils();

        public MainGUI()
        {
            InitializeComponent();

            sortComboBox.SelectedIndex = 0;
            aboutString = "TagDraco " + Program.VERSION + " developped by Adrelyr.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
            tagManager = new TagManager();
        }

        /// <summary>
        /// Clears everything and resets the tool to its starting state
        /// </summary>
        void Clear(bool clearLists)
        {
            mainPanel.Controls.Clear();
            mainPanel.RowStyles.Clear();
            mainPanel.VerticalScroll.Value = 0;
            mainPanel.VerticalScroll.Maximum = 0;
            mainPanel.HorizontalScroll.Value = 0;
            mainPanel.AutoScroll = false;


            titleBox.Text        = string.Empty;
            artistBox.Text       = string.Empty;
            trackArtistsBox.Text = string.Empty;
            albumBox.Text        = string.Empty;
            yearBox.Text         = string.Empty;
            trackBox.Text        = string.Empty;
            genreBox.Text        = string.Empty;
            if (coverBox.Image != null)
            {
                coverBox.Image.Dispose();
                coverBox.Image = null;
            }
                
            if(clearLists)
                tagManager.ClearFiles();
            progressBar.Value  = 0;

            mainPanel.AutoScroll = true;
        }

        ///<summary>
        ///Populates the main panel with <c>TrackPanel</c> containing 
        ///the MP3 Metadata
        ///</summary>
        void PopulateMainPanel()
        {
            status.Text = "Displaying...";
            status.Update();

            int fileCount = tagManager.Tags.Count;

            progressBar.Value = 0;
            progressBar.Maximum = fileCount;

            mainPanel.RowStyles.Clear();
            mainPanel.RowCount = fileCount;

            List<Control> trackpanels = new List<Control>();

            for (int i = 0; i < fileCount; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle());

                TrackPanel trackPanel = new TrackPanel(i, tagManager.Tags[i], tagManager);
                trackPanel.TrackPanelClicked += OnTrackPanelClicked;
                trackPanel.Dock = DockStyle.Fill;
                trackpanels.Add(trackPanel);

                progressBar.Value = i + 1;
            }

            mainPanel.Controls.AddRange(trackpanels.ToArray());
            mainPanel.Refresh();
            status.Text = "Waiting.";
        }

        ///<summary>
        ///Populates the track details with the tags contained in the tagmanager at the clicked index
        ///</summary>
        void PopulateDetailBox()
        {
            Core.Tag tags = tagManager.GetTagsAtIndex(selectedTrackPanel.TagIndex);

            albumBox.Text        = tags.Album;
            artistBox.Text       = tags.GetJoinedArtists();
            trackArtistsBox.Text = tags.GetJoinedContributingArtists();
            titleBox.Text        = string.IsNullOrEmpty(tags.Title) ? "" : tags.Title;
            yearBox.Text         = tags.Year.ToString();
            genreBox.Text        = tags.GetJoinedGenres();
            trackBox.Text        = tags.Track.ToString();

            TagLib.File f = TagLib.File.Create(tags.FilePath);
            if (f.Tag.Pictures.Length != 0)
            {
                try
                {
                    Image cover = utils.IPictureToImage(f.Tag.Pictures[0], 512);
                    coverBox.Image = cover;
                    albumCover = cover;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            f.Dispose();
            //tags.Dispose();
        }

        /// <summary>
        /// Sorts the list of files by the selected option
        /// </summary>
        void SortFiles()
        {
            if (sortComboBox.SelectedIndex == 0)
            {
                tagManager.SortByTrackNumberAsc();
            }
            else if (sortComboBox.SelectedIndex == 1)
            {
                tagManager.SortByTrackNumberDesc();
            }

            PopulateMainPanel();
        }

        #region EVENTS
        private void OpenFileClicked(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Properties.Settings.Default.lastMusicPath;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Clear(true);
                tagManager.RetrieveTagsFromFiles(openFileDialog.FileNames.ToList());
                SortFiles();

                Properties.Settings.Default.lastMusicPath = Path.GetDirectoryName(openFileDialog.FileNames[0]);
                
                Properties.Settings.Default.Save();

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
                tagManager.RetrieveTagsFromFiles(allfiles.ToList());
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
            if (mainPanel.Controls.Count == 0)
                return;
            SortFiles();
            mainPanel.AutoScroll = false;
            mainPanel.Controls.Clear();
            mainPanel.AutoScroll = true;
            PopulateMainPanel();
        }

        private void ChangeCoverClicked(object sender, EventArgs e)
        {
            imageBrowser.InitialDirectory = Properties.Settings.Default.lastImagePath;
            if (imageBrowser.ShowDialog() == DialogResult.OK)
            {
                albumCover = Image.FromFile(imageBrowser.FileName);
                coverBox.Image = albumCover;

                Properties.Settings.Default.lastImagePath = Path.GetDirectoryName(imageBrowser.FileName);
                
                Properties.Settings.Default.Save();
            }
            else
            {
                return;
            } 
        }

        private void UpdateTrack(object sender, EventArgs e)
        {
            if(mainPanel.Controls.Count == 0 || selectedTrackPanel == null)
                return;

            TagWriter writer = new TagWriter();
            Core.Tag tags = tagManager.GetTagsAtIndex(selectedTrackPanel.TagIndex);
            if (writer.UpdateFile(tags.FilePath,
                albumCover,
                titleBox.Text,
                trackArtistsBox.Text,
                artistBox.Text,
                albumBox.Text,
                Convert.ToUInt32(yearBox.Text),
                Convert.ToUInt32(trackBox.Text),
                genreBox.Text))
            MessageBox.Show("Updated Tags!", "Updated Tags", MessageBoxButtons.OK, MessageBoxIcon.Information);

            tags.AlbumCover   = utils.ResizeImage(albumCover, 24, 24);
            tags.Title        = titleBox.Text;
            tags.TrackArtists = trackArtistsBox.Text.Split(',');
            tags.AlbumArtists = artistBox.Text.Split(',');
            tags.Year         = Convert.ToUInt32(yearBox.Text);
            tags.Track        = Convert.ToUInt32(trackBox.Text);
            tags.Genres       = genreBox.Text.Split(',');

            tagManager.Tags[selectedTrackPanel.TagIndex] = tags;

            selectedTrackPanel.UpdatePicture();
            selectedTrackPanel.Update();
            tags.Dispose();
        } 
        
        private void UpdateAlbum(object sender, EventArgs e)
        {
            if(mainPanel.Controls.Count == 0 || selectedTrackPanel == null)
                return;

            progressBar.Value = 0;
            progressBar.Maximum = mainPanel.Controls.Count;
            status.Text = "Updating Album..";

            TagWriter writer = new TagWriter();
            string[] paths = new string[mainPanel.Controls.Count];
            foreach(Control c in mainPanel.Controls)
            {
                
                TrackPanel panel = c as TrackPanel;
                Core.Tag tags = tagManager.GetTagsAtIndex(panel.TagIndex);
                paths.Append(tags.FilePath);
                tags.AlbumCover = utils.ResizeImage(albumCover, 24, 24);
                tags.TrackArtists = trackArtistsBox.Text.Split(',');
                tags.AlbumArtists = artistBox.Text.Split(',');
                tags.Year = Convert.ToUInt32(yearBox.Text);
                tags.Track = Convert.ToUInt32(trackBox.Text);
                tags.Genres = genreBox.Text.Split(',');
                tagManager.Tags[panel.TagIndex] = tags;
                panel.UpdatePicture();
                panel.Update();
                progressBar.Value += 1;
                tags.Dispose();
            }

            writer.UpdateAlbum(paths,
                albumBox.Text,
                trackArtistsBox.Text,
                artistBox.Text,
                albumCover,
                Convert.ToUInt32(yearBox.Text),
                genreBox.Text);
        }

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

        private void ApplyAutoGeneratedTracknNumberToFile(object sender, EventArgs e)
        {
            if (mainPanel.Controls.Count == 0)
                return;

            progressBar.Value = 0;
            progressBar.Maximum = mainPanel.Controls.Count;
            status.Text = "Updating Tracks Number..";

            TagWriter writer = new TagWriter();
            foreach (Control c in mainPanel.Controls)
            {
                TrackPanel panel = c as TrackPanel;
                if(tagManager.GetTagsAtIndex(panel.TagIndex).Track == 0)
                {
                    tagManager.GetTagsAtIndex(panel.TagIndex).Track = (uint)panel.TagIndex + 1;

                    writer.UpdateTrackNumber(tagManager.Tags[panel.TagIndex].FilePath, (uint)panel.TagIndex + 1);
                }
               

                progressBar.Value += 1;
            }
            SortTypeChanged(null, null);
            status.Text = "Waiting.";

        }

        private void trackBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SaveCoverMenuClicked(object sender, EventArgs e)
        {
            if (coverBox.Image == null) return;
            if(saveCoverDialog.ShowDialog() == DialogResult.OK)
            {
                coverBox.Image.Save(saveCoverDialog.FileName);
                MessageBox.Show("Saved Cover!");
            }
            
        }

        #endregion


    }
}
