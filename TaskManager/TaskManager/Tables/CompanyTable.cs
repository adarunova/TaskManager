using System;

namespace TaskManager.Tables
{
    public class CompanyTable
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public Guid CompanyID { get; set; }

        public string CompanyEmail { get; set; }

        public string CompanyName { get; set; }

        public string CompanyPassword { get; set; }
    }
}
