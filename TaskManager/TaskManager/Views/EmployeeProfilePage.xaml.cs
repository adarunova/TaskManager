using System;
using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{
    public partial class EmployeeProfilePage : ContentPage
    {
        public EmployeeProfilePage(UserTable user)
        {
            InitializeComponent();

            BindingContext = new EmployeeProfileViewModel(user, Navigation);

            if (IdentityService.ProfileType == Models.ProfileType.Company)
            {
                assignTask.IsEnabled = false;
                assignTask.IsVisible = false;
            }
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}