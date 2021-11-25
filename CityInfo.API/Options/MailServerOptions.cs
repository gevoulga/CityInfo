using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Options
{
    public class MailServerOptions
    {
        public const string Section = "MailServer";
        public bool Enabled { get; set; }
        public MailSettings MailSettings { get; set; }
    }

    public class MailSettings
    {
        [Required]
        public string MailTo { get; set; } = string.Empty;
        [Required] public string MailFrom { get; set; }
    }
}