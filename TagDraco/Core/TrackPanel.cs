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
        readonly string NO_TITLE_STRING = "No Title";
        readonly int TEXT_LABELS_X_OFFSET = 56;
        readonly int PATH_LABEL_Y_OFFSET = 24;
        readonly int TITLE_LABEL_Y_OFFSET = 10;
        readonly string PLAY_IN_PLAYER_MENU_STRING = "Open in music player";
        readonly string REMOVE_FROM_LIST_MENU_STRING = "Remove from list";
        readonly string OPEN_IN_EXPLORER_MENU_STRING = "Show in explorer";
        readonly string EXPLORER = "explorer.exe";

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
            
            CoverBox.Size = IMG_SIZE;
            Image img = pictureUtils.ResizeImage(tags.AlbumCover, IMG_SIZE.Width, IMG_SIZE.Height);
            CoverBox.Image = img;
            CoverBox.Location = IMG_LOCATION;

            int displayedIndex = tagIndex+1;
            Color indexColor;

            if (tags.Track != 0)                                                                                //Choses the color for the track index label. if set in metadata, gray. if not, yellow.
            {
                displayedIndex = (int)tags.Track;
                indexColor = Color.Gray;
            }
            else
            {
                indexColor = Color.Yellow;
            }

            IndexLabel = new Label
            {
                Text = displayedIndex.ToString("D3"),                                                           // D3 used to get the track number formated to 001 instead of 1
                AutoSize = true,
                Location = IDX_LBL_LOCATION,
                ForeColor = indexColor
            };

            Label = new Label
            {
                Text = tags.Title==string.Empty?NO_TITLE_STRING : tags.Title,
                AutoSize = true,
                Location = new Point(TEXT_LABELS_X_OFFSET, TITLE_LABEL_Y_OFFSET),
                ForeColor = tags.Title == string.Empty ? TagDracoColors.DarkBlite : Color.White
            };

            PathLabel = new Label
            {
                Text = tags.FilePath,
                AutoSize = true,
                ForeColor = TagDracoColors.DarkBlite,
                Location = new Point(TEXT_LABELS_X_OFFSET, PATH_LABEL_Y_OFFSET)
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
            MouseEnter += hoverHandler;
            MouseLeave += exitHandler;
            MouseClick += clickHandler;

            Controls.Add(CoverBox);
            Controls.Add(IndexLabel);
            Controls.Add(Label);
            Controls.Add(PathLabel);     

            MenuItem itemPlayInMusicPlayer = new MenuItem()
            {
                Text=PLAY_IN_PLAYER_MENU_STRING,
                
            };
            itemPlayInMusicPlayer.Click += new EventHandler(OnPlayClicked);

            MenuItem itemShowPath = new MenuItem()
            {
                Text = OPEN_IN_EXPLORER_MENU_STRING,
            };
            itemShowPath.Click += new EventHandler(OnPathClicked);

            MenuItem itemRemoveFromList = new MenuItem()
            {
                Text = REMOVE_FROM_LIST_MENU_STRING,
            };
            itemRemoveFromList.Click += new EventHandler(OnRemoveClicked);

            CtxMenu.MenuItems.Add(itemPlayInMusicPlayer);
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
                FileName = EXPLORER
            };

            Process.Start(startInfo);
        }

        private void OnPathClicked(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = "/select,"+ tagManager.GetTagsAtIndex(TagIndex).FilePath,
                FileName = EXPLORER
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
