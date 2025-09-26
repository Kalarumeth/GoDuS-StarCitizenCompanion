using StarCitizenCompanion.Repository;

namespace StarCitizenCompanion.Models
{
    public class NotificationEvent
    {
        public int Id { get; set; }
        public Tag Tag { get; set; }
        public string MessageComposer { get; set; }
        public string MessageNotify { get; set; }
        public Log Log { get; set; }
        public Message Message { get; set; }
    }

    public class Log
    {
        public int Id { get; set; }
        public string RawMessage { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class Message
    {
        public int Id { get; set; }
        public string Victim { get; set;}
        public string Zone { get; set;}
        public string Killer { get; set;}
        public string DamageType { get; set; }
    }
}
