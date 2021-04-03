using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dry_cleaning_program.Models
{
    /// <summary>
    /// Модель клиента.
    /// </summary>
    public class ClientModel
    {
        /// <summary>
        /// Дата рождения клиента.
        /// </summary>
        public DateTime BirthDate { get; }

        /// <summary>
        /// Имя клиента.
        /// </summary>
        public string ClientName { get; }

        /// <summary>
        /// Отчество клиента.
        /// </summary>
        public string ClientPatronymic { get; }

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public string ClientSurname { get; }

        /// <summary>
        /// Количество заказов, сделанных клиентом.
        /// </summary>
        public int OrdersNumber { get; }

        /// <summary>
        /// Номер телефона клиента.
        /// </summary>
        public string PhoneNumber { get; }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="surname">Фамилия клиента</param>
        /// <param name="name">Имя клиента</param>
        /// <param name="patronymic">Отчество клиента</param>
        /// <param name="phoneNumber">Номер телефона клиента</param>
        /// <param name="birthDate">Дата рождения клиента</param>
        public ClientModel(string surname, string name, string patronymic, string phoneNumber, DateTime birthDate)
        {
            ClientSurname = surname;
            ClientName = name;
            ClientPatronymic = patronymic;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
        }


        /// <summary>
        /// Конструктор с параметрами, устанавливающий значение количества заказов.
        /// </summary>
        /// <param name="surname">Фамилия клиента</param>
        /// <param name="name">Имя клиента</param>
        /// <param name="patronymic">Отчество клиента</param>
        /// <param name="phoneNumber">Номер телефона клиента</param>
        /// <param name="birthDate">Дата рождения клиента</param>
        /// <param name="ordersNumber">Количество заказов, сделанных клиентом</param>
        public ClientModel(string surname, string name, string patronymic, string phoneNumber, DateTime birthDate, int ordersNumber)
            : this(surname, name, patronymic, phoneNumber, birthDate)
        {
            OrdersNumber = ordersNumber;
        }

        public override bool Equals(Object obj)
        {
            ClientModel client = obj as ClientModel;
            if (client == null)
                return false;
            else
                return PhoneNumber.Equals(client.PhoneNumber);
        }

        public override int GetHashCode()
        {
            return this.PhoneNumber.GetHashCode();
        }
    }
}
