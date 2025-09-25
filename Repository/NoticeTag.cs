using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenCompanion.Repository
{
    public enum Tag
    {
        None = 0,
        ActorDeath = 1,
    }

    public static class NoticeTag
    {
        public static string GetTag(Tag stag)
        {
            return stag switch
            {
                Tag.ActorDeath => "<Actor Death>",

            };
        }

        public static string GetTagRegex(Tag stag)
        {
            return stag switch
            {
                Tag.ActorDeath => "<Actor Death>",

            };
        }
    }
}