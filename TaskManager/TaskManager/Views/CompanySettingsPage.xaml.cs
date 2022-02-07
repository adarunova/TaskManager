using System;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{
    public partial class CompanySettingsPage : ContentPage
    {
        public CompanySettingsPage()
        {
            InitializeComponent(); 
            
            BindingContext = new CompanySettingsViewModel(Navigation);
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}