using System.Windows.Forms;
//137, 137
namespace TFive_Windows_Information
{
    public partial class frm_magnify : Form
    {

        public frm_magnify()
        {
            InitializeComponent();
            magnifyingGlass1.UpdateTimer.Start();
        }
    }
}
