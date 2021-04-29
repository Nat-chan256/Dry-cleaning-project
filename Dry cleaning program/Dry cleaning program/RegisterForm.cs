using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dry_cleaning_program.Controllers;
using Dry_cleaning_program.Models;

namespace Dry_cleaning_program
{
    public partial class RegisterForm : Form
    {
        // Контроллеры
        private ClientsController ClientController;
        private OrdersController OrdrsController;
        private ServicesController ServiceController;

        // Стоимость доставки
        private int deliveryCost = 500; 

        public RegisterForm()
        {
            InitializeComponent();

            //Контроллеры
            ClientController = new ClientsController();
            OrdrsController = new OrdersController();
            ServiceController = new ServicesController();


            //// Вкладка "Приём вещей"
            //fillChooseClientComboBox();

            //// Вкладка "Возврат вещей"
            FillOrdersComboBox();
            fillServicesComboBox();
            dtpReturn.MinDate = dtpReception.Value;

            //// Вкладка "Услуги"
            fillServicesListBox();
        }


        //====================================Вкладка "Приём вещей"===========================================

        //------------------------------------Обработчики событий---------------------------------------------

        // Переход на вкладку
        private void tpReception_Click(object sender, EventArgs e)
        {
            fillServicesComboBox();
        }


        // Обработчики событий для полей ввода
        private void tbName_Enter(object sender, EventArgs e)
        {
            TextBox tbName = (TextBox)sender;
            if (tbName.Text == (string)tbName.Tag && tbName.ForeColor == Color.Silver)
            {
                tbName.Text = "";
                tbName.ForeColor = Color.Black;
            }
        }

        private void tbName_Leave(object sender, EventArgs e)
        {
            TextBox tbName = (TextBox)sender;
            if (tbName.Text == "")
            {
                tbName.Text = (string)tbName.Tag;
                tbName.ForeColor = Color.Silver;
            }
        }

        private void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
                e.Handled = true;
        }

        private void mtbPhoneNumber_Enter(object sender, EventArgs e)
        {
            MaskedTextBox mtbPassportSeries = (MaskedTextBox)sender;
            if (mtbPassportSeries.Text == (string)mtbPassportSeries.Tag && mtbPassportSeries.ForeColor == Color.Silver)
            {
                mtbPassportSeries.Text = "";
                mtbPassportSeries.ForeColor = Color.Black;
            }
        }

        private void mtbPhoneNumber_Leave(object sender, EventArgs e)
        {
            MaskedTextBox mtbPassportSeries = (MaskedTextBox)sender;
            if (mtbPassportSeries.Text == "")
            {
                mtbPassportSeries.Text = (string)mtbPassportSeries.Tag;
                mtbPassportSeries.ForeColor = Color.Silver;
            }
        }

