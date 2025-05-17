using RunGroopWebApp.Models;

namespace RunGroopWebApp.ViewModels
{
    public class ListClubByCityViewModel
    {
        public IEnumerable<Club> Clubs { get; set; } = new List<Club>();
        public bool NoClubWarning { get; set; } = false;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
