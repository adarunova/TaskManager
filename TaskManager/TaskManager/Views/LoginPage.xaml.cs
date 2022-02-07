using System;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{

    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
            companyLabel.Text = "Your company";

            MessagingCenter.Subscribe<ChooseCompanyPage, string>(this, "CompanyChoose", (sender, args) =>
            {
                (BindingContext as LoginViewModel).Company = args;
                companyLabel.Text = args;
            });
        }

        private async void OnCompanyLabelTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ChooseCompanyPage());
        }
    }
}