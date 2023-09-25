using Npgsql;
using System;

namespace try2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Aqsin1213;Database=Mytask;";
            int userIdToSelect = 12;
            int userIdToUpdate = 1; 
            int userIdToDelete = 6; 

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                       
                        SelectUser(connection, transaction, userIdToSelect);

                        UpdateUserName(connection, transaction, userIdToUpdate, "UpdatedNameAgshin");

                        DeleteUser(connection, transaction, userIdToDelete);
                        InsertNewUser(connection, transaction, 12, "mmmm@mmm.com", "pass", "someht", "someh", DateTime.Now);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                       
                        transaction.Rollback();
                        Console.WriteLine("Transaction Error: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        static void InsertNewUser(NpgsqlConnection connection, NpgsqlTransaction transaction, int user_id, string email, string password, string name, string location, DateTime join_date)
        {
            using (NpgsqlCommand insertCmd = new NpgsqlCommand())
            {
                insertCmd.Connection = connection;
                insertCmd.Transaction = transaction;

                insertCmd.CommandText = "INSERT INTO users (user_id, email, password, name, location, join_date) VALUES (@user_id, @email, @password, @name, @location, @join_date)";
                insertCmd.Parameters.AddWithValue("@user_id", user_id);
                insertCmd.Parameters.AddWithValue("@email", email);
                insertCmd.Parameters.AddWithValue("@password", password);
                insertCmd.Parameters.AddWithValue("@name", name);
                insertCmd.Parameters.AddWithValue("@location", location);
                insertCmd.Parameters.AddWithValue("@join_date", join_date);

                int insertRowsAffected = insertCmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Inserted: {insertRowsAffected}");
            }
        }

        static void UpdateUserName(NpgsqlConnection connection, NpgsqlTransaction transaction, int userIdToUpdate, string newName)
        {
            using (NpgsqlCommand updateCmd = new NpgsqlCommand())
            {
                updateCmd.Connection = connection;
                updateCmd.Transaction = transaction;

                updateCmd.CommandText = "UPDATE users SET name = @newName WHERE user_id = @userId";
                updateCmd.Parameters.AddWithValue("@newName", newName);
                updateCmd.Parameters.AddWithValue("@userId", userIdToUpdate);

                int updateRowsAffected = updateCmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Updated: {updateRowsAffected}");
            }
        }

        static void DeleteUser(NpgsqlConnection connection, NpgsqlTransaction transaction, int userIdToDelete)
        {
            using (NpgsqlCommand deleteCmd = new NpgsqlCommand())
            {
                deleteCmd.Connection = connection;
                deleteCmd.Transaction = transaction;

                deleteCmd.CommandText = "DELETE FROM users WHERE user_id = @userId";
                deleteCmd.Parameters.AddWithValue("@userId", userIdToDelete);

                int deleteRowsAffected = deleteCmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Deleted: {deleteRowsAffected}");
            }
        }

        static void SelectUser(NpgsqlConnection connection, NpgsqlTransaction transaction, int userId)
        {
            using (NpgsqlCommand selectCmd = new NpgsqlCommand())
            {
                selectCmd.Connection = connection;
                selectCmd.Transaction = transaction;

                selectCmd.CommandText = "SELECT user_id, email, name, location FROM users WHERE user_id = @userId";
                selectCmd.Parameters.AddWithValue("@userId", userId);

                using (NpgsqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userIdSelected = reader.GetInt32(0);
                        string email = reader.GetString(1);
                        string name = reader.GetString(2);
                        string location = reader.GetString(3);

                        Console.WriteLine($"Selected User Info - User ID: {userIdSelected}, Email: {email}, Name: {name}, Location: {location}");
                    }
                    else
                    {
                        Console.WriteLine("User not found.");
                    }
                }
            }
        }
    }
}
