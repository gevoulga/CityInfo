using System.ComponentModel.DataAnnotations;

namespace CarvedRock.Web.Clients
{
    public class CarvedRockApiOptions
    {
        public const string Section = "CarvedRockAPI";

        [Required]
        public Uri CarvedRockApiUri { get; set; }
    }
}