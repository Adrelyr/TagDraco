namespace TagDraco.GUI
{
    partial class AlbumUpdateWin
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
            this.coverBox = new System.Windows.Forms.PictureBox();
            this.changePicBtn = new System.Windows.Forms.Button();
            this.albumNameLabel = new System.Windows.Forms.Label();
            this.albumAuthorLabel = new System.Windows.Forms.Label();
            this.albumYearLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.authorBox = new System.Windows.Forms.TextBox();
            this.yearBox = new System.Windows.Forms.TextBox();
            this.updateAlbum = new System.Windows.Forms.Button();
            this.imageBrowser = new System.Windows.Forms.OpenFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.coverBox)).BeginInit();
            this.SuspendLayout();
            // 
            // coverBox
            // 
            this.coverBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.coverBox.Location = new System.Drawing.Point(12, 12);
            this.coverBox.Name = "coverBox";
            this.coverBox.Size = new System.Drawing.Size(256, 256);
            this.coverBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.coverBox.TabIndex = 8;
            this.coverBox.TabStop = false;
            this.coverBox.WaitOnLoad = true;
            // 
            // changePicBtn
            // 
            this.changePicBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.changePicBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.changePicBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.changePicBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changePicBtn.ForeColor = System.Drawing.Color.White;
            this.changePicBtn.Location = new System.Drawing.Point(12, 274);
            this.changePicBtn.Name = "changePicBtn";
            this.changePicBtn.Size = new System.Drawing.Size(256, 23);
            this.changePicBtn.TabIndex = 9;
            this.changePicBtn.Text = "Change Album cover";
            this.changePicBtn.UseVisualStyleBackColor = false;
            this.changePicBtn.Click += new System.EventHandler(this.changePicBtn_Click);
            // 
            // albumNameLabel
            // 
            this.albumNameLabel.AutoSize = true;
            this.albumNameLabel.ForeColor = System.Drawing.Color.White;
            this.albumNameLabel.Location = new System.Drawing.Point(274, 12);
            this.albumNameLabel.Name = "albumNameLabel";
            this.albumNameLabel.Size = new System.Drawing.Size(67, 13);
            this.albumNameLabel.TabIndex = 11;
            this.albumNameLabel.Text = "Album Name";
            // 
            // albumAuthorLabel
            // 
            this.albumAuthorLabel.AutoSize = true;
            this.albumAuthorLabel.ForeColor = System.Drawing.Color.White;
            this.albumAuthorLabel.Location = new System.Drawing.Point(274, 59);
            this.albumAuthorLabel.Name = "albumAuthorLabel";
            this.albumAuthorLabel.Size = new System.Drawing.Size(81, 13);
            this.albumAuthorLabel.TabIndex = 13;
            this.albumAuthorLabel.Text = "Album Author(s)";
            // 
            // albumYearLabel
            // 
            this.albumYearLabel.AutoSize = true;
            this.albumYearLabel.ForeColor = System.Drawing.Color.White;
            this.albumYearLabel.Location = new System.Drawing.Point(274, 106);
            this.albumYearLabel.Name = "albumYearLabel";
            this.albumYearLabel.Size = new System.Drawing.Size(61, 13);
            this.albumYearLabel.TabIndex = 15;
            this.albumYearLabel.Text = "Album Year";
            // 
            // nameBox
            // 
            this.nameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.nameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nameBox.ForeColor = System.Drawing.Color.White;
            this.nameBox.Location = new System.Drawing.Point(277, 28);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(309, 20);
            this.nameBox.TabIndex = 16;
            // 
            // authorBox
            // 
            this.authorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.authorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.authorBox.ForeColor = System.Drawing.Color.White;
            this.authorBox.Location = new System.Drawing.Point(277, 75);
            this.authorBox.Name = "authorBox";
            this.authorBox.Size = new System.Drawing.Size(309, 20);
            this.authorBox.TabIndex = 17;
            // 
            // yearBox
            // 
            this.yearBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.yearBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yearBox.ForeColor = System.Drawing.Color.White;
            this.yearBox.Location = new System.Drawing.Point(277, 122);
            this.yearBox.Name = "yearBox";
            this.yearBox.Size = new System.Drawing.Size(309, 20);
            this.yearBox.TabIndex = 18;
            // 
            // updateAlbum
            // 
            this.updateAlbum.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.updateAlbum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(24)))));
            this.updateAlbum.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.updateAlbum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateAlbum.ForeColor = System.Drawing.Color.White;
            this.updateAlbum.Location = new System.Drawing.Point(277, 249);
            this.updateAlbum.Name = "updateAlbum";
            this.updateAlbum.Size = new System.Drawing.Size(309, 48);
            this.updateAlbum.TabIndex = 19;
            this.updateAlbum.Text = "Update Album (Enter)";
            this.updateAlbum.UseVisualStyleBackColor = false;
            this.updateAlbum.Click += new System.EventHandler(this.updateAlbum_Click);
            // 
            // imageBrowser
            // 
            this.imageBrowser.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png)|*.jpg; *.jpeg; *.jpe; *.jfif; *" +
    ".png";
            // 
            // progressBar1
            // 
            this.progressBar1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.progressBar1.Location = new System.Drawing.Point(277, 225);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(309, 18);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 20;
            // 
            // AlbumUpdateWin
            // 
            this.AcceptButton = this.updateAlbum;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(598, 309);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.updateAlbum);
            this.Controls.Add(this.yearBox);
            this.Controls.Add(this.authorBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.albumYearLabel);
            this.Controls.Add(this.albumAuthorLabel);
            this.Controls.Add(this.albumNameLabel);
            this.Controls.Add(this.coverBox);
            this.Controls.Add(this.changePicBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AlbumUpdateWin";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Album";
            ((System.ComponentModel.ISupportInitialize)(this.coverBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox coverBox;
        private System.Windows.Forms.Button changePicBtn;
        private System.Windows.Forms.Label albumNameLabel;
        private System.Windows.Forms.Label albumAuthorLabel;
        private System.Windows.Forms.Label albumYearLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox authorBox;
        private System.Windows.Forms.TextBox yearBox;
        private System.Windows.Forms.Button updateAlbum;
        private System.Windows.Forms.OpenFileDialog imageBrowser;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}