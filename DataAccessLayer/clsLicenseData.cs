using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public class clsLicenseData
    {
        public static DataTable GetAllLicensesList()
        {
            DataTable dtLicense = new DataTable();
            string query = "select * from Licenses;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dtLicense.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string sourceName = "DVLD1";
                    // Create the event source if it does not exist
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return dtLicense;
        }

        public static DataTable GetAllLicensesListForDriver(int DriverID)
        {
            DataTable dtLicense = new DataTable();
            string query = @"select l.ApplicationID,l.DriverID,l.IssueDate,l.ExpirationDate,l.IsActive,l.PaidFees,lc.ClassName 
                             from Licenses l 
                             inner join LicenseClasses lc on l.LicenseClass=lc.LicenseClassID
                             where l.DriverID=@DriverID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DriverID", DriverID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dtLicense.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string sourceName = "DVLD1";
                    // Create the event source if it does not exist
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return dtLicense;
        }

        public static bool IsExist(int LicenseID)
        {
            bool IsFound = false;
            string query = "Select IsFound=1 From Licenses where LicenseID=@LicenseID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        IsFound = reader.HasRows;
                    }
                }
                catch (Exception ex)
                {
                    IsFound = false;
                    string sourceName = "DVLD1";
                    // Create the event source if it does not exist
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return IsFound;
        }

        public static bool FindByPersonID(int PersonID, ref int LicenseID, ref int ApplicationID, ref int DriverID,
                                          ref int LicenseClass, ref DateTime IssueDate, ref DateTime ExpirationDate,
                                          ref string Notes, ref decimal PaidFees, ref bool IsActive, ref int IssueReason,
                                          ref int CreatedByUserID)
        {
            bool IsFound = false;
            string query = @"select l.LicenseID,l.ApplicationID,l.DriverID,l.LicenseClass,l.IssueDate,l.ExpirationDate,
                             l.Notes,l.PaidFees,l.IsActive,l.IssueReason,l.CreatedByUserID 
                             from Licenses l 
                             inner join Applications a on l.ApplicationID=a.ApplicationID
                             where a.ApplicantPersonID=@PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            LicenseID = (int)reader["l.LicenseID"];
                            ApplicationID = (int)reader["l.ApplicationID"];
                            DriverID = (int)reader["l.DriverID"];
                            LicenseClass = (int)reader["l.LicenseClass"];
                            IssueDate = (DateTime)reader["l.IssueDate"];
                            ExpirationDate = (DateTime)reader["l.ExpirationDate"];
                            Notes = reader["l.Notes"] == DBNull.Value ? "" : (string)reader["l.Notes"];
                            PaidFees = (decimal)reader["l.PaidFees"];
                            IsActive = (bool)reader["l.IsActive"];
                            IssueReason = (int)reader["l.IssueReason"];
                            CreatedByUserID = (int)reader["l.CreatedByUserID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsFound = false;
                    string sourceName = "DVLD1";
                    // Create the event source if it does not exist
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return IsFound;
        }
        public static int AddNew(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
                          string Notes, decimal PaidFees, bool IsActive, int IssueReason, int CreatedByUserID)
        {
            int ID = -1;
            string query = @"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, 
                      Notes, PaidFees, IsActive, IssueReason, CreatedByUserID) 
                     VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, 
                             @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                    select scope_Identity();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                command.Parameters.AddWithValue("@Notes", Notes ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    object result=command.ExecuteScalar();
                    if (result!=null&&int.TryParse(result.ToString(),out int InsertedID))
                    {
                        ID = InsertedID;
                    }
                }
                catch (Exception ex)
                {
                    string sourceName = "DVLD1";
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return ID;
        }
        public static bool Update(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
                          string Notes, decimal PaidFees, bool IsActive, int IssueReason, int UpdatedByUserID)
        {
            bool isSuccess = false;
            string query = @"UPDATE Licenses SET 
                     ApplicationID = @ApplicationID, 
                     DriverID = @DriverID, 
                     LicenseClass = @LicenseClass, 
                     IssueDate = @IssueDate, 
                     ExpirationDate = @ExpirationDate, 
                     Notes = @Notes, 
                     PaidFees = @PaidFees, 
                     IsActive = @IsActive, 
                     IssueReason = @IssueReason, 
                     UpdatedByUserID = @UpdatedByUserID
                     WHERE LicenseID = @LicenseID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                command.Parameters.AddWithValue("@Notes", Notes ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@UpdatedByUserID", UpdatedByUserID);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    isSuccess = rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    string sourceName = "DVLD1";
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return isSuccess;
        }
        public static bool Delete(int LicenseID)
        {
            bool isSuccess = false;
            string query = "DELETE FROM Licenses WHERE LicenseID = @LicenseID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    isSuccess = rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    string sourceName = "DVLD1";
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return isSuccess;
        }

        public static bool FindByLicenseID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
                                            ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
                                            ref decimal PaidFees, ref bool IsActive, ref int IssueReason,
                                            ref int CreatedByUserID)
        {
            bool IsFound = false;
            string query = @"Select * From Licenses where LicenseID=@LicenseID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            ApplicationID = (int)reader["ApplicationID"];
                            DriverID = (int)reader["DriverID"];
                            LicenseClass = (int)reader["LicenseClass"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];
                            Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
                            PaidFees = (decimal)reader["PaidFees"];
                            IsActive = (bool)reader["IsActive"];
                            IssueReason = (int)reader["IssueReason"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsFound = false;
                    string sourceName = "DVLD1";
                    // Create the event source if it does not exist
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return IsFound;
        }
        public static bool DeactivateLicense(int LicenseID)
        {
            bool IsDeactivated = false;

            string query = @"  update Licenses set IsActive=0 where LicenseID=@LicenseID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                 

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        IsDeactivated = reader.HasRows;
                    }
                    
                }
                catch (Exception ex)
                {

                    string sourceName = "DVLD1";
                    // Create the event source if it does not exist
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return IsDeactivated;
        }
        public static int GetActiveLicenseIDByPersonIDForLicenseClass(int PersonID,int LicenseClassID)
        {
            int ActiveLicenseID = -1;
            
            string query = @"select LicenseID from Licenses lic join Applications app on lic.ApplicationID=app.ApplicationID where LicenseClass=@LicenseClass and app.ApplicantPersonID=@PersonID and IsActive=1;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID",PersonID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

                try
                {
                    connection.Open();
                    object result= command.ExecuteScalar();
                    if (result != null&&int.TryParse(result.ToString(),out int ID))
                    {
                        ActiveLicenseID = ID;
                    }
                     
                }
                catch (Exception ex)
                {
                     
                    string sourceName = "DVLD1";
                    // Create the event source if it does not exist
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }

            return ActiveLicenseID;
        }













    }
}
