using TaskManager.Models;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class NoteViewModel
    {
        public Tasks TappedTask { get; set; }

        public Command DoneTapped { get; }

        public Command BackTapped { get; }

        public INavigation Navigation { get; }

        public NoteViewModel(Tasks task, INavigation navigation)
        {
            Navigation = navigation;
            TappedTask = task;
            DoneTapped = new Command(OnDoneTapped);
            BackTapped = new Command(OnBack);
        }

        private async void OnDoneTapped()
        {
            await Navigation.PopModalAsync();
        }

        private async void OnBack()
        {
            var result = await App.Current.MainPage.DisplayAlert("Alert", "All changes won't be saved. Do you want to exit?", "OK", "CANCEL");
            if (result)
            {
                await Navigation.PopModalAsync();
            }
        }

    }
}
