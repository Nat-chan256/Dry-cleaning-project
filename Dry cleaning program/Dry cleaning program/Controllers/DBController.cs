using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dry_cleaning_program.Models;

namespace Dry_cleaning_program.Controllers
{
    public class DBController
    {
        private MySqlConnection connection;
        private static DBController instance;

        
        private DBController()
        {
            OpenConnection();
            CreateDatabase();
            CreateTables();

            instance = this;
        }




        /// <summary>
        /// Геттер для получения списка всех клиентов.
        /// </summary>
        /// <returns>Список объектов ClientModel, соответствующих всем клиентам в БД</returns>
        public List<ClientModel> GetAllClientsData()
        {
            List<ClientModel> result = new List<ClientModel>();

            string query = "SELECT * FROM `Clients`;";

            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataReader reader = command.ExecuteReader();

            //Перебираем записи в reader
            while (reader.Read())
            {
                //datesReader.Read();
                try
                {
                    result.Add(new ClientModel(reader[1].ToString(), /*surname*/
                        reader[2].ToString(), /*name*/
                        reader[3].ToString(), /*patronymic*/
                        reader[0].ToString(), /*phone number*/
                        DateTime.Parse(reader[4].ToString()), /*birthDate*/
                        int.Parse(reader[5].ToString())/*orders number*/));
                }
                catch (IndexOutOfRangeException ex)
                {
                    continue;
                }
            }

            reader.Close();

            return result;
        }


        /// <summary>
        /// Получение списка всех текущих заказов.
        /// </summary>
        /// <returns> Список текущих заказов </returns>
        public List<OrderModel> GetAllCurrentOrdersData()
        {
            List<OrderModel> result = new List<OrderModel>();

            string query = "SELECT * FROM Orders WHERE ReturnDate IS NULL";

            //string query = "SELECT * FROM Orders";

            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataReader reader = command.ExecuteReader();

            //Отдельный запрос для дат
            //string datesQuery = "SELECT FORMAT(ReceptionDate, 'yyyy/mm/dd') FROM Orders";

            //MySqlCommand selectDatesCommand = new MySqlCommand(datesQuery, connection);

            //MySqlDataReader datesReader = selectDatesCommand.ExecuteReader();


            //Перебираем записи в reader
            while (reader.Read())
            {
                //datesReader.Read();
                try
                {
                    result.Add(new OrderModel((int)reader[0], /*id*/
                        reader[1].ToString(), /*clientPhoneNumber*/
                        reader[2].ToString(), /*serviceName*/
                        int.Parse(reader[3].ToString()), /*stuffsNumber*/
                        DateTime.Parse(reader[4].ToString()), /*receptionDate*/
                        bool.Parse(reader[6].ToString())));
                }
                catch (IndexOutOfRangeException ex)
                {
                    continue;
                }
            }

            reader.Close();

            return result;
        }

        /// <summary>
        /// Получение списка всех услуг.
        /// </summary>
        /// <returns>Список услуг химчистки</returns>
        public List<ServiceModel> GetAllServices()
        {
            List<ServiceModel> result = new List<ServiceModel>();

            string query = "SELECT * FROM `Services`;";

            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataReader reader = command.ExecuteReader();

            //Перебираем записи в reader
            while (reader.Read())
            {
                try
                {
                    result.Add(new ServiceModel(reader[0].ToString(), /*service name*/
                        int.Parse(reader[1].ToString()) /*cost*/
                        ));
                }
                catch (IndexOutOfRangeException ex)
                {
                    continue;
                }
            }

            reader.Close();

            return result;
        }


        /// <summary>
        /// Геттер для получения экземпляра класса DBController.
        /// Предусмотрено использование только одного экземпляра класса DBController в проекте.
        /// </summary>
        /// <returns>экзмепляр класса DBController</returns>
        public static DBController GetInstantce()
        {
            if (instance == null)
            {
                instance = new DBController();
            }

            return instance;
        }


        /// <summary>
        /// Установка значения атрибута "Дата возврата" для заказа с указанным id.
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="returnDate">Устанавливаемая дата возврата</param>
        public void SetReturnDate(int orderId, DateTime returnDate)
        {
            string query = $"UPDATE `Orders` SET ReturnDate = '{returnDate.ToString("dd/MM/yyyy")}' WHERE Id = {orderId};";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }


        /// <summary>
        /// Закрытие соединения.
        /// </summary>
        public void CloseConnection()
        {
            connection.Close();
        }




        //Создание таблицы клиентов
        private void CreateClientsTable()
        {
            string query = "CREATE TABLE IF NOT EXISTS `Clients` " +
                "(PhoneNumber VARCHAR(30) NOT NULL, " +
                "ClientSurname VARCHAR(30) NOT NULL, " +
                "ClientName VARCHAR(30) NOT NULL, " +
                "ClientPatronymic VARCHAR(40) NOT NULL, " +
                "BirthDate DATE NOT NULL, " +
                "OrdersNumber SMALLINT DEFAULT 0," +
                "PRIMARY KEY (PhoneNumber));";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }

