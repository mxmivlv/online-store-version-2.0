using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Notifications.Wpf.Core;

namespace Module_18
{
    public partial class Client
    {
        public event Action<string> Error;
        private NotificationManager notificationManager = new NotificationManager();
        private List<Client> tempListClient = new List<Client>();
        private List<PurchaseClient> tempListPurchaseClient = new List<PurchaseClient>();

        public int Id { get; set; }
        public string SurnameClient { get; set; }
        public string NameClient { get; set; }
        public string PatronymicClient { get; set; }
        public long NumberPhoneClient { get; set; }
        public string EmailClient { get; set; }

        public Client() { }

        private Client(string surnameClient, string nameClient, string patronymicClient, long numberPhoneClient, string emailClient)
        {
            SurnameClient = surnameClient;
            NameClient = nameClient;
            PatronymicClient = patronymicClient;
            NumberPhoneClient = numberPhoneClient;
            EmailClient = emailClient;
        }

        /// <summary>
        /// Вывод данных на форму
        /// </summary>
        /// <param name="collectionClient">Коллекция для клиентов</param>
        /// <param name="collectionPurchaseClient">Коллекция для покупок</param>
        public async void PrintDbAsync(ObservableCollection<Client> collectionClient, ObservableCollection<PurchaseClient> collectionPurchaseClient)
        {
            try
            {
                using (MSSQLShopContext context = new MSSQLShopContext())
                {
                    await Task.Run(() =>
                    {
                        tempListClient = context.Clients.ToList();
                        tempListPurchaseClient = context.PurchaseClients.ToList();
                    });
                }

                foreach (Client client in tempListClient)
                    collectionClient.Add(client);

                foreach (PurchaseClient puchaseClient in tempListPurchaseClient)
                    collectionPurchaseClient.Add(puchaseClient);

                tempListClient.Clear();
                tempListPurchaseClient.Clear();
            }
            catch (Exception ex)
            {
                Error?.Invoke($"{ex.Message}");
            }
        }

        /// <summary>
        /// Метод ошибки
        /// </summary>
        /// <param name="text">Текст ошибки</param>
        public async void ErrorAsync(string text)
        {
            await Task.Run(() => notificationManager.ShowAsync(new NotificationContent
            {
                Title = $"Ошибка",
                Message = $"{text}",
                Type = NotificationType.Error
            }));
        }

        /// <summary>
        /// Добавление клиента в БД
        /// </summary>
        /// <param name="surnameClient">Фамилия клиента</param>
        /// <param name="nameClient">Имя клиента</param>
        /// <param name="patronymicClient">Отчество клиента</param>
        /// <param name="numberPhoneClient">Номер телефона клиента</param>
        /// <param name="emailClient">Почта клиента</param>
        /// <param name="collectionClient">Коллекция клиентов</param>
        public async void AddClientAsync(string surnameClient, string nameClient, string patronymicClient, string numberPhoneClient, string emailClient, 
            ObservableCollection<Client> collectionClient)
        {
            try
            {
                //Проверка на пустые поля
                if (surnameClient != null && nameClient != null && patronymicClient != null && emailClient != null)
                {
                    long tempPhone;
                    bool flagAddClient = false;

                    //Если номер телефона пустой, присваеваем значение 0
                    if (numberPhoneClient == null)
                    {
                        tempPhone = 0;
                        flagAddClient = true;
                    }
                    else
                    {
                        bool flagTempPhone = long.TryParse(numberPhoneClient, out tempPhone);
                        char[] tempArrayNumberPhone = new char[11];
                        char[] lenghtNumberPhone = numberPhoneClient.ToString().ToCharArray();

                        //Если номер телефона соответсвует стандарту и конвертации 
                        if (lenghtNumberPhone.Length == tempArrayNumberPhone.Length && flagTempPhone)
                        {
                            flagAddClient = true;
                        }
                    }

                    //Если добавление разрешено
                    if (flagAddClient)
                    {
                        using (MSSQLShopContext context = new MSSQLShopContext())
                        {
                            Client client = new Client(surnameClient, nameClient, patronymicClient, tempPhone, emailClient);
                            context.Clients.Add(client);
                            context.SaveChanges();

                            await Task.Run(() =>
                            {
                                tempListClient = context.Clients.ToList();
                            });
                        }

                        collectionClient.Clear();

                        foreach (Client cl in tempListClient)
                            collectionClient.Add(cl);

                        tempListClient.Clear();
                    }
                    else
                        Error?.Invoke($"Номер телефона не корректен");
                }
                else
                    Error?.Invoke($"Поля не заполнены");
            }
            catch (Exception ex)
            {
                Error?.Invoke($"{ex.Message}");
            }
        }

