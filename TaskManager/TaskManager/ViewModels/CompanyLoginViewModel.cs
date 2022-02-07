using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    public class CompanyLoginViewModel : BaseViewModel
    {

        // Company.
        private string _company;

        // Company password.
        private string _password;


        /// <summary>
        /// Company.
        /// </summary>
        public string Company
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Company password.
        /// </summary>
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// CompanyLogin button command.
        /// </summary>
        public ICommand LoginCompany { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CompanyLoginViewModel()
        {
            LoginCompany = new Command(async () => await PerformLogin());
        }


        /// <summary>
        /// Handles a click on the LoginUser button.
        /// </summary>
        private async Task PerformLogin()
        {
            var response = DocumentDBService.ServiceClientInstance.LoginCompany(Company, Password);

            if (response != null)
            {
                IdentityService.ProfileType = ProfileType.Company;
                IdentityService.Company = response;
                App.Current.MainPage = new CompanyProfilePage();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alert", $"Invalid password", "OK");
            }

        }
    }
}
