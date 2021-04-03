using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dry_cleaning_program.Controllers;
using Dry_cleaning_program.Models;
using System.Collections.Generic;

namespace DBTest
{
    [TestClass]
    public class DBTest
    {
        // Используемый в тестах БД контроллер
        private DBController dbController;

        // Тестовые клиенты
        private List<ClientModel> testClients;

        // Тестовые заказы
        private List<OrderModel> testOrders;

        // Тестовые услуги
        private List<ServiceModel> testServices;

        [TestInitialize]
        public void SetUp()
        {
            // Получаем экземпляр класса БД контроллер
            dbController = DBController.GetInstantce();

            //Создаем списки для тестирования
            testClients = new List<ClientModel>();
            testOrders = new List<OrderModel>();
            testServices = new List<ServiceModel>();
        }

        //Проверка 
        [TestMethod]
        public void TestGettinDBgInstance()
        {
            DBController testDBController = DBController.GetInstantce();

            Assert.AreEqual(testDBController, dbController);
        }

        [TestCleanup]
        public void CloseConnection()
        {
            RemoveTestData();
            dbController.CloseConnection();
        }

        public void RemoveTestData()
        {
            // Удаляем все созданные в ходе тестирования заказы
            foreach (OrderModel order in testOrders)
                dbController.Remove(order);

            // Удаляем всех созданных в ходе тестирования клиентов
            foreach (ClientModel client in testClients)
                dbController.Remove(client);

            // Удаляем все созданные в ходе тестирования услуги
            foreach (ServiceModel service in testServices)
                dbController.Remove(service);
        }
    }
}
