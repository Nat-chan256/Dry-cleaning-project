using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dry_cleaning_program.Models;

namespace Dry_cleaning_program.Controllers
{
    /// <summary>
    /// Класс Контроллер услуг.
    /// </summary>
    public class ServicesController
    {
        // Кэш услуг
        private List<ServiceModel> ServicesCache;

        private DBController DbController;

        /// <summary>
        /// Список услуг химчистки.
        /// </summary>
        public List<ServiceModel> Services
        {
            get
            {
                if (ServicesCache == null || ServicesCache.Count == 0)
                    ServicesCache = DbController.GetAllServices();
                return ServicesCache;
            }
        }

        /// <summary>
        /// Конструктор без параметров, инициализирующий БД контроллер.
        /// </summary>
        public ServicesController()
        {
            DbController = DBController.GetInstantce();
        }



        /// <summary>
        /// Получение услуги по её названию.
        /// </summary>
        /// <param name="serviceName">Название услуги</param>
        /// <returns></returns>
        public ServiceModel GetService(string serviceName)
        {
            foreach (ServiceModel service in Services)
            {
                if (service.ServiceName == serviceName)
                {
                    return service;
                }
            }
            return null;
        }


        /// <summary>
        /// Сохранение услуги в БД.
        /// </summary>
        /// <param name="service">Сохраняемая услуга</param>
        public void Save(ServiceModel service)
        {
            DbController.Save(service);
            ServicesCache.Add(service);
        }

        /// <summary>
        /// Сериализация заказов. Заказы берутся из кэша заказов.
        /// </summary>
        public void SerializeServices()
        {
            Serializer serializer = Serializer.GetInstnce();
            serializer.Serialize(ServicesCache);
        }
    }
}
