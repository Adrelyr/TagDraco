using System.Drawing;

namespace TagDraco.GUI
{
    partial class MainGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGUI));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkThemeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightThemeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.status = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.updateAlbum = new System.Windows.Forms.Button();
            this.trackBox = new System.Windows.Forms.TextBox();
            this.genreBox = new System.Windows.Forms.TextBox();
            this.yearBox = new System.Windows.Forms.TextBox();
            this.titleBox = new System.Windows.Forms.TextBox();
            this.contArtistsBox = new System.Windows.Forms.TextBox();
            this.artistBox = new System.Windows.Forms.TextBox();
            this.albumBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.changePicBtn = new System.Windows.Forms.Button();
            this.changeTagsBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imageBrowser = new System.Windows.Forms.OpenFileDialog();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.AddExtension = false;
            this.openFileDialog1.Filter = "MP3 files (*.mp3)|*.mp3|Wav files (*.wav)|*.wav";
            this.openFileDialog1.Multiselect = true;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(824, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.fileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileToolStripMenuItem.Image")));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clearToolStripMenuItem.Image")));
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripMenuItem.Image")));
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::TagDraco.Properties.Resources.about__Custom_;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // themeToolStripMenuItem
            // 
            this.themeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.darkThemeMenuItem,
            this.lightThemeMenuItem});
            this.themeToolStripMenuItem.Enabled = false;
            this.themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            this.themeToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.themeToolStripMenuItem.Text = "Theme (To be Redone)";
            // 
            // darkThemeMenuItem
            // 
            this.darkThemeMenuItem.Checked = true;
            this.darkThemeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.darkThemeMenuItem.Name = "darkThemeMenuItem";
            this.darkThemeMenuItem.Size = new System.Drawing.Size(180, 22);
            this.darkThemeMenuItem.Text = "Dark";
            this.darkThemeMenuItem.Click += new System.EventHandler(this.DarkThemeOptionClicked);
            // 
            // lightThemeMenuItem
            // 
            this.lightThemeMenuItem.Name = "lightThemeMenuItem";
            this.lightThemeMenuItem.Size = new System.Drawing.Size(180, 22);
            this.lightThemeMenuItem.Text = "Light";
            this.lightThemeMenuItem.Click += new System.EventHandler(this.LightThemeOptionClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.status);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.updateAlbum);
            this.groupBox1.Controls.Add(this.trackBox);
            this.groupBox1.Controls.Add(this.genreBox);
            this.groupBox1.Controls.Add(this.yearBox);
            this.groupBox1.Controls.Add(this.titleBox);
            this.groupBox1.Controls.Add(this.contArtistsBox);
            this.groupBox1.Controls.Add(this.artistBox);
            this.groupBox1.Controls.Add(this.albumBox);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.changePicBtn);
            this.groupBox1.Controls.Add(this.changeTagsBtn);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 713);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
            // 
            // status
            // 
            this.status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(6, 612);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(46, 13);
            this.status.TabIndex = 17;
            this.status.Text = "Waiting.";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(11, 628);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(254, 19);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 16;
            // 
            // updateAlbum
            // 
            this.updateAlbum.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.updateAlbum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.updateAlbum.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.updateAlbum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateAlbum.ForeColor = System.Drawing.Color.White;
            this.updateAlbum.Location = new System.Drawing.Point(135, 653);
            this.updateAlbum.Name = "updateAlbum";
            this.updateAlbum.Size = new System.Drawing.Size(130, 48);
            this.updateAlbum.TabIndex = 15;
            this.updateAlbum.Text = "Update Album (Ctrl-Shift-S)";
            this.updateAlbum.UseVisualStyleBackColor = false;
            this.updateAlbum.Click += new System.EventHandler(this.updateAlbum_Click);
            // 
            // trackBox
            // 
            this.trackBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.trackBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trackBox.ForeColor = System.Drawing.Color.White;
            this.trackBox.Location = new System.Drawing.Point(10, 270);
            this.trackBox.Name = "trackBox";
            this.trackBox.Size = new System.Drawing.Size(256, 20);
            this.trackBox.TabIndex = 6;
            // 
            // genreBox
            // 
            this.genreBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.genreBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genreBox.ForeColor = System.Drawing.Color.White;
            this.genreBox.Location = new System.Drawing.Point(10, 231);
            this.genreBox.Name = "genreBox";
            this.genreBox.Size = new System.Drawing.Size(256, 20);
            this.genreBox.TabIndex = 5;
            // 
            // yearBox
            // 
            this.yearBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.yearBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yearBox.ForeColor = System.Drawing.Color.White;
            this.yearBox.Location = new System.Drawing.Point(10, 192);
            this.yearBox.Name = "yearBox";
            this.yearBox.Size = new System.Drawing.Size(256, 20);
            this.yearBox.TabIndex = 4;
            // 
            // titleBox
            // 
            this.titleBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.titleBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.titleBox.ForeColor = System.Drawing.Color.White;
            this.titleBox.Location = new System.Drawing.Point(10, 153);
            this.titleBox.Name = "titleBox";
            this.titleBox.Size = new System.Drawing.Size(256, 20);
            this.titleBox.TabIndex = 3;
            // 
            // contArtistsBox
            // 
            this.contArtistsBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.contArtistsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contArtistsBox.ForeColor = System.Drawing.Color.White;
            this.contArtistsBox.Location = new System.Drawing.Point(10, 114);
            this.contArtistsBox.Name = "contArtistsBox";
            this.contArtistsBox.Size = new System.Drawing.Size(256, 20);
            this.contArtistsBox.TabIndex = 2;
            // 
            // artistBox
            // 
            this.artistBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.artistBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.artistBox.ForeColor = System.Drawing.Color.White;
            this.artistBox.Location = new System.Drawing.Point(10, 75);
            this.artistBox.Name = "artistBox";
            this.artistBox.Size = new System.Drawing.Size(256, 20);
            this.artistBox.TabIndex = 1;
            // 
            // albumBox
            // 
            this.albumBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.albumBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.albumBox.ForeColor = System.Drawing.Color.White;
            this.albumBox.Location = new System.Drawing.Point(10, 36);
            this.albumBox.Name = "albumBox";
            this.albumBox.Size = new System.Drawing.Size(256, 20);
            this.albumBox.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.pictureBox1.Location = new System.Drawing.Point(10, 324);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 256);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 308);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Album Cover";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Contributing Artists";
            // 
            // changePicBtn
            // 
            this.changePicBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.changePicBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.changePicBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.changePicBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changePicBtn.ForeColor = System.Drawing.Color.White;
            this.changePicBtn.Location = new System.Drawing.Point(10, 586);
            this.changePicBtn.Name = "changePicBtn";
            this.changePicBtn.Size = new System.Drawing.Size(256, 23);
            this.changePicBtn.TabIndex = 7;
            this.changePicBtn.Text = "Change picture";
            this.changePicBtn.UseVisualStyleBackColor = false;
            this.changePicBtn.Click += new System.EventHandler(this.changePicBtnPressed);
            // 
            // changeTagsBtn
            // 
            this.changeTagsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.changeTagsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.changeTagsBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.changeTagsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changeTagsBtn.ForeColor = System.Drawing.Color.White;
            this.changeTagsBtn.Location = new System.Drawing.Point(10, 653);
            this.changeTagsBtn.Name = "changeTagsBtn";
            this.changeTagsBtn.Size = new System.Drawing.Size(119, 48);
            this.changeTagsBtn.TabIndex = 8;
            this.changeTagsBtn.Text = "Update File\r\n(Ctrl-S)";
            this.changeTagsBtn.UseVisualStyleBackColor = true;
            this.changeTagsBtn.Click += new System.EventHandler(this.saveMetadataBtnPressed);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Track";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Genre";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Year";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Title";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Album Artists";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Album";
            // 
            // imageBrowser
            // 
            this.imageBrowser.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png)|*.jpg; *.jpeg; *.jpe; *.jfif; *" +
    ".png";
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.AutoScroll = true;
            this.mainPanel.AutoScrollMargin = new System.Drawing.Size(10, 0);
            this.mainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.mainPanel.Location = new System.Drawing.Point(294, 33);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10, 10, 20, 10);
            this.mainPanel.Size = new System.Drawing.Size(518, 718);
            this.mainPanel.TabIndex = 5;
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(824, 761);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(840, 800);
            this.Name = "MainGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TagDraco";
            this.Load += new System.EventHandler(this.TagDraco_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainGUI_KeyDown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox genreBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox yearBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox artistBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox albumBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button changePicBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox trackBox;
        private System.Windows.Forms.Button changeTagsBtn;
        private System.Windows.Forms.OpenFileDialog imageBrowser;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox contArtistsBox;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button updateAlbum;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkThemeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightThemeMenuItem;
    }
}

