using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels
{
    public class HomeUserCreateViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int? ZipCode { get; set; }
    }
}
