using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FAES.ChamadorProcessos;
using Lambda.Generic;
using TagMp3Saito;

namespace TagMp3Saito_WindowsFormsApplication
{
    public interface IFormComunicator
    {
        string Mesage { set; }
        int TotalItens { set; }
        int ActualItem { set; }
        bool ShowProgressBar { set; }
        string ShowError { set; }
        void FinishSaving();
    }

    public partial class FrmTagMp3Saito : Form
    {
        //threads: http://www.codeproject.com/KB/cs/threadsafeforms.aspx?print=true
        private IFormComunicator iForm;
        public MusicList musicList;
        private MusicLoader musicLoader;
        private Thread thread;

        public FrmTagMp3Saito()
        {
            InitializeComponent();
        }

        public string Mesage
        {
            set { toolStripStatusLabel1.Text = value; }
        }

        public bool ShowProgressBar
        {
            set { toolStripProgressBar1.Visible = value; }
        }

        public int TotalItens
        {
            set { toolStripProgressBar1.Maximum = value; }
        }

        public int ActualItem
        {
            set { toolStripProgressBar1.Value = value; }
        }

        public string ShowError
        {
            set
            {
                var log = new FrmLog { LogMesage = value };
                log.ShowDialog();
            }
        }

        public void FinishSaving()
        {
            musicList.Clear();
            musicLoader.Clear();
            btn_Load_Csv_And_Save_Mp3.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            musicLoader = new MusicLoader();
            iForm = (IFormComunicator)Wrapper.Create(typeof(IFormComunicator), this);
        }

        private void GetNewCSVFile()
        {
            textBox_CSV_FilePath.Text = Path.Combine(Path.GetTempPath(),
                                             "__TagMp3Saito_Temp_CSV_" + StringTools.Get_Date_Hour() + ".txt");
        }

        private void btnDrop_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void btnDrop_DragDrop(object sender, DragEventArgs e)
        {
            DropedSomething(e);
        }

        private void btn_Load_Csv_And_Save_Mp3_Click(object sender, EventArgs e)
        {
            FinishSavingCSV();
        }

        private static string GetExcelPath()
        {
            string excel2010DefaultPath =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)).FullName +
                @"\Microsoft Office\Office14\EXCEL.EXE";
            if (File.Exists(excel2010DefaultPath))
                return excel2010DefaultPath;

