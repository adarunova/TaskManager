using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManager.Views
{
    public partial class EmployeesPage : ContentPage
    {
        EmployeesViewModel _viewModel;

        public EmployeesPage()
        {
            InitializeComponent();

            if (IdentityService.ProfileType != ProfileType.Company)
            {
                addEmployeeIconButton.IsEnabled = false;
                addEmployeeIconButton.IsVisible = false;
            }

            BindingContext = _viewModel = new EmployeesViewModel(Navigation);
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

        private async void OnAddEmployee(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NewUserPage());
        }
    }
}