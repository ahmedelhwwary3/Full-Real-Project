using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationData
    {








        public static DataTable GetAllLocalDrivingLicenseApplicationsList()
        {
            DataTable dtLocalDrivingLicenseApplications = new DataTable();
            string query = @"select * from LocalDrivingLicenseApplications_View order by LocalDrivingLicenseApplications_View.ApplicationDate desc;";

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
                            dtLocalDrivingLicenseApplications.Load(reader);
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
            return dtLocalDrivingLicenseApplications;
        }

        public static bool IsExist(int LocalDrivingLicenseApplicationID)
        {
            bool IsFound = false;
            string query = "Select IsFound=1 From LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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

        public static bool FindByID(int LocalDrivingLicenseApplicationID, ref int LicenseClassID, ref int ApplicationID)
        {
            bool IsFound = false;
            string query = @"  Select * From LocalDrivingLicenseApplications Where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID;";

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
                            LicenseClassID = (int)reader["LicenseClassID"];
                            ApplicationID = (int)reader["ApplicationID"];
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

        public static bool FindByApplicationID(int ApplicationID, ref int LicenseClassID, ref int LocalDrivingLicenseApplicationID)
        {
            bool IsFound = false;
            string query = @"select * from LocalDrivingLicenseApplications ld join Applications a on a.ApplicationID=ld.ApplicationID 
                     where a.ApplicationID=@ApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            LicenseClassID = (int)reader["ld.LicenseClassID"];
                            LocalDrivingLicenseApplicationID = (int)reader["ld.LocalDrivingLicenseApplicationID"];
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

        public static int AddNewLocalDrivingLicenseApplication(int LicenseClassID, int ApplicationID)
        {
            int NewID = -1;
            string query = @"Insert Into LocalDrivingLicenseApplications ( LicenseClassID, ApplicationID) 
                                 values ( @LicenseClassID, @ApplicationID);
                                 select Scope_Identity();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
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

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int LicenseClassID, int ApplicationID)
        {
            int AffectedRows = 0;
            string query = @"Update LocalDrivingLicenseApplications set LicenseClassID=@LicenseClassID
                                                               ,ApplicationID=@ApplicationID
                                 where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int AffectedRows = 0;
            string query = @"Delete From LocalDrivingLicenseApplications Where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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






        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool TestResult = false;
            string query2 = @"select top 1 t.TestResult as TestResult from
                      (Tests t join TestAppointments ta on t.TestAppointmentID=ta.TestAppointmentID
                      join LocalDrivingLicenseApplications ld on ld.LocalDrivingLicenseApplicationID=ta.LocalDrivingLicenseApplicationID 
                      join TestTypes tt on tt.TestTypeID=ta.TestTypeID )
                      where (ld.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID And tt.TestTypeID=@TestTypeID)
                      order by ta.AppointmentDate desc";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query2, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && bool.TryParse(result.ToString(), out bool Result))
                    {
                        TestResult = Result;
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
            return TestResult;
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool TestResult = false;
            string query2 = @"select top 1 isFound=1
                      from (TestAppointments ta join Tests t on t.TestAppointmentID=ta.TestAppointmentID
                      join LocalDrivingLicenseApplications ld on ld.LocalDrivingLicenseApplicationID=ta.LocalDrivingLicenseApplicationID )
                      where (ld.LocalDrivingLicenseApplicationID=1 And ta.TestTypeID=1)
                      order by ta.TestAppointmentID desc";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query2, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    SqlDataReader Reader = command.ExecuteReader();
                    TestResult = Reader.HasRows;
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
            return TestResult;
        }

        public static int TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            int TotalTrialsPerTest = 0;
            string query = @"SELECT TotalTrialsPerTest = count(TestID)
                     FROM LocalDrivingLicenseApplications INNER JOIN
                          TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                          Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                     WHERE
                     (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                     AND(TestAppointments.TestTypeID = @TestTypeID)";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int Trials))
                    {
                        TotalTrialsPerTest = Trials;
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
            return TotalTrialsPerTest;
        }

        public static int GetActiveScheduledTestID(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            int TestAppointmentID = -1;
            string query = @"SELECT TestAppointments.TestAppointmentID
                     FROM LocalDrivingLicenseApplications FUll outer JOIN
                          TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID Full Outer JOIN
                          Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID Full Outer JOIN
                          TestTypes ON TestAppointments.TestTypeID = TestTypes.TestTypeID
                     Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID And TestTypes.TestTypeID=@TestTypeID And IsLocked=0";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int ID))
                    {
                        TestAppointmentID = ID;
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
            return TestAppointmentID;
        }

        public static bool DoesHaveActiveScheduledTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return (GetActiveScheduledTestID(LocalDrivingLicenseApplicationID, TestTypeID) != -1);
        }

        public static int GetAllPassedTests(int LocalDrivingLicenseApplicationID)
        {
            int AllPassedTests = 0;
            string query = @"select Count(tt.TestTypeID) as ActiveLicenseID from (
                     TestAppointments ta join Tests t on t.TestAppointmentID=ta.TestAppointmentID
                     join TestTypes tt on tt.TestTypeID=ta.TestTypeID
                     join LocalDrivingLicenseApplications ld on ld.LocalDrivingLicenseApplicationID=ta.LocalDrivingLicenseApplicationID)
                     where ld.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID And t.TestResult=1";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int TotalPassed))
                    {
                        AllPassedTests = TotalPassed;
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
            return AllPassedTests;
        }

        public static bool DoesPassAllTestTypes(int LocalDrivingLicenseApplicationID)
        {
            return (GetAllPassedTests(LocalDrivingLicenseApplicationID) == 3);
        }

        public static int GetActiveLicenseID(int LocalDrivingLicenseApplicationID)
        {
            int ActiveLicenseID = -1;
            string query = @"select li.LicenseID from (Licenses li join LocalDrivingLicenseApplications ld on ld.ApplicationID= li.ApplicationID
                     join Applications ap on ap.ApplicationID=ld.ApplicationID)
                     where (ld.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID And li.IsActive=1)";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int ID))
                    {
                        ActiveLicenseID = ID;
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
            return ActiveLicenseID;
        }












    }
}