            string excel2007DefaultPath =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)).FullName +
                @"\Microsoft Office\Office12\EXCEL.EXE";
            if (File.Exists(excel2007DefaultPath))
                return excel2007DefaultPath;

            string excel2003DefaultPath =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)).FullName +
                @"\Microsoft Office\Office11\EXCEL.EXE";
            if (File.Exists(excel2003DefaultPath))
                return excel2003DefaultPath;

            string excel2002DefaultPath =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)).FullName +
                @"\Microsoft Office\Office10\EXCEL.EXE";
            if (File.Exists(excel2002DefaultPath))
                return excel2002DefaultPath;

            string excel1997DefaultPath =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)).FullName +
                @"\MSOffice\Office97\Excel\Excel.exe";
            if (File.Exists(excel1997DefaultPath))
                return excel1997DefaultPath;

            return string.Empty;
        }


        private void DropedSomething(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            musicLoader.Sources = new List<string>();

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
                musicLoader.Sources.Add(file);

            thread = new Thread(LoadMusics);
            thread.Start();
        }

        private void LoadMusics()
        {
            iForm.ShowProgressBar = true;

            musicLoader.LoadingMusicNumber += musicLoader_LoadingMusicNumber;
            int total;
            iForm.TotalItens = total = musicLoader.LoadPaths().Length;

            iForm.Mesage = "processing " + total + " files";
            musicList = new MusicList();
            musicList = musicLoader.GetMusicList();
            iForm.Mesage = musicList.Count + " mp3 files ready to edit";

            iForm.ShowProgressBar = false;
        }

        private void musicLoader_LoadingMusicNumber(int actual)
        {
            iForm.ActualItem = actual;
        }

        private void CallExcel()
        {
            try
            {
                if (!File.Exists(textBox_CSV_FilePath.Text))
                    SaveInitialCSV();

                OpenExcel_CsvFile();
                toolStripStatusLabel1.Text = "waiting to save updates to mp3 files";
                btn_Load_Csv_And_Save_Mp3.Enabled = true;
            }
            catch (Exception ex)
            {
                if (ex.Message != "no songs loaded to save the CSV")
                    throw;
            }
        }

        private void SaveInitialCSV()
        {
            GetNewCSVFile();
            var musicCsv = new MusicCsv(musicList);
            musicCsv.SaveCsvFile(textBox_CSV_FilePath.Text);
        }

        private void OpenExcel_CsvFile()
        {
            var proc = new ProcessCaller(this) { FileName = GetExcelFileName() };

            if (proc.FileName.Length == 0)
                return;

            var param = new StringBuilder();
            param.Append(" \"");
            param.Append(textBox_CSV_FilePath.Text);
            param.Append("\"");
            proc.Arguments = param.ToString();
            proc.Start();
        }

        private static string GetExcelFileName()
        {
            if (ConfigurationManager.AppSettings["Excel_Path"].Length > 0)
            {
                if (File.Exists(ConfigurationManager.AppSettings["Excel_Path"]))
                    return ConfigurationManager.AppSettings["Excel_Path"];

                MessageBox.Show("Excel path invalid at TagMp3_Saito.exe.config. \r\n" +
                                "Please, edit TagMp3_Saito.exe.config located at this application path \r\n" +
                                "(like: C:\\Program Files\\TagMp3Saito\\TagMp3Saito_0.71)", "Error opening Excel",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                return GetExcelPath();

            MessageBox.Show("No Excel Application detected. \r\n" +
                            "Please, edit TagMp3_Saito.exe.config located at this " + "application path \r\n"
                            + "(like: C:\\Program Files\\TagMp3Saito\\TagMp3Saito_0.71)", "Error opening Excel",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

            return string.Empty;
        }

        private void FinishSavingCSV()
        {
            if (!File.Exists(textBox_CSV_FilePath.Text))
                return;

            thread = new Thread(Load_Save_Work);
            thread.Start();
        }

        private void Load_Save_Work()
        {
            iForm.ShowProgressBar = true;
            int countItens = 0;

            var sbErrors = new StringBuilder();
            var musCsv = new MusicCsv(textBox_CSV_FilePath.Text);

            try
            {
                musicList = musCsv.Load();
            }
            catch (Exception ex)
            {
                sbErrors.Append("<Load_Save_Work>\r\n");
                sbErrors.Append("musicList = musCsv.Load();\r\n");
                sbErrors.Append(ex.Message);
                sbErrors.Append("\r\n");
                sbErrors.Append("\t");
                sbErrors.Append("textBox_CSV_FilePath.Text = ");
                sbErrors.Append(textBox_CSV_FilePath.Text);
                sbErrors.Append("\r\n");
                sbErrors.Append("</Load_Save_Work>");
            }

            if (musicList == null)
                return;

            iForm.TotalItens = musicList.Count;

            iForm.Mesage = "saving " + musicList.Count + " files...";

            foreach (Music mus in musicList)
            {
                try
                {
                    mus.Save(musicList.FieldList);
                }
                catch (Exception ex)
                {
                    sbErrors.Append("<Load_Save_Work>\r\n");
                    sbErrors.Append("mus.Save();\r\n");
                    sbErrors.Append(ex.Message);
                    sbErrors.Append("\r\n");
                    sbErrors.Append("\t");
                    sbErrors.Append("mus = ");
                    sbErrors.Append(mus.FullPath);
                    sbErrors.Append("\r\n");
                    sbErrors.Append("</Load_Save_Work>\r\n");
                }

                countItens++;
                iForm.ActualItem = countItens;
            }
            iForm.Mesage = countItens + " saved";

            if (sbErrors.Length > 0)
                iForm.ShowError = sbErrors.ToString();

            iForm.ShowProgressBar = false;
            iForm.FinishSaving();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var frmModal = new FrmSetColumns();
            frmModal.ShowDialog();
        }


        private void buttonOpenCsvFile_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox_CSV_FilePath.Text))
            {
                var fi = new FileInfo(textBox_CSV_FilePath.Text);
                openFileDialog_CsvFile.FileName = fi.FullName;
                openFileDialog_CsvFile.InitialDirectory = fi.DirectoryName;
            }
            else
            {
                openFileDialog_CsvFile.InitialDirectory = Path.GetTempPath();
            }

            openFileDialog_CsvFile.ShowDialog();
        }

        private void openFileDialog_CsvFile_FileOk(object sender, CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                textBox_CSV_FilePath.Text = openFileDialog_CsvFile.FileName;

                //The file exist then maybe can be updated already
                var fi = new FileInfo(textBox_CSV_FilePath.Text);
                if (fi.Exists)
                {
                    toolStripStatusLabel1.Text = "waiting to save updates to mp3 files";
                    btn_Load_Csv_And_Save_Mp3.Enabled = true;
                }
            }
        }

        private void buttonOpenExcel_Click(object sender, EventArgs e)
        {
            CallExcel();
        }

        private void buttonOpenTxt_Click(object sender, EventArgs e)
        {
            if(!File.Exists(textBox_CSV_FilePath.Text))
                return;

            var p = new Process();
            p.StartInfo.FileName = textBox_CSV_FilePath.Text;
            p.Start();
        }
    }
}