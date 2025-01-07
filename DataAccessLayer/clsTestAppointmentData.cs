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
    public class clsTestAppointmentData
    {



        public static DataTable GetAllTestAppointmentsListAllTypes(int LocalDrivingLicenseApplicationID)
        {
            DataTable dtTestAppointment = new DataTable();
            string query = @"select * from TestAppointments_View Where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dtTestAppointment.Load(reader);
                        }
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
            return dtTestAppointment;
        }

        public static DataTable GetAllTestAppointmentsListPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dtTestAppointment = new DataTable();
            string query = @"SELECT
                         dbo.TestAppointments.TestAppointmentID,
                         dbo.TestAppointments.LocalDrivingLicenseApplicationID,
                         dbo.TestTypes.TestTypeTitle,
                         dbo.LicenseClasses.ClassName,
                         dbo.TestAppointments.AppointmentDate,
                         dbo.TestAppointments.PaidFees,
                         dbo.People.FirstName + ' ' + dbo.People.SecondName + ' ' + ISNULL(dbo.People.ThirdName, '') + ' ' + dbo.People.LastName AS FullName,
                         dbo.TestAppointments.IsLocked
                     FROM
                         dbo.TestAppointments
                         INNER JOIN dbo.TestTypes ON dbo.TestAppointments.TestTypeID = dbo.TestTypes.TestTypeID
                         INNER JOIN dbo.LocalDrivingLicenseApplications ON dbo.TestAppointments.LocalDrivingLicenseApplicationID = dbo.LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                         INNER JOIN dbo.Applications ON dbo.LocalDrivingLicenseApplications.ApplicationID = dbo.Applications.ApplicationID
                         INNER JOIN dbo.People ON dbo.Applications.ApplicantPersonID = dbo.People.PersonID
                         INNER JOIN dbo.LicenseClasses ON dbo.LocalDrivingLicenseApplications.LicenseClassID = dbo.LicenseClasses.LicenseClassID
                     WHERE
                         dbo.TestAppointments.TestTypeID = @TestTypeID
                         AND dbo.LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dtTestAppointment.Load(reader);
                        }
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
            return dtTestAppointment;
        }

        public static bool GetLastTestAppointmentInfo(int LocalDrivingLicenseApplicationID, int TestTypeID, ref int TestAppointmentID, ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;
            string query = @"select top 1 * from TestAppointments ta
                     where ta.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID And ta.TestTypeID=@TestTypeID
                     order by TestAppointmentID desc;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = (decimal)reader["PaidFees"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];
                            RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsFound = false;
                    string sourceName = "DVLD1";
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }
            return IsFound;
        }

        public static bool IsExist(int TestAppointmentID)
        {
            bool IsFound = false;
            string query = "Select IsFound=1 From TestAppointments where TestAppointmentID=@TestAppointmentID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }
            return IsFound;
        }









        public static bool FindintstAppointmentID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;
            string query = "Select * From TestAppointments where TestAppointmentID=@TestAppointmentID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            TestTypeID = (int)reader["TestTypeID"];
                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = (decimal)reader["PaidFees"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];
                            RetakeTestApplicationID = reader["RetakeTestApplicationID"] != DBNull.Value ? (int)reader["RetakeTestApplicationID"] : -1;
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

        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int NewID = -1;
            string query = @"Insert Into TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID) 
                     values (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,@IsLocked,@RetakeTestApplicationID);
                     select Scope_Identity();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID != -1 ? RetakeTestApplicationID : (object)DBNull.Value);

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
                    string sourceName = "DVLD1";
                    if (!EventLog.SourceExists(sourceName))
                    {
                        EventLog.CreateEventSource(sourceName, "Application");
                    }
                    EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
                }
            }
            return NewID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int AffectedRows = 0;
            string query = @"Update TestAppointments set TestTypeID=@TestTypeID,LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID,AppointmentDate=@AppointmentDate,PaidFees=@PaidFees,CreatedByUserID=@CreatedByUserID,IsLocked=@IsLocked,RetakeTestApplicationID=@RetakeTestApplicationID              
                     where TestAppointmentID=@TestAppointmentID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

                try
                {
                    connection.Open();
                    AffectedRows = command.ExecuteNonQuery();
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
            return (AffectedRows > 0);
        }

        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            int AffectedRows = 0;
            string query = "Delete From TestAppointments Where TestAppointmentID=@TestAppointmentID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                try
                {
                    connection.Open();
                    AffectedRows = command.ExecuteNonQuery();
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
            return (AffectedRows > 0);
        }

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            string query = @"select t.TestID from TestAppointments ta join Tests t on t.TestAppointmentID=ta.TestAppointmentID
                     where ta.TestAppointmentID=@TestAppointmentID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int ID))
                    {
                        TestID = ID;
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
            return TestID;
        }






    }
}
