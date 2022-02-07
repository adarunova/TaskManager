using System;
using TaskManager.Models;
using Xamarin.Forms;
using TaskManager.ViewModels;
using TaskManager.Services;
using FFImageLoading.Svg.Forms;

namespace TaskManager.Views
{

    public partial class CompanyProfilePage : ContentPage
    {

        CompanyProfileViewModel _viewModel;

        public CompanyProfilePage()
        {
            InitializeComponent();

            if (IdentityService.ProfileType != ProfileType.Company)
            {
                logotype.IsEnabled = true;
                logotype.WidthRequest = 17;
                logotype.HeightRequest = 17;
                logotype.Source = SvgImageSource.FromFile("back.svg");
                settingsIconButton.IsEnabled = false;
                settingsIconButton.IsVisible = false;
            }
            else
            {
                addNews.IsEnabled = false;
                addNews.IsVisible = false;
                logotype.IsEnabled = false;
            }

            BindingContext = _viewModel = new CompanyProfileViewModel(IdentityService.Company, Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); 
        }


        private async void OnAddNewEmployeeTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EmployeesPage());
        }

        private async void OnSettingsTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CompanySettingsPage());
        }

    }
}