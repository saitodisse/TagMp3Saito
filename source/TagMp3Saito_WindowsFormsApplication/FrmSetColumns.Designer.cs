namespace TagMp3Saito_WindowsFormsApplication
{
    partial class FrmSetColumns
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
            if (disposing && (components != null)) {
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
            this.checkedListBoxColumns = new System.Windows.Forms.CheckedListBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBoxColumns
            // 
            this.checkedListBoxColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxColumns.FormattingEnabled = true;
            this.checkedListBoxColumns.Items.AddRange(new object[] {
            "Artist",
            "DiscNumber",
            "Album",
            "TrackNumber",
            "Title",
            "Genre",
            "Year",
            "OriginalArtist",
            "Accompaniment_ArtistAlbum",
            "CommentOne",
            "Subtitle",
            "FullPath"});
            this.checkedListBoxColumns.Location = new System.Drawing.Point(12, 17);
            this.checkedListBoxColumns.Name = "checkedListBoxColumns";
            this.checkedListBoxColumns.Size = new System.Drawing.Size(174, 169);
            this.checkedListBoxColumns.TabIndex = 2;
            this.checkedListBoxColumns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkedListBoxColumns_KeyDown);
            // 
            // btnHelp
            // 
            this.btnHelp.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnHelp.Location = new System.Drawing.Point(185, 0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(15, 17);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "?";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // FrmSetColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 204);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.checkedListBoxColumns);
            this.MaximumSize = new System.Drawing.Size(216, 240);
            this.MinimumSize = new System.Drawing.Size(216, 240);
            this.Name = "FrmSetColumns";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Columns";
            this.Load += new System.EventHandler(this.FrmSetColumns_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSetColumns_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxColumns;
        private System.Windows.Forms.Button btnHelp;
    }
}