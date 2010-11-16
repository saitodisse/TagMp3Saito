using System.Windows.Forms;

namespace TagMp3Saito_WindowsFormsApplication
{
    public partial class FrmLog : Form
    {
        public FrmLog()
        {
            InitializeComponent();
        }

        public string LogMesage
        {
            set { txtLog.Text = value; }
        }
    }
}