using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{

    public partial class CompanyLoginPage : ContentPage
    {
        public CompanyLoginPage()
        {
            InitializeComponent();

            BindingContext = new CompanyLoginViewModel();
            companyLabel.Text = "Your company";

            MessagingCenter.Subscribe<ChooseCompanyPage, string>(this, "CompanyChoose", (sender, args) =>
            {
                (BindingContext as CompanyLoginViewModel).Company = args;
                companyLabel.Text = args;
            });
        }

        private async void OnCompanyLabelTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ChooseCompanyPage());
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}