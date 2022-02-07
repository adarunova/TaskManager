using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tables;
using TaskManager.Views;
using Xamarin.Forms;

namespace TaskManager.ViewModels
{
    class TaskDetailViewModel : BaseViewModel
    {
        private Tasks _tappedTask;

        private bool _assignedTask;

        public Tasks TappedTask
        {
            get => _tappedTask;
            set
            {
                _tappedTask = value;
                OnPropertyChanged();
            }
        }

        private UserTable _user;

        public int AssignedCollectionHeight => TappedTask.AssignedEmployees.Count * 40;

        public int CompletedCollectionHeight => TappedTask.CompletedTaskEmployees.Count * 40;

        public Command NotesTapped { get; }

        public Command SubmitTask { get; }

        public INavigation Navigation { get; }

        public TaskDetailViewModel(UserTable user, Tasks task, INavigation navigation, bool assignedTask)
        {
            _user = user;
            _assignedTask = assignedTask;
            Navigation = navigation;
            TappedTask = task;
            NotesTapped = new Command(OnNotesTapped);
            SubmitTask = new Command(OnSubmitTask);

            MessagingCenter.Subscribe<NotePage, string>(this, "NoteChanged", (sender, args) =>
            {
                _tappedTask.Notes = args;
                TappedTask = _tappedTask;
            });
        }

        private async void OnNotesTapped()
        {
            await Navigation.PushModalAsync(new NotePage(TappedTask));
        }

        private async void OnSubmitTask()
        {
            try
            {
                if (!_assignedTask)
                {
                    await DocumentDBService.ServiceClientInstance.DeleteTask(TappedTask);

                    await Navigation.PopModalAsync();
                    return;
                }

                foreach (var user in TappedTask.AssignedEmployees)
                {
                    if (_user.UserID == user.UserID)
                    {
                        TappedTask.AssignedEmployees.Remove(user);
                        break;
                    }
                }

                TappedTask.CompletedTaskEmployees.Add(_user);

                if (TappedTask.AssignedEmployees.Count == 0)
                {
                    TappedTask.Completed = true;
                }

                await DocumentDBService.ServiceClientInstance.UpdateTask(TappedTask);

                await Navigation.PopModalAsync();
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Error. Please, try to submit the task later.", "OK", "CANCEL");
            }
        }
    }
}
