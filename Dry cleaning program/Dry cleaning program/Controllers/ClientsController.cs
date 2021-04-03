using Dry_cleaning_program.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dry_cleaning_program.Controllers
{
    class ClientsController
    {
        // Кэш клиентов
        private List<ClientModel> clientsCache;

        /// <summary>
        /// Список всех клиентов в БД
        /// </summary>
        public List<ClientModel> Clients
        {
            get
            {
                if (clientsCache == null || clientsCache.Count == 0)
                    clientsCache = dbController.GetAllClientsData();
                return clientsCache;
            }
        }

        private DBController dbController;

        /// <summary>
        /// Конструктор без параметров, инициализирующий кэш клиентов и экземпляр БД контроллера.
        /// </summary>
        public ClientsController()
        {
            dbController = DBController.GetInstantce();
            clientsCache = dbController.GetAllClientsData();
        }

        /// <summary>
        /// Получчение клиента по его номеру телефона.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона клиента</param>
        /// <returns>Модель искомого клиента или null, если клиент с таким номером не найден</returns>
        public ClientModel GetClient(string phoneNumber)
        {
            foreach (ClientModel client in Clients)
            {
                if (client.PhoneNumber == phoneNumber)
                {
                    return client;
                }
            }
            return null;
        }


        /// <summary>
        /// Сохранение пользователя в БД, если его там ещё нет.
        /// <param name="client">Сохраняемый клиент</param>
        /// </summary>
        public void Save(ClientModel client)
        {
            if (clientsCache.Contains(client)) // Если клиент делает заказ не первый раз
            {
                dbController.IncreaseOrdersNumber(client); // Увечиваем соответствующий атрибут в БД
                return;
            }

            dbController.Save(client);
            clientsCache.Add(client);
        }
    }
}
