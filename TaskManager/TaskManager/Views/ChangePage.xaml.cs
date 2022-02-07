using System;
using TaskManager.Models;
using TaskManager.Services;
using Xamarin.Forms;

namespace TaskManager.Views
{
    public partial class ChangePage : ContentPage
    {
        private SettingsType _settingsType;

        public ChangePage(string title, string subtitle, SettingsType settingsType)
        {
            InitializeComponent();

            titleLabel.Text = title;
            subtitleLabel.Text = subtitle;
            _settingsType = settingsType;

        }

        private async void OnBack(object sender, EventArgs e)
        {
            var result = await App.Current.MainPage.DisplayAlert("Alert", "All changes won't be saved. Do you want to exit?", "OK", "CANCEL");
            if (result)
            {
                await Navigation.PopModalAsync();
            }
        }


        private async void OnChange(object sender, EventArgs e)
        {
            var user = IdentityService.User;
            switch (_settingsType)
            {
                case SettingsType.Name:

                    if (String.IsNullOrEmpty(entry.Text))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", $"Write the correct name.", "OK");
                        return;
                    }
                    MakeCorrectNames();
                    user.Name = entry.Text;
                    await DocumentDBService.ServiceClientInstance.UpdateUser(user);
                    MessagingCenter.Send(this, "UserChanged", user);
                    break;

                case SettingsType.Surname:

                    if (String.IsNullOrEmpty(entry.Text))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", $"Write the correct surname.", "OK");
                        return;
                    }
                    MakeCorrectNames();
                    user.Surname = entry.Text;
                    await DocumentDBService.ServiceClientInstance.UpdateUser(user);
                    MessagingCenter.Send(this, "UserChanged", user);
                    break;

                case SettingsType.UserPassword:

                    if (String.IsNullOrEmpty(entry.Text))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", $"Write the correct password.", "OK");
                        return;
                    }
                    user.Password = entry.Text;
                    await DocumentDBService.ServiceClientInstance.UpdateUser(user);
                    MessagingCenter.Send(this, "UserChanged", user);
                    break;

                case SettingsType.CompanyPassword:

                    if (String.IsNullOrEmpty(entry.Text))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", $"Write the correct password.", "OK");
                        return;
                    }
                    var company = IdentityService.Company;
                    company.CompanyPassword = entry.Text;
                    await DocumentDBService.ServiceClientInstance.UpdateCompany(company);
                    MessagingCenter.Send(this, "CompanyChanged", user);
                    break;
            }

            await Navigation.PopModalAsync();
        }

        private void MakeCorrectNames()
        {
            if (entry.Text.Length > 0 && entry.Text[0].ToString().ToUpper() != entry.Text[0].ToString())
            {
                entry.Text = entry.Text.Length == 1 ? entry.Text[0].ToString().ToUpper() : entry.Text[0].ToString().ToUpper() + entry.Text.Substring(1);
            }
        }

    }
}