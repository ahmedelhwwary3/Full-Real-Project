using BusinessLayer;
using PresentationLayer.Applications.LocalDrivingLicenseApplications.Controls;
using PresentationLayer.Licenses;
using PresentationLayer.Licenses.LocalLicenses;
using PresentationLayer.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Applications.LocalDrivingLicenseApplications
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        private DataTable _dtLocalDrivingLicenseApplications;
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewLocalApp_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicenseApplication frm=new frmAddEditLocalDrivingLicenseApplication();
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null,null);
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplicationsList();
            dgvLDLA.DataSource = _dtLocalDrivingLicenseApplications;
            lblRecords.Text = dgvLDLA.Rows.Count.ToString();
            cbFilterColumn.Text = "None";
            if (dgvLDLA.Rows.Count > 0)
            {
                dgvLDLA.Columns[0].HeaderText = "LDL AppID";
                dgvLDLA.Columns[0].Width = 120;

                dgvLDLA.Columns[1].HeaderText = "Driving Class";
                dgvLDLA.Columns[1].Width = 190;

                dgvLDLA.Columns[2].HeaderText = "National No";
                dgvLDLA.Columns[2].Width = 150;

                dgvLDLA.Columns[3].HeaderText = "Full Name";
                dgvLDLA.Columns[3].Width = 280;

                dgvLDLA.Columns[4].HeaderText = "Application Date";
                dgvLDLA.Columns[4].Width = 170;

                dgvLDLA.Columns[5].HeaderText = "Passed Tests";
                dgvLDLA.Columns[5].Width = 100;

                dgvLDLA.Columns[6].HeaderText = "Status";
                dgvLDLA.Columns[6].Width = 100;
            }
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLocalDrivingLicenseApplicationInfo frm=new frmShowLocalDrivingLicenseApplicationInfo((int)dgvLDLA.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicenseApplication frm = new frmAddEditLocalDrivingLicenseApplication((int)dgvLDLA.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void cbFilterColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterColumn.Text != "None");
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            
            string FilterColumn = "";
            switch (cbFilterColumn.Text)
            {
                case "Local Driving License Application ID":
                    {
                        FilterColumn = "LocalDrivingLicenseApplicationID";
                        break;
                    }
                case "Driving Class":
                    {
                        FilterColumn = "ClassName";
                        break;
                    }
                case "National No":
                    {
                        FilterColumn = "NationalNo";
                        break;
                    }
                case "Full Name":
                    {
                        FilterColumn = "FullName";
                        break;
                    }
                case "Application Date":
                    {
                        FilterColumn = "ApplicationDate";
                        break;
                    }
                case "Passed Tests":
                    {
                        FilterColumn = "PassedTests";
                        break;
                    }
                case "Status":
                    {
                        FilterColumn = "Status";
                        break;
                    }
                default:
                    {

                        break;
                    }
            }
            if (cbFilterColumn.Text == "None" || txtFilterValue.Text.Trim() == "")
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecords.Text = dgvLDLA.Rows.Count.ToString();
                return;
            }
            if (cbFilterColumn.Text == "Passed Tests" || cbFilterColumn.Text == "Local Driving License Application ID")
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());

            }
            else
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterColumn.Text == "Passed Tests" || cbFilterColumn.Text == "Local Driving License Application ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar)&&!char.IsControl(e.KeyChar);
            }
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete this application?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID((int)dgvLDLA.CurrentRow.Cells[0].Value);
                if(LocalApp!=null)
                {
                    if (LocalApp.Delete())
                    {
                        MessageBox.Show("Application was deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmListLocalDrivingLicenseApplications_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Application was delete Failed Because The Local Driving License Application already has an appointment", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
               
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID((int)dgvLDLA.CurrentRow.Cells[0].Value);

            if (LocalApp!=null)
            {
                if(MessageBox.Show("Are you sure you want to cancel this application ?","Confirm",
                    MessageBoxButtons.OKCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.OK)
                {
                    if (LocalApp.Cancel())
                    {
                        MessageBox.Show("Local Driving License Application was cancelled successfully", "cancelled",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmListLocalDrivingLicenseApplications_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Local Driving License Application cancel Failed", "Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if(!int.TryParse((dgvLDLA.CurrentRow.Cells[0].Value).ToString(),out int value))
            {
                return;
            }
            int LocalDrivingLicenseApplicationID = value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if (LocalDrivingLicenseApplication==null)
            {
                MessageBox.Show("Local Driving License Application was not found with ID:"+LocalDrivingLicenseApplicationID.ToString(),"Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            int PassedTests= (int)dgvLDLA.CurrentRow.Cells[5].Value;
            bool IsLicenseExist = LocalDrivingLicenseApplication.IsLicenseIssued();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled=( LocalDrivingLicenseApplication.ApplicationStatus==clsApplication.enApplicationStatus.New&&PassedTests==3); ;
            editApplicationToolStripMenuItem.Enabled = (!(LocalDrivingLicenseApplication.ApplicationStatus==clsApplication.enApplicationStatus.Completed|| LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.Cancelled) &&!IsLicenseExist);
            deleteApplicationToolStripMenuItem.Enabled = (!IsLicenseExist && LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            cancelApplicationToolStripMenuItem.Enabled = editApplicationToolStripMenuItem.Enabled;
            showLicenseToolStripMenuItem.Enabled = IsLicenseExist;

            bool IsPassVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.Vision);
            bool IsPassWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.Written);
            bool IsPassStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.Street);
            scheduleTestsToolStripMenuItem.Enabled = (PassedTests < 3);
            if(scheduleTestsToolStripMenuItem.Enabled)
            {
                visionTestToolStripMenuItem.Enabled = (!IsPassVisionTest);
                writtenTestToolStripMenuItem.Enabled= (IsPassVisionTest&&!IsPassWrittenTest);
                streetTestToolStripMenuItem.Enabled = (IsPassVisionTest && IsPassWrittenTest && !IsPassStreetTest);

            }


        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID= (int)dgvLDLA.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm=new frmListTestAppointments(LocalDrivingLicenseApplicationID,clsTestType.enTestType.Vision);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LocalDrivingLicenseApplicationID, clsTestType.enTestType.Written);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LocalDrivingLicenseApplicationID, clsTestType.enTestType.Street);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLDLA.CurrentRow.Cells[0].Value;

            int LicenseID = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(
               LocalDrivingLicenseApplicationID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();

            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDrivingLicenesForFirstTime frm = new frmIssueDrivingLicenesForFirstTime((int)dgvLDLA.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null,null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID((int)dgvLDLA.CurrentRow.Cells[0].Value);
            frmShowLicenseHistory frm=new frmShowLicenseHistory(LocalDrivingLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();

        }
    }
}
