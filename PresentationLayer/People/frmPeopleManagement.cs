using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.People
{
    public partial class frmPeopleManagement : Form
    {
        
        public frmPeopleManagement()
        {
            InitializeComponent();
            
        }
        private static DataTable _dtAllPeople = clsPerson.GetAllPeopleList();
        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false,"PersonID","NationalNo","FirstName","SecondName","ThirdName","LastName","GendorCaption","DateOfBirth", "Nationality", "Phone","Email");
        private void _RefreshList()
        {
            frmPeopleManagement_Load(null, null);
        }
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();
            frm.ShowDialog();
            _RefreshList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    {
                        FilterColumn = "PersonID";
                        break;
                    }
                case "National No":
                    {
                        FilterColumn = "NationalNo";
                        break;
                    }
                case "First Name":
                    {
                        FilterColumn = "FirstName";
                        break;
                    }
                case "Second Name":
                    {
                        FilterColumn = "SecondName";
                        break;
                    }
                case "Third Name":
                    {
                        FilterColumn = "ThirdName";
                        break;
                    }
                case "Last Name":
                    {
                        FilterColumn = "LastName";
                        break;
                    }
                 
                case "Gendor Caption":
                    {
                        FilterColumn = "GendorCaption";
                        break;
                    }
                case "Address":
                    {
                        FilterColumn = "Address";
                        break;

                    }
                case "Phone":
                    {
                        FilterColumn = "Phone";
                        break;
                    }
                case "Email":
                    {
                        FilterColumn = "Email";
                        break;
                    }
                case "Nationality":
                    {
                        FilterColumn = "Nationality";
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            
            if (cbFilterBy.Text == "None" || txtFilterValue.Text.Trim() == "")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblTotalRecords.Text = dgvPeopleList.Rows.Count.ToString();
                return;
            }
            if (cbFilterBy.Text == "Person ID")
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            }
            lblTotalRecords.Text = dgvPeopleList.Rows.Count.ToString();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None"&&_dtAllPeople.Rows.Count!=0);
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }
         
        private void frmPeopleManagement_Load(object sender, EventArgs e)
        {
            
            cbFilterBy.Text = "None";
            _dtPeople = clsPerson.GetAllPeopleList();
            dgvPeopleList.DataSource = _dtPeople;
            lblTotalRecords.Text = dgvPeopleList.Rows.Count.ToString();
            if (dgvPeopleList.Rows.Count > 0)
            {
                dgvPeopleList.Columns[0].Width = 110;
                dgvPeopleList.Columns[0].HeaderText = "Person ID";

                dgvPeopleList.Columns[1].Width = 110;
                dgvPeopleList.Columns[1].HeaderText = "National No";

                dgvPeopleList.Columns[2].Width = 110;
                dgvPeopleList.Columns[2].HeaderText = "First Name";

                dgvPeopleList.Columns[3].Width = 110;
                dgvPeopleList.Columns[3].HeaderText = "Second Name";

                dgvPeopleList.Columns[4].Width = 100;
                dgvPeopleList.Columns[4].HeaderText = "Third Name";

                dgvPeopleList.Columns[5].Width = 100;
                dgvPeopleList.Columns[5].HeaderText = "Last Name";

                dgvPeopleList.Columns[6].Width = 100;
                dgvPeopleList.Columns[6].HeaderText = "Gendor";

                dgvPeopleList.Columns[7].Width = 120;
                dgvPeopleList.Columns[7].HeaderText = "Date Of Birth";

                dgvPeopleList.Columns[8].Width = 140;
                dgvPeopleList.Columns[8].HeaderText = "Country Name";

                dgvPeopleList.Columns[9].Width = 150;
                dgvPeopleList.Columns[9].HeaderText = "Phone";

                dgvPeopleList.Columns[10].Width = 200;
                dgvPeopleList.Columns[10].HeaderText = "Email";


            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void showPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonCard frm = new frmShowPersonCard((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddNewPerson.PerformClick();
            _RefreshList    ();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvPeopleList.CurrentRow.Cells[0].Value;
            if (MessageBox.Show("Are you sure you want to delete this SelectedPersonInfo?", "confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                 
               if (clsPerson.DeletePerson(ID))
               {
                    _RefreshList();
                   MessageBox.Show("Person was deleted successfully", "Delete succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               else
               {
                   MessageBox.Show("Error:Person delete Failed as he is linked with application in system", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
             

            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Method is not implemented", "Stup", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Method is not implemented", "Stup", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
