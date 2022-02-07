using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Tables;

namespace TaskManager.Services
{
    class DocumentDBService
    {
        // A service client instance.
        private static DocumentDBService _serviceClientInstance;

        private DocumentClient _client;

        private Uri _usersCollectionLink = UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.UsersCollectionId);

        private Uri _companiesCollectionLink = UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.CompaniesCollectionId);

        private Uri _tasksCollectionLink = UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.TasksCollectionId);

        private Uri _newsCollectionLink = UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.NewsCollectionId);



        /// <summary>
        /// A service client instance.
        /// </summary>
        public static DocumentDBService ServiceClientInstance
        {
            get
            {
                if (_serviceClientInstance == null)
                {
                    _serviceClientInstance = new DocumentDBService();
                }
                return _serviceClientInstance;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DocumentDBService()
        {
            _client = new DocumentClient(new System.Uri(Constants.AccountURL), Constants.AccountKey);
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
            try
            {
                var user = new UserTable()
                {
                    UserID = Guid.NewGuid(),
                    Username = username,
                    Password = password,
                    Company = company,
                    Name = name,
                    Surname = surname,
                    PhoneNumber = phoneNumber,
                    Email = email
                };

                var result = await _client.CreateDocumentAsync(_usersCollectionLink, user);
                return true;

            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// Registers a new company.
        /// </summary>
        /// <param name="companyName">The company name.</param>
        /// <param name="companyPassword">The company password.</param>
        public async Task<bool> RegisterCompany(string companyEmail, string companyName, string companyPassword)
        {
            try
            {
                var company = new CompanyTable()
                {
                    CompanyID = Guid.NewGuid(),
                    CompanyEmail = companyEmail,
                    CompanyName = companyName,
                    CompanyPassword = companyPassword
                };

                var result = await _client.CreateDocumentAsync(_companiesCollectionLink, company);
                return true;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">The user's company password.</param>
        /// <param name="company">The user's company id.</param>
        public UserTable LoginUser(string username, string password, string company)
        {
            try
            {
                var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 };
                var user = _client.CreateDocumentQuery<UserTable>(_usersCollectionLink, options)
                    .Where(v => v.Company == company)
                    .Where(v => v.Username == username)
                    .Where(v => v.Password == password)
                    .AsEnumerable()
                    .FirstOrDefault();

                return user;
            }
            catch
            {
                return null;
            }

        }


        /// <summary>
        /// Logins a company
        /// </summary>
        /// <param name="companyName">The company name.</param>
        /// <param name="companyPassword">The company password.</param>
        public CompanyTable LoginCompany(string companyName, string companyPassword)
        {
            try
            {
                var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 };
                var company = _client.CreateDocumentQuery<CompanyTable>(_companiesCollectionLink, options)
                    .Where(v => v.CompanyName == companyName)
                    .Where(v => v.CompanyPassword == companyPassword)
                    .AsEnumerable()
                    .FirstOrDefault();

                return company;
            }
            catch
            {
                return null;
            }
        }


        public async Task UpdateUser(UserTable user)
        {
            try
            {
                if (IdentityService.ProfileType == ProfileType.Employee)
                {
                    await _client.ReplaceDocumentAsync(UriFactory.
                        CreateDocumentUri(Constants.DatabaseId, Constants.UsersCollectionId, IdentityService.User.UserID.ToString()), user);
                    IdentityService.User = user;
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(@"ERROR {0}", ex.Message);
            }
        }

        public async Task UpdateCompany(CompanyTable company)
        {
            try
            {
                if (IdentityService.ProfileType == ProfileType.Company)
                {
                    await _client.ReplaceDocumentAsync(UriFactory.
                        CreateDocumentUri(Constants.DatabaseId, Constants.CompaniesCollectionId, IdentityService.Company.CompanyID.ToString()), company);
                    IdentityService.Company = company;
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(@"ERROR {0}", ex.Message);
            }
        }


        /// <summary>
        /// Gets all exist users.
        /// </summary>
        public List<UserTable> GetUsers(string company)
        {
            try
            {
                var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 };
                var users = _client.CreateDocumentQuery<UserTable>(_usersCollectionLink, options)
                    .Where(v => v.Company == company)
                    .AsEnumerable()
                    .ToList();

                return users;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gets all exist companies.
        /// </summary>
        public List<CompanyTable> GetCompanies()
        {
            try
            {
                var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 };
                var companies = _client.CreateDocumentQuery<CompanyTable>(_companiesCollectionLink, options).AsEnumerable().ToList();

                return companies;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gets employee tasks.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Employee tasks.</returns>
        public async Task<List<Tasks>> GetEmployeeTasks(UserTable user)
        {
            try
            {
                var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 };
                var tasks = _client.CreateDocumentQuery<Tasks>(_tasksCollectionLink, options).AsDocumentQuery();

                var tasksList = new List<Tasks>();
                while (tasks.HasMoreResults)
                {
                    foreach (var task in await tasks.ExecuteNextAsync<Tasks>())
                    {
                        if (task.AssignedEmployees != null)
                        {
                            if (task.AssignedEmployees.Any(employee => employee.UserID == user.UserID))
                            {
                                if (task.CompletedTaskEmployees == null)
                                {
                                    task.CompletedTaskEmployees = new ObservableCollection<UserTable>();
                                }
                                tasksList.Add(task);
                            }
                        }
                    }
                }

                return tasksList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets issued tasks.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<List<Tasks>> GetIssuedTasks(UserTable user)
        {
            try
            {
                var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 };
                var tasks = _client.CreateDocumentQuery<Tasks>(_tasksCollectionLink, options).AsDocumentQuery();

                var tasksList = new List<Tasks>();
                while (tasks.HasMoreResults)
                {
                    foreach (var task in await tasks.ExecuteNextAsync<Tasks>())
                    {
                        if (task.Employer.UserID == user.UserID)
                        {
                            if (task.CompletedTaskEmployees == null)
                            {
                                task.CompletedTaskEmployees = new ObservableCollection<UserTable>();
                            }
                            if (task.AssignedEmployees == null)
                            {
                                task.AssignedEmployees = new ObservableCollection<UserTable>();
                            }
                            tasksList.Add(task);
                        }
                    }
                }

                return tasksList;
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// Assigns a new task to an employee.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task<bool> AssignTaskToEmployees(Tasks task)
        {
            try
            {
                var result = await _client.CreateDocumentAsync(_tasksCollectionLink, task);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task UpdateTask(Tasks task)
        {
            try
            {
                await _client.ReplaceDocumentAsync(UriFactory.
                    CreateDocumentUri(Constants.DatabaseId, Constants.TasksCollectionId, task.TaskID.ToString()), task);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }

        
        public async Task DeleteTask(Tasks task)
        {
            try
            {
                await _client.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(Constants.DatabaseId, Constants.TasksCollectionId, task.TaskID.ToString()),
                    new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });
            }
            catch { }
        }

        public async Task<bool> AddNews(News news)
        {
            try
            {
                var result = await _client.CreateDocumentAsync(_newsCollectionLink, news);
                return true;

            }
            catch
            {
                return false;
            }
        }


        public async Task<List<News>> GetCompanyNews(CompanyTable company)
        {
            try
            {
                var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = -1 };
                var news = _client.CreateDocumentQuery<News>(_newsCollectionLink, options)
                    .Where(v => v.CompanyID == company.CompanyID)
                    .AsEnumerable()
                    .ToList();

                if (news != null)
                {
                    for (int i = 0; i < news.Count; i++)
                    {
                        if (news[i].RemovalDay < DateTime.Today)
                        {
                            await _client.DeleteDocumentAsync(
                                UriFactory.CreateDocumentUri(Constants.DatabaseId, Constants.NewsCollectionId, news[i].NewsID.ToString()),
                                new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });

                            news.RemoveAt(i);
                            i--;
                        }
                    }
                }

                return news;
            }
            catch
            {
                return null;
            }
        }

    }
}
