using Module_18.Commands;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Module_18.ViewModels
{
    internal class ViewModelMainWindow : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private Client client;

        #region Заголовки, label
        public string Title { get { return title; } }
        public string GroupboxTitleClient { get { return groupboxTitleClient; } }
        public string Surname { get { return surname; } }
        public string Name { get { return name; } }
        public string Patronymic { get { return patronymic; } }
        public string NumberPhone { get { return numberPhone; } }
        public string Email { get { return email; } }
        public string GroupboxTitleBuyer { get { return groupboxTitleBuyer; } }
        public string CodeProduct { get { return codeProduct; } }
        public string NameProduct { get { return nameProduct; } }

        private string title = "Онлайн магазин";
        private string groupboxTitleClient = "Работа с клиентами";
        private string surname = "Фамилия";
        private string name = "Имя";
        private string patronymic = "Отчество";
        private string numberPhone = "Номер телефона";
        private string email = "Почта";
        private string groupboxTitleBuyer = "Работа с покупками клиентов";
        private string codeProduct = "Код продукта";
        private string nameProduct = "Название продукта";
        #endregion

        #region Для работы БД
        public ObservableCollection<Client> CollectionClient 
        {
            get { return collectionClient; }
            set 
            {
                if (collectionClient == value)
                    return;
                collectionClient = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CollectionClient)));
            }
        }
        public ObservableCollection<PurchaseClient> CollectionPurchaseClient
        {
            get { return collectionPurchaseClient; }
            set
            {
                if (collectionPurchaseClient == value)
                    return;
                collectionPurchaseClient = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CollectionPurchaseClient)));
            }
        }
        public Client SelectedClientDataGrid 
        {
            get { return selectedClientDataGrid; }
            set 
            {
                if (selectedClientDataGrid == value)
                    return;
                selectedClientDataGrid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SelectedClientDataGrid)));
            }
        }
        public string TextboxSurname
        {
            get { return textboxSurname; }
            set
            {
                if (textboxSurname == value)
                    return;
                textboxSurname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.textboxSurname)));
            }
        }
        public string TextboxName
        {
            get { return textboxName; }
            set
            {
                if (textboxName == value)
                    return;
                textboxName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.textboxName)));
            }
        }
        public string TextboxPatronymic
        {
            get { return textboxPatronymic; }
            set
            {
                if (textboxPatronymic == value)
                    return;
                textboxPatronymic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.textboxPatronymic)));
            }
        }
        public string TextboxPhoneNumber
        {
            get { return textboxPhoneNumber; }
            set
            {
                if (textboxPhoneNumber == value)
                    return;
                textboxPhoneNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TextboxPhoneNumber)));
            }
        }
        public string TextboxEmail
        {
            get { return textboxEmail; }
            set
            {
                if (textboxEmail == value)
                    return;
                textboxEmail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.textboxEmail)));
            }
        }
        public string TextboxCodeProduct
        {
            get { return textboxCodeProduct; }
            set
            {
                if (textboxCodeProduct == value)
                    return;
                textboxCodeProduct = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TextboxCodeProduct)));
            }
        }
        public string TextboxNameProduct
        {
            get { return textboxNameProduct; }
            set
            {
                if (textboxNameProduct == value)
                    return;
                textboxNameProduct = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TextboxNameProduct)));
            }
        }

        private ObservableCollection<Client> collectionClient;
        private ObservableCollection<PurchaseClient> collectionPurchaseClient;
        private Client selectedClientDataGrid;
        private string textboxSurname;
        private string textboxName;
        private string textboxPatronymic;
        private string textboxPhoneNumber;
        private string textboxEmail;
        private string textboxCodeProduct;
        private string textboxNameProduct;
        #endregion

        #region Команды
        #region Команда добавления клиента
        public ICommand CommandAddClient { get; }
        private void onCommandAddClientExecuted(object p)
        {
            client.AddClientAsync(TextboxSurname, TextboxName, TextboxPatronymic, TextboxPhoneNumber, TextboxEmail, CollectionClient);

            TextboxSurname = null;
            TextboxName = null;
            TextboxPatronymic = null;
            TextboxPhoneNumber = null;
            TextboxEmail = null;

        }
        private bool commandAddClientExecute(object p)
        {
            return true;
        }
        #endregion

        #region Команда удаления клиента
        public ICommand CommandDeleteClient { get; }
        private void onCommandDeleteClientExecuted(object p)
        {
            client.DeleteClientAsync(SelectedClientDataGrid,CollectionClient);
        }
        private bool commandDeleteClientExecute(object p) => true;
        #endregion

        #region Команда обновления данных
        public ICommand CommandUpdateClient { get; }
        private void onCommandUpdateClientExecuted(object p)
        {
            client.UpdateClientAsync(SelectedClientDataGrid,CollectionClient);
        }
        private bool commandUpdateClientExecute(object p) => true;
        #endregion

        #region Команда загрузки покупок у данного клиента
        public ICommand CommandLoadPurchaseClient { get; }
        private void onCommandLoadPurchaseClientExecuted(object p)
        {
            client.PrintPurchasesInClient(SelectedClientDataGrid, CollectionPurchaseClient);
        }
        private bool commandLoadPurchaseClientExecute(object p) => true;
        #endregion

        #region Команда добавление покупки клиенту
        public ICommand CommandAddBuyer { get; }
        private void onCommandAddBuyerExecuted(object p)
        {
            client.AddPurchasesInClient(SelectedClientDataGrid, TextboxCodeProduct, TextboxNameProduct,CollectionPurchaseClient);
            TextboxCodeProduct = null;
            TextboxNameProduct = null;
        }
        private bool commandAddBuyerExecute(object p) => true;
        #endregion
        #endregion

        public ViewModelMainWindow()
        {
            client = new Client();
            CommandAddClient = new Command(onCommandAddClientExecuted, commandAddClientExecute);
            CommandDeleteClient = new Command(onCommandDeleteClientExecuted, commandDeleteClientExecute);
            CommandUpdateClient = new Command(onCommandUpdateClientExecuted, commandUpdateClientExecute);
            CommandLoadPurchaseClient = new Command(onCommandLoadPurchaseClientExecuted, commandLoadPurchaseClientExecute);
            CommandAddBuyer = new Command(onCommandAddBuyerExecuted, commandAddBuyerExecute);
            CollectionClient = new ObservableCollection<Client>();
            CollectionPurchaseClient = new ObservableCollection<PurchaseClient>();
            client.Error += client.ErrorAsync;
            client.PrintDbAsync(CollectionClient, CollectionPurchaseClient);
        }
    }
}
