using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TagDraco.Core;
using System.Linq;
using TagLib;

namespace TagDraco.GUI
{
    public partial class MainGUI : Form
    {
        const short IMG_SIZE = 256;
        const char COMA = ';';
        const string VERSION = "1.2.18";
        const string ABOUT_STRING = "TagDraco " + VERSION + " developped by Adrelyr.\nUsing TagLib-Sharp by https://github.com/mono/taglib-sharp \n";
        private Dictionary<int,Reader> tagMap = new Dictionary<int, Reader>();
        
        private short selectedIndex = 0;

        private readonly Color DARK_BLAY = Color.FromArgb(20, 20, 24);
        private readonly Color BLAY = Color.FromArgb(47, 49, 60);
        private PictureUtils utils = new PictureUtils();

        public MainGUI()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear(true);
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "") return;
            if (openFileDialog1.FileNames.Length == 0) return;
            
            int index = 0;
            progressBar1.Maximum = openFileDialog1.FileNames.Length * 2;
            List<string> files = openFileDialog1.FileNames.ToList();
            List<Reader> tagFiles = files.ConvertAll(filePath => new Reader(filePath));

            tagFiles.Sort(delegate(Reader x, Reader y)
            {
                if (x.tagFile.Tag.Track < y.tagFile.Tag.Track)
                    return -1;
                else if (x.tagFile.Tag.Track == y.tagFile.Tag.Track)
                    return 0;
                else
                    return 1;
            });

            foreach(Reader reader in tagFiles)
            {
                tagMap.Add(index, reader);
                progressBar1.Value += 1;
                status.Text = "Reading file " + index.ToString() + " out of " + openFileDialog1.FileNames.Length.ToString() + "...";
                status.Update();
                index++;
            }

            /*//Probably the worst way to sort by track number but oh well
            while (files.Count>0)
            {
                Reader r = new Reader(files[listIndex]);
                int track = (int)r.tagFile.Tag.Track;
                
                //If the track is 0 (taglib's default for no track number)
                if (track == 0)
                {
                    tagMap.Add(index, r);
                    index++;
                    files.RemoveAt(listIndex);
                    if (listIndex > files.Count - 1)
                        listIndex = 0;
                    progressBar1.Value += 1;
                    status.Text = "Reading file " + index.ToString() + " out of " + openFileDialog1.FileNames.Length.ToString() + "...";
                    status.Update();
                    continue;
                }

                //If the track matches the current index (+1 because tracks start at 1)
                if (track == index + 1)
                {
                    tagMap.Add(index, r);
                    index++;
                    files.RemoveAt(listIndex);
                    if (listIndex > files.Count - 1)
                        listIndex = 0;
                    progressBar1.Value += 1;
                    status.Text = "Reading file " + index.ToString() + " out of " + openFileDialog1.FileNames.Length.ToString() + "...";
                    status.Update();
                    continue;
                }

                listIndex++;
                if (listIndex > files.Count-1)
                    listIndex = 0;
            }*/
           
