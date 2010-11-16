namespace TagMp3Saito_WindowsFormsApplication
{
    partial class FrmTagMp3Saito
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
            this.btnDrop = new System.Windows.Forms.Button();
            this.btn_Load_Csv_And_Save_Mp3 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConfig = new System.Windows.Forms.Button();
            this.textBox_CSV_FilePath = new System.Windows.Forms.TextBox();
            this.buttonOpenCsvFile = new System.Windows.Forms.Button();
            this.openFileDialog_CsvFile = new System.Windows.Forms.OpenFileDialog();
            this.buttonOpenExcel = new System.Windows.Forms.Button();
            this.buttonOpenTxt = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDrop
            // 
            this.btnDrop.AllowDrop = true;
            this.btnDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDrop.Location = new System.Drawing.Point(38, 12);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(225, 63);
            this.btnDrop.TabIndex = 1;
            this.btnDrop.Text = "Drop mp3 files/folders here";
            this.btnDrop.UseVisualStyleBackColor = true;
            this.btnDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnDrop_DragDrop);
            this.btnDrop.DragOver += new System.Windows.Forms.DragEventHandler(this.btnDrop_DragOver);
            // 
            // btn_Load_Csv_And_Save_Mp3
            // 
            this.btn_Load_Csv_And_Save_Mp3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Load_Csv_And_Save_Mp3.Enabled = false;
            this.btn_Load_Csv_And_Save_Mp3.Location = new System.Drawing.Point(33, 115);
            this.btn_Load_Csv_And_Save_Mp3.Name = "btn_Load_Csv_And_Save_Mp3";
            this.btn_Load_Csv_And_Save_Mp3.Size = new System.Drawing.Size(281, 35);
            this.btn_Load_Csv_And_Save_Mp3.TabIndex = 1;
            this.btn_Load_Csv_And_Save_Mp3.Text = "Close and save the excel file.\r\n...then click here";
            this.btn_Load_Csv_And_Save_Mp3.UseVisualStyleBackColor = true;
            this.btn_Load_Csv_And_Save_Mp3.Click += new System.EventHandler(this.btn_Load_Csv_And_Save_Mp3_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 159);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(356, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(0, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "1.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(0, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "2.";
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfig.Image = global::TagMp3Saito_WindowsFormsApplication.Properties.Resources.engine;
            this.btnConfig.Location = new System.Drawing.Point(320, 119);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(24, 26);
            this.btnConfig.TabIndex = 5;
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // textBox_CSV_FilePath
            // 
            this.textBox_CSV_FilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_CSV_FilePath.BackColor = System.Drawing.Color.White;
            this.textBox_CSV_FilePath.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textBox_CSV_FilePath.Location = new System.Drawing.Point(35, 83);
            this.textBox_CSV_FilePath.Name = "textBox_CSV_FilePath";
            this.textBox_CSV_FilePath.Size = new System.Drawing.Size(280, 20);
            this.textBox_CSV_FilePath.TabIndex = 6;
            // 
            // buttonOpenCsvFile
            // 
            this.buttonOpenCsvFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenCsvFile.Location = new System.Drawing.Point(321, 83);
            this.buttonOpenCsvFile.Name = "buttonOpenCsvFile";
            this.buttonOpenCsvFile.Size = new System.Drawing.Size(24, 20);
            this.buttonOpenCsvFile.TabIndex = 7;
            this.buttonOpenCsvFile.Text = "...";
            this.buttonOpenCsvFile.UseVisualStyleBackColor = true;
            this.buttonOpenCsvFile.Click += new System.EventHandler(this.buttonOpenCsvFile_Click);
            // 
            // openFileDialog_CsvFile
            // 
            this.openFileDialog_CsvFile.Filter = "txt|*.txt|All Files|*.*";
            this.openFileDialog_CsvFile.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_CsvFile_FileOk);
            // 
            // buttonOpenExcel
            // 
            this.buttonOpenExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenExcel.Location = new System.Drawing.Point(269, 19);
            this.buttonOpenExcel.Name = "buttonOpenExcel";
            this.buttonOpenExcel.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenExcel.TabIndex = 8;
            this.buttonOpenExcel.Text = "Excel";
            this.buttonOpenExcel.UseVisualStyleBackColor = true;
            this.buttonOpenExcel.Click += new System.EventHandler(this.buttonOpenExcel_Click);
            // 
            // buttonOpenTxt
            // 
            this.buttonOpenTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenTxt.Location = new System.Drawing.Point(269, 45);
            this.buttonOpenTxt.Name = "buttonOpenTxt";
            this.buttonOpenTxt.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenTxt.TabIndex = 8;
            this.buttonOpenTxt.Text = "TXT";
            this.buttonOpenTxt.UseVisualStyleBackColor = true;
            this.buttonOpenTxt.Click += new System.EventHandler(this.buttonOpenTxt_Click);
            // 
            // FrmTagMp3Saito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 181);
            this.Controls.Add(this.buttonOpenTxt);
            this.Controls.Add(this.buttonOpenExcel);
            this.Controls.Add(this.buttonOpenCsvFile);
            this.Controls.Add(this.textBox_CSV_FilePath);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btn_Load_Csv_And_Save_Mp3);
            this.Controls.Add(this.btnDrop);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 219);
            this.MinimumSize = new System.Drawing.Size(213, 219);
            this.Name = "FrmTagMp3Saito";
            this.Text = "TagMp3Saito";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDrop;
        private System.Windows.Forms.Button btn_Load_Csv_And_Save_Mp3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.TextBox textBox_CSV_FilePath;
        private System.Windows.Forms.Button buttonOpenCsvFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog_CsvFile;
        private System.Windows.Forms.Button buttonOpenExcel;
        private System.Windows.Forms.Button buttonOpenTxt;

    }
}

