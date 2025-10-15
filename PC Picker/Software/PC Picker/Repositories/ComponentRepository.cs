using DBLayer;
using Microsoft.Win32;
using PC_Picker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PC_Picker.Repositories
{
    public class ComponentRepository
    {
        public static Component GetComponent (int id)
        {
            Component component = null;
            string sql = $@"SELECT * FROM Component c
                   LEFT JOIN Processor p ON c.Id = p.ComponentId
                   LEFT JOIN Motherboard m ON c.Id = m.ComponentId
                   LEFT JOIN GraphicsCard g ON c.Id = g.ComponentId
                   LEFT JOIN Memory mem ON c.Id = mem.ComponentId
                   LEFT JOIN Storage s ON c.Id = s.ComponentId
                   WHERE c.Id = {id}";

            DB.OpenConnection();
            var reader = DB.GetDataReader(sql);

            if (reader.HasRows)
            {
                reader.Read();
                component = CreateObject(reader);
                reader.Close();
            }

            DB.CloseConnection();
            return component;
        }
        public static List<Component> GetComponents()
        {
            List<Component> components = new List<Component>();

            string sql = @"SELECT * FROM Component c
                   LEFT JOIN Processor p ON c.Id = p.ComponentId
                   LEFT JOIN Motherboard m ON c.Id = m.ComponentId
                   LEFT JOIN GraphicsCard g ON c.Id = g.ComponentId
                   LEFT JOIN Memory mem ON c.Id = mem.ComponentId
                   LEFT JOIN Storage s ON c.Id = s.ComponentId";

            DB.OpenConnection();
            var reader = DB.GetDataReader(sql);

            while (reader.Read()) 
            {
                Component component = CreateObject(reader);
                components.Add(component);
            }
            reader.Close();
            DB.CloseConnection();

            return components;
        }

        private static void FillComponent(Component component, SqlDataReader reader)
        {
            component.Id = int.Parse(reader["Id"].ToString());
            component.Name = reader["Name"].ToString();
            component.Manufacturer = reader["Manufacturer"].ToString();
            component.Price = decimal.Parse(reader["Price"].ToString());
            component.Category = reader["Category"].ToString();
            component.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
            component.EmployeeId = int.Parse(reader["EmployeeId"].ToString());
        }

        private static Component CreateObject(SqlDataReader reader)
        {
            string category = reader["Category"].ToString();
            Component component = null;

            switch (category)
            {
                case "Processor":
                    var processor = new Processor();
                    FillComponent(processor, reader);
                    processor.Socket = reader["ProcessorSocket"]?.ToString();
                    processor.CoreCount = reader["CoreCount"] != DBNull.Value ? int.Parse(reader["CoreCount"].ToString()) : 0;
                    processor.Frequency = reader["Frequency"] != DBNull.Value ? float.Parse(reader["Frequency"].ToString()) : 0f;
                    processor.Cache = reader["Cache"] != DBNull.Value ? int.Parse(reader["Cache"].ToString()) : 0;
                    component = processor;
                    break;
                case "Motherboard":
                    var motherboard = new Motherboard();
                    FillComponent(motherboard, reader);
                    motherboard.Chipset = reader["Chipset"]?.ToString();
                    motherboard.Socket = reader["MotherboardSocket"]?.ToString();
                    motherboard.FormFactor = reader["FormFactor"]?.ToString();
                    motherboard.RamSlots = reader["RamSlots"] != DBNull.Value ? int.Parse(reader["RamSlots"].ToString()) : 0;
                    component = motherboard;
                    break;
                case "GraphicsCard":
                    var graphicsCard = new GraphicsCard();
                    FillComponent(graphicsCard, reader);
                    graphicsCard.Memory = reader["GraphicsCardMemory"] != DBNull.Value ? int.Parse(reader["GraphicsCardMemory"].ToString()) : 0;
                    graphicsCard.MemoryType = reader["GraphicsCardMemoryType"]?.ToString();
                    graphicsCard.GPU = reader["GPU"]?.ToString();
                    graphicsCard.Interface = reader["GraphicsCardInterface"]?.ToString();
                    component = graphicsCard;
                    break;
                case "Memory":
                    var memory = new Memory();
                    FillComponent(memory, reader);
                    memory.Capacity = reader["MemoryCapacity"] != DBNull.Value ? int.Parse(reader["MemoryCapacity"].ToString()) : 0;
                    memory.MemoryType = reader["MemoryMemoryType"]?.ToString();
                    memory.Speed = reader["Speed"] != DBNull.Value ? int.Parse(reader["Speed"].ToString()) : 0;
                    component = memory;
                    break;
                case "Storage":
                    var storage = new Storage();
                    FillComponent(storage, reader);
                    storage.Capacity = reader["StorageCapacity"] != DBNull.Value ? int.Parse(reader["StorageCapacity"].ToString()) : 0;
                    storage.Interface = reader["StorageInterface"]?.ToString();
                    storage.StorageType = reader["StorageType"]?.ToString();
                    component = storage;
                    break;
            }
            return component;
        }

        public static void AddComponent(Component component, Employee employee)
        {
            string sql = $"INSERT INTO Component (Name, Manufacturer, Price, Category, CreatedAt, EmployeeId) " +
                         $"VALUES ('{component.Name}', '{component.Manufacturer}', {component.Price}, '{component.Category}', GETDATE(), {employee.Id});";

            DB.OpenConnection();
            DB.ExecuteCommand(sql);


            if (component is Processor processor)
            {
                sql = $"INSERT INTO Processor (ComponentId, CoreCount, Frequency, ProcessorSocket, Cache) " +
                       $"VALUES (SCOPE_IDENTITY(), {processor.CoreCount}, {processor.Frequency}, '{processor.Socket}', {processor.Cache});";
            }
            else if (component is Motherboard motherboard)
            {
                sql = $"INSERT INTO Motherboard (ComponentId, Chipset, MotherboardSocket, FormFactor, RamSlots) " +
                       $"VALUES (SCOPE_IDENTITY(), '{motherboard.Chipset}', '{motherboard.Socket}', '{motherboard.FormFactor}', '{motherboard.RamSlots}');";
            }
            else if (component is GraphicsCard graphicsCard)
            {
                sql = $"INSERT INTO GraphicsCard (ComponentId, GraphicsCardMemory, GraphicsCardMemoryType, GPU, GraphicsCardInterface) " +
                       $"VALUES (SCOPE_IDENTITY(), {graphicsCard.Memory}, '{graphicsCard.MemoryType}', '{graphicsCard.GPU}', '{graphicsCard.Interface}');";
            }
            else if (component is Memory memory)
            {
                sql = $"INSERT INTO Memory (ComponentId, MemoryCapacity, MemoryMemoryType, Speed) " +
                       $"VALUES (SCOPE_IDENTITY(), {memory.Capacity}, '{memory.MemoryType}', '{memory.Speed}');";
            }
            else if (component is Storage storage)
            {
                sql = $"INSERT INTO Storage (ComponentId, StorageCapacity, StorageType, StorageInterface) " +
                       $"VALUES (SCOPE_IDENTITY(), {storage.Capacity}, '{storage.StorageType}', '{storage.Interface}');";
            }

            DB.ExecuteCommand(sql);
            DB.CloseConnection();
        }

        public static void UpdateComponent(Component component, Employee employee)
        {
            string sql = $"UPDATE Component SET " +
                         $"Name = '{component.Name}', " +
                         $"Manufacturer = '{component.Manufacturer}', " +
                         $"Price = {component.Price}, " +
                         $"Category = '{component.Category}', " +
                         $"CreatedAt = GETDATE(), " +
                         $"EmployeeId = {employee.Id} " +
                         $"WHERE Id = {component.Id};";

            DB.OpenConnection();
            DB.ExecuteCommand(sql);

            if (component is Processor processor)
            {
                sql = $"UPDATE Processor SET " +
                       $"CoreCount = {processor.CoreCount}, " +
                       $"Frequency = {processor.Frequency}, " +
                       $"ProcessorSocket = '{processor.Socket}', " +
                       $"Cache = {processor.Cache} " +
                       $"WHERE ComponentId = {component.Id};";
            } 
            else if (component is Motherboard motherboard)
            {
                sql = $"UPDATE Motherboard SET " +
                       $"Chipset = '{motherboard.Chipset}', " +
                       $"MotherboardSocket = '{motherboard.Socket}', " +
                       $"FormFactor = '{motherboard.FormFactor}', " +
                       $"RamSlots = {motherboard.RamSlots} " +
                       $"WHERE ComponentId = {component.Id};";
            }
            else if (component is Storage storage)
            {
                sql = $"UPDATE Storage SET " +
                       $"StorageCapacity = {storage.Capacity}, " +
                       $"StorageType = '{storage.StorageType}', " +
                       $"StorageInterface = '{storage.Interface}' " +
                       $"WHERE ComponentId = {component.Id};";
            }
            else if (component is GraphicsCard graphicsCard)
            {
                sql = $"UPDATE GraphicsCard SET " +
                       $"GraphicsCardMemory = {graphicsCard.Memory}, " +
                       $"GraphicsCardMemoryType = '{graphicsCard.MemoryType}', " +
                       $"GPU = '{graphicsCard.GPU}', " +
                       $"GraphicsCardInterface = '{graphicsCard.Interface}' " +
                       $"WHERE ComponentId = {component.Id};";
            }
            else if (component is Memory memory)
            {
                sql = $"UPDATE Memory SET " +
                       $"MemoryCapacity = {memory.Capacity}, " +
                       $"MemoryMemoryType = '{memory.MemoryType}', " +
                       $"Speed = '{memory.Speed}' " +
                       $"WHERE ComponentId = {component.Id};";
            }

            DB.ExecuteCommand(sql);
            DB.CloseConnection();
        }

        public static void DeleteComponent(Component component)
        {
            string sql = $"DELETE FROM Component WHERE Id = {component.Id};";

            DB.OpenConnection();
            DB.ExecuteCommand(sql);

            if (component is Processor)
            {
                sql = $"DELETE FROM Processor WHERE ComponentId = {component.Id};";
            }
            else if (component is Motherboard)
            {
                sql = $"DELETE FROM Motherboard WHERE ComponentId = {component.Id};";
            }
            else if (component is GraphicsCard)
            {
                sql = $"DELETE FROM GraphicsCard WHERE ComponentId = {component.Id};";
            }
            else if (component is Memory)
            {
                sql = $"DELETE FROM Memory WHERE ComponentId = {component.Id};";
            }
            else if (component is Storage)
            {
                sql = $"DELETE FROM Storage WHERE ComponentId = {component.Id};";
            }

            DB.ExecuteCommand(sql);
            DB.CloseConnection();
        }
    }
}
