using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TagDraco.GUI;
using TagLib;

namespace TagDraco.Core
{
    class TrackPanel : Panel
    {
        readonly Size MAX_SIZE = new Size(2048, 32);
        readonly Size MIN_SIZE = new Size(256, 32);
        readonly Size IMG_SIZE = new Size(24, 24);

        readonly Padding PADDING = new Padding(10);

        readonly Point IMG_LOCATION = new Point(4, 4); 
        readonly Point IDX_LBL_LOCATION = new Point(32, 10);
        //readonly Point LBL_LOCATION = new Point(32, 4);

        readonly AnchorStyles ANCHOR_MASK = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
        readonly ThemeManager theme;

        public Label Label { get; set; }
        public Label IndexLabel { get; set; }

        public ContextMenu ctxMenu { get; } = new ContextMenu();
        

        static PictureUtils utils = new PictureUtils();

        Tag tags;
        string path;

        public TrackPanel(ThemeManager theme, TagLib.Tag tags, int index, string path)
        {
            this.tags = tags;
            this.theme = theme;
            this.path = path;

            Anchor = ANCHOR_MASK;
            BackColor = theme.ActiveTheme.Equals(ThemeManager.Theme.Dark)?ThemeManager.Blay:ThemeManager.Blite;
            MinimumSize = MIN_SIZE;
            MaximumSize = MAX_SIZE;
            Padding = PADDING;

            Dictionary<uint, Image> hashCodes = new Dictionary<uint, Image>();

            IPicture iPicture = null;
            Image cover = null;
            if (tags.Pictures.Length != 0)
            {
                iPicture = tags.Pictures[0];
                uint hash = iPicture.Data.Checksum;
                if (!hashCodes.ContainsKey(hash))
                    hashCodes.Add(hash, utils.IPictureToImage(iPicture, 24));
                cover = hashCodes[hash];
            }

            PictureBox coverBox = new PictureBox();
            coverBox.Size = IMG_SIZE;
            coverBox.Image = cover;
            coverBox.Location = IMG_LOCATION;

            if(tags.Track != 0)
            {
                index = (int)tags.Track;
            }

            IndexLabel = new Label
            {
                Text = index.ToString("###"),
                AutoSize = true,
                Location = IDX_LBL_LOCATION,
                ForeColor = Color.Gray
            };

            Label = new Label
            {
                Text = tags.Title==null?"No Title" : tags.Title,
                AutoSize = true,
                Location = new Point(40, 10),
                ForeColor = tags.Title == null ? Color.Gray : theme.ActiveTheme.Equals(ThemeManager.Theme.Dark) ? Color.White : Color.Black
            };

            MouseEventHandler clickHandler = new MouseEventHandler(OnClick);
            EventHandler exitHandler = new EventHandler(OnExit);
            EventHandler hoverHandler = new EventHandler(OnHover);

            Label.MouseEnter += hoverHandler;
            Label.MouseLeave += exitHandler;
            Label.MouseClick += clickHandler;

            Controls.Add(coverBox);
            Controls.Add(IndexLabel);
            Controls.Add(Label);
            MouseEnter += hoverHandler;
            MouseLeave += exitHandler;
            MouseClick += clickHandler;

            MenuItem itemPlayInMP = new MenuItem()
            {
                Text="Open in music player",
                
            };
            itemPlayInMP.Click += new EventHandler(OnPlayClicked);

            MenuItem itemShowPath = new MenuItem()
            {
                Text = "Show in explorer",

            };
            itemShowPath.Click += new EventHandler(OnPathClicked);

            ctxMenu.MenuItems.Add(itemPlayInMP);
            ctxMenu.MenuItems.Add(itemShowPath);

            //Paint += new PaintEventHandler(RePaint);
        }

        private void OnPlayClicked(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = path,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }

        private void OnPathClicked(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = "/select,"+path,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }

        private void RePaint(object sender, PaintEventArgs e)
        {
            Label.ForeColor = theme.ActiveTheme.Equals(ThemeManager.Theme.Dark) ? Color.White : Color.Black;
            BackColor = theme.ActiveTheme.Equals(ThemeManager.Theme.Dark) ? ThemeManager.Blay : ThemeManager.Blite;
            //Refresh();
        }

        void OnHover(object sender, EventArgs e)
        {
            if (theme.ActiveTheme.Equals(ThemeManager.Theme.Dark)){
                BackColor = ThemeManager.LightBlay;
            }
            else
            {
                BackColor = ThemeManager.LighterBlite;
            }
           
        }

        void OnExit(object sender, EventArgs e)
        {
            if (theme.ActiveTheme.Equals(ThemeManager.Theme.Dark))
            {
                BackColor = ThemeManager.Blay;
            }
            else
            {
                BackColor = ThemeManager.Blite;
            }
        }

        void OnClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left)){
                if (theme.ActiveTheme.Equals(ThemeManager.Theme.Dark))
                {
                    BackColor = ThemeManager.LighterBlay;
                }
                else
                {
                    BackColor = ThemeManager.DarkerBlite;
                }
            }
            else if(e.Button.Equals(MouseButtons.Right))
            {
                ctxMenu.Show(this, e.Location);
            }
            
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
            

        }
    }
}
