using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class UserSettingsViewModel : BaseViewModel
    {
        private UserTable _user;


        public UserTable User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }


        public Command LogoutTapped { get; }

        public Command ChangeNameTapped { get; }

        public Command ChangeSurnameTapped { get; }

        public Command ChangePasswordTapped { get; }


        public INavigation Navigation { get; }

        public UserSettingsViewModel(INavigation navigation)
        {
            _user = IdentityService.User;
            Navigation = navigation;
            LogoutTapped = new Command(OnLogoutTapped);
            ChangeNameTapped = new Command(OnChangeNameTapped);
            ChangeSurnameTapped = new Command(OnChangeSurnameTapped);
            ChangePasswordTapped = new Command(OnChangePasswordTapped);

            MessagingCenter.Subscribe<ChangePage, UserTable>(this, "UserChanged", (sender, args) =>
            {
                _user = args;
                User = _user;
            });
        }

        private void OnLogoutTapped()
        {
            IdentityService.ProfileType = Models.ProfileType.None;
            IdentityService.User = null;
            IdentityService.Company = null;

            App.Current.MainPage = new LoginPage();
        }

        private async void OnChangeNameTapped()
        {
            await Navigation.PushModalAsync(new ChangePage("CHANGE NAME", "New Name", Models.SettingsType.Name));
        }

        private async void OnChangeSurnameTapped()
        {
            await Navigation.PushModalAsync(new ChangePage("CHANGE SURNAME", "New Surname", Models.SettingsType.Surname));
        }

        private async void OnChangePasswordTapped()
        {
            await Navigation.PushModalAsync(new ChangePage("CHANGE PASSWORD", "New Password", Models.SettingsType.UserPassword));
        }
    }
}
