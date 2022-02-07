using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Services;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class RegisterCompanyViewModel : BaseViewModel
    {

        // Company email.
        private string _companyEmail;

        // Company name.
        private string _companyName;

        // Company password.
        private string _companyPassword;


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
        /// Company password.
        /// </summary>
        public string CompanyPassword
        {
            get => _companyPassword;
            set
            {
                _companyPassword = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// CompanyLogin button command.
        /// </summary>
        public ICommand CreateCompany { get; }

        public INavigation Navigation { get; } 


        /// <summary>
        /// Constructor.
        /// </summary>
        public RegisterCompanyViewModel(INavigation navigation)
        {
            Navigation = navigation;
            CreateCompany = new Command(async () => await RegisterCompany());
        }


        /// <summary>
        /// Registers a new company.
        /// </summary>
        private async Task RegisterCompany()
        {
            if (!IsValidEmail())
            {
                await App.Current.MainPage.DisplayAlert("Alert",
                    "Email addres is not valid",
                    "OK");
            }
            else
            {
                var companyWithNameExists = DocumentDBService.ServiceClientInstance.GetCompanies().Any(v => v.CompanyName == CompanyName);
                var companyWithEmailExists = DocumentDBService.ServiceClientInstance.GetCompanies().Any(v => v.CompanyEmail == CompanyEmail);

                if (!companyWithEmailExists && !companyWithNameExists)
                {
                    var response = await DocumentDBService.ServiceClientInstance.RegisterCompany(CompanyEmail, CompanyName, CompanyPassword);

                    await App.Current.MainPage.DisplayAlert("Alert",
                        response == true ?
                        "Company created successfully" :
                        "Company created unsuccessfully",
                        "OK");

                    await Navigation.PopModalAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert",
                        companyWithNameExists ?
                        $"A company with the name \"{CompanyName}\" already exists, please, change the name" :
                        $"A company with the email \"{CompanyEmail}\" already exists, please, change the email",
                        "OK");
                }
            }
        }

        private bool IsValidEmail()
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(CompanyEmail);
                return address.Address == CompanyEmail;
            }
            catch
            {
                return false;
            }
        }

    }
}
