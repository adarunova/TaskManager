using System;
using System.Collections.ObjectModel;
using System.Linq;
using TaskManager.Services;
using TaskManager.Tables;
using Xamarin.Forms;

namespace TaskManager.Views
{

    public partial class ChooseEmployeePage : ContentPage
    {

        public static ObservableCollection<UserTable> Employees { get; private set; }

        private UserTable User { get; }

        public ChooseEmployeePage(UserTable user)
        {
            InitializeComponent();

            BindingContext = this;

            User = user;
            Employees = new ObservableCollection<UserTable>();
            GetEmployeesList();
            SearchResultsCollection.ItemsSource = GetSearchResults(SearchCompanyBar.Text);
        }

        private void GetEmployeesList()
        {
            var response = DocumentDBService.ServiceClientInstance.GetUsers(User.Company);
            response.ForEach(v => Employees.Add(v));
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchResultsCollection.ItemsSource = GetSearchResults(e.NewTextValue);
        }

        public ObservableCollection<UserTable> GetSearchResults(string queryString)
        {
            if (String.IsNullOrEmpty(queryString))
            {
                return Employees;
            }
            var normalizedQuery = queryString?.ToLower() ?? "";
            var newCollection = new ObservableCollection<UserTable>();
            Employees.Where(v => (v.Name + " " + v.Surname).ToLowerInvariant().Contains(normalizedQuery)).ToList().ForEach(v => newCollection.Add(v));
            return newCollection;
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Navigation.PopModalAsync();
            MessagingCenter.Send(this, "EmployeeChoose", SearchResultsCollection.SelectedItem as UserTable);
        }
    }
}