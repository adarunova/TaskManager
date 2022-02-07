using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class NewUserViewModel : BaseViewModel
    {
        private string _username;

        private string _password;

        private string _name;

        private string _surname;

        private string _phoneNumber;

        private string _email;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public Command AddCommand { get; }

        public Command CancelCommand { get; }

        public INavigation Navigation { get; }

        public NewUserViewModel(INavigation navigation)
        {
            Navigation = navigation;
            AddCommand = new Command(OnAdd, ValidData);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => AddCommand.ChangeCanExecute();
        }

        private bool ValidData()
        {
            return !String.IsNullOrWhiteSpace(_username) && !String.IsNullOrWhiteSpace(_password) &&
                !String.IsNullOrWhiteSpace(_name) && !String.IsNullOrWhiteSpace(_surname) &&
                !String.IsNullOrWhiteSpace(_phoneNumber) && !String.IsNullOrWhiteSpace(_email);
        }


        private async void OnCancel()
        {
            await Navigation.PopModalAsync();
        }

        private async void OnAdd()
        {
            if (!ValidData())
            {
                await App.Current.MainPage.DisplayAlert("Alert", $"Write the correct information about the new employee.", "OK");
            }
            else
            {
                await RegisterUser();
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        private async Task RegisterUser()
        {
            if (!IsValidEmail())
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Email addres is not valid", "OK");
            }
            else
            {
                var userExists = DocumentDBService.ServiceClientInstance.GetUsers(IdentityService.Company.CompanyName)
                    .Where(v => v.Username == Username || v.PhoneNumber == PhoneNumber || v.Email == Email)
                    .FirstOrDefault();

                if (userExists == null)
                {
                    MakeCorrectNames();
                    
                    var response = await DocumentDBService.ServiceClientInstance
                        .RegisterUser(Username, Password, IdentityService.Company.CompanyName, Name, Surname, PhoneNumber, Email);
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "The user already exists.", "OK");
                }
            }
        }

        private void MakeCorrectNames()
        {
            if (Name.Length > 0 && Name[0].ToString().ToUpper() != Name[0].ToString())
            {
                Name = Name.Length == 1 ? Name[0].ToString().ToUpper() : Name[0].ToString().ToUpper() + Name.Substring(1);
            }
            if (Surname.Length > 0 && Surname[0].ToString().ToUpper() != Surname[0].ToString())
            {
                Surname = Surname.Length == 1 ? Surname[0].ToString().ToUpper() : Surname[0].ToString().ToUpper() + Surname.Substring(1);
            }
        }

        private bool IsValidEmail()
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(Email);
                return address.Address == Email;
            }
            catch
            {
                return false;
            }
        }

    }
}
