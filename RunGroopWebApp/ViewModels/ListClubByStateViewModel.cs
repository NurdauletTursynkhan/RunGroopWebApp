using RunGroopWebApp.Models;

namespace RunGroopWebApp.ViewModels
{
    public class ListClubByStateViewModel
    {
        public IEnumerable<Club> Clubs { get; set; } = new List<Club>();
        public bool NoClubWarning { get; set; } = false;
        public string State { get; set; } = string.Empty;
    }
}
