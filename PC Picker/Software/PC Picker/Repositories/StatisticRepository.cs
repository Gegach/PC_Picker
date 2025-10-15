using DBLayer;
using PC_Picker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PC_Picker.Repositories
{
    public class StatisticRepository
    {
        public static List<Statistic> GetStatistics (int id)
        {
             List<Statistic> statistics = new List<Statistic>();

            string sql = $"SELECT * FROM Statistic WHERE ComponentId = '{id}'";

            DB.OpenConnection();
            var reader = DB.GetDataReader(sql);

            while (reader.Read())
            {
                Statistic statistic = CreateObject(reader);
                statistics.Add(statistic);
            }

            reader.Close();
            DB.CloseConnection();

            return statistics;
        }

        private static Statistic CreateObject(SqlDataReader reader)
        {
            int statisticId = int.Parse(reader["StatisticId"].ToString());
            int componentId = int.Parse(reader["ComponentId"].ToString());
            int employeeId = int.Parse(reader["EmployeeId"].ToString());
            DateTime actionDate = DateTime.Parse(reader["ActionDate"].ToString());
            string actionType = reader["ActionType"].ToString();
            int quantity = int.Parse(reader["Quantity"].ToString());

            var statistic = new Statistic
            {
                StatisticId = statisticId,
                ComponentId = componentId,
                EmployeeId = employeeId,
                ActionDate = actionDate,
                ActionType = actionType,
                Quantity = quantity
            };

            return statistic;
        }

        public static void AddStatistic (Component component, Employee employee)
        {
            string sql = $"INSERT INTO Statistic (ComponentId, EmployeeId, ActionDate, ActionType, Quantity) " +
                $"VALUES ('{component.Id}', '{employee.Id}', GETDATE(), 'Odabrana', 1);";

            DB.OpenConnection();
            DB.ExecuteCommand(sql);
            DB.CloseConnection();
        }
        public static List<Models.Component> GetMostSelectedComponents()
        {
            var mostSelectedComponents = new List<Models.Component>();
            int n = 10;

            string sql = $"SELECT TOP {n} s.ComponentId, COUNT(*) AS SelectionCount " +
                $"FROM Statistic s " +
                $"WHERE s.ActionType = 'odabrana' " +
                $"GROUP BY s.ComponentId " +
                $"ORDER BY SelectionCount DESC ";

            DB.OpenConnection();
            var reader = DB.GetDataReader(sql);

            while (reader.Read())
            {
                int componentId = int.Parse(reader["ComponentId"].ToString());
                int selectionCount = int.Parse(reader["SelectionCount"].ToString());
                var component = ComponentRepository.GetComponent(componentId);

                mostSelectedComponents.Add(component);
            }
            reader.Close();
            DB.CloseConnection();

            return mostSelectedComponents;
        }
    }
}
