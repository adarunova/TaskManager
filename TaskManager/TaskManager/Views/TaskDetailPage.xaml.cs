using System;
using TaskManager.Models;
using TaskManager.Tables;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{
    public partial class TaskDetailPage : ContentPage
    {
        public TaskDetailPage(UserTable user, Tasks task, bool assignedTask)
        {
            InitializeComponent();
            BindingContext = new TaskDetailViewModel(user, task, Navigation, assignedTask);

            if (!assignedTask)
            {
                if (task.Completed)
                {
                    submitButton.Text = "OK";
                    submitButton.WidthRequest = 100;
                }
                else
                {
                    submitButton.IsEnabled = false;
                    submitButton.IsVisible = false;
                }
            }
        }

        private async void OnBack(object sender, EventArgs e)
        {
            //MessagingCenter.Send(this, "Refresh", new EventArgs());
            await Navigation.PopModalAsync();
        }

    }
}