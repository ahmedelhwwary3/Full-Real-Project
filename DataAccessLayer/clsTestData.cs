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
    public class clsTestData
    {

        public static DataTable GetAllTestsList()
        {
            DataTable dtTest = new DataTable();
            string query = "select * from Tests order by Tests.TestID ;";
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
                            dtTest.Load(reader);
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
            return dtTest;
        }

        public static bool IsExist(int TestID)
        {
            bool IsFound = false;
            string query = "Select IsFound=1 From Tests where TestID=@TestID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestID", TestID);
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

        public static int GetLastTestIDByPersonAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID, int TestTypeID)
        {
            int TestID = -1;

            string query = @"select top 1 t.TestID from TestAppointments ta inner join Tests t on t.TestAppointmentID=ta.TestAppointmentID 
                            inner join LocalDrivingLicenseApplications ld on ld.LocalDrivingLicenseApplicationID=ta.LocalDrivingLicenseApplicationID
                            inner join LicenseClasses lc on lc.LicenseClassID=ld.LicenseClassID
                            inner join Applications a on a.ApplicationID=ld.ApplicationID
                            where ta.TestTypeID=@TestTypeID And a.ApplicantPersonID=@PersonID And lc.LicenseClassID=@LicenseClassID
                            order by t.TestID desc
                            ";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool IsFound = false;
            string query = "Select * From Tests where TestID=@TestID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestID", TestID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            Notes = (string)reader["Notes"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
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

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int NewID = -1;
            string query = @"update TestAppointments set TestAppointments.IsLocked=1 where TestAppointmentID=@TestAppointmentID
                             insert into Tests (TestAppointmentID,TestResult,Notes,CreatedByUserID) Values(@TestAppointmentID,@TestResult,@Notes,@CreatedByUserID);
                             select SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                command.Parameters.AddWithValue("@TestResult", TestResult);
                command.Parameters.AddWithValue("@Notes", Notes);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int AffectedRows = 0;
            string query = @"Update Test set  TestAppointmentID=@TestAppointmentID , TestResult=@TestResult,Notes=@Notes,CreatedByUserID=@CreatedByUserID
                                    where TestID=@TestID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestID", TestID);
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                command.Parameters.AddWithValue("@TestResult", TestResult);
                command.Parameters.AddWithValue("@Notes", Notes);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool DeleteTest(int TestID)
        {
            int AffectedRows = 0;
            string query = "Delete From Tests Where TestID=@TestID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestID", TestID);
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

        public static int GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            int PassedTestCount = 0;
            string query = @"select count(t.TestID)asPassedTests from Tests t inner join TestAppointments ta on t.TestAppointmentID=ta.TestAppointmentID
                             inner join LocalDrivingLicenseApplications l on l.LocalDrivingLicenseApplicationID=ta.LocalDrivingLicenseApplicationID
                             where l.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID And t.TestResult=1;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int Count))
                    {
                        PassedTestCount = Count;
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
            return PassedTestCount;
        }
    }
}
