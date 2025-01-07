using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class clsApplicationTypeData
    {
        public static bool IsExist(int ApplicationTypeID)
        {
            bool IsFound = false;
            string query = "Select IsFound=1 From ApplicationTypes where ApplicationTypeID=@ApplicationTypeID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
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
                    LogError(ex);
                }
            }
            return IsFound;
        }

        public static bool Find(int ApplicationTypeID, ref string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            bool IsFound = false;
            string query = "Select * From ApplicationTypes where ApplicationTypeID=@ApplicationTypeID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                            ApplicationFees = (decimal)reader["ApplicationFees"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
            return IsFound;
        }

        public static DataTable GetAllApplicationTypesList()
        {
            DataTable dtUsers = new DataTable();
            string query = "Select * From ApplicationTypes;";

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
                            dtUsers.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
            return dtUsers;
        }

        public static int AddNewApplicationType(string ApplicationTypeTitle, decimal ApplicationTypeFees)
        {
            int NewID = -1;
            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle, ApplicationTypeFees) 
                             values (@ApplicationTypeTitle, @ApplicationTypeFees);
                             select Scope_Identity();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                command.Parameters.AddWithValue("@ApplicationTypeFees", ApplicationTypeFees);

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
                    LogError(ex);
                }
            }
            return NewID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            int AffectedRows = 0;
            string query = @"Update ApplicationTypes set ApplicationTypeTitle=@ApplicationTypeTitle,
                                                     ApplicationFees=@ApplicationFees
                             where ApplicationTypeID=@ApplicationTypeID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

                try
                {
                    connection.Open();
                    AffectedRows = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
            return (AffectedRows > 0);
        }

        private static void LogError(Exception ex)
        {
            string sourceName = "DVLD1";
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }
            EventLog.WriteEntry(sourceName, $"{ex}", EventLogEntryType.Error);
        }
    }
}
