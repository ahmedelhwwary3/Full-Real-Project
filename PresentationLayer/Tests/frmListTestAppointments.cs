using BusinessLayer;
using PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Tests
{
    public partial class frmListTestAppointments : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private DataTable _dtTestAppointmentsList;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.Vision;
        public frmListTestAppointments(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            _TestTypeID= TestTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _LoadTestTypeImage()
        {
            switch (_TestTypeID)
            {
                case clsTestType.enTestType.Vision:
                    {
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }
                case clsTestType.enTestType.Written:
                    {
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestType.Street:
                    {
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }
        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImage();
            ctrlDrivingLicenesApplicationInfo1.LoadLocalDrivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);
            _dtTestAppointmentsList=clsTestAppointment.GetAllTestAppointmentsListPerTestType(_LocalDrivingLicenseApplicationID,_TestTypeID);
            dgvTestAppointments.DataSource= _dtTestAppointmentsList;
            lblRecords.Text=dgvTestAppointments.Rows.Count.ToString();
            if(dgvTestAppointments.Rows.Count>0)
            {
                dgvTestAppointments.Columns[0].HeaderText = "Test Appointment ID";
                dgvTestAppointments.Columns[0].Width = 100;

                dgvTestAppointments.Columns[1].HeaderText = "Local Driving License Application ID";
                dgvTestAppointments.Columns[0].Width = 100;

                dgvTestAppointments.Columns[2].HeaderText = "Test Type Title";
                dgvTestAppointments.Columns[2].Width = 180;

                dgvTestAppointments.Columns[3].HeaderText = "Class Name";
                dgvTestAppointments.Columns[3].Width = 220;

                dgvTestAppointments.Columns[4].HeaderText = "Appointment Date";
                dgvTestAppointments.Columns[4].Width = 180;

                dgvTestAppointments.Columns[5].HeaderText = "Paid Fees";
                dgvTestAppointments.Columns[5].Width = 100;

                dgvTestAppointments.Columns[6].HeaderText = "Full Name";
                dgvTestAppointments.Columns[6].Width = 200;

                dgvTestAppointments.Columns[7].HeaderText = "Is Locked";
                dgvTestAppointments.Columns[7].Width = 100;

            }



        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            if (LocalDrivingLicenseApplication.IsThereActiveScheduledTestAppointment(_TestTypeID))
            {
                MessageBox.Show("Error:There is an active scheduled Test appointment","Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }


            clsTest LastTest = LocalDrivingLicenseApplication.GetLastTestAppointmentInfoPerTestType(_TestTypeID);
            //The Person did not have any Test appointment For this type before
            if (LastTest==null)
            {
                //Add New Mode
                frmScheduleTest frm=new frmScheduleTest(_LocalDrivingLicenseApplicationID,_TestTypeID);
                frm.ShowDialog();
                frmListTestAppointments_Load(null, null);
                return;
            }
            if(LastTest.TestResult==true)
            {
                //Already Passed this Test Type before
                MessageBox.Show("Error:This Person already Passed this test type before","Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            //If Not Null Or Passed -->(Failed)

            frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseApplicationID,_TestTypeID);
            frm1.ShowDialog();
            frmListTestAppointments_Load(null,null);        
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID =(int) dgvTestAppointments.CurrentRow.Cells[0].Value;
            clsTestAppointment TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            if (TestAppointment.IsLocked)
            {
                MessageBox.Show("Error:This Test Appointment is locked.You can not update it","Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            frmScheduleTest frm=new frmScheduleTest(_LocalDrivingLicenseApplicationID,_TestTypeID, TestAppointmentID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);



        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;
            clsTestAppointment TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            int TestID =TestAppointment.GetTestID();
            frmTakeScheduledTest frm = new frmTakeScheduledTest(_TestTypeID,TestAppointmentID, TestID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null,null);
        }
    }
}
