using System;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{
    public partial class UserSettingsPage : ContentPage
    {
        public UserSettingsPage()
        {
            InitializeComponent();

            BindingContext = new UserSettingsViewModel(Navigation);
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}