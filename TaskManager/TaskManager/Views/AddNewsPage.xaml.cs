using System;
using TaskManager.Tables;
using TaskManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewsPage : ContentPage
    {
        public AddNewsPage(UserTable user)
        {
            InitializeComponent();

            BindingContext = new AddNewsViewModel(user, Navigation);
        }

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}