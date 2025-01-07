using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class clsCountryData
    {
        public static DataTable GetAllCountriesList()
        {
            DataTable dtCountry = new DataTable();
            string query = "Select * From Countries;";

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
                            dtCountry.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
            return dtCountry;
        }

        public static bool GetCountryInfoByID(int CountryID, ref string CountryName)
        {
            bool IsFound = false;
            string query = "Select * From Countries where CountryID=@CountryID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryID", CountryID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            CountryName = (string)reader["CountryName"];
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

        public static bool GetCountryInfoByName(string CountryName, ref int CountryID)
        {
            bool IsFound = false;
            string query = "Select * From Countries where CountryName=@CountryName;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryName", CountryName);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IsFound = true;
                            CountryID = (int)reader["CountryID"];
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
