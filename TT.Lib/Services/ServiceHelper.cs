using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace TT.Lib.Services
{
    public static class ServiceHelper
    {
        public static async Task ExecuteReaderAsync(
            string connectionString, 
            string query, 
            CommandType commandType, 
            Func<SqlDataReader, Task> readData,
            SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = commandType;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    await readData(reader);
                }
            }
        }
    }
}