        private void CreateDatabase()
        {
            string query = "CREATE DATABASE IF NOT EXISTS `DryCleaners`;";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }

        // Создание таблицы заказов
        private void CreateOrdersTable()
        {
            string query = "CREATE TABLE IF NOT EXISTS `Orders` " +
               "(Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT," +
               "ClientPhoneNumber VARCHAR(30) NOT NULL," +
               "ServiceName VARCHAR(50) NOT NULL," +
               "StuffsNumber TINYINT DEFAULT 1," +
               "ReceptionDate DATE NOT NULL," +
               "ReturnDate DATE," +
               "WithDelivery BOOL DEFAULT 0," +
               "FOREIGN KEY (ClientPhoneNumber) REFERENCES Clients(PhoneNumber)," +
               "FOREIGN KEY (ServiceName) REFERENCES Services(ServiceName));";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }

        //Создание таблиц услуг
        private void CreateServicesTable()
        {
            string query = "CREATE TABLE IF NOT EXISTS `Services` " +
               "(ServiceName VARCHAR(50) NOT NULL PRIMARY KEY," +
               "Cost SMALLINT NOT NULL);";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }

        // Создание таблиц в базе данных
        private void CreateTables()
        {
            CreateClientsTable();
            CreateServicesTable();
            CreateOrdersTable();
        }


        /// <summary>
        /// Увеличение поля "Количество заказов" в БД у клиента.
        /// </summary>
        /// <param name="client">Клиент, для которого нужно увеличить значение поля</param>
        public void IncreaseOrdersNumber(ClientModel client)
        {
            string query = $"UPDATE `Clients` SET OrdersNumber = OrdersNumber + 1 WHERE PhoneNumber = {client.PhoneNumber};";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }

        private void OpenConnection()
        {
            connection = new MySqlConnection("server=localhost;port=3306;database=DryCleaners;user=root;password=;");
            connection.Open();
        }

        /// <summary>
        /// Удаление клиента из БД.
        /// </summary>
        /// <param name="client">Удаляемый клиент</param>
        public void Remove(ClientModel client)
        {
            string query = $"DELETE FROM `Clients` WHERE PhoneNumber = {client.PhoneNumber};";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Удаление заказа из БД.
        /// </summary>
        /// <param name="order">Удаляемый заказ</param>
        public void Remove(OrderModel order)
        {
            string query = $"DELETE FROM `Orders` WHERE Id = {order.Id};";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Удаление услуги из БД.
        /// </summary>
        /// <param name="service">Удаляемая услуга</param>
        public void Remove(ServiceModel service)
        {
            string query = $"DELETE FROM `Services` WHERE ServiceName = {service.ServiceName};";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandTimeout = 60;

            command.ExecuteNonQuery();
        }



        /// <summary>
        /// Сохранение клиента в базе данных.
        /// </summary>
        /// <param name="client">Сохраняемый клиент</param>
        public void Save(ClientModel client)
        {
            string query = $"INSERT INTO `Clients` (PhoneNumber, ClientSurname, ClientName, ClientPatronymic, BirthDate) " +
                $"VALUES('{client.PhoneNumber}', '{client.ClientSurname}', '{client.ClientName}', '{client.ClientPatronymic}', " +
                $"'{client.BirthDate.Date.ToString("dd/MM/yyyy")}');";

            MySqlCommand command = new MySqlCommand(query, connection);

            command.ExecuteNonQuery();
        }


        /// <summary>
        /// Сохранение заказа в базе данных.
        /// </summary>
        /// <param name="order">сохраняемый заказ</param>
        public void Save(OrderModel order)
        {
            string query = $"INSERT INTO `Orders` (ClientPhoneNumber, ServiceName, StuffsNumber, ReceptionDate, WithDelivery) VALUES('{order.ClientPhoneNumber}', " +
                $"'{order.ServiceName}', {order.StuffsNumber}, '{order.ReceptionDate.Date.ToString("dd/MM/yyyy")}', {order.WithDelivery});";

            MySqlCommand command = new MySqlCommand(query, connection);

            command.ExecuteNonQuery();
        }



        /// <summary>
        /// Сохранение услуги в базе данных.
        /// </summary>
        /// <param name="order">Сохраняемая услуга</param>
        public void Save(ServiceModel service)
        {
            string query = $"INSERT INTO `Services` (ServiceName, Cost) " +
                $"VALUES('{service.ServiceName}', {service.Cost});";

            MySqlCommand command = new MySqlCommand(query, connection);

            command.ExecuteNonQuery();
        }




    }
}
