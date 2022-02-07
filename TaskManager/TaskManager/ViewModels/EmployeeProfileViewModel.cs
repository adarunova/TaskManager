using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class EmployeeProfileViewModel : BaseViewModel
    {
        private UserTable _user;

        public string EmployeeName { get; }

        public string EmployeeSurname { get; }

        public string EmployeeCompany { get; }

        public string EmployeePhoneNumber { get; }

        public string EmployeeEmail { get; }


        public Command AssignTaskTapped { get; }

        public INavigation Navigation { get; }

        public EmployeeProfileViewModel(UserTable user, INavigation navigation)
        {
            _user = user;
            Navigation = navigation;
            EmployeeName = user.Name;
            EmployeeSurname = " " + user.Surname;
            EmployeeCompany = user.Company;
            EmployeePhoneNumber = user.PhoneNumber;
            EmployeeEmail = user.Email;
            AssignTaskTapped = new Command(OnAssignTaskTapped);
        }

        private async void OnAssignTaskTapped()
        {
            if (IdentityService.User != null)
            {
                await Navigation.PushModalAsync(new AssignTaskPage(IdentityService.User, _user));
            }
        }

    }
}
