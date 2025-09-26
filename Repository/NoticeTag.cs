using System.Text.RegularExpressions;

namespace StarCitizenCompanion.Repository
{
    public enum Tag
    {
        None = 0,
        ActorDeath = 1,
    }

    public static class NoticeTag
    {
        public static Tag GetTag(string stag)
        {
            if (string.IsNullOrWhiteSpace(stag))
                return Tag.None;

            var match = Regex.Match(stag, @"\[Notice\]\s*<(?<event>[^>]+)>", RegexOptions.Compiled);
            if (!match.Success)
                return Tag.None;

            return _map.TryGetValue(match.Groups["event"].Value, out var tag)
                ? tag
                : Tag.None;
        }

        private static readonly Dictionary<string, Tag> _map = new()
        {
            { "Actor Death", Tag.ActorDeath },
        };

        public static string GetTagRegex(Tag stag)
        {
            return stag switch
            {
                Tag.ActorDeath => @"CActor::Kill:\s'([^']+)'.*?zone\s'([^']+)'\s+killed by\s'([^']+)'.*?damage type\s'([^']+)'",

            };
        }
    }
}