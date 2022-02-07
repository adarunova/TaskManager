using System;
using System.Linq;
using TaskManager.Tables;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{

    public partial class AssignTaskPage : ContentPage
    {
        public AssignTaskPage(UserTable user, params object[] employee)
        {
            InitializeComponent();

            BindingContext = employee.Length > 0 ? 
                new AssignTaskViewModel(user, Navigation, employee) : 
                new AssignTaskViewModel(user, Navigation);

            MessagingCenter.Subscribe<ChooseEmployeePage, UserTable>(this, "EmployeeChoose", (sender, args) =>
            {
                if (!(BindingContext as AssignTaskViewModel).AssignedEmployees.Any(v => v.UserID == args.UserID))
                {
                    (BindingContext as AssignTaskViewModel).AssignedEmployees.Add(args);
                    (BindingContext as AssignTaskViewModel).CollectionHeight += 40;
                }
            });
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}