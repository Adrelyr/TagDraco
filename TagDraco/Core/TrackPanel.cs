using System;
using System.Drawing;
using System.Windows.Forms;

namespace TagDraco.Core
{
    class TrackPanel : Panel
    {
        private readonly Color LIGHTER_BLAY = Color.FromArgb(67, 69, 80);
        private readonly Color BLAY = Color.FromArgb(47, 49, 60);
        private readonly Color LIGHT_BLAY = Color.FromArgb(57, 59, 70);
        public Label label { get; set; }
        public TrackPanel(string filename, Image cover, string title)
        {
            this.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            this.BackColor = BLAY;
            //this.Location = new System.Drawing.Point(4, 4);
            this.Size = new System.Drawing.Size(512, 32);
            this.Padding = new Padding(10);

            PictureBox coverBox = new PictureBox();
            coverBox.Size = new Size(24, 24);
            coverBox.Image = cover;
            coverBox.Location = new Point(4, 4);

            label = new Label();
            label.Text = filename + "  ---  " + title;
            label.AutoSize = true;
            label.Location = new Point(32, 4);
            label.ForeColor = Color.White;

            label.MouseEnter += new EventHandler(this.onHover);
            label.MouseLeave += new EventHandler(this.onExit);
            label.MouseClick += new MouseEventHandler(this.onClick);

            this.Controls.Add(coverBox);
            this.Controls.Add(label);
            this.MouseEnter += new EventHandler(this.onHover);
            this.MouseLeave += new EventHandler(this.onExit);
            this.MouseClick += new MouseEventHandler(this.onClick);

            
        }

        void onHover(object sender, EventArgs e)
        {
            BackColor = LIGHT_BLAY;
        }

        void onExit(object sender, EventArgs e)
        {
            BackColor = BLAY;
        }

        void onClick(object sender, MouseEventArgs e)
        {
            BackColor = LIGHTER_BLAY;
        }

    }
}
