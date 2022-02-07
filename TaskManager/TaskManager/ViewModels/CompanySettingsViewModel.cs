using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class CompanySettingsViewModel : BaseViewModel
    {
        private CompanyTable _company;


        public CompanyTable Company
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChanged();
            }
        }


        public Command LogoutTapped { get; }

        public Command ChangeNameTapped { get; }

        public Command ChangeSurnameTapped { get; }

        public Command ChangePasswordTapped { get; }


        public INavigation Navigation { get; }

        public CompanySettingsViewModel(INavigation navigation)
        {
            _company = IdentityService.Company;
            Navigation = navigation;
            LogoutTapped = new Command(OnLogoutTapped);
            ChangePasswordTapped = new Command(OnChangePasswordTapped);

            MessagingCenter.Subscribe<ChangePage, CompanyTable>(this, "CompanyChanged", (sender, args) =>
            {
                _company = args;
                Company = _company;
            });
        }

        private void OnLogoutTapped()
        {
            IdentityService.ProfileType = Models.ProfileType.None;
            IdentityService.User = null;
            IdentityService.Company = null;

            App.Current.MainPage = new LoginPage();
        }

        private async void OnChangePasswordTapped()
        {
            await Navigation.PushModalAsync(new ChangePage("CHANGE PASSWORD", "New Password", Models.SettingsType.CompanyPassword));
        }
    }
}
