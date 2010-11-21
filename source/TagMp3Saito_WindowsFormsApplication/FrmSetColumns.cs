using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TagMp3Saito_WindowsFormsApplication
{
    public partial class FrmSetColumns : Form
    {
        public FrmSetColumns()
        {
            InitializeComponent();
        }

        private void FrmSetColumns_Load(object sender, EventArgs e)
        {
            CheckCreateConfigFile();
            LoadList();
        }


        private void CheckCreateConfigFile()
        {
            var doc = new XDocument();
            var root = new XElement("TagMp3Saito-Column-Config");
            string path = new DirectoryInfo(Environment.CurrentDirectory).FullName + @"\columnsConfig.xml";
            List<ConfigColumn> customList = ConfigColumn_Default();

            if (!File.Exists(path))
            {
                // First time only
                foreach (ConfigColumn columnConfig in customList)
                    root.Add(new XElement(columnConfig.Name, new XAttribute("show", columnConfig.Enabled)));
                doc.Add(root);
                doc.Save(path);
            }
            else
            {
                // Alredy exist. Update?
                doc = XDocument.Load(path);
                root = doc.Elements("TagMp3Saito-Column-Config").First();

                // load current default
                foreach (ConfigColumn columnConfig in customList)
                {
                    // the element does not exist?
                    if (root.Descendants(columnConfig.Name).Count() == 0)
                    {
                        //include new element
                        root.Add(new XElement(columnConfig.Name, new XAttribute("show", columnConfig.Enabled)));
                    }
                }
                doc.Save(path);
            }
        }

        private List<ConfigColumn> ConfigColumn_Default()
        {
            var columnList = new List<ConfigColumn>();
            columnList.Add(new ConfigColumn("Artist", true));
            columnList.Add(new ConfigColumn("DiscNumber", false));
            columnList.Add(new ConfigColumn("Album", true));
            columnList.Add(new ConfigColumn("TrackNumber", true));
            columnList.Add(new ConfigColumn("Title", true));
            columnList.Add(new ConfigColumn("Genre", false));
            columnList.Add(new ConfigColumn("Year", true));
            columnList.Add(new ConfigColumn("OriginalArtist", false));
            columnList.Add(new ConfigColumn("Accompaniment_ArtistAlbum", false));
            columnList.Add(new ConfigColumn("CommentOne", false));
            columnList.Add(new ConfigColumn("Subtitle", false));
            columnList.Add(new ConfigColumn("FullPath", true));
            return columnList;
        }

        private void checkedListBoxColumns_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control || e.Alt || e.Shift)
            {
                object selected = checkedListBoxColumns.SelectedItem;
                if (selected == null) return;
                bool selectedChecked = checkedListBoxColumns.CheckedItems.Contains(selected);
                int indx = checkedListBoxColumns.Items.IndexOf(selected);
                int totl = checkedListBoxColumns.Items.Count;

                int? indxTarget = null;

                if (e.KeyCode == Keys.Down)
                {
                    if (indx == totl - 1)
                    {
                        indxTarget = 0;
                    }
                    else
                    {
                        indxTarget = indx + 1;
                    }
                    e.Handled = true;
                }

                if (e.KeyCode == Keys.Up)
                {
                    if (indx == 0)
                    {
                        indxTarget = totl - 1;
                    }
                    else
                    {
                        indxTarget = indx - 1;
                    }
                    e.Handled = true;
                }

                if (indxTarget != null)
                {
                    checkedListBoxColumns.Items.Remove(selected);
                    checkedListBoxColumns.Items.Insert((int) indxTarget, selected);
                    checkedListBoxColumns.SetSelected((int) indxTarget, true);
                    checkedListBoxColumns.SetItemChecked((int) indxTarget, selectedChecked);
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ctrl + Up: move item up\r\nCtrl + Down: move item down", "Help", MessageBoxButtons.OK,
                            MessageBoxIcon.Question);
        }

        private void FrmSetColumns_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveXml();
        }

        private void LoadList()
        {
            checkedListBoxColumns.Items.Clear();

            string path = new DirectoryInfo(Environment.CurrentDirectory).FullName + @"\columnsConfig.xml";
            XDocument doc = XDocument.Load(path);

            foreach (XElement ele in doc.Elements("TagMp3Saito-Column-Config").Descendants())
            {
                checkedListBoxColumns.Items.Add(ele.Name, Convert.ToBoolean(ele.Attribute("show").Value));
            }
        }

        private void SaveXml()
        {
            var doc = new XDocument();
            var root = new XElement("TagMp3Saito-Column-Config");
            string path = new DirectoryInfo(Environment.CurrentDirectory).FullName + @"\columnsConfig.xml";

            for (int i = 0; i < checkedListBoxColumns.Items.Count; i++)
                root.Add(new XElement(checkedListBoxColumns.Items[i].ToString(),
                                      new XAttribute("show",
                                                     checkedListBoxColumns.GetItemCheckState(i).Equals(
                                                         CheckState.Checked))));
            doc.Add(root);
            doc.Save(path);
        }
    }

    [Serializable]
    public class ConfigColumn
    {
        public ConfigColumn()
        {
        }

        public ConfigColumn(string name, bool enabled)
        {
            Name = name;
            Enabled = enabled;
        }

        public bool Enabled { get; set; }
        public string Name { get; set; }
    }
}