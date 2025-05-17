using Microsoft.AspNetCore.Http;

namespace RunGroopWebApp.ViewModels
{
    public class EditUserProfileViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public IFormFile? Image { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
