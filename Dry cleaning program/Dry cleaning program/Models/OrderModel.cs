using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dry_cleaning_program.Models
{
    /// <summary>
    /// Класс Модель заказа.
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int Id { get;  }

        /// <summary>
        /// Номер телефона клиента.
        /// </summary>
        public string ClientPhoneNumber { get; }

        /// <summary>
        /// Название услуги.
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        /// Количество принятых вещей.
        /// </summary>
        public int StuffsNumber { get; }

        /// <summary>
        /// Дата приёма вещей.
        /// </summary>
        public DateTime ReceptionDate { get; }

        /// <summary>
        /// С доставкой на дом или нет.
        /// </summary>
        public bool WithDelivery { get; }


        /// <summary>
        /// Конструктор с параметрами без возможности установки ID.
        /// </summary>
        /// <param name="clientPhoneNumber">Номер телефона клиента</param>
        /// <param name="serviceName">Название услуги</param>
        /// <param name="stuffsNumber">Количество принимаемых вещей</param>
        /// <param name="receptionDate">Дата приёма вещей</param>
        /// <param name="withDelivery">С доставкой на дом или без нее</param>
        public OrderModel(string clientPhoneNumber, string serviceName, int stuffsNumber, DateTime receptionDate,
            bool withDelivery)
        {
            ClientPhoneNumber = clientPhoneNumber;
            ServiceName = serviceName;
            StuffsNumber = stuffsNumber;
            ReceptionDate = receptionDate;
            WithDelivery = withDelivery;
        }

        /// <summary>
        /// Конструктор, устанавливающий значений Id заказа помимо прочих параметров.
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>>
        /// <param name="clientPhoneNumber">Номер телефона клиента</param>
        /// <param name="serviceName">Название услуги</param>
        /// <param name="stuffsNumber">Количество принимаемых вещей</param>
        /// <param name="receptionDate">Дата приёма вещей</param>
        /// <param name="withDelivery">С доставкой на дом или без нее</param>
        public OrderModel(int id, string clientPhoneNumber, string serviceName, int stuffsNumber, DateTime receptionDate,
            bool withDelivery) : this(clientPhoneNumber, serviceName, stuffsNumber, receptionDate, withDelivery)
        {
            Id = id;
        }
    }
}
