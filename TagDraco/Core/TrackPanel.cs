using System;
using System.Drawing;
using System.Windows.Forms;
using TagDraco.GUI;

namespace TagDraco.Core
{
    class TrackPanel : Panel
    {
        readonly Size MAX_SIZE = new Size(2048, 32);
        readonly Size MIN_SIZE = new Size(256, 32);
        readonly Size IMG_SIZE = new Size(24, 24);

        readonly Padding PADDING = new Padding(10);

        readonly Point IMG_LOCATION = new Point(4, 4); 
        readonly Point LBL_LOCATION = new Point(32, 4);

        readonly AnchorStyles ANCHOR_MASK = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

        readonly ThemeManager theme;

        public Label Label { get; set; }
        public TrackPanel(string filename, Image cover, string title, ThemeManager theme)
        {
            this.theme = theme;

            Anchor = ANCHOR_MASK;
            BackColor = theme.ActiveTheme.Equals(ThemeManager.Theme.Dark)?ThemeManager.Blay:ThemeManager.Blite;
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
                ForeColor = theme.ActiveTheme.Equals(ThemeManager.Theme.Dark) ? Color.White : Color.Black
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

            //Paint += new PaintEventHandler(RePaint);
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
            if (theme.ActiveTheme.Equals(ThemeManager.Theme.Dark))
            {
                BackColor = ThemeManager.LighterBlay;
            }
            else
            {
                BackColor = ThemeManager.DarkerBlite;
            }
        }

    }
}
