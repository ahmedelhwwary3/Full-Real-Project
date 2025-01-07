using BusinessLayer;
using PresentationLayer.Global;
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
    public partial class frmAddEditLocalDrivingLicenseApplication : Form
    {
        private int _SelectedPersonID;
        public enum enMode { AddNew, Update };
        public enMode Mode;
        private int _LocalDrivingLicenseApplicationID;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public frmAddEditLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            Mode = enMode.AddNew;
        }
        public frmAddEditLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            Mode = enMode.Update;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(Mode==enMode.Update)
            {
                btnSave.Enabled = true;
                tcAddNewUser.SelectedTab = tpApplicationInfo;
                tpApplicationInfo.Enabled = true;
                return;
            }


            //AddNew Mode
            if (_SelectedPersonID == -1)
            {
                MessageBox.Show("No Person was selected", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnSave.Enabled = true;
            tcAddNewUser.SelectedTab = tpApplicationInfo;
            tpApplicationInfo.Enabled = true;
            return;

        }

        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _FillLicenseClassesInComboBox()
        {

            DataTable LicenseClasses = clsLicenseClass.GetAllLicenseClasssList();
            foreach (DataRow r in LicenseClasses.Rows)
            {
                cbLicenesClass.Items.Add(r["ClassName"]);
            }

        }
        private void _ResetDefaultValues()
        {
            _FillLicenseClassesInComboBox();
            if(Mode==enMode.AddNew)
            {
                _LocalDrivingLicenseApplication=new clsLocalDrivingLicenseApplication();
                this.Text = "Add New Local Driving License Application";
                lblAddEditLocalDrivingLicenseApplication.Text = "Add New Local Driving License Application";
                btnSave.Enabled = false;
                tpApplicationInfo.Enabled = false;
                lblApplicationDate.Text = clsFormat.DateToShortString(DateTime.Now);
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalDrivingLicenseService).ApplicationFees.ToString();
                lblCreatedByUserName.Text=clsGlobal.CurrentUser.UserName;
                lblLocalDrivingLicenseApplicationID.Text = "[???]";
                ctrlPersonCardWithFilter1.FilterFocus();
                cbLicenesClass.SelectedIndex = 2;//ordinary class
            }
            else
            {
                this.Text = "Edit Local Driving License Application";
                lblAddEditLocalDrivingLicenseApplication.Text = "Edit Local Driving License Application";
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
            }
        }
        private void _LoadData()
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicenseApplication==null)
            {
                MessageBox.Show("Local Driving License Application is not found with ID:"+_LocalDrivingLicenseApplicationID.ToString()
                    ,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
      
            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            lblApplicationDate.Text = clsFormat.DateToShortString(_LocalDrivingLicenseApplication.ApplicationDate);
            lblApplicationFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUserName.Text = _LocalDrivingLicenseApplication.CreatedByUser.UserName;
            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplicationID.ToString();
            cbLicenesClass.SelectedIndex = cbLicenesClass.FindString(_LocalDrivingLicenseApplication.LicenseClass.ClassName);
            cbLicenesClass.SelectedIndex = 0;


        }
        private void frmAddEditLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if(Mode==enMode.Update)
            {
                _LoadData();
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsLicenseClass.FindByClassName(cbLicenesClass.Text).LicenseClassID;
            if (clsLocalDrivingLicenseApplication.DoesPersonHaveActiveApplicationForLicenseClass(_SelectedPersonID,clsApplication.enApplicationType.NewLocalDrivingLicenseService,LicenseClassID))
            {
                MessageBox.Show("This Person has an active Local Driving License Application with This License Class","Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }


            if(clsLicense.IsThereActiveLicenseForPersonPerLicenseClass(_SelectedPersonID,LicenseClassID))
            {
                MessageBox.Show("This Person has already a License with This License Class", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }





            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.ApplicantPersonID = _SelectedPersonID;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationID = clsLocalDrivingLicenseApplication.GetActiveApplicationID(_SelectedPersonID, clsApplication.enApplicationType.NewLocalDrivingLicenseService);
            _LocalDrivingLicenseApplication.LicenseClassID = clsLicenseClass.FindByClassName(cbLicenesClass.Text).LicenseClassID;
            _LocalDrivingLicenseApplication.LicenseClassID = 3;
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalDrivingLicenseService).ApplicationFees;
            if (_LocalDrivingLicenseApplication.Save())
            {
                MessageBox.Show("Local Driving License Application was Saved Successfully", "saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                Mode = enMode.Update;
                this.Text = "Edit Local Driving License Application";
                lblAddEditLocalDrivingLicenseApplication.Text = "Edit Local Driving License Application";
            }
            else
            {
                MessageBox.Show("Local Driving License Application Save Failed", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
