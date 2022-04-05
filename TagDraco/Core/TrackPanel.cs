using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TagDraco.GUI;

namespace TagDraco.Core
{
    class TrackPanel : Panel
    {
        readonly Size MAX_SIZE = new Size(4096, 46);
        readonly Size MIN_SIZE = new Size(256, 46);
        readonly Size IMG_SIZE = new Size(24, 24);

        readonly Padding PADDING = new Padding(10);

        readonly Point IMG_LOCATION = new Point(4, 4); 
        readonly Point IDX_LBL_LOCATION = new Point(32, 10);

        readonly AnchorStyles ANCHOR_MASK = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

        public Label Label { get; set; }
        public Label IndexLabel { get; set; }
        public Label PathLabel { get; set; }

        public int TagIndex { get; set; }

        public ContextMenu CtxMenu { get; } = new ContextMenu();

        public EventHandler TrackPanelClicked;

        public PictureBox CoverBox { get; set; } = new PictureBox();

        private PictureUtils pictureUtils { get; set; } = new PictureUtils();
        private TagManager tagManager { get; set; }

        public TrackPanel(int tagIndex, Tag tags, TagManager manager)
        {
            BackColor = TagDracoColors.Blay;

            TagIndex = tagIndex;
            tagManager = manager;

            Anchor = ANCHOR_MASK;
            MinimumSize = MIN_SIZE;
            MaximumSize = MAX_SIZE;
            Padding = PADDING;

            Dictionary<uint, Image> hashCodes = new Dictionary<uint, Image>();
            Image fromTags = tags.AlbumCover;
            
            CoverBox.Size = IMG_SIZE;
            Image img = pictureUtils.ResizeImage(fromTags, IMG_SIZE.Width, IMG_SIZE.Height);
            CoverBox.Image = img;
            CoverBox.Location = IMG_LOCATION;

            int displayedIndex = tagIndex+1;
            Color indexColor;

            if (tags.Track != 0)
            {
                displayedIndex = (int)tags.Track;
                indexColor = Color.Gray;
            }
            else
            {
                indexColor = Color.LightGreen;
            }

            IndexLabel = new Label
            {
                Text = displayedIndex.ToString("###"),
                AutoSize = true,
                Location = IDX_LBL_LOCATION,
                ForeColor = indexColor
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

            Controls.Add(CoverBox);
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

        public void UpdatePicture()
        {
            CoverBox.Image = tagManager.GetTagsAtIndex(TagIndex).AlbumCover;
        }

        private void OnRemoveClicked(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
        }

        private void OnPlayClicked(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = tagManager.GetTagsAtIndex(TagIndex).FilePath,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }

        private void OnPathClicked(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = "/select,"+ tagManager.GetTagsAtIndex(TagIndex).FilePath,
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
