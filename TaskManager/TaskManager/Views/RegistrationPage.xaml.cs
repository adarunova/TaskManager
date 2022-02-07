using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManager.Views
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            BindingContext = new RegisterCompanyViewModel(Navigation);
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}