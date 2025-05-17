using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace RunGroopWebApp.ViewModels
{
    public class EditClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public string ImageUrl { get; set; } // 🔥 Вот это добавляем!
        public ClubCategory ClubCategory { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}