            /*foreach (string fileName in openFileDialog1.FileNames) { 
                tagMap.Add(index, new Reader(fileName));
                Console.WriteLine("[Main] - Loaded tags from file {0} at index {1}", fileName, index);
                progressBar1.Value += 1;
                status.Text = "Reading file " +index.ToString()+" out of "+openFileDialog1.FileNames.Length.ToString() + "...";
                status.Update();
                index++;
            }*/
            LoadMetaData(0);
        }

        private void saveMetadataBtnPressed(object sender, EventArgs e)
        {
            if (tagMap.Count == 0) return;
            tagMap[selectedIndex].tag.Title = titleBox.Text;
            tagMap[selectedIndex].tag.Performers = contArtistsBox.Text.Split(COMA);
            tagMap[selectedIndex].tag.AlbumArtists = artistBox.Text.Split(COMA);
            tagMap[selectedIndex].tag.Album = albumBox.Text;
            tagMap[selectedIndex].tag.Year = uint.Parse(yearBox.Text);
            tagMap[selectedIndex].tag.Track = uint.Parse(trackBox.Text);
            tagMap[selectedIndex].tag.Genres = genreBox.Text.Split(COMA);
            Writer writer = new Writer(tagMap[selectedIndex]);
            writer.UpdateTags(tagMap[selectedIndex].tagFile, pictureBox1.Image);
            tagMap[selectedIndex].GetTagsFromFile(tagMap[selectedIndex].tagFile.Name);
            ClearTrackPanels();
            LoadMetaData(selectedIndex);
        }

        private void changePicBtnPressed(object sender, EventArgs e)
        {
            imageBrowser.ShowDialog();
            if (imageBrowser.FileName==("")) return;

            Bitmap newBitmap = new Bitmap(imageBrowser.FileName);
            pictureBox1.Image = utils.ResizeImage(newBitmap, new System.Drawing.Size(256, 256));
            newBitmap.Dispose();
        }
        void LoadMetaData(int index)
        { 
            ClearTrackPanels();
            GC.Collect();
            Clear(false);
            int panelYPos=10;
            status.Text = "Displaying " + tagMap.Count.ToString() + " values...";
            status.Refresh();
            status.Update();
            Thread.Sleep(10);

            foreach (Reader read in tagMap.Values)
            {
                IPicture p = null;
                Image finalCover = null;
                if (read.tag.Pictures.Length != 0)
                {
                    p = read.tag.Pictures[0];
                    finalCover = utils.IPictureToImage(p, 24);
                }
                TrackPanel trackPanel = new TrackPanel(read.tagFile.Name, finalCover, read.tag.Title);
                trackPanel.Location = new Point(10, panelYPos);
                trackPanel.Padding = new Padding(10);
                trackPanel.Size = new Size(panel1.Width - 20, 32);

                this.panel1.Controls.Add(trackPanel);
                trackPanel.MouseClick += new MouseEventHandler(this.onTrackPanelClick);
                trackPanel.label.MouseClick += new MouseEventHandler(this.onTrackPanelClick);
                panelYPos += 42;

                if(progressBar1.Value == progressBar1.Maximum)
                {
                    progressBar1.Value = progressBar1.Maximum/2;
                }
                    progressBar1.Value += 1;
            }
            loadMetadataIntoDetailsBox(tagMap[index].tag);
            status.Text = "Done.";
        }

        void onTrackPanelClick(object sender, MouseEventArgs e)
        {
            Control panel = (Control)sender;
            Clear(false);
            if (sender is Label)
            {
                selectedIndex = (short)panel1.Controls.IndexOf(panel.Parent);
            }
            else
            {
                selectedIndex = (short)panel1.Controls.IndexOf(panel);
            }
            loadMetadataIntoDetailsBox(tagMap[selectedIndex].tag);
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
            panel1.Controls.Clear();
        }

        void Clear(bool clearDictionary)
        {
            GC.Collect();
            if (clearDictionary)
            {
                tagMap.Clear();
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
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            imageBrowser.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        private void updateAlbum_Click(object sender, EventArgs e)
        {
            if (tagMap.Count == 0) return;
            foreach(Reader reader in tagMap.Values)
            {
                reader.tag.Title = titleBox.Text;
                reader.tag.Performers = contArtistsBox.Text.Split(COMA);
                reader.tag.AlbumArtists = artistBox.Text.Split(COMA);
                reader.tag.Album = albumBox.Text;
                reader.tag.Year = uint.Parse(yearBox.Text);
                reader.tag.Genres = genreBox.Text.Split(COMA);
                Writer writer = new Writer(reader);
                writer.UpdateTags(reader.tagFile, pictureBox1.Image);
            }
            
            tagMap[selectedIndex].GetTagsFromFile(tagMap[selectedIndex].tagFile.Name);
            ClearTrackPanels();
            LoadMetaData(selectedIndex);
        }
    }
}
