using BusinessLayer;
using PresentationLayer.Global;
using PresentationLayer.Licenses;
using PresentationLayer.Licenses.LocalLicenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Applications.ReleaseDetainedLicense
{
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _DetainID = -1;
        public clsDetainedLicense _DetainedLicense;
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }
        public frmReleaseDetainedLicense(int DetainID)
        {
            InitializeComponent();
            _DetainID=DetainID;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm=new frmShowLicenseInfo(clsDetainedLicense.FindByDetainID(_DetainID).LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseHistory frm = new frmShowLicenseHistory(clsDetainedLicense.FindByDetainID(_DetainID).License.Driver.PersonID);
            frm.ShowDialog();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            int ApplicationID = -1;
            clsDetainedLicense DetainedLicense = clsDetainedLicense.FindByLicenseID(_DetainID);
            bool IsReleased = DetainedLicense.Release(decimal.Parse(lblFineFees.Text),clsGlobal.CurrentUser.UserID,ref ApplicationID);
            if(!IsReleased)
            {
                MessageBox.Show("Error:License was not released","Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            clsDetainedLicense ReleasedLicense = clsDetainedLicense.FindByLicenseID(_DetainID);
            lblApplicationID.Text = ReleasedLicense.ReleaseApplicationID.ToString();
            btnRelease.Enabled = false;
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;
            MessageBox.Show("Release succeeded", "Released",
                MessageBoxButtons.OK,MessageBoxIcon.Information);
            






        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            btnRelease.Enabled = false;
           

            if (_DetainID==-1)
            {
                return;
            }
            _DetainedLicense = clsDetainedLicense.FindByDetainID(_DetainID);
            if(_DetainedLicense==null)
            {
                MessageBox.Show("Error:no Detained license is found with DetainID"+_DetainID.ToString(),"Error"
                ,MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_DetainedLicense.LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
           
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _DetainID = obj;
            if(_DetainID==-1)
            {
                ctrlDriverLicenseInfoWithFilter1.Clear();
                MessageBox.Show("Error:No license is found with DetainID:"+_DetainID.ToString(),"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DetainedLicense = clsDetainedLicense.FindByLicenseID(_DetainID);
            
            if(!_DetainedLicense.IsDetained())
            {
                ctrlDriverLicenseInfoWithFilter1.Clear();
                MessageBox.Show("Error:license is not detained","Error"
                    ,MessageBoxButtons.OK,MessageBoxIcon.Error); 
                return;
            }
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense).ApplicationFees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblDetainDate.Text=_DetainedLicense.DetainDate.ToString();
            lblDetainID.Text=_DetainedLicense.DetainID.ToString();
            lblFineFees.Text =_DetainedLicense.FineFees.ToString();
            lblLicenseID.Text=_DetainedLicense.LicenseID.ToString();
            btnRelease.Enabled = true;
            lblTotalFees.Text = ((decimal.Parse(lblFineFees.Text)+decimal.Parse(lblApplicationFees.Text))).ToString();
        }
    }
}
