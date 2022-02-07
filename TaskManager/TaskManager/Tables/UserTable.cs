using System;
using TaskManager.Models;

namespace TaskManager.Tables
{
    public class UserTable : IComparable
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public Guid UserID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Company { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Initials => Surname.Length > 0 ? Name.Length > 0 ? Surname[0].ToString() + Name[0].ToString() : Surname[0].ToString() : String.Empty;

        public int CompareTo(object obj)
        {
            UserTable user = (UserTable)obj;

            int compare = string.Compare(Surname, user.Surname, StringComparison.CurrentCulture);
            if (compare == 0)
            {
                compare = string.Compare(Name, user.Name, StringComparison.CurrentCulture);
            }
            return compare;
        }
    }
}
