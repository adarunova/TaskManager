using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    public class CompanyProfileViewModel : BaseViewModel
    {

        private CompanyTable _company;

        // Company name.
        private string _companyName;

        // Company email.
        private string _companyEmail;

        // Count оf employees.
        private string _countOfEmployees;


        private ObservableCollection<News> _news;


        /// <summary>
        /// Company name.
        /// </summary>
        public string CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Company email.
        /// </summary>
        public string CompanyEmail
        {
            get => _companyEmail;
            set
            {
                _companyEmail = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Count оf employees.
        /// </summary>
        public string CountOfEmployees
        {
            get => _countOfEmployees;
            set
            {
                _countOfEmployees = value;
                OnPropertyChanged();
            }
        }

        public Command LoadCompanyProfileCommand { get; }

        public Command AddNewsTapped { get; }

        public ObservableCollection<News> News
        {
            get => _news;
            set
            {
                _news = value;
                OnPropertyChanged();
            }
        }


        public INavigation Navigation { get; }

        public CompanyProfileViewModel(CompanyTable company, INavigation navigation)
        {
            _company = company;
            Navigation = navigation;
            CompanyName = company.CompanyName;
            CompanyEmail = company.CompanyEmail;
            News = new ObservableCollection<News>();

            AddNewsTapped = new Command(OnAddNewsTapped);

            LoadCompanyProfileCommand = new Command(async () => await ExecuteLoadCompanyProfileCommand());
            LoadCompanyProfileCommand.Execute(null);
        }

        private async Task ExecuteLoadCompanyProfileCommand()
        {
            IsBusy = true;

            try
            {
                var employees = DocumentDBService.ServiceClientInstance.GetUsers(_company.CompanyName);
                if (employees != null)
                {
                    CountOfEmployees = employees.Count.ToString() + " employees";
                }
                else
                {
                    CountOfEmployees = "0 employees";
                }

                var news = await DocumentDBService.ServiceClientInstance.GetCompanyNews(_company);
                if (news != null)
                {
                    News = new ObservableCollection<News>(news);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnAddNewsTapped()
        {
            if (IdentityService.User != null)
            {
                await Navigation.PushModalAsync(new AddNewsPage(IdentityService.User));
            }
        }

    }
}
