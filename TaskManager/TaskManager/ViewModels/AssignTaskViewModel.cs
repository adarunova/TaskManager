using System;
using System.Collections.ObjectModel;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class AssignTaskViewModel : BaseViewModel
    {

        private string _taskName;

        private string _description;

        private DateTime _deadline;

        private int _collectionHeight;

        private ObservableCollection<UserTable> _assignedEmployees;


        public string TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                OnPropertyChanged();
            }
        }


        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }


        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
                OnPropertyChanged();
            }
        }

        public int CollectionHeight
        {
            get
            {
                return _collectionHeight;
            }
            set
            {
                _collectionHeight = value;
                OnPropertyChanged();
            }
        }

        public DateTime MinDate => DateTime.Today;

        private UserTable Employer { get; }


        public Command DoneTapped { get; }

        public Command AssignEmployeeTapped { get; }

        public Command<UserTable> DeleteEmployeeTapped { get; }

        public ObservableCollection<UserTable> AssignedEmployees 
        {
            get
            {
                return _assignedEmployees;
            }
            set
            {
                _assignedEmployees = value;
                CollectionHeight = _assignedEmployees.Count * 40;
                OnPropertyChanged();
            }
        }

        public INavigation Navigation { get; }


        public AssignTaskViewModel(UserTable user, INavigation navigation, params object[] employee)
        {
            Navigation = navigation;
            Employer = user;
            Deadline = DateTime.Today;
            DoneTapped = new Command(OnDoneTapped);
            AssignEmployeeTapped = new Command(OnAssignEmployee);
            AssignedEmployees = new ObservableCollection<UserTable>();

            if (employee.Length > 0)
            {
                AssignedEmployees.Add((UserTable)employee[0]);
                CollectionHeight += 40;
            }

            DeleteEmployeeTapped = new Command<UserTable>(OnDeleteEmployeeTapped);
        }


        private async void OnAssignEmployee()
        {
            await Navigation.PushModalAsync(new ChooseEmployeePage(Employer));
        }

        private void OnDeleteEmployeeTapped(UserTable employee)
        {
            if (employee == null)
            {
                return;
            }

            AssignedEmployees.Remove(employee);
            CollectionHeight -= 40;
        }

        private async void OnDoneTapped()
        {
            if (String.IsNullOrEmpty(TaskName))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Enter the task name.", "OK");
            }
            else if (AssignedEmployees.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Assign employees to the task.", "OK");
            }
            else
            {
                var task = new Tasks
                {
                    TaskID = Guid.NewGuid(),
                    TaskName = TaskName,
                    Description = Description,
                    Deadline = Deadline,
                    Completed = false,
                    Employer = Employer,
                    Notes = String.Empty,
                    AssignedEmployees = AssignedEmployees,
                    CompletedTaskEmployees = new ObservableCollection<UserTable>()
                };

                var response = await DocumentDBService.ServiceClientInstance.AssignTaskToEmployees(task);

                await App.Current.MainPage.DisplayAlert("Alert",
                        response == true ?
                        "Task assigned successfully" :
                        "Task assigned unsuccessfully",
                        "OK");

                await Navigation.PopModalAsync();
            }
        }
    }
}
