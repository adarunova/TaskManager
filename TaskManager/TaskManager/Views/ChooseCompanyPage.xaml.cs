using System;
using System.Collections.ObjectModel;
using System.Linq;
using TaskManager.Services;
using TaskManager.Tables;
using Xamarin.Forms;

namespace TaskManager.Views
{
    public partial class ChooseCompanyPage : ContentPage
    {
        public static ObservableCollection<CompanyTable> Companies { get; private set; }

        public ChooseCompanyPage()
        {
            InitializeComponent();

            BindingContext = this;

            Companies = new ObservableCollection<CompanyTable>();
            GetCompaniesList();
            SearchResultsCollection.ItemsSource = GetSearchResults(SearchCompanyBar.Text);
        }

        private void GetCompaniesList()
        {
            var response = DocumentDBService.ServiceClientInstance.GetCompanies();
            response.ForEach(v => Companies.Add(v));
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchResultsCollection.ItemsSource = GetSearchResults(e.NewTextValue);
        }

        public ObservableCollection<CompanyTable> GetSearchResults(string queryString)
        {
            if (String.IsNullOrEmpty(queryString))
            {
                return Companies;
            }
            var normalizedQuery = queryString?.ToLower() ?? "";
            var newCollection = new ObservableCollection<CompanyTable>();
            Companies.Where(v => v.CompanyName.ToLowerInvariant().Contains(normalizedQuery)).ToList().ForEach(v => newCollection.Add(v));
            return newCollection;
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Navigation.PopModalAsync();
            MessagingCenter.Send(this, "CompanyChoose", (SearchResultsCollection.SelectedItem as CompanyTable).CompanyName);
        }
    }
}