using TaskManager.Views;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using TaskManager.Services;
using System.Linq;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        // Username.
        private string _username;

        // User password.
        private string _password;

        // The company where the user works.
        private string _company;

        /// <summary>
        /// Username.
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// User password.
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
        /// The company where the user works.
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
        /// LoginUser button command.
        /// </summary>
        public ICommand LoginUser { get; }

        /// <summary>
        /// RegisterCompany button command.
        /// </summary>
        public ICommand RegisterCompany { get; }

        /// <summary>
        /// CompanyLogin button command.
        /// </summary>
        public ICommand LoginCompany { get; }

        public INavigation Navigation { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            LoginUser = new Command(async () => await PerformLogin());
            RegisterCompany = new Command(OnRegisterCompanyCliked);
            LoginCompany = new Command(OnLoginCompanyCliked);
        }

        /// <summary>
        /// Handles a click on the RegisterCompany button.
        /// </summary>
        private async void OnRegisterCompanyCliked()
        {
            await Navigation.PushModalAsync(new RegistrationPage());
        }

        private async void OnLoginCompanyCliked()
        {
            await Navigation.PushModalAsync(new CompanyLoginPage());
        }


        /// <summary>
        /// Handles a click on the LoginUser button.
        /// </summary>
        /// <returns></returns>
        private async Task PerformLogin()
        {
            var response = DocumentDBService.ServiceClientInstance.LoginUser(Username, Password, Company);

            if (response != null)
            {
                IdentityService.ProfileType = ProfileType.Employee;
                IdentityService.User = response;
                var company = DocumentDBService.ServiceClientInstance.GetCompanies().Where(v => v.CompanyName == response.Company).FirstOrDefault();
                IdentityService.Company = company;

                App.Current.MainPage = new AppShell();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alert", "User not found or incorrect password", "OK");
            }

        }
    }
}
