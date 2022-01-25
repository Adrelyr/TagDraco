using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TagDraco.GUI;
using TagLib;

namespace TagDraco.Core
{
    class TrackPanel : Panel
    {
        readonly Size MAX_SIZE = new Size(2048, 46);
        readonly Size MIN_SIZE = new Size(256, 46);
        readonly Size IMG_SIZE = new Size(24, 24);

        readonly Padding PADDING = new Padding(10);

        readonly Point IMG_LOCATION = new Point(4, 4); 
        readonly Point IDX_LBL_LOCATION = new Point(32, 10);

        readonly AnchorStyles ANCHOR_MASK = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

        public Label Label { get; set; }
        public Label IndexLabel { get; set; }
        public Label PathLabel { get; set; }

        public int Index { get; set; }

        public ContextMenu CtxMenu { get; } = new ContextMenu();

        public EventHandler TrackPanelClicked;

        private PictureUtils pictureUtils { get; set; } = new PictureUtils();

        public Tags Tags { get; private set; }

        public TrackPanel(int index, Tags tags)
        {
            BackColor = TagDracoColors.Blay;

            Index = index;
            Tags = tags;

            Anchor = ANCHOR_MASK;
            MinimumSize = MIN_SIZE;
            MaximumSize = MAX_SIZE;
            Padding = PADDING;

            Dictionary<uint, Image> hashCodes = new Dictionary<uint, Image>();

            PictureBox coverBox = new PictureBox();
            coverBox.Size = IMG_SIZE;
            Image img = pictureUtils.ResizeImage(tags.AlbumCover, IMG_SIZE.Width, IMG_SIZE.Height);
            coverBox.Image = img;
            coverBox.Location = IMG_LOCATION;

            if (tags.Track != 0)
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
                Text = tags.Title==string.Empty?"No Title" : tags.Title,
                AutoSize = true,
                Location = new Point(48, 10),
                ForeColor = tags.Title == string.Empty ? TagDracoColors.DarkBlite : Color.White
            };

            PathLabel = new Label
            {
                Text = tags.FilePath,
                AutoSize = true,
                ForeColor = TagDracoColors.DarkBlite,
                Location = new Point(48, 24)
            };

            MouseEventHandler clickHandler = new MouseEventHandler(OnClick);
            EventHandler exitHandler = new EventHandler(OnExit);
            EventHandler hoverHandler = new EventHandler(OnHover);

            Label.MouseEnter += hoverHandler;
            Label.MouseLeave += exitHandler;
            Label.MouseClick += clickHandler;
            PathLabel.MouseEnter += hoverHandler;
            PathLabel.MouseLeave += exitHandler;
            PathLabel.MouseClick += clickHandler;

            Controls.Add(coverBox);
            Controls.Add(IndexLabel);
            Controls.Add(Label);
            Controls.Add(PathLabel);

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

            MenuItem itemRemoveFromList = new MenuItem()
            {
                Text = "Remove from list",
            };
            itemRemoveFromList.Click += new EventHandler(OnRemoveClicked);

            CtxMenu.MenuItems.Add(itemPlayInMP);
            CtxMenu.MenuItems.Add(itemShowPath);
            CtxMenu.MenuItems.Add(itemRemoveFromList);
        }

        private void OnRemoveClicked(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
        }

        private void OnPlayClicked(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = Tags.FilePath,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }

        private void OnPathClicked(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = "/select,"+Tags.FilePath,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }

        void OnHover(object sender, EventArgs e)
        {
            BackColor = TagDracoColors.LightBlay;
        }

        void OnExit(object sender, EventArgs e)
        {
            BackColor = TagDracoColors.Blay;
        }

        void OnClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left)){
                BackColor = TagDracoColors.LighterBlay;
            }
            else if(e.Button.Equals(MouseButtons.Right))
            {
                CtxMenu.Show(this, e.Location);
            }
            OnTrackPanelClicked(e);
        }

        protected virtual void OnTrackPanelClicked(EventArgs e)
        {
            TrackPanelClicked.Invoke(this, e);
        }
    }
}
