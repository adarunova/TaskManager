using System;

namespace TaskManager.Models
{
    public class News
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public Guid NewsID { get; set; }

        public Guid CompanyID { get; set; }

        public string PublisherName { get; set; }

        public string NewsText { get; set; }

        public DateTime PublishDay { get; set; }

        public DateTime RemovalDay { get; set; }

        public string PublicationInformation => $"{PublisherName} {String.Format("{0:dd.MM}", PublishDay.Date)}-{String.Format("{0:dd.MM}", RemovalDay.Date)}";

    }
}
