using DBLayer;
using PC_Picker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Picker.Repositories
{
    public class EmployeeRepository
    {
        public static Employee GetEmployee(string username)
        {
            string sql = $"SELECT * FROM Employee WHERE Username = '{username}'";
            return FetchEmployee(sql);
        }

        private static Employee FetchEmployee(string sql)
        {
            DB.OpenConnection();
            var reader = DB.GetDataReader(sql);
            Employee employee = null;

            if (reader.HasRows)
            {
                reader.Read();
                employee = CreateObject(reader);
                reader.Close();
            }
            DB.CloseConnection();

            return employee;
        }

        private static Employee CreateObject(SqlDataReader reader)
        {
            int id = int.Parse(reader["Id"].ToString());
            string firstName = reader["FirstName"].ToString();
            string lastName = reader["LastName"].ToString();
            string username = reader["Username"].ToString();
            string password = reader["Password"].ToString();
            string email = reader["Email"].ToString();

            var employee = new Employee
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Password = password,
                Email = email
            };

            return employee;
        }
    }
}