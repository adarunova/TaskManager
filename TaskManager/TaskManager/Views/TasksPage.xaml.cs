using System;
using System.ComponentModel;
using TaskManager.Services;
using TaskManager.ViewModels;

using Xamarin.Forms;

namespace TaskManager.Views
{
    [DesignTimeVisible(false)]
    public partial class TasksPage : ContentPage
    {
        TasksViewModel _viewModel;

        public TasksPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new TasksViewModel(IdentityService.User, Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

    }
}