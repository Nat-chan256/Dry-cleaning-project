using Dry_cleaning_program.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Dry_cleaning_program
{
    public class Serializer
    {
        private static string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private static string pathToExe = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
        private string pathToClientsFile = pathToExe + "Dry cleaning program\\bin\\Debug\\clients.dat";
        private string pathToOrdersFile = pathToExe + "Dry cleaning program\\bin\\Debug\\orders.dat";
        private string pathToServicesFile = pathToExe + "Dry cleaning program\\bin\\Debug\\services.dat";

        private BinaryFormatter formatter;

        // Объект сериалайзера
        private static Serializer instance;

        // Конструктор, инициализирующий объект formatter
        private Serializer()
        {
            formatter = new BinaryFormatter();
        }


        /// <summary>
        /// Получение экзмепляра класса Serializer.
        /// Предполагается использование одного экземпляра в проекте.
        /// </summary>
        /// <returns>Экземпляр класса Serializer</returns>
        public static Serializer GetInstnce()
        {
            if (instance == null)
            {
                instance = new Serializer();
            }

            return instance;
        }


        /// <summary>
        /// Десериализация услуг.
        /// </summary>
        /// <returns>Список десериализованных услуг</returns>
        public List<ServiceModel> DeserializeServices()
        {
            List<ServiceModel> result = new List<ServiceModel>();
            // десериализация из файла 
            using (FileStream fs = new FileStream(pathToServicesFile, FileMode.OpenOrCreate))
            {
                result = (List<ServiceModel>)formatter.Deserialize(fs);
            }

            return result;

        }

         /// <summary>
        /// Десериализация заказов.
        /// </summary>
        /// <returns>Список десериализованных заказов</returns>
        public List<OrderModel> DeserializeOrders()
        {
            List<OrderModel> result = new List<OrderModel>();
            // десериализация из файла 
            using (FileStream fs = new FileStream(pathToOrdersFile, FileMode.OpenOrCreate))
            {
                result = (List<OrderModel>)formatter.Deserialize(fs);
            }

            return result;
        }

        /// <summary>
        /// Десериализация клиентов.
        /// </summary>
        /// <returns>Десериализованный клиент</returns>
        public List<ClientModel> DeserializeClients()
        {
            List<ClientModel> result = new List<ClientModel>();
            // десериализация из файла 
            using (FileStream fs = new FileStream(pathToClientsFile, FileMode.OpenOrCreate))
            {
                result = (List<ClientModel>)formatter.Deserialize(fs);
            }

            return result;
        }




        /// <summary>
        /// Бинарная сериализация услуги.
        /// </summary>
        /// <param name="service">Сериализуемая услуга</param>
        public void Serialize(List<ServiceModel> service)
        { 
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(pathToServicesFile, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, service);
            }
        }

        /// <summary>
        /// Бинарная сериализация заказа.
        /// </summary>
        /// <param name="order">Сериализуемый заказ</param>
        public void Serialize(List<OrderModel> order)
        { 
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(pathToOrdersFile, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, order);
            }
        }

        /// <summary>
        /// Бинарная сериализация клиентов.
        /// </summary>
        /// <param name="clients">Сериализуемые клиенты</param>
        public void Serialize(List<ClientModel> clients)
        {
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(pathToClientsFile, FileMode.Open))
            {
                formatter.Serialize(fs, clients);
            }
        }


    }
}
