using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TagDraco.Core;
using System.Linq;
using TagLib;
using System.IO;

namespace TagDraco.GUI
{
    public partial class MainGUI : Form
    {
        const short IMG_SIZE = 256;
        const char COMA = ';';
        static string VERSION;
        static string ABOUT_STRING;
        const int TRACKPANEL_Y_OFFSET = 42;

        MouseEventHandler mouseClick; 

        short selectedIndex = 0;

        PictureUtils utils = new PictureUtils();
        Reader tagReader;
        ThemeManager themeManager;

        public MainGUI()
        {
            InitializeComponent();
            tagReader = new Reader(this);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                VERSION = "debug";
            }
            else
            {
                VERSION = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            ABOUT_STRING = "TagDraco " + VERSION + " developped by Adrelyr.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
            
            mouseClick = new MouseEventHandler(onTrackPanelClick);
            themeManager = new ThemeManager(this);
            bool theme = Properties.Settings.Default.theme;
            themeManager.ChangeTheme( theme ? ThemeManager.Theme.Dark : ThemeManager.Theme.Light);
            lightThemeMenuItem.Checked = !theme;
            darkThemeMenuItem.Checked = theme;
        }

        public void InitStatus(int total)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = total;
        }

        public void UpdateStatus(string message, int progress, int total)
        {
            progressBar1.Value=progress;
            status.Text = message + " : "+progress+"/"+total;
            status.Update();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear(true);
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "") return;
            if (openFileDialog1.FileNames.Length == 0) return;
            Properties.Settings.Default.lastMusicPath = Path.GetDirectoryName(openFileDialog1.FileName);
            Properties.Settings.Default.Save();
            openFileDialog1.InitialDirectory = Properties.Settings.Default.lastMusicPath;
            List<string> files = openFileDialog1.FileNames.ToList();
            tagReader.CreateTagLibFiles(files);
            tagReader.SortByTrackNumber();

