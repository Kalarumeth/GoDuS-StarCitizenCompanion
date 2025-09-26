using StarCitizenCompanion.Data;
using StarCitizenCompanion.Models;
using System.Text.RegularExpressions;


namespace StarCitizenCompanion.Repository
{
    public static class NotifyFormatter
    {
        public static event Action<NotificationEvent> OnNotificationSaved;

        public static NotificationEvent Notify(string log)
        {
            var _ne = new NotificationEvent()
            {
                Tag = NoticeTag.GetTag(log),
                Log = new Log()
                {
                    RawMessage = log,
                    Date = DateTime.Now
                }
            };

            if (_ne.Tag == Tag.None)
                return _ne;

            var regex = new Regex(
                NoticeTag.GetTagRegex(_ne.Tag),
                RegexOptions.Compiled
            );

            var match = regex.Match(_ne.Log.RawMessage);
            if (match.Success)
            {
                _ne.Message = new Message()
                {
                    Victim = NPCDetect(match.Groups[1].Value),
                    Zone = match.Groups[2].Value,
                    Killer = NPCDetect(match.Groups[3].Value),
                    DamageType = match.Groups[4].Value,
                };

                _ne.MessageComposer = MessageComposer(_ne);
                _ne.MessageNotify = MessageNotify(_ne);

                SaveAndNotify(_ne);

                return _ne;
            }
            return _ne;
        }

        private static string NPCDetect(string value)
        {
            if (value.Contains("NPC"))
            {
                var match = Regex.Match(value, @"^(?<prefix>PU)_(?<faction>\w+)_(?<role>\w+)_(?<category>\w+)_(?<npcType>\w+)_(?<zone>\w+)_(?<subzone>\w+)_(?<id>\d+)$");
                value = $"[{match.Groups["npcType"].Value}] {match.Groups["zone"].Value} {match.Groups["subzone"].Value}";
            }
            return value;
        }

        private static string MessageComposer(NotificationEvent _ne)
        {
            return _ne.Tag switch
            {
                Tag.ActorDeath => $"[{_ne.Message.Zone}]\r\n{_ne.Message.Killer} ☠️ {_ne.Message.Victim}\r\n{_ne.Message.DamageType}"
            };
        }

        private static string MessageNotify(NotificationEvent _ne)
        {
            return _ne.Tag switch
            {
                Tag.ActorDeath => $"{_ne.Message.Killer} ☠️ {_ne.Message.Victim}"
            };
        }

        private static void SaveAndNotify(NotificationEvent _ne)
        {

            using (var db = new Context())
            {
                db.Notifications.Add(_ne);
                db.SaveChanges();
            }

            OnNotificationSaved?.Invoke(_ne);
        }
    }
}
