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
        public class clsDriverData
        {
            public static DataTable GetAllDriversList()
            {
                DataTable dtDriver = new DataTable();
                string query = "select * from Drivers_View ;";
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
                                dtDriver.Load(reader);
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
                return dtDriver;
            }

            public static bool IsExist(int DriverID)
            {
                bool IsFound = false;
                string query = "Select IsFound=1 From Drivers where DriverID=@DriverID;";
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
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

            public static bool IsDriver(int PersonID)
            {
                bool IsFound = false;
                string query = "Select IsFound=1 From Drivers where PersonID=@PersonID;";
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
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

            public static int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
            {
                int NewID = -1;
                string query = @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate ) 
                             values (@PersonID,@CreatedByUserID,@CreatedDate);
                             select Scope_Identity();";
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

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

            public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
            {
                int AffectedRows = 0;
                string query = @"Update Driver set  Driver=@Driver, PersonID=@PersonID, CreatedByUserID=@CreatedByUserID, CreatedDate=@CreatedDate
                             where DriverID=@DriverID;";
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

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

            public static bool DeleteDriver(int DriverID)
            {
                int AffectedRows = 0;
                string query = "Delete From Drivers Where DriverID=@DriverID;";
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

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

            public static bool FindByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
            {
                bool IsFound = false;
                string query = @"Select * From Drivers where PersonID=@PersonID;";
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
                                DriverID = (int)reader["DriverID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
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

            public static bool FindByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
            {
                bool IsFound = false;
                string query = @"Select * From Drivers where DriverID=@DriverID;";
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                PersonID = (int)reader["PersonID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
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

            public static DataTable GetAllLocalLicenses(int DriverID)
            {
                DataTable dtDriver = new DataTable();
                string query = @"select LicenseID, ApplicationID, ClassName, IssueDate, ExpirationDate, IsActive
                             from Licenses join LicenseClasses on LicenseClassID = LicenseClass 
                             where DriverID = @DriverID;";
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
                                dtDriver.Load(reader);
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
                return dtDriver;
            }

            public static DataTable GetAllInternationalLicenses(int DriverID)
            {
                DataTable dtDriver = new DataTable();
                string query = @"select * from InternationalLicenses where DriverID = @DriverID;";
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
                                dtDriver.Load(reader);
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
                return dtDriver;
            }
        }
    }


