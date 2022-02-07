using System.ComponentModel;
using TaskManager.Services;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{
    [DesignTimeVisible(false)]
    public partial class IssuedTasksPage : ContentPage
    {
        IssuedTasksViewModel _viewModel;

        public IssuedTasksPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new IssuedTasksViewModel(IdentityService.User, Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}