﻿using System;
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
        #region Const values
        const int ALBUM_COVER_SIZE = 512;
        const byte TRACKPANEL_X = 10;
        const byte TRACKPANEL_Y_MULTIPLICATOR = 42;
        const byte TAG_ALBUM_COVER_SIZE = 24;

        const string STATUS_DISPLAYING_TRACKS_STRING = "Displaying...";
        const string STATUS_WAITING_STRING = "Waiting.";
        const string STATUS_UPDATING_ALBUM = "Updating Album...";
        const string STATUS_APPLY_AUTOGENERATED_TRACKS = "Updating track numbers...";
        const string FILE_CHOOSER_MP3 = "*.mp3";
        const string MESSAGE_UPDATE_TRACK_TITLE = "Updated tags";
        const string MESSAGE_UPDATE_TRACK = "Updated Tags!";
        const string MESSAGE_SAVED_COVER = "Saved Cover!";
        const string MESSAGE_ABOUT_TITLE = "About TagDraco";
        #endregion


        readonly string aboutString;

        TagManager tagManager;
        TrackPanel selectedTrackPanel;
        Image albumCover;
        PictureUtils utils = new PictureUtils();

        public MainGUI()
        {
            InitializeComponent();

            sortComboBox.SelectedIndex = 0;
            aboutString = "TagDraco " + Program.VERSION + " developped by Draconicode.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
            tagManager = new TagManager();
        }

        /// <summary>
        /// Clears everything and resets the tool to its starting state
        /// </summary>
        void Clear(bool clearLists)
        {
            //Main panel control clearing
            mainPanel.Controls.Clear();
            mainPanel.RowStyles.Clear();
            mainPanel.VerticalScroll.Value = 0;
            mainPanel.VerticalScroll.Maximum = 0;
            mainPanel.HorizontalScroll.Value = 0;
            mainPanel.AutoScroll = false;

            //Detail box clearing
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
                
            //Clears the list if clearList is set. Used to clear the list on a "reset" clear but not on a sorting clear
            if(clearLists)
                tagManager.ClearFiles();
            progressBar.Value  = 0;

            //Resets the main panel scrollbar to the top
            mainPanel.AutoScroll = true;
        }

        ///<summary>
        ///Populates the main panel with <c>TrackPanel</c> containing 
        ///the MP3 Metadata
        ///</summary>
        void PopulateMainPanel()
        {
            //Updates the status, sets the max value of the progressBar and clears any rowstyles left in the mainPanel.
            status.Text = STATUS_DISPLAYING_TRACKS_STRING;
            status.Update();

            int fileCount = tagManager.Tags.Count;

            progressBar.Value = 0;
            progressBar.Maximum = fileCount;

            mainPanel.RowStyles.Clear();
            mainPanel.RowCount = fileCount;

            List<Control> trackpanels = new List<Control>();

            //Creating a trackPanel for every track contained in the list of Tags in Tagmanager.
            for (int i = 0; i < fileCount; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle());

                TrackPanel trackPanel = new TrackPanel(i, tagManager.Tags[i], tagManager);
                trackPanel.TrackPanelClicked += OnTrackPanelClicked;
                trackPanel.Dock = DockStyle.Fill;
                trackpanels.Add(trackPanel);

                progressBar.Value = i + 1;
            }

            //adds all the trackpanels controls in one go. (faster than adding it at each loop)
            mainPanel.Controls.AddRange(trackpanels.ToArray());
            mainPanel.Refresh();
            status.Text = STATUS_WAITING_STRING;
        }

        ///<summary>
        ///Populates the track details with the tags contained in the tagmanager at the clicked index
        ///</summary>
        void PopulateDetailBox()
        {
            //Retrieving the tags from the index of the trackPanel
            Core.Tag tags = tagManager.GetTagsAtIndex(selectedTrackPanel.TagIndex);

            //Populating the detail box
            albumBox.Text        = tags.Album;
            artistBox.Text       = tags.GetJoinedArtists();
            trackArtistsBox.Text = tags.GetJoinedContributingArtists();
            titleBox.Text        = string.IsNullOrEmpty(tags.Title) ? "" : tags.Title;
            yearBox.Text         = tags.Year.ToString();
            genreBox.Text        = tags.GetJoinedGenres();
            trackBox.Text        = tags.Track.ToString();

            //Retrieves the album cover directly from the mp3 file. (to avoid having all the non resized images in memory at all times)
            TagLib.File f = TagLib.File.Create(tags.FilePath);
            if (f.Tag.Pictures.Length != 0)
            {
                try
                {
                    Image cover = utils.IPictureToImage(f.Tag.Pictures[0], ALBUM_COVER_SIZE);
                    coverBox.Image = cover;
                    albumCover = cover;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            f.Dispose();
        }

        /// <summary>
        /// Instructs the TagManager to sort the list of files by the selected option
        /// </summary>
        void SortFiles()
        {
            switch (sortComboBox.SelectedIndex)
            {
                case 0: tagManager.SortByTrackNumberAsc();
                    break;
                case 1: tagManager.SortByTrackNumberDesc();
                    break;
                case 2: tagManager.SortAlphabetically();
                    break;
                case 3: tagManager.SortAlphabeticallyReverse();
                    break;
            }

            PopulateMainPanel();
        }

        #region EVENTS
        private void OpenFileClicked(object sender, EventArgs e)
        {
            //Looks at what the last saved path is
            openFileDialog.InitialDirectory = Properties.Settings.Default.lastMusicPath;                          
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                //If files are selected, Reset clear, tag handling and sorting.
                Clear(true);
                tagManager.RetrieveTagsFromFiles(openFileDialog.FileNames.ToList());
                SortFiles();

                //Saves the path of the folder the dialog was in
                Properties.Settings.Default.lastMusicPath = Path.GetDirectoryName(openFileDialog.FileNames[0]);
                
                Properties.Settings.Default.Save();

                //yeet
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

                //We get all files contained in the folder we selected
                string[] allfiles = Directory.GetFiles(folderBrowserDialog.SelectedPath, FILE_CHOOSER_MP3, SearchOption.AllDirectories);

                //Reset clear, tag handling and sorting
                Clear(true);
                tagManager.RetrieveTagsFromFiles(allfiles.ToList());
                SortFiles();

                //yeet
                folderBrowserDialog.Dispose();
            }
            else
            {
                return;
            }
        }

        private void OnTrackPanelClicked(object sender, EventArgs e)
        {
            //resets the fancy border on the previously clicked trackPanel
            if(selectedTrackPanel != null)
                selectedTrackPanel.BorderStyle = BorderStyle.None;

            //Retrieves the selected trackPanel and puts a fancy border around it
            TrackPanel trackPanel = (TrackPanel)sender;
            trackPanel.BorderStyle = BorderStyle.FixedSingle;

            selectedTrackPanel = trackPanel;
            
            //Well I mean the method is self explanatory on what it does
            PopulateDetailBox();
        }

        private void mainPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            //Redraws the contents of the mainPanel when a trackPanel is removed.
            int previousScrollPos = mainPanel.VerticalScroll.Value;
            mainPanel.VerticalScroll.Value = 0;
            for (int i = 0; i < mainPanel.Controls.Count; i++)
            {
                mainPanel.Controls[i].Location = new Point(TRACKPANEL_X, i * TRACKPANEL_Y_MULTIPLICATOR);
            }
            mainPanel.VerticalScroll.Value = previousScrollPos;
            mainPanel.Refresh();
        }

        private void ClearMenuClicked(object sender, EventArgs e)
        {
            //Do I really need to comment on that
            //ah yes, true means its a Reset clear as opposed to a Sort clear
            Clear(true);
        }

        private void SortTypeChanged(object sender, EventArgs e)
        {
            //When a new sorting type is selected we sort, clear the mainPanel and populate it again with the sorted values
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
            //event fired when clicking on the change image cover then saves the current path so it can be opened again on
            //the next click
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
            //do nothing id nothing is loaded
            if(mainPanel.Controls.Count == 0 || selectedTrackPanel == null)
                return;

            //Creates an instance of the TagWriter which then updates the mp3 tags 
            //using what the user put in the detail box
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
            MessageBox.Show(MESSAGE_UPDATE_TRACK, MESSAGE_UPDATE_TRACK_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information); //hey user your stuff has been updated woo

            //Updates the tag itself with the data in the detail box
            tags.AlbumCover   = utils.ResizeImage(albumCover, 24, 24);
            tags.Title        = titleBox.Text;
            tags.TrackArtists = trackArtistsBox.Text.Split(',');
            tags.AlbumArtists = artistBox.Text.Split(',');
            tags.Year         = Convert.ToUInt32(yearBox.Text);
            tags.Track        = Convert.ToUInt32(trackBox.Text);
            tags.Genres       = genreBox.Text.Split(',');

            //Updates the trackPanel withe the new information
            selectedTrackPanel.UpdatePicture();
            selectedTrackPanel.Update();
        } 
        
        private void UpdateAlbum(object sender, EventArgs e)
        {
            //do nothing id nothing is loaded
            if (mainPanel.Controls.Count == 0 || selectedTrackPanel == null)
                return;

            //prepares for updating a few files
            progressBar.Value = 0;
            progressBar.Maximum = mainPanel.Controls.Count;
            status.Text = STATUS_UPDATING_ALBUM;

            //Creates an instance of the TagWriter
            TagWriter writer = new TagWriter();
            string[] paths = new string[mainPanel.Controls.Count];
            foreach(Control c in mainPanel.Controls)
            {
                //Retrieves the track panel and the path contained at its index in the tagList, then retrieves the album info from the detail box
                TrackPanel panel = c as TrackPanel;
                Core.Tag tags = tagManager.GetTagsAtIndex(panel.TagIndex);
                paths.Append(tags.FilePath);
                tags.AlbumCover = utils.ResizeImage(albumCover, TAG_ALBUM_COVER_SIZE, TAG_ALBUM_COVER_SIZE);
                tags.AlbumArtists = artistBox.Text.Split(',');
                tags.Year = Convert.ToUInt32(yearBox.Text);
                tags.Genres = genreBox.Text.Split(',');
                tagManager.Tags[panel.TagIndex] = tags;
                panel.UpdatePicture();
                panel.Update();
                progressBar.Value += 1;
                tags.Dispose();
            }

            //Updates all the tracks loaded with the new album info
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
            //Updates the track with a keystroke instead of clicking the button
            //ctrl-s for single track saving
            if (e.Control && e.KeyCode == Keys.S)
            {
                UpdateTrack(null, null);
            }
            //ctrl-shift-s for the album saving
            else if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                UpdateAlbum(null, null);
            }
        }

        private void ApplyAutoGeneratedTracknNumberToFile(object sender, EventArgs e)
        {
            //do nothing id nothing is loaded
            if (mainPanel.Controls.Count == 0)
                return;

            //updates the status and progressBar
            progressBar.Value = 0;
            progressBar.Maximum = mainPanel.Controls.Count;
            status.Text = STATUS_APPLY_AUTOGENERATED_TRACKS;

            //creates an instance of the TagWriter (yes, again) and updates only the track number.
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
            //Re sorting since we changed the tracks
            SortTypeChanged(null, null);
            status.Text = STATUS_WAITING_STRING;

        }

        private void trackBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Allowing only numbers in the track texbox in the detail box
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SaveCoverMenuClicked(object sender, EventArgs e)
        {
            //Saves the cover in the coverBox PictureBox into a pretty file
            if (coverBox.Image == null) return;
            if(saveCoverDialog.ShowDialog() == DialogResult.OK)
            {
                coverBox.Image.Save(saveCoverDialog.FileName);
                MessageBox.Show(MESSAGE_SAVED_COVER);
            }
            
        }

        private void AboutMenuClicked(object sender, EventArgs e)
        {
            //hello its me
            MessageBox.Show(this, aboutString, MESSAGE_ABOUT_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

       
    }
}
