using RunGroopWebApp.Models;

namespace RunGroopWebApp.ViewModels;

public class UserDetailViewModel
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int? Pace { get; set; }
    public int? Mileage { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;

    public string Location => string.Join(", ", new[] { City, State }.Where(s => !string.IsNullOrEmpty(s)));
}
