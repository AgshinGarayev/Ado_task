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
            int userIdToUpdate = 2;
            int userIdToDelete = 2; 

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        InsertNewUser(connection, transaction, 7, "email@tryemail.com", "password123", "Username123", "mylocation");




                        using (NpgsqlCommand updateCmd = new NpgsqlCommand())
                        {
                            updateCmd.Connection = connection;
                            updateCmd.Transaction = transaction;

                            updateCmd.CommandText = "UPDATE users SET name = @newName WHERE user_id = @userId";
                            updateCmd.Parameters.AddWithValue("@newName", "UpdatedNameAgshin");
                            updateCmd.Parameters.AddWithValue("@userId", userIdToUpdate);

                            int updateRowsAffected = updateCmd.ExecuteNonQuery();
                            Console.WriteLine($"Rows Updated: {updateRowsAffected}");
                        }


                        using (NpgsqlCommand deleteCmd = new NpgsqlCommand())
                        {
                            deleteCmd.Connection = connection;
                            deleteCmd.Transaction = transaction;

                            deleteCmd.CommandText = "DELETE FROM users WHERE user_id = @userId";
                            deleteCmd.Parameters.AddWithValue("@userId", userIdToDelete);

                            int deleteRowsAffected = deleteCmd.ExecuteNonQuery();
                            Console.WriteLine($"Rows Deleted: {deleteRowsAffected}");
                        }


                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Handle errors and rollback the transaction
                        transaction.Rollback();
                        Console.WriteLine("Transaction Error: " + ex.Message);
                    }
                }
            }
        }



        static void InsertNewUser(NpgsqlConnection connection, NpgsqlTransaction transaction, int user_id, string email, string password, string name, string location)
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
                insertCmd.Parameters.AddWithValue("@join_date", DateTime.Now);

                int insertRowsAffected = insertCmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Inserted: {insertRowsAffected}");
            }
        }
    }
}
