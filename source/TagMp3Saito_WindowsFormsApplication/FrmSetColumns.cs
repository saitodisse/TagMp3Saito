using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using TagMp3Saito;

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


        private void CheckCreateConfigFile(string path = null)
        {
            if (path == null)
                path = Mp3FieldsHelper.GetDefaultPath();

            if (!File.Exists(path))
            {
                Mp3FieldsHelper.SaveJSON(MusicCsv.GetDefaultMp3Fields());
            }
            else
            {
                //TODO: Alredy exist. Update?
                //var fileReader = new StreamReader(path, Encoding.Unicode);
                //var objetoSerializado = fileReader.ReadToEnd();
                //var mp3Fields = JSONSerializer.JSonDeserialize <List<Mp3Field>>(objetoSerializado);
            }
        }

        private void checkedListBoxColumns_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control || e.Alt || e.Shift)
            {
                object selected = checkedListBoxColumns.SelectedItem;

                if (selected == null)
                    return; //não existe selecionado

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
                    checkedListBoxColumns.Items.Insert((int)indxTarget, selected);
                    checkedListBoxColumns.SetSelected((int)indxTarget, true);
                    checkedListBoxColumns.SetItemChecked((int)indxTarget, selectedChecked);
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
            Mp3FieldsHelper.SaveJSON(GetMp3FieldsFromcheckedListaBox(checkedListBoxColumns));
        }

        private List<Mp3Field> GetMp3FieldsFromcheckedListaBox(CheckedListBox checkedListBox)
        {
            return (from object item in checkedListBox.Items
                    select item as Mp3Field).ToList();
        }

        private void LoadList(string path = null)
        {
            checkedListBoxColumns.Items.Clear();

            List<Mp3Field> mp3Fields = Mp3FieldsHelper.GetMp3FieldsFromJSON(path);

            foreach (var mp3Field in mp3Fields)
            {
                checkedListBoxColumns.Items.Add(mp3Field, mp3Field.Active);
            }
        }


        private void checkedListBoxColumns_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var mp3Field = checkedListBoxColumns.Items[e.Index] as Mp3Field;
            mp3Field.Active = e.NewValue == CheckState.Checked;
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