            LoadMetaData(0);
        }

        private void saveMetadataBtnPressed(object sender, EventArgs e)
        {
            if (tagReader.IsEmpty) return;
            Writer writer = new Writer();

            string title = titleBox.Text;
            string performers = contArtistsBox.Text;
            string albumArtists = artistBox.Text;
            string album = albumBox.Text;
            uint year = uint.Parse(yearBox.Text);
            uint track = uint.Parse(trackBox.Text);
            string genres = genreBox.Text;
            
            if(writer.UpdateFile(tagReader.sortedFilePaths[selectedIndex], pictureBox1.Image, title, performers, albumArtists
                ,album, year, track, genres))
            {
                MessageBox.Show("Tags successfuly updated.", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            ClearTrackPanels();
            LoadMetaData(selectedIndex);
        }

        private void changePicBtnPressed(object sender, EventArgs e)
        {
            imageBrowser.ShowDialog();
            if (imageBrowser.FileName==("")) return;

            Image image = Image.FromFile(imageBrowser.FileName);
            pictureBox1.Image = utils.ResizeImage(image, new Size(512, 512));
            image.Dispose();
            Properties.Settings.Default.lastImagePath = Path.GetDirectoryName(imageBrowser.FileName);
            imageBrowser.InitialDirectory = Properties.Settings.Default.lastImagePath;
            Properties.Settings.Default.Save();
        }

        void LoadMetaData(int index)
        { 
            ClearTrackPanels();
            Clear(false);
            int panelYPos=10;
            int totalValues = tagReader.sortedFilePaths.Count;
            Thread.Sleep(10);
            InitStatus(totalValues);

            

            int listIndex=1;
            foreach (string path in tagReader.sortedFilePaths.Values)
            {

                TrackPanel trackPanel = new TrackPanel(themeManager,tagReader.GetTagsFromPath(path), listIndex, path)
                {
                    Location = new Point(10, panelYPos),
                    Size = new Size(mainPanel.ClientSize.Width - 20, 32)
                };
                mainPanel.Controls.Add(trackPanel);
                
                trackPanel.MouseClick += mouseClick;
                trackPanel.Label.MouseClick += mouseClick;
                panelYPos += TRACKPANEL_Y_OFFSET;

                UpdateStatus("Displaying Files", (panelYPos / 42), totalValues);
                trackPanel.Label.Location = new Point(trackPanel.IndexLabel.Size.Width + 32 + 4,10);
                listIndex++;
            }
            loadMetadataIntoDetailsBox(tagReader.GetTagsFromPath(tagReader.sortedFilePaths[index]));
            status.Text = "Done.";
        }

        void onTrackPanelClick(object sender, MouseEventArgs e)
        {
            Control panel = (Control)sender;
            if (e.Button.Equals(MouseButtons.Left))
            {
                GC.Collect();
                
                Clear(false);
                if (sender is Label)
                {
                    selectedIndex = (short)mainPanel.Controls.IndexOf(panel.Parent);
                }
                else
                {
                    selectedIndex = (short)mainPanel.Controls.IndexOf(panel);
                }
                loadMetadataIntoDetailsBox(tagReader.GetTagsFromPath(tagReader.sortedFilePaths[selectedIndex]));
            }
            else if(e.Button.Equals(MouseButtons.Right))
            {
                if (panel.Parent is TrackPanel)
                {
                    TrackPanel trackPanel = (TrackPanel)panel.Parent;
                    trackPanel.ctxMenu.Show(panel, e.Location);
                }
                
            }
            
        }

        private void loadMetadataIntoDetailsBox(Tag tag)
        {
            
            titleBox.Text = tag.Title;
            albumBox.Text = tag.Album;
            artistBox.Text = tag.JoinedAlbumArtists;
            titleBox.Text = tag.Title;
            yearBox.Text = tag.Year.ToString();
            genreBox.Text = tag.JoinedGenres;
            trackBox.Text = tag.Track.ToString();
            contArtistsBox.Text = tag.JoinedPerformers;

            try
            {
                if (tag.Pictures.Length==0)
                {
                    pictureBox1.Image = null;
                    return;
                } 
                pictureBox1.Image = utils.IPictureToImage(tag.Pictures[0], IMG_SIZE);
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }


        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear(true);
            ClearTrackPanels();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ABOUT_STRING, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void ClearTrackPanels()
        {
            foreach(Control control in mainPanel.Controls)
            {
                control.Dispose();
            }
            mainPanel.Controls.Clear();
            
        }

        void Clear(bool clearDictionary)
        {
            if (clearDictionary)
            {
                tagReader.ClearFiles();
                progressBar1.Value = 0;
            }
            titleBox.Text = "";
            albumBox.Text = "";
            artistBox.Text = "";
            trackBox.Text = "";
            genreBox.Text = "";
            yearBox.Text = "";
            contArtistsBox.Text = "";
            pictureBox1.Image = null;
            status.Text = "Waiting.";
        }

        private void TagDraco_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.lastMusicPath.Equals("none"))
            {
                Properties.Settings.Default.lastMusicPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }

            if (Properties.Settings.Default.lastImagePath.Equals("none"))
            {
                Properties.Settings.Default.lastImagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            openFileDialog1.InitialDirectory = Properties.Settings.Default.lastMusicPath;
            imageBrowser.InitialDirectory = Properties.Settings.Default.lastImagePath;
            Properties.Settings.Default.Save();
        }

        private void updateAlbum_Click(object sender, EventArgs e)
        {
            if(tagReader.IsEmpty) return;
            string performers = contArtistsBox.Text;
            string albumArtists = artistBox.Text;
            string album = albumBox.Text;
            uint year = uint.Parse(yearBox.Text);
            string genres = genreBox.Text;
            Image cover = pictureBox1.Image;

            Writer tagWriter = new Writer();

            if(tagWriter.UpdateAlbum(tagReader.sortedFilePaths, album, performers, albumArtists, cover, year, genres))
            {
                MessageBox.Show("Tags successfuly updated.", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearTrackPanels();
            LoadMetaData(selectedIndex);
        }

        private void DarkThemeOptionClicked(object sender, EventArgs e)
        {
            darkThemeMenuItem.Checked = true;
            lightThemeMenuItem.Checked = false;
            themeManager.ChangeTheme(ThemeManager.Theme.Dark);
            Properties.Settings.Default.theme = true;
            Properties.Settings.Default.Save();
        }

        private void LightThemeOptionClicked(object sender, EventArgs e)
        {
            darkThemeMenuItem.Checked = false;
            lightThemeMenuItem.Checked = true;
            themeManager.ChangeTheme(ThemeManager.Theme.Light);
            Properties.Settings.Default.theme = false;
            Properties.Settings.Default.Save();
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            RePaintGroupBox(groupBox1, e.Graphics, themeManager.ActiveTheme.Equals(ThemeManager.Theme.Dark) ? Color.White : Color.Black);
        }


        void RePaintGroupBox(GroupBox box, Graphics g, Color color)
        {
            Brush textBrush = new SolidBrush(color);
            Brush borderBrush = new SolidBrush(color);
            Pen borderPen = new Pen(borderBrush);
            SizeF strSize = g.MeasureString(box.Text, box.Font);
            Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                           box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                           box.ClientRectangle.Width - 1,
                                           box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

            // Clear text and border
            g.Clear(this.BackColor);

            // Draw text
            g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

            // Drawing Border
            //Left
            g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
            //Right
            g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            //Bottom
            g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            //Top1
            g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
            //Top2
            g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
        }

        private void MainGUI_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                saveMetadataBtnPressed(null, null);
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                updateAlbum_Click(null, null);
            }
        }
    }
}
