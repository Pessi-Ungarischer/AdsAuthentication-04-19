using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdsAuthentication.Data
{
    public class AdsAuthenticationRepository
    {

        private static string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=AdsAuthentication; Integrated Security=true;";

        public void SignUp(User user, string password)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Members (Name, Email, PasswordHash) " +
                                  "VALUES (@name, @email, @passwordHash)";
            command.Parameters.AddWithValue("@passwordHash", BCrypt.Net.BCrypt.HashPassword(password));
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email);
            connection.Open();

            command.ExecuteNonQuery();
        }

        public bool Login(string email, string password)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Members " +
                                  "WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return false;
            }


            string passwordHash = (string)reader["PasswordHash"];

            if (!BCrypt.Net.BCrypt.Verify(password, passwordHash))
            {
                return false;

            }
            return true;
        }


        public void NewAd(Ad ad)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Ads (ListerId, DATE, PhoneNum, Description) " +
                                  "VALUES (@listerId, @date, @PhoneNum, @description)";
            command.Parameters.AddWithValue("@listerId", ad.ListerId);
            command.Parameters.AddWithValue("@date", ad.Date);
            command.Parameters.AddWithValue("@PhoneNum", ad.PhoneNum);
            command.Parameters.AddWithValue("@description", ad.Description);
            connection.Open();

            command.ExecuteNonQuery();
        }

        public User GetUserByEmail(string email)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Members " +
                                  "WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();


            using SqlDataReader reader = command.ExecuteReader();

            if(!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"],
            };
        }

        public List<Ad> GetAds()
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT a.*, m.Name FROM Ads a " +
                                  "JOIN Members m " +
                                  "ON A.ListerId = m.Id";
            connection.Open();


            SqlDataReader reader = command.ExecuteReader();
            List<Ad> ads = new();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    ListerId = (int)reader["ListerId"],
                    ListerName = (string)reader["Name"],
                    PhoneNum = (string)reader["PhoneNum"],
                    Date = (DateTime)reader["Date"],
                    Description = (string)reader["Description"]
                });

            }
            return ads;
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Ads " +
                                  "WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
