using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class ProfileViewModel : BaseViewModel
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

        public ProfileViewModel()
        {
            User = IdentityService.User;
            MessagingCenter.Subscribe<ChangePage, UserTable>(this, "UserChanged", (sender, args) =>
            {
                _user = args;
                User = _user;
            });
        }

    }
}
