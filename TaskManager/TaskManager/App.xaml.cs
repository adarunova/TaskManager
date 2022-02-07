using System;
using Xamarin.Forms;
using TaskManager.Services;
using TaskManager.Views;

namespace TaskManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
