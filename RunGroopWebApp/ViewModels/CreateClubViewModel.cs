using Microsoft.AspNetCore.Http;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.ViewModels
{
    public class CreateClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
        public IFormFile? Image { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public string AppUserId { get; set; } = string.Empty;
    }
}
