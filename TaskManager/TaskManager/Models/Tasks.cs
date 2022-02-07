using System;
using System.Collections.ObjectModel;
using TaskManager.Tables;

namespace TaskManager.Models
{
    public class Tasks : IComparable
    {
        private readonly string[] Colors = new string[] { "#FF623C", "#FF4E3D", "#F53100", "#2B4D66" };


        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public Guid TaskID { get; set; }


        public string TaskName { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public string Notes { get; set; }

        public bool Completed { get; set; }

        public UserTable Employer { get; set; }

        public string Color
        {
            get
            {
                if (Completed)
                {
                    return "#5CA9FF";
                }
                if ((Deadline - DateTime.Now).Days > 10)
                {
                    return Colors[0];
                }
                else if ((Deadline - DateTime.Now).Days > 4)
                {
                    return Colors[1];
                }
                else if ((Deadline - DateTime.Now).Days > -1)
                {
                    return Colors[2];
                }
                else
                {
                    return Colors[3];
                }
            }
        }


        public ObservableCollection<UserTable> AssignedEmployees { get; set; }

        public ObservableCollection<UserTable> CompletedTaskEmployees { get; set; }


        public string DaysLeft => (Deadline - DateTime.Now).Days >= 0 ? (Deadline - DateTime.Now).Days + " days left" : "The deadline has passed";

        public string TaskFrom => Employer.Name + " " + Employer.Surname;

        public string State => Completed ? "COMPLETED" : "UNFINISHED";


        public int CompareTo(object obj)
        {
            Tasks tasks = (Tasks) obj;
            if (Deadline > tasks.Deadline)
            {
                return 1;
            }
            else if (Deadline < tasks.Deadline)
            {
                return -1;
            }
            return 0;
        }
    }
}
