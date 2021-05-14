using System;
using System.Drawing;
using System.Windows.Forms;

namespace TagDraco.Core
{
    class TrackPanel : Panel
    {
        readonly Color LIGHTER_BLAY = Color.FromArgb(67, 69, 80);
        readonly Color BLAY = Color.FromArgb(47, 49, 60);
        readonly Color LIGHT_BLAY = Color.FromArgb(57, 59, 70);

        readonly Size MAX_SIZE = new Size(2048, 32);
        readonly Size MIN_SIZE = new Size(256, 32);
        readonly Size IMG_SIZE = new Size(24, 24);

        readonly Padding PADDING = new Padding(10);

        readonly Point IMG_LOCATION = new Point(4, 4); 
        readonly Point LBL_LOCATION = new Point(32, 4);

        readonly AnchorStyles ANCHOR_MASK = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

        public Label Label { get; set; }
        public TrackPanel(string filename, Image cover, string title)
        {
            Anchor = ANCHOR_MASK;
            BackColor = BLAY;
            MinimumSize = MIN_SIZE;
            MaximumSize = MAX_SIZE;
            Padding = PADDING;

            PictureBox coverBox = new PictureBox();
            coverBox.Size = IMG_SIZE;
            coverBox.Image = cover;
            coverBox.Location = IMG_LOCATION;

            Label = new Label
            {
                Text = filename + "  ---  " + title,
                AutoSize = true,
                Location = LBL_LOCATION,
                ForeColor = Color.White
            };

            MouseEventHandler clickHandler = new MouseEventHandler(OnClick);
            EventHandler exitHandler = new EventHandler(OnExit);
            EventHandler hoverHandler = new EventHandler(OnHover);

            Label.MouseEnter += hoverHandler;
            Label.MouseLeave += exitHandler;
            Label.MouseClick += clickHandler;

            Controls.Add(coverBox);
            Controls.Add(Label);
            MouseEnter += hoverHandler;
            MouseLeave += exitHandler;
            MouseClick += clickHandler;
        }

        void OnHover(object sender, EventArgs e)
        {
            BackColor = LIGHT_BLAY;
        }

        void OnExit(object sender, EventArgs e)
        {
            BackColor = BLAY;
        }

        void OnClick(object sender, MouseEventArgs e)
        {
            BackColor = LIGHTER_BLAY;
        }

    }
}
