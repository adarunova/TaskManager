using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class IssuedTasksViewModel : BaseViewModel
    {
        private UserTable _user;

        private bool _hasTasks;

        private bool _noTasks;

        private ObservableCollection<Tasks> _issuedTasks;

        public Command LoadIssuedTasksCommand { get; }

        public Command AssignTaskTapped { get; }

        public Command<Tasks> TaskTapped { get; }

        public bool HasTasks
        {
            get => _hasTasks;
            set
            {
                _hasTasks = value;
                OnPropertyChanged();
            }
        }

        public bool NoTasks
        {
            get => _noTasks;
            set
            {
                _noTasks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Tasks> IssuedTasks
        {
            get => _issuedTasks;
            set
            {
                _issuedTasks = value;
                OnPropertyChanged();
            }
        }

        public INavigation Navigation { get; }

        public IssuedTasksViewModel(UserTable user, INavigation navigation)
        {
            _user = user;
            Navigation = navigation;
            IssuedTasks = new ObservableCollection<Tasks>();
            HasTasks = false;
            NoTasks = true;
            LoadIssuedTasksCommand = new Command(async () => await ExecuteLoadIssuedTasksCommand());
            TaskTapped = new Command<Tasks>(OnTaskTapped);
            AssignTaskTapped = new Command(OnAssignTaskTapped);
            LoadIssuedTasksCommand.Execute(null);
        }

        async Task ExecuteLoadIssuedTasksCommand()
        {
            IsBusy = true;

            try
            {
                IssuedTasks.Clear();
                IssuedTasks = new ObservableCollection<Tasks>(await DocumentDBService.ServiceClientInstance.GetIssuedTasks(_user));
                HasTasks = IssuedTasks.Count != 0;
                NoTasks = !HasTasks;
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
        }


        private async void OnAssignTaskTapped()
        {
            await Navigation.PushModalAsync(new AssignTaskPage(_user));
        }


        private async void OnTaskTapped(Tasks task)
        {
            if (task == null)
            {
                return;
            }

            await Navigation.PushModalAsync(new TaskDetailPage(_user, task, false));

        }
    }
}