        /// <summary>
        /// Удаление клиента из БД
        /// </summary>
        /// <param name="client">Выбранный клиент</param>
        /// <param name="collectionClient">Коллекция клиентов</param>
        public async void DeleteClientAsync(Client client, ObservableCollection<Client> collectionClient)
        {
            try
            {
                using (MSSQLShopContext context = new MSSQLShopContext())
                {
                    await Task.Run(() =>
                    {
                        var tempClient = context.Clients.Where(item => item.Id == client.Id).FirstOrDefault();
                        context.Clients.Remove(tempClient);
                        context.SaveChanges();

                        tempListClient = context.Clients.ToList();

                    });
                }

                collectionClient.Clear();

                foreach (Client cl in tempListClient)
                    collectionClient.Add(cl);

                tempListClient.Clear();
            }
            catch (Exception ex)
            {
                Error?.Invoke($"{ex.Message}");
            }
        }

        /// <summary>
        /// Обновления данных клиентов
        /// </summary>
        /// <param name="client">Выбранный клиент</param>
        /// <param name="collectionClient">Коллекция клиентов</param>
        public async void UpdateClientAsync(Client client, ObservableCollection<Client> collectionClient)
        {
            try
            {
                using (MSSQLShopContext context = new MSSQLShopContext())
                {
                    await Task.Run(() =>
                    {
                        var tempClient = context.Clients.Where(item => item.Id == client.Id).FirstOrDefault();
                        tempClient.SurnameClient = client.SurnameClient;
                        tempClient.NameClient = client.NameClient;
                        tempClient.PatronymicClient = client.PatronymicClient;
                        tempClient.NumberPhoneClient = client.NumberPhoneClient;
                        tempClient.EmailClient = client.EmailClient;
                        context.SaveChanges();

                        tempListClient = context.Clients.ToList();
                    });  
                }

                collectionClient.Clear();

                foreach (Client cl in tempListClient)
                    collectionClient.Add(cl);

                tempListClient.Clear();
            }
            catch (Exception ex)
            {
                Error?.Invoke($"{ex.Message}");
            }
        }

        /// <summary>
        /// Вывод покупок у выбранного клиента
        /// </summary>
        /// <param name="client">Выбранный клиент</param>
        /// <param name="collectionPurchaseClient">Коллекция покупок</param>
        public async void PrintPurchasesInClient(Client client, ObservableCollection<PurchaseClient> collectionPurchaseClient)
        {
            try
            {
                using (MSSQLShopContext context = new MSSQLShopContext())
                {
                    await Task.Run(() =>
                    {
                        tempListPurchaseClient = context.PurchaseClients.Where(item => item.EmailBuyer == client.EmailClient).ToList();
                    }); 
                }

                collectionPurchaseClient.Clear();

                foreach (var puchaseClient in tempListPurchaseClient)
                    collectionPurchaseClient.Add(puchaseClient);

                tempListPurchaseClient.Clear();
            }
            catch (Exception ex)
            {
                Error?.Invoke($"{ex.Message}");
            }
        }

        /// <summary>
        /// Добавления покупок клиенту
        /// </summary>
        /// <param name="client">Выбранный клиент</param>
        /// <param name="codeProduct">Код продукта</param>
        /// <param name="nameProduct">Наименование продукта</param>
        /// <param name="collectionPurchaseClient">Коллекция покупок</param>
        public async void AddPurchasesInClient(Client client, string codeProduct, string nameProduct, ObservableCollection<PurchaseClient> collectionPurchaseClient)
        {
            try
            {
                //Проверка на пустые поля
                if (client != null && codeProduct != null && nameProduct != null)
                {
                    long tempCodeProduct;
                    bool flagCodeProduct = long.TryParse(codeProduct, out tempCodeProduct);

                    //Если верная конвертация
                    if (flagCodeProduct)
                    {
                        PurchaseClient tempPurchaseClient = new PurchaseClient(client.EmailClient, tempCodeProduct, nameProduct);

                        using (MSSQLShopContext context = new MSSQLShopContext())
                        {
                            await Task.Run(() =>
                            {
                                context.PurchaseClients.Add(tempPurchaseClient);
                                context.SaveChanges();
                                tempListPurchaseClient = context.PurchaseClients.Where(item => item.EmailBuyer == client.EmailClient).ToList();
                            });
                        }

                        collectionPurchaseClient.Clear();

                        foreach (PurchaseClient puchaseClient in tempListPurchaseClient)
                        {
                            collectionPurchaseClient.Add(puchaseClient);
                        }

                        tempListPurchaseClient.Clear();
                    }
                    else
                        Error?.Invoke("Не верный код продукта");
                }
                else
                    Error?.Invoke("Поля не заполнены");
            }
            catch (Exception ex)
            {
                Error?.Invoke($"{ex.Message}");
            }
        }
    }
}
