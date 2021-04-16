using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dry_cleaning_program.Models
{
    [Serializable]
    /// <summary>
    /// Модель услуги.
    /// </summary>
    public class ServiceModel
    {
        /// <summary>
        /// Цена услуги для одной вещи.
        /// </summary>
        public int Cost { get; }

        /// <summary>
        /// Название услуги.
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        /// Конструтор, устанавливающий значения всех полей.
        /// </summary>
        /// <param name="serviceName">Название услуги</param>
        /// <param name="cost">Стоимость услуги за одну вещь</param>
        public ServiceModel(string serviceName, int cost)
        {
            ServiceName = serviceName;
            Cost = cost;
        }
    }
}
