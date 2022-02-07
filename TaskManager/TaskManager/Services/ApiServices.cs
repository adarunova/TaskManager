using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using TaskManager.Models;
using TaskManager.Tables;

namespace TaskManager.Services
{
    public class ApiServices
    {
        // A service client instance.
        private static ApiServices _serviceClientInstance;

        // Firebase.
        private FirebaseClient _firebase;


        /// <summary>
        /// A service client instance.
        /// </summary>
        public static ApiServices ServiceClientInstance
        {
            get
            {
                if (_serviceClientInstance == null)
                {
                    _serviceClientInstance = new ApiServices();
                }
                return _serviceClientInstance;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApiServices()
        {
            _firebase = new FirebaseClient("https://taskmanagement-f8aab-default-rtdb.firebaseio.com/");
        }



        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">The user's company password.</param>
        /// <param name="company">The user's company name.</param>
        public async Task<bool> RegisterUser(string username, string password, string company, 
                                             string name, string surname, string phoneNumber, string email)
        {
            var result = await _firebase
                .Child("UserTable")
                .PostAsync(new UserTable() 
                { 
                    UserID = Guid.NewGuid(), 
                    Username = username, 
                    Password = password,
                    Company = company,
                    Name = name,
                    Surname = surname,
                    PhoneNumber = phoneNumber,
                    Email = email
                });

            return result.Object != null;
        }



        /// <summary>
        /// Registers a new company.
        /// </summary>
        /// <param name="companyName">The company name.</param>
        /// <param name="companyPassword">The company password.</param>
        public async Task<bool> RegisterCompany(string companyEmail, string companyName, string companyPassword)
        {
            var result = await _firebase
                .Child("CompanyTable")
                .PostAsync(new CompanyTable() 
                { 
                    CompanyID = Guid.NewGuid(), 
                    CompanyEmail = companyEmail,
                    CompanyName = companyName, 
                    CompanyPassword = companyPassword
                });

            return result.Object != null;
        }

        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">The user's company password.</param>
        /// <param name="company">The user's company id.</param>
        public async Task<UserTable> LoginUser(string username, string password, string company)
        {
            // Get the user.
            var user = (await _firebase
                .Child("UserTable")
                .OnceAsync<UserTable>())
                .Where(v => v.Object.Company == company)
                .Where(v => v.Object.Username == username)
                .Where(v => v.Object.Password == password).FirstOrDefault();

            if (user != null)
            {
                //return user;
                return user.Object;
            }
            return null;
        }


        /// <summary>
        /// Logins a company
        /// </summary>
        /// <param name="companyName">The company name.</param>
        /// <param name="companyPassword">The company password.</param>
        public async Task<CompanyTable> LoginCompany(string companyName, string companyPassword)
        {
            // Get a company.
            var company = (await _firebase
                .Child("CompanyTable")
                .OnceAsync<CompanyTable>())
                .Where(v => v.Object.CompanyName == companyName)
                .Where(v => v.Object.CompanyPassword == companyPassword).FirstOrDefault();

            if (company != null)
            {
                return company.Object;
            }
            return null;
        }


        /// <summary>
        /// Gets all exist users.
        /// </summary>
        public async Task<List<UserTable>> GetUsers(string company)
        {
            var users = new List<UserTable>();
            (await _firebase.Child("UserTable")
                .OnceAsync<UserTable>())
                .ToList()
                .Where(v => v.Object.Company == company)
                .ToList()
                .ForEach(v => users.Add(v.Object));

            return users;
        }


        /// <summary>
        /// Gets all exist companies.
        /// </summary>
        public async Task<List<CompanyTable>> GetCompanies()
        {
            var companies = new List<CompanyTable>();
            (await _firebase.Child("CompanyTable").OnceAsync<CompanyTable>()).ToList().ForEach(v => companies.Add(v.Object));
            
            return companies;
        }


        /// <summary>
        /// Gets employee tasks.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<List<Tasks>> GetEmployeeTasks(UserTable user)
        {
            var tasks = await _firebase.Child("TaskTable").OnceAsync<Tasks>();

            var userTasks = new List<Tasks>();
            foreach (var task in tasks)
            {
                if (task.Object.AssignedEmployees != null)
                {
                    if (task.Object.AssignedEmployees.Any(employee => employee.UserID == user.UserID))
                    {
                        userTasks.Add(new Tasks()
                        {
                            TaskID = task.Object.TaskID,
                            TaskName = task.Object.TaskName,
                            Description = task.Object.Description,
                            Employer = task.Object.Employer,
                            Completed = task.Object.Completed,
                            Notes = task.Object.Notes,
                            Deadline = task.Object.Deadline,
                            AssignedEmployees = task.Object.AssignedEmployees,
                            CompletedTaskEmployees = task.Object.CompletedTaskEmployees != null ?
                                           task.Object.CompletedTaskEmployees :
                                           new ObservableCollection<UserTable>()
                        });
                    }
                }
            }

            if (userTasks != null)
            {
                return userTasks;
            }
            return null;
        }

        /// <summary>
        /// Gets issued tasks.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<List<Tasks>> GetIssuedTasks(UserTable user)
        {
            var tasks = (await _firebase
              .Child("TaskTable")
              .OnceAsync<Tasks>())
              .Where(v => v.Object.Employer.UserID == user.UserID)
              .Select(item => new Tasks()
              {
                  TaskID = item.Object.TaskID,
                  TaskName = item.Object.TaskName,
                  Description = item.Object.Description,
                  Employer = item.Object.Employer,
                  Completed = item.Object.Completed,
                  Notes = item.Object.Notes,
                  Deadline = item.Object.Deadline,
                  AssignedEmployees = item.Object.AssignedEmployees != null ?
                                      item.Object.AssignedEmployees :
                                      new ObservableCollection<UserTable>(),
                  CompletedTaskEmployees = item.Object.CompletedTaskEmployees != null ? 
                                           item.Object.CompletedTaskEmployees : 
                                           new ObservableCollection<UserTable>()
              }).ToList();

            if (tasks != null)
            {
                return new List<Tasks>(tasks);
            }
            return null;
        }



        /// <summary>
        /// Assigns a new task to an employee.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task<bool> AssignTaskToEmployees(Tasks task)
        {
            var result = await _firebase.Child("TaskTable").PostAsync(task);

            return result.Object != null;
        }


        public async Task<bool> UpdateTaskNotes(Tasks task)
        {
            var UpdateTaskNotesDb = (await _firebase
             .Child("TaskTable")
             .OnceAsync<Tasks>())
             .Where(v => v.Object.TaskID == task.TaskID)
             .FirstOrDefault();

            await _firebase.Child("TaskTable").Child(UpdateTaskNotesDb.Key).PutAsync(task);

            if (UpdateTaskNotesDb.Object != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAssignedEmployees(Tasks task)
        {
            var UpdateTaskNotesDb = (await _firebase
             .Child("TaskTable")
             .OnceAsync<Tasks>())
             .Where(v => v.Object.TaskID == task.TaskID)
             .FirstOrDefault();

            await _firebase.Child("TaskTable").Child(UpdateTaskNotesDb.Key).PutAsync(task);

            if (UpdateTaskNotesDb.Object != null)
            {
                return true;
            }
            return false;
        }


        public async Task<bool> DeleteTask(Tasks task)
        {
            var DeleteUserDb = (await _firebase
             .Child("TaskTable")
             .OnceAsync<Tasks>()).Where(a => a.Object.TaskID == task.TaskID).FirstOrDefault();

            await _firebase.Child("TaskTable").Child(DeleteUserDb.Key).DeleteAsync();

            if (DeleteUserDb.Object != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddNews(News news)
        {
            var result = await _firebase.Child("NewsTable").PostAsync(news);

            return result.Object != null;
        }

        public async Task<List<News>> GetCompanyNews(CompanyTable company)
        {
            var news = (await _firebase
              .Child("NewsTable")
              .OnceAsync<News>())
              .Where(v => v.Object.CompanyID == company.CompanyID)
              .Select(item => new News()
              {
                  NewsID = item.Object.NewsID,
                  CompanyID = item.Object.CompanyID,
                  NewsText = item.Object.NewsText,
                  PublishDay = item.Object.PublishDay,
                  RemovalDay = item.Object.RemovalDay,
                  PublisherName = item.Object.PublisherName
              }).ToList();



            if (news != null)
            {
                for (int i = 0; i < news.Count; i++)
                {
                    if (news[i].RemovalDay < DateTime.Today)
                    {
                        var DeleteNewsDb = (await _firebase
                            .Child("NewsTable")
                            .OnceAsync<News>()).Where(v => v.Object.NewsID == news[i].NewsID).FirstOrDefault();

                        await _firebase.Child("NewsTable").Child(DeleteNewsDb.Key).DeleteAsync();

                        news.RemoveAt(i);
                        i--;

                    }
                }

                return new List<News>(news);
            }
            return null;
        }

    }
}
