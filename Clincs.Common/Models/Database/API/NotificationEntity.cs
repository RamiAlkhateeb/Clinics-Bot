using System;

namespace Clincs.Common.Models.Database.API
{
    public class NotificationEntity
    {
        public int Id { get; set; }

        private string _email;
        private DateTime _receivedOn = DateTime.UtcNow;
        
        public string ShortDescription { get; set; }
        public string RequestId { get; set; }
        public string UserId { get; set; }

        public string Email
        {
            get => _email;
            set => _email = value.ToLower();
        }
        public string RequestSummary { get; set; }
        public bool? IsSent { get; set; }

        public string Body { get; set; }
        public string Url { get; set; }
        public string Attachment { get; set; }
        public string RequestNumber { get; set; }
        public string InstanceId { get; set; }
        public string ApprovalType { get; set; }
        public string Subject { get; set; }
        public DateTime ReceivedOn
        {
            get => _receivedOn;
            set => _receivedOn = value;
        }
        public DateTime? SentOn { get; set; }
        public string NotificationType { get; set; }
        public string Status { get; set; }
        public string AdditionalData { get; set; }
    }
}
