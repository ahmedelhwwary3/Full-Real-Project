using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Runtime.CompilerServices;

namespace BusinessLayer
{
    public class clsLicense
    {

        public enum enIssueReason { FirstTime = 1, Renew = 2, ReplacementForDamaged = 3, ReplacementForLost = 4 };
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public clsApplication Application { get; set; }
        public int DriverID { get; set; }
        public clsDriver Driver { get; set; }
        public int LicenseClass { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public string IssueReasonTest
        { get { return GetIssueReasonText(IssueReason); } }
        public int CreatedByUserID { get; set; }
        public clsUser User { get; set; }

        public enum enMode { AddNew, Update }
        enMode Mode;

        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IsActive = false;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IssueReason = 0;
            this.Notes = "";
            this.PaidFees = -1;
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        private clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.Application = clsApplication.FindBaseApplication(this.ApplicationID);
            this.DriverID = DriverID;
            this.Driver = clsDriver.FindByDriverID(this.DriverID);
            this.LicenseClass = LicenseClass;
            this.LicenseClassInfo = clsLicenseClass.FindByID(this.LicenseClass);
            this.IsActive = IsActive;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IssueReason = IssueReason;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.User = clsUser.FindByUserID(this.CreatedByUserID);



            Mode = enMode.Update;
        }
        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNew(this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (int)this.IssueReason, this.CreatedByUserID);
            return (this.LicenseID != -1);
        }
        private bool _UpdateLicense()
        {
            return clsLicenseData.Update(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (int)this.IssueReason, this.CreatedByUserID);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewLicense())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateLicense();
                    }
                default:
                    break;
            }
            return false;
        }


        public static bool IsExist(int LicenseID)
        {
            return clsLicenseData.IsExist(LicenseID);
        }
        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClass = -1, CreatedByUserID = -1;
            decimal PaidFees = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            bool IsActive = false;
            int IssueReason = 1;

            if (clsLicenseData.FindByLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);


            }
            else
            {
                return null;
            }
        }
        public static clsLicense FindByPersonID(int PersonID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClass = -1, CreatedByUserID = -1;
            decimal PaidFees = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            bool IsActive = false;
            int IssueReason = 1;
            int LicenseID = -1;

            if (clsLicenseData.FindByPersonID(PersonID,ref LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);


            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllLicensesList()
        {
            return clsLicenseData.GetAllLicensesList();
        }
        public static DataTable GetAllLicensesListForDriver(int DriverID)
        {
            return clsLicenseData.GetAllLicensesListForDriver(DriverID);
        }
        public static bool DeleteLicense(int LicenseID)
        {
            return clsLicenseData.Delete(LicenseID);
        }
        public static bool IsThereActiveLicenseForPersonPerLicenseClass(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonIDForLicenseClass(PersonID, LicenseClassID) != -1);
        }
        public static int GetActiveLicenseIDByPersonIDForLicenseClass(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonIDForLicenseClass(PersonID, LicenseClassID);
        }
        public bool Deactivate()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }
        public Boolean IsDateExpirated()
        {
            return (this.ExpirationDate > DateTime.Now);
        }
        public static string GetIssueReasonText(enIssueReason Reason)
        {
            switch (Reason)
            {
                case enIssueReason.FirstTime:
                    return "FirstTime";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.ReplacementForLost:
                    return "ReplacementForLost";
                default:
                    return "ReplacementForDamaged";
            }
        }
        public clsLicense Renew(string Notes,int CreatedByUserID)
        {
            clsApplication Application=new clsApplication();
            Application.ApplicantPersonID = this.Driver.Person.PersonID;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = clsApplication.enApplicationType.RenewDrivingLicenseService;
            Application.CreatedByUserID = CreatedByUserID;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicenseService).ApplicationFees;
            Application.LastStatusDate= DateTime.Now;
            if(!Application.Save())
            {
                return null;
            }
            clsLicense NewLicense=new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.Notes= Notes;
            NewLicense.CreatedByUserID= CreatedByUserID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.IsActive = true;
            NewLicense.IssueDate=DateTime.Now;
            NewLicense.IssueReason = enIssueReason.Renew;
            NewLicense.LicenseClass=this.LicenseClass;
            NewLicense.PaidFees=this.LicenseClassInfo.ClassFees;
            if(!NewLicense.Save())
            {
                return null;
            }
            Deactivate();
            return NewLicense;
        }
        public clsLicense Replace(int CreatedByUserID,int ApplicationTypeID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.Driver.Person.PersonID;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (clsApplication.enApplicationType)ApplicationTypeID;
            Application.CreatedByUserID = CreatedByUserID;
            Application.PaidFees = clsApplicationType.Find(ApplicationTypeID).ApplicationFees;
            Application.LastStatusDate = DateTime.Now;
            if (!Application.Save())
            {
                return null;
            }
            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.Notes = Notes;
            NewLicense.CreatedByUserID = CreatedByUserID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.IsActive = true;
            NewLicense.IssueDate = DateTime.Now;
            if(ApplicationTypeID==(int)clsApplication.enApplicationType.ReplacementForLostDrivingLicense)
            {
                NewLicense.IssueReason = enIssueReason.ReplacementForLost;
            }
            else
            {
                NewLicense.IssueReason = enIssueReason.ReplacementForDamaged;
            }
           
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            if (!NewLicense.Save())
            {
                return null;
            }
            Deactivate();
            return NewLicense;
        }
        public int Detain(decimal PaidFees,int CreatedByUserID)
        {
            clsDetainedLicense Detained= new clsDetainedLicense();
            Detained.DetainDate = DateTime.Now;
            Detained.FineFees = PaidFees;
            Detained.CreatedByUserID = CreatedByUserID;
            Detained.LicenseID = this.LicenseID;
            Detained.IsReleased = false;
            if(!Detained.Save())
            {
                return -1;
            }
            return Detained.LicenseID;
        }

        public bool IsLicenseDetained()
        {
            return clsDetainedLicenseData.IsLicenseDetained(this.LicenseID);
        }








    }
}



    

