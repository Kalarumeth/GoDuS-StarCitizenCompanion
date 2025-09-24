using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StarCitizenCompanion.Repository
{
    public class NotifyFormatter
    {
        public string ActorDeath(string log)
        {
            var regex = new Regex(
            @"CActor::Kill:\s'([^']+)'.*?zone\s'([^']+)'\s+killed by\s'([^']+)'.*?damage type\s'([^']+)'",
            RegexOptions.Compiled);

            var match = regex.Match(log);
            if (match.Success)
            {
                string killedActor = match.Groups[1].Value;   // dopo CActor::Kill:
                string zone = match.Groups[2].Value;          // dopo zone
                string killer = match.Groups[3].Value;        // dopo killed by
                string damageType = match.Groups[4].Value;    // dopo damage type

                Console.WriteLine($"Killed Actor: {killedActor}");
                Console.WriteLine($"Zone: {zone}");
                Console.WriteLine($"Killed By: {killer}");
                Console.WriteLine($"Damage Type: {damageType}");

                return $"[{zone}]\r\n{killer} ☠️ {killedActor}\r\n{damageType}";
            }
            return "";
        }
    }
}
