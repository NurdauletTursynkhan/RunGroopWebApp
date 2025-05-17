using Microsoft.AspNetCore.Http;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.ViewModels
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } // Сделали `nullable`
        public string? URL { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; } = new Address(); // Инициализируем Address
        public RaceCategory RaceCategory { get; set; }
    }
}
