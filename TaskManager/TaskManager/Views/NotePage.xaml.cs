using System;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.ViewModels;
using Xamarin.Forms;

namespace TaskManager.Views
{
    public partial class NotePage : ContentPage
    {
        public Tasks TappedTask { get; set; }

        public NotePage(Tasks task)
        {
            InitializeComponent();

            TappedTask = task;
            BindingContext = new NoteViewModel(task, Navigation);
        }

        private async void OnBack(object sender, EventArgs e)
        {
            var result = await App.Current.MainPage.DisplayAlert("Alert", "All changes won't be saved. Do you want to exit?", "OK", "CANCEL");
            if (result)
            {
                await Navigation.PopModalAsync();
            }
        }

        private async void OnDone(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "NoteChanged", editor.Text);


            TappedTask.Notes = editor.Text;

            await DocumentDBService.ServiceClientInstance.UpdateTask(TappedTask);

            await Navigation.PopModalAsync();
        }


    }
}