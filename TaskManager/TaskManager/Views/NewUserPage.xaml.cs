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
    public partial class NewUserPage : ContentPage
    {
        public NewUserPage()
        {
            InitializeComponent();
            BindingContext = new NewUserViewModel(Navigation);
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}