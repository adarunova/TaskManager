using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class TasksViewModel : BaseViewModel
    {
        private UserTable _user;

        private bool _hasTasks;

        private bool _calendarIsVisible;

        private const string DateWithTaskColor = "#FF623C";

        private const string DateWithoutTaskColor = "#2B4D66";

        private ObservableCollection<Tasks> _tasks;

        public bool HasTasks
        {
            get => _hasTasks;
            set
            {
                _hasTasks = value;
                OnPropertyChanged();
            }
        }

        public bool CalendarIsVisible
        {
            get => _calendarIsVisible;
            set
            {
                _calendarIsVisible = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Date> Dates { get; }

        public Command LoadTasksCommand { get; }

        public Command<Tasks> TaskTapped { get; }

        public ObservableCollection<Tasks> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }

        public INavigation Navigation { get; }

        public TasksViewModel(UserTable user, INavigation navigation)
        {
            _user = user;
            Navigation = navigation;
            Dates = new ObservableCollection<Date>();
            Tasks = new ObservableCollection<Tasks>();
            HasTasks = false;
            CalendarIsVisible = false;
            TaskTapped = new Command<Tasks>(OnTaskTapped);
            LoadTasksCommand = new Command(async () => await ExecuteLoadTasksCommand());
            LoadTasksCommand.Execute(null);
        }
        

        async Task ExecuteLoadTasksCommand()
        {
            IsBusy = true;

            try
            {
                Tasks.Clear();

                Tasks = new ObservableCollection<Tasks>(await DocumentDBService.ServiceClientInstance.GetEmployeeTasks(_user));

                HasTasks = Tasks.Count == 0;
                CalendarIsVisible = !HasTasks;

                LoadDates();
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

        private void LoadDates()
        {
            Dates.Clear();

            var dateInit = DateTime.Today;
            var dateEnd = DateTime.Today;

            foreach(var item in Tasks)
            {
                if (item.Deadline < dateInit)
                {
                    dateInit = item.Deadline;
                }
                if (item.Deadline > dateEnd)
                {
                    dateEnd = item.Deadline;
                }
            }

            for (var day = dateInit.Date; day.Date <= dateEnd.Date; day = day.AddDays(1))
            {
                string color = DateWithoutTaskColor;
                if (Tasks.Any(v => v.Deadline == day))
                {
                    color = DateWithTaskColor;
                }
                Dates.Add(new Date()
                {
                    Day = String.Format("{0:00}", day.Day),
                    Month = day.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US")),
                    DayOfWeek = day.DayOfWeek.ToString().Substring(0, 3),
                    Color = color,
                });
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnTaskTapped(Tasks task)
        {
            if (task == null)
            {
                return;
            }

            await Navigation.PushModalAsync(new TaskDetailPage(_user, task, true));
        }
    }
}
