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
using System.IO;
using PresentationLayer.Global;

namespace PresentationLayer.People
{
    public partial class frmAddEditPerson : Form
    {
        private int _PersonID;
        private clsPerson _Person;
        public enum enMode { AddNew, Update };
        enMode Mode;
        public enum enGendor
        { Male = 0, Female = 1 }

        public frmAddEditPerson()
        {
            InitializeComponent();
            Mode = enMode.AddNew;
        }
        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            Mode = enMode.Update;
        }

        public delegate void DataBackEventHandler(object sender, int PersonID);
        public DataBackEventHandler DataBack;




        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.DefaultExt = ".JPG";
            openFileDialog1.Filter = "All files (*.*)|*.*|(*.PNG)|*.PNG|(*.JPG)|*.JPG|(*.JPEG)|*.JPEG";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Select An Image";
            openFileDialog1.InitialDirectory = $"F:\\";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ImagePath = openFileDialog1.FileName;
                pbPersonImage.ImageLocation = ImagePath;
                if (pbPersonImage.ImageLocation != null)
                {
                    llRemove.Visible = true;
         
                }
            }
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;
            llRemove.Visible = false;
        }
        private bool HandlePersonImage()
        {
            //Image has been (removed or changed)
            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    //if  ( changed )..Delete the old image from disk
                    if(File.Exists(_Person.ImagePath))
                    {
                        File.Delete(_Person.ImagePath);
                    }
                   
                }
                //it will be the source file after the next function is performed
                if(pbPersonImage.ImageLocation==null)
                {
                    _Person.ImagePath = null;
                    return true;
                }
                string SourceFile = pbPersonImage.ImageLocation.ToString();
                if (clsUtil.CopyImageToImagesFileAndGetItsSourceFile(ref SourceFile))
                {
                    _Person.ImagePath = SourceFile;
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not validated! check red icon messages", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _Person.Email = txtEmail.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.NationalityCountryID = clsCountry.Find(cbCountries.Text).CountryID;
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpBirth.Value;
            _Person.FirstName = txtFirst.Text.Trim();
            _Person.SecondName = txtSecond.Text.Trim();
            _Person.ThirdName = txtThird.Text.Trim();
            _Person.LastName = txtLast.Text.Trim();
            if (rbMale.Checked)
                _Person.Gendor = (short)enGendor.Male;
            else
                _Person.Gendor = (short)enGendor.Female;
            _Person.Phone = txtPhone.Text.Trim();

            if (!HandlePersonImage())
                return;
            if (_Person.Save())
            {
                //Send PersonID to Subscribed Forms
                DataBack?.Invoke(this, _Person.PersonID);
                Mode = enMode.Update;
                lblTitle.Text = "Update Person";
                this.Text = "Update Person";
                lblPersonID.Text = _Person.PersonID.ToString();
                MessageBox.Show(" Person was saved successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show(" Person save failed", "Save failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountriesList();
            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountries.Items.Add(row["CountryName"]);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _ResetDefaultValues()
        {
            //AddNew      
            lblTitle.Text = "Add New Person";
            this.Text = "Add New Person";
            lblPersonID.Text = "N/A";
            txtFirst.Text = "";
            txtSecond.Text = "";
            txtThird.Text = "";
            txtLast.Text = "";
            txtEmail.Text = "";
            txtNationalNo.Text = "";
            txtPhone.Text = "";
            dtpBirth.MaxDate = DateTime.Now.AddYears(-18);//Eldest Person in system
            dtpBirth.MinDate = DateTime.Now.AddYears(-100);//Oldest Person in system..1924
            dtpBirth.Value = dtpBirth.MaxDate;
            rbMale.Checked = true;
            txtAddress.Text = "";
            llRemove.Visible = false;
            _FillCountriesInComboBox();
            cbCountries.SelectedIndex = cbCountries.FindString("Egypt");//That is easier than remembering the index of egypt by yourself
            txtFirst.Focus();
            _Person = new clsPerson();
        }
        private void _LoadData()
        {

            lblTitle.Text = "Update Person";
            this.Text = "Update Person";
            _Person = clsPerson.Find(_PersonID);
            if (_Person != null)
            {
                llRemove.Visible =(_Person.ImagePath!="");
                dtpBirth.Value = _Person.DateOfBirth;
                if (_Person.Gendor == 1)
                    rbMale.PerformClick();

                else
                    rbFemale.PerformClick();
                txtFirst.Text = _Person.FirstName;
                txtSecond.Text = _Person.SecondName;
                txtThird.Text = _Person.ThirdName;
                txtLast.Text = _Person.LastName;
                txtNationalNo.Text = _Person.NationalNo;
                txtPhone.Text = _Person.Phone;
                txtAddress.Text = _Person.Address;
                cbCountries.SelectedIndex = cbCountries.FindString(_Person.Country.CountryName);
                if (_Person.ImagePath != "")
                {
                    if (File.Exists(_Person.ImagePath))
                    {
                        pbPersonImage.ImageLocation = _Person.ImagePath;
                    }

                }


            }
            else
            {
                MessageBox.Show("No Person is found with ID:" + _PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;//To ensure that no code will run after 
            }
        }
        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonImage.Image = Resources.Male_512;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonImage.Image = Resources.Female_512;
        }
        private void txtFirst_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirst.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFirst, "First name is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFirst, null);
            }
        }

        private void txtSecond_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSecond.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtSecond, "Second name is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtSecond, null);
            }
        }

        private void txtLast_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLast.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLast, "Last name is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLast, null);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "NationalNo is required");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, null);
            }
            if (Mode == enMode.AddNew && clsPerson.IsExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "There is a person already having this national No");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, null);
            }



        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Email is required");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, null);
            }

            if (Mode == enMode.AddNew && !clsValidation.IsValidEmail(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Email is not valid");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }



        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtAddress, "Address is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtAddress, null);
            }
        }

        private void frmAddEditPerson_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSave.PerformClick();
            }
        }
    }
}
