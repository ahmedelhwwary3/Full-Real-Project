using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Licenses.InternationalLicenses
{
    public partial class frmShowInternationaLicenseInfo : Form
    {
        private int _InternationalLicenseID;
        public frmShowInternationaLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();
            _InternationalLicenseID = InternationalLicenseID;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         
        private void frmShowInternationaLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlInternationalLicenseInfo2.LoadInfo(_InternationalLicenseID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
