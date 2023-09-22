using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace try2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Aqsin1213;Database=Mytask;";

            Users newUser = new Users
            {
                user_id = 1,
                name = "Test",
                email = "somthing@something.com",
                password = "password",
                location = "Baku",
                join_date = DateTime.Now,                
            };

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    cmd.CommandText = "INSERT INTO users (user_id, email, password, name, location, join_date) VALUES (@id, @email, @password, @name, @location, @join_date)";
                    cmd.Parameters.AddWithValue("@id", newUser.user_id);
                    cmd.Parameters.AddWithValue("@email", newUser.email);
                    cmd.Parameters.AddWithValue("password", newUser.password);
                    cmd.Parameters.AddWithValue("@name", newUser.name);
                    cmd.Parameters.AddWithValue("@location", newUser.location);
                    cmd.Parameters.AddWithValue("@join_date", newUser.join_date);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add user.");
                    }
                }
            }
        }
    }
}
