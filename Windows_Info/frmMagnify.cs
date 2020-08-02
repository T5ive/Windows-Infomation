using System.Windows.Forms;
//137, 137
namespace TFive_Windows_Information
{
    public partial class frmMagnify : Form
    {

        public frmMagnify()
        {
            InitializeComponent();
            magnifyingGlass1.UpdateTimer.Start();
        }
    }
}