        private void mtbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }


        // Расчет стоимости после выбора услуги
        private void cbServiceName_SelectedValueChanged(object sender, EventArgs e)
        {
            if (sender is NumericUpDown && cbServiceName.SelectedItem == null)
            {
                return;
            }

            int stuffsNumber = (int)nudStuffsNumber.Value;
            int serviceCost = ServiceController.GetService(cbServiceName.SelectedItem.ToString()).Cost;
            int resultCost = stuffsNumber * serviceCost;
            if (cbWithDelivery.Checked)
            {
                resultCost += deliveryCost;
            }

            tbCost.Text = resultCost.ToString();
        }

        // Обработчики событий для кнопок
        private void bSaveOrder_Click(object sender, EventArgs e)
        {
            //Проверка заполненности всех полей
            if (AreAllFieldsFilled(tpReception) == false)
            {
                MessageBox.Show("Не все поля заполнены.");
                return;
            }

            try
            {
                // Сохраняем клиента
                ClientController.Save(new ClientModel(tbSurname.Text, tbName.Text, tbPatronymic.Text, mtbPhoneNumber.Text,
                    dtpBirthDate.Value));

                // Сохраняем заказ
                OrdrsController.Save(new OrderModel(mtbPhoneNumber.Text, cbServiceName.SelectedItem.ToString(),
                    (int)nudStuffsNumber.Value, dtpReception1.Value, cbWithDelivery.Checked));
               
                MessageBox.Show("Заказ успешно добавлен.");

                ClearAllFieldsOnReceptionTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить заказ.");
                MessageBox.Show(ex.Message);
            }
        }



        // Заполнение комбоБокса "Наименование услуги" во вкладках "Прием вещей" и "Возврат вещей"
        private void fillServicesComboBox()
        {
            // Вкладка "Прием вещей"
            cbServiceName.Items.Clear();
            List<ServiceModel> services = ServiceController.Services;
            foreach (ServiceModel service in services) //Заполняем комбобокс услугами
            {
                cbServiceName.Items.Add(service.ServiceName);
            }

            // Вкладка "Возврат вещей"
            cbServiceNameReturnTab.Items.Clear();
            foreach (ServiceModel service in services) //Заполняем комбобокс услугами
            {
                cbServiceNameReturnTab.Items.Add(service.ServiceName);
            }


        }

        // Очистка полей на вкладке
        private void ClearAllFieldsOnReceptionTab()
        {
            tbName.Text = tbName.Tag.ToString();

            tbSurname.Text = tbSurname.Tag.ToString();

            tbPatronymic.Text = tbPatronymic.Tag.ToString();

            mtbPhoneNumber.Text = mtbPhoneNumber.Tag.ToString();

            nudStuffsNumber.Value = 1;

            tbCost.Text = "";


            cbWithDelivery.Checked = false;
        }

        //====================================Вкладка "Возврат вещей"===========================================

        // Преход на вкладку "Возврат вещей"
        private void tpReturn_Click(object sender, EventArgs e)
        {
            FillOrdersComboBox();
        }

        // Нажатие на комбобокс "Выбрать заказ"
        private void cbChooseOrder_Click(object sender, EventArgs e)
        {
            FillOrdersComboBox();
        }


        // Изменение значения в кобмоБоксе "Выбрать заказ"
        private void cbChooseOrder_SelectedValueChanged(object sender, EventArgs e)
        {
            string[] chosenOrder = ((ComboBox)sender).SelectedItem.ToString().Split();

            OrderModel order = OrdrsController.GetOrder(int.Parse(chosenOrder[0].Remove(0, 1)));
            ClientModel client = ClientController.GetClient(chosenOrder[1]);

            FillFieldsWithClientAndOrderData(client, order);
        }


        // Заполнение полей на вкладке данными клиента и заказа
        private void FillFieldsWithClientAndOrderData(ClientModel client, OrderModel order)
        {
            //Задаем значения полей
            tbClientSurnameReturnTab.Text = client.ClientSurname;
            tbClientNameReturnTab.Text = client.ClientName;
            tbClientPatronymicReturnTab.Text = client.ClientPatronymic;
            mtbPhoneNumberReturnTab.Text = client.PhoneNumber;

            nudStuffsNumber.Value = order.StuffsNumber;
            foreach (var item in cbServiceNameReturnTab.Items)
            {
                if (item.ToString() == order.ServiceName)
                {
                    cbServiceNameReturnTab.SelectedItem = item;
                    break;
                }
            }
            dtpReception.Value = order.ReceptionDate;
            tbTotalCost.Text = (ServiceController.GetService(order.ServiceName).Cost * order.StuffsNumber).ToString();
            dtpReturn.Value = DateTime.Now;

            //Меняем цвет текста полей
            foreach (Object child in tpReturn.Controls)
            {
                if (child is TextBoxBase)
                {
                    ((TextBoxBase)child).ForeColor = Color.Black;
                }
            }
        }

        // Заполнение комбоБокса "Выбрать заказ"
        private void FillOrdersComboBox()
        {
            cbChooseOrder.Items.Clear();
            List<OrderModel> orders = OrdrsController.Orders;
            foreach (OrderModel order in orders) //Заполняем комбобокс заказами
            {
                cbChooseOrder.Items.Add($"#{order.Id} {order.ClientPhoneNumber} {order.ServiceName}");
            }
        }

        // Нажатие кнопки "Сохранить"
        private void bSaveOrderReturn_Click(object sender, EventArgs e)
        {
            try
            {
                OrdrsController.SetReturnDate(int.Parse(cbChooseOrder.SelectedItem.ToString().Split()[0].Remove(0, 1)), dtpReturn.Value);
                MessageBox.Show("Заказ успешно изменен.");
                ClearFieldsOnReturnTab();
                FillOrdersComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось внести изменения в базу данных");
            }
            
        }

        // Очистка полей на вкладке
        private void ClearFieldsOnReturnTab()
        {
            tbClientNameReturnTab.Text = tbClientNameReturnTab.Tag.ToString();

            tbClientSurnameReturnTab.Text = tbClientSurnameReturnTab.Tag.ToString();

            tbClientPatronymicReturnTab.Text = tbClientPatronymicReturnTab.Tag.ToString();

            mtbPhoneNumberReturnTab.Text = mtbPhoneNumberReturnTab.Tag.ToString();

            nudStuffsNumReturnTab.Value = 1;

            tbTotalCost.Text = "";


            cbWithDeliveryReturnTab.Checked = false;
        }


        //====================================Вкладка "Услуги"===========================================

        // Загрузка списка услуг
        private void fillServicesListBox()
        {
            List<ServiceModel> services = ServiceController.Services;

            foreach (ServiceModel service in services)
            {
                lbServices.Items.Add(service.ServiceName + " " + service.Cost.ToString());
            }
        }

        // Нажатие кнопки "Добавить"
        private void bAddService_Click(object sender, EventArgs e)
        {
            if (tbServiceName.TextLength == 0 || tbCostServicesTab.TextLength == 0)
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }

            ServiceController.Save(new ServiceModel(tbServiceName.Text, int.Parse(tbCostServicesTab.Text)));

            lbServices.Items.Add(tbServiceName.Text + " " + tbCostServicesTab.Text);

            fillServicesComboBox();
        }


        //====================================Вспомогательные методы===========================================

        // Проверка заполненности всех полей на вкладке tab
        private bool AreAllFieldsFilled(TabPage tab)
        {
            if (tab == tpReception)
            {
                if (tbSurname.ForeColor != Color.Black ||
                    tbName.ForeColor != Color.Black ||
                    tbPatronymic.ForeColor != Color.Black ||
                    mtbPhoneNumber.ForeColor != Color.Black ||
                    cbServiceName.SelectedItem == null)
                {
                    return false;
                }
            }
            else if (tab == tpReturn)
            {
                if (cbChooseOrder.SelectedItem == null)
                {
                    return false;
                }
            }

            return true;
        }


        // Закрытие соединения с БД и сериализация данных
        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DBController dbController = DBController.GetInstantce();
            dbController.CloseConnection();

            ClientController.SerializeClients();
            OrdrsController.SerializeOrders();
            ServiceController.SerializeServices();
        }
    }
}
