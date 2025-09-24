using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenCompanion.Models
{
    public class NotificationEvent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Message { get; set; }
    }
}
