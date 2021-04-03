using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dry_cleaning_program.Models;

namespace Dry_cleaning_program.Controllers
{
    /// <summary>
    /// Класс-контроллер для заказов.
    /// </summary>
    class OrdersController
    {
        // Текущие заказы
        private List<OrderModel> OrdersCache;

        /// <summary>
        /// Список активных заказов.
        /// </summary>
        public List<OrderModel> Orders
        {
            get
            {
                if (OrdersCache == null || OrdersCache.Count == 0)
                    OrdersCache = DbController.GetAllCurrentOrdersData();
                return OrdersCache;
            }
        }

        private DBController DbController;

        
        /// <summary>
        /// Конструктор без параметров, инициализирующий кэш заказов и БД контроллер.
        /// </summary>
        public OrdersController()
        {
            DbController = DBController.GetInstantce();
            OrdersCache = DbController.GetAllCurrentOrdersData();
        }


        /// <summary>
        /// Получение заказа по id.
        /// </summary>
        /// <param name="id">Идентификатор возвращаемого заказа</param>
        /// <returns></returns>
        public OrderModel GetOrder(int id)
        {
            foreach (OrderModel order in Orders)
            {
                if (order.Id == id)
                {
                    return order;
                }
            }
            return null;
        }


        /// <summary>
        /// Установка значения даты возврата вещей для заказа с идентификатором id.
        /// </summary>
        /// <param name="id">Идентификатор изменяемого заказа</param>
        public void SetReturnDate(int id, DateTime returnDate)
        {
            DbController.SetReturnDate(id, returnDate);
            OrdersCache.Remove(GetOrder(id));
        }

        /// <summary>
        /// Сохранение заказа в БД
        /// </summary>
        /// <param name="order">Сохраняемый заказ</param>
        public void Save(OrderModel order)
        {
            DbController.Save(order);
            OrdersCache.Add(order);
        }
    }
}
