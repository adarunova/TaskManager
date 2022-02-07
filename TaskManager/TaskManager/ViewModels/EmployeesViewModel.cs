using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class EmployeesViewModel : BaseViewModel
    {

        private UserTable _selectedEmployee;

        private Alphabet _selectedLetter;

        private ObservableCollection<UserTable> _employees;

        private ObservableCollection<Alphabet> _alphabet;

        public ObservableCollection<UserTable> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Alphabet> Alphabet
        {
            get => _alphabet;
            set
            {
                _alphabet = value;
                OnPropertyChanged();
            }
        }

        public Command LoadEmployeesCommand { get; }

        public Command BackPage { get; }

        public Command<UserTable> EmployeeTapped { get; }

        public Command LetterTapped { get; }

        public UserTable SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);
                OnEmployeeSelected(value);
            }
        }

        public Alphabet SelectedLetter
        {
            get => _selectedLetter;
            set
            {
                SetProperty(ref _selectedLetter, value);
            }
        }

        public INavigation Navigation { get; }


        public EmployeesViewModel(INavigation navigation)
        {
            Navigation = navigation;
            EmployeeTapped = new Command<UserTable>(OnEmployeeSelected);
            LetterTapped = new Command(OnLetterTapped);
            LoadEmployeesCommand = new Command(ExecuteLoadEmployeesCommand);
            Alphabet = new ObservableCollection<Alphabet>() { new Alphabet { Letter = "All" } };
            Employees = new ObservableCollection<UserTable>();
        }

        private void ExecuteLoadEmployeesCommand()
        {
            IsBusy = true;

            try
            {
                List<Alphabet> alphabet = new List<Alphabet>();
                alphabet.Add(new Alphabet() { Letter = "All" });

                var employees = DocumentDBService.ServiceClientInstance.GetUsers(IdentityService.Company.CompanyName);
                if (employees != null)
                {
                    employees.Sort();
                    employees.ForEach(v => 
                    {
                        if (!alphabet.Any(a => a.Letter == v.Surname[0].ToString()))
                        {
                            alphabet.Add(new Alphabet { Letter = v.Surname[0].ToString() });
                        }
                    });

                    Employees = new ObservableCollection<UserTable>(employees);
                    Alphabet = new ObservableCollection<Alphabet>(alphabet);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public void OnAppearing()
        {
            IsBusy = true;
            SelectedEmployee = null;
            SelectedLetter = new Alphabet() { Letter = "All" };
        }


        async void OnEmployeeSelected(UserTable employee)
        {
            if (employee == null)
            {
                return;
            }

            await Navigation.PushModalAsync(new EmployeeProfilePage(employee));
        }

        private void OnLetterTapped()
        {

            if (SelectedLetter == null)
            {
                return;
            }

            Employees.Clear();

            var employees = DocumentDBService.ServiceClientInstance.GetUsers(IdentityService.Company.CompanyName);
            if (employees != null)
            {
                employees.Sort();
                employees.ForEach(v =>
                {
                    if (SelectedLetter.Letter == "All" || v.Surname[0].ToString() == SelectedLetter.Letter)
                    {
                        Employees.Add(v);
                    }
                });
            }
        }
    }
}
