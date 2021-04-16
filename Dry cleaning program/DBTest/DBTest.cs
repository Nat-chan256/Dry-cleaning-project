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

            //Тестовые клиенты
            testClients = new List<ClientModel>();

            testClients.Add(new ClientModel("Васильченко", "Василий", "Андреевич", "89001231223", new DateTime(1978, 4, 7)));
            testClients.Add(new ClientModel("Мельников", "Иван", "Петрович", "89012222222", new DateTime(1979, 4, 7)));
            testClients.Add(new ClientModel("Ромашкина", "Василиса", "Геннадьевна", "89671919199", new DateTime(1970, 4, 7)));

            // Тестовые услуги
            testServices = new List<ServiceModel>();
            testServices.Add(new ServiceModel("Химчистка мебели", 1700));
            testServices.Add(new ServiceModel("Химчистка одежды", 500));
            testServices.Add(new ServiceModel("Химчистка обуви", 2000));

            // Тестовые заказы
            testOrders = new List<OrderModel>();
            testOrders.Add(new OrderModel("89001231223", "Химчистка мебели", 1, new DateTime(2021, 4, 13, 10, 34, 12), true));
            testOrders.Add(new OrderModel("89012222222", "Химчистка мебели", 3, new DateTime(2021, 4, 14, 10, 34, 12), true));
            testOrders.Add(new OrderModel("89671919199", "Химчистка обуви", 1, new DateTime(2021, 3, 13, 10, 34, 12), false));
        }



        //Проверка сохранения и получения списка пользователей из БД
        [TestMethod]
        public void TestSaveAndGetClients()
        {
            // Формируем ожидаемый список
            List<ClientModel> expectedList = dbController.GetAllClientsData();
            foreach (ClientModel client in testClients)
            {
                expectedList.Add(client);
                dbController.Save(client);
            }

            CollectionAssert.AreEqual(expectedList, dbController.GetAllClientsData());
        }


        // Проверка получения списка текущих клиентов из БД
        [TestMethod]
        public void TestGettingCurrentOrders()
        {
            // Формируем ожидаемый список
            List<OrderModel> expectedList = dbController.GetAllCurrentOrdersData();

            // Сохраняем клиентов в БД
            foreach (ClientModel client in testClients)
            {
                dbController.Save(client);
            }

            // Сохраняем услуги в БД
            foreach (ServiceModel service in testServices)
            {
                dbController.Save(service);
            }

            // Сохраняем заказы в БД
            foreach (OrderModel order in testOrders)
            {
                dbController.Save(order);
                expectedList.Add(order);
            }

            // "Заврешаем" один из заказов
            List<OrderModel> ordersFromDb = dbController.GetAllOrdersData();
            foreach (OrderModel order in ordersFromDb)
            {
                if (order.ClientPhoneNumber == testOrders[0].ClientPhoneNumber && order.ReceptionDate == testOrders[0].ReceptionDate)
                {
                    expectedList.Remove(order);
                    break;
                }
            }

            dbController.SetReturnDate(testOrders[0].Id, DateTime.Now);

            // Удаляем заказ из ожидаемого списка
            expectedList.Remove(ordersFromDb[0]);

            // Сравниваем
            Assert.AreEqual(expectedList, dbController.GetAllCurrentOrdersData());
        }

        //Проверка, получаем ли мы один и тот же экземпляр класса DBController при вызове метода GetInstance()
        [TestMethod]
        public void TestGettingDBInstance()
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
