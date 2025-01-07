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
    public class clsPersonData
    {
        public static DataTable GetAllPeopleList()
        {




            DataTable dtPeople = new DataTable();
            string query = @"SELECT dbo.People.PersonID, dbo.People.NationalNo, dbo.People.FirstName, dbo.People.SecondName, ISNULL(dbo.People.ThirdName,'') as ThirdName, dbo.People.LastName, 
                           CASE WHEN dbo.People.Gendor = 1 THEN 'Male' ELSE 'Female' END AS GendorCaption, dbo.People.DateOfBirth, dbo.Countries.CountryName as Nationality, dbo.People.Phone, dbo.People.Email
                           FROM dbo.Countries INNER JOIN
                           dbo.People ON dbo.Countries.CountryID = dbo.People.NationalityCountryID;";
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
                            dtPeople.Load(reader);
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
            return dtPeople;
        }

        public static bool IsExist(int PersonID)
        {
            bool IsFound = false;
            string query = "SELECT 1 FROM People WHERE PersonID = @PersonID";
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

        public static bool IsExist(string NationalNo)
        {
            bool IsFound = false;
            string query = "SELECT 1 FROM People WHERE NationalNo = @NationalNo";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
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

        public static bool FindByPersonID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref int Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool IsFound = false;
            string query = "SELECT * FROM People WHERE PersonID = @PersonID";
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

                            NationalNo = (string)reader["NationalNo"];
                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];
                            ThirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"];
                            LastName = (string)reader["LastName"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gendor = (int)reader["Gendor"];
                            Address = (string)reader["Address"];
                            Phone = (string)reader["Phone"];
                            Email = (string)reader["Email"];
                            NationalityCountryID = (int)reader["NationalityCountryID"];
                            ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"];
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

        public static bool FindByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref int Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool IsFound = false;
            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            PersonID = (int)reader["PersonID"];
                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];
                            ThirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"];
                            LastName = (string)reader["LastName"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gendor = (int)reader["Gendor"];
                            Address = (string)reader["Address"];
                            Phone = (string)reader["Phone"];
                            Email = (string)reader["Email"];
                            NationalityCountryID = (int)reader["NationalityCountryID"];
                            ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"];
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



        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, int Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int NewID = -1;
            string query = @"Insert Into People (NationalNo,FirstName,SecondName,ThirdName,LastName,DateOfBirth,Gendor,Address,Phone,Email,NationalityCountryID,ImagePath) 
                                         values (@NationalNo,@FirstName,@SecondName,@ThirdName,@LastName,@DateOfBirth,@Gendor,@Address,@Phone,@Email,@NationalityCountryID,@ImagePath);
                                         select Scope_Identity();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                    command.Parameters.AddWithValue("@ImagePath", ImagePath ?? "");
                    command.Parameters.AddWithValue("@ThirdName", ThirdName ?? "");

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    {
                        NewID = InsertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return NewID;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, int Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int AffectedRows = 0;
            string query = @"Update People set NationalNo=@NationalNo,
                                               FirstName=@FirstName,
                                               SecondName=@SecondName,
                                               ThirdName=@ThirdName,
                                               LastName=@LastName,
                                               DateOfBirth=@DateOfBirth,
                                               Gendor=@Gendor,
                                               Address=@Address,
                                               Phone=@Phone,
                                               Email=@Email,
                                               NationalityCountryID=@NationalityCountryID,
                                               ImagePath=@ImagePath
                                         where PersonID=@PersonID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                    command.Parameters.AddWithValue("@ImagePath", ImagePath ?? "");
                    command.Parameters.AddWithValue("@ThirdName", ThirdName ?? "");

                    connection.Open();
                    AffectedRows = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return (AffectedRows > 0);
        }

        public static bool DeletePerson(int PersonID)
        {
            int AffectedRows = 0;
            string query = "Delete From People Where PersonID=@PersonID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();
                    AffectedRows = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return (AffectedRows > 0);
        }

        private static void LogError(Exception ex)
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
}
