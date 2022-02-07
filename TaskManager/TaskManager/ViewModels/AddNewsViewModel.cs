using System;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tables;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class AddNewsViewModel : BaseViewModel
    {
        private string _newsText;

        private DateTime _removalDay;

        private DateTime _publishDay;


        public string NewsText
        {
            get => _newsText;
            set
            {
                _newsText = value;
                OnPropertyChanged();
            }
        }

        public DateTime PublishDay
        {
            get => _publishDay;
            set
            {
                _publishDay = value;
                OnPropertyChanged();
            }
        }

        public DateTime RemovalDay
        {
            get => _removalDay;
            set
            {
                _removalDay = value;
                OnPropertyChanged();
            }
        }

        public DateTime MinDate => DateTime.Today;

        private UserTable Publisher { get; }


        public Command DoneTapped { get; }


        public INavigation Navigation { get; }


        public AddNewsViewModel(UserTable user, INavigation navigation)
        {
            Navigation = navigation;
            Publisher = user;
            PublishDay = DateTime.Today;
            RemovalDay = DateTime.Today;
            DoneTapped = new Command(OnDoneTapped);
        }

        private async void OnDoneTapped()
        {
            if (String.IsNullOrEmpty(NewsText))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Write a text.", "OK");
            }
            else if (RemovalDay < PublishDay)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Set the correct dates.", "OK");
            }
            else
            {
                var news = new News
                {
                    NewsID = Guid.NewGuid(),
                    CompanyID = IdentityService.Company.CompanyID,
                    NewsText = NewsText,
                    PublishDay = PublishDay,
                    RemovalDay = RemovalDay,
                    PublisherName = Publisher.Name + " " + Publisher.Surname
                };

                var response = await DocumentDBService.ServiceClientInstance.AddNews(news);

                await App.Current.MainPage.DisplayAlert("Alert",
                        response == true ?
                        "News created successfully" :
                        "News created unsuccessfully",
                        "OK");

                await Navigation.PopModalAsync();
            }
        }
    }
}
