using System;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();

            BindingContext = new ProfileViewModel();
        }

        private async void OnCompanyProfileTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CompanyProfilePage());
        }

        private async void OnSettingsTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new UserSettingsPage());
        }

    }
}