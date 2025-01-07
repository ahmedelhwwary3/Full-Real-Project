using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class clsApplicationData
    {
        public static DataTable GetAllApplicationsList()
        {
            DataTable dtApplications = new DataTable();
            string query = "select * from Applications order by ApplicationDate desc;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            dtApplications.Load(reader);
                        }
                        reader.Close();
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
                    finally
                    {
                        connection.Close();
                    }
                    return dtApplications;

                }

            }
              
            
            
        }
        public static bool IsExist(int ApplicationID)
        {

            bool IsFound = false;
            string query = "Select IsFound=1 From Applications where ApplicationID=@ApplicationID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        IsFound = reader.HasRows;
                        reader.Close();

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
                    finally
                    {
                        connection.Close();
                    }
                    return IsFound;
                }
            }
 
            
            
        }
        public static bool Find(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref int ApplicationStatus, ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            bool IsFound = false;
            string query = "Select * From Applications where ApplicationID=@ApplicationID;";
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            IsFound = true;
                            ApplicantPersonID = (int)reader["ApplicantPersonID"];
                            ApplicationDate = (DateTime)reader["ApplicationDate"];
                            ApplicationTypeID = (int)reader["ApplicationTypeID"];
                            ApplicationStatus = (int)reader["ApplicationStatus"];
                            LastStatusDate = (DateTime)reader["LastStatusDate"];
                            PaidFees = (decimal)reader["PaidFees"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];

                        }
                        reader.Close();

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
                    finally
                    {
                        connection.Close();
                    }
                    return IsFound;
                }
            }
            
            
        }
        public static int AddNewApplication(int ApplicantPersonID, int ApplicationTypeID, DateTime ApplicationDate, int ApplicationStatus, DateTime LastStatusDate, int CreatedByUserID,decimal PaidFees)
        {
            int NewID = -1;
            string query = @"Insert Into Applications ( ApplicantPersonID,  ApplicationTypeID, ApplicationDate,  ApplicationStatus, LastStatusDate,  CreatedByUserID,  PaidFees) 
                                         values (@ApplicantPersonID,  @ApplicationTypeID, @ApplicationDate,  @ApplicationStatus, @LastStatusDate,  @CreatedByUserID,  @PaidFees);
                                         select Scope_Identity();";
     
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                        {
                            NewID = InsertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        NewID = -1;
                        string sourceName = "DVLD1";
                        // Create the event source if it does not exist
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return NewID;
                }
            }
              
            
        }
        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, int ApplicationTypeID, DateTime ApplicationDate, int ApplicationStatus, DateTime LastStatusDate, int CreatedByUserID, decimal PaidFees)
        {
            int AffectedRows = 0;
            string query = @"Update Applications set ApplicantPersonID=@ApplicantPersonID
                                                    ,ApplicationTypeID=@ApplicationTypeID
                                                    ,ApplicationDate=@ApplicationDate
                                                    ,ApplicationStatus=@ApplicationStatus
                                                    ,LastStatusDate=@LastStatusDate
                                                    ,CreatedByUserID=@CreatedByUserID
                                                    ,PaidFees=@PaidFees
                                         where ApplicationID=@ApplicationID;";
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);




                    try
                    {
                        connection.Open();
                        AffectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        AffectedRows = 0;
                        string sourceName = "DVLD1";
                        // Create the event source if it does not exist
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return (AffectedRows > 0);
                }
            }
            
        }
        public static bool DeleteApplication(int ApplicationID)
        {
            int AffectedRows = 0;
            string query = "Delete From Applications Where ApplicationID=@ApplicationID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {


                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    try
                    {
                        connection.Open();
                        AffectedRows = command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        AffectedRows = 0;
                        string sourceName = "DVLD1";
                        // Create the event source if it does not exist
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return (AffectedRows > 0);
                }
            }
           
        }
        //For any Application Type(NewLocal-Renew-Retake-Release...)
        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;
            string query = @"select ApplicationID as ActiveAppID 
                            from Applications 
                            where ApplicantPersonID=1 And ApplicationStatus=1 And ApplicationTypeID=1;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            ActiveApplicationID = ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        ActiveApplicationID = -1;
                        string sourceName = "DVLD1";
                        // Create the event source if it does not exist
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return ActiveApplicationID;
                }
            }
                
            
            
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return (GetActiveApplicationID(PersonID,ApplicationTypeID)!=-1);
        }
        public static int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID,int ApplicationTypeID,int LicenseClassID)
        {
            int ActiveApplicationID = -1;
            string query = @"select Applications.ApplicationID 
                             from Applications inner join LocalDrivingLicenseApplications on Applications.ApplicationID=LocalDrivingLicenseApplications.ApplicationID
                             where ApplicantPersonID=1 And ApplicationTypeID=@ApplicationTypeID And ApplicationStatus=1 And LocalDrivingLicenseApplications.LicenseClassID=@LicenseClassID;";
        
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            ActiveApplicationID = ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        ActiveApplicationID = -1;
                        string sourceName = "DVLD1";
                        // Create the event source if it does not exist
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return ActiveApplicationID;
                }
            }
               
            
        }
        //For Only Application Type=(NewLocal)
        public static bool DoesPersonHaveActiveApplicationForLicenseClass(int ApplicantPersonID, int ApplicationTypeID, int LicenseClassID)
        {
            return (GetActiveApplicationIDForLicenseClass(ApplicantPersonID, ApplicationTypeID,LicenseClassID) != -1);
        }
        public static bool UpdateStatus(int ApplicationID,int NewStatus)
        {
            int AffectedRows = 0;
            string query = @"Update Applications set ApplicationStatus=@ApplicationStatus
                                                    ,LastStatusDate=@LastStatusDate
                                    where ApplicationID=@ApplicationID;";
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
                    command.Parameters.AddWithValue("@ApplicationStatus", NewStatus);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);




                    try
                    {
                        connection.Open();
                        AffectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        AffectedRows = 0;
                        string sourceName = "DVLD1";
                        // Create the event source if it does not exist
                        if (!EventLog.SourceExists(sourceName))
                        {
                            EventLog.CreateEventSource(sourceName, "Application");
                        }
                        EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return (AffectedRows > 0);
                }
            }
                
            
        }




    }
}
