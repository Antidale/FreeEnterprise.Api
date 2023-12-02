namespace FreeEnterprise.Api.Models
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
    public class Entrant
    {
        public int id { get; set; }
        public string user_name { get; set; } = string.Empty;
        public string user_id { get; set; } = string.Empty;
        public string pronouns { get; set; } = string.Empty;

        public Entrant() { }

        public Entrant(string userId, string userName, string pronouns)
        {
            user_id = userId;
            user_name = userName;
            this.pronouns = pronouns;
        }

        public Entrant(int id, string userId, string userName, string pronouns)
        {
            this.id = id;
            user_id = userId;
            user_name = userName;
            this.pronouns = pronouns;
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
