namespace RunGroopWebApp.ViewModels;

public class UserViewModel
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int? Pace { get; set; }
    public int? Mileage { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;

   
    public string Location => (City, State) switch
    {
        (not null and var city, not null and var state) => $"{city}, {state}",
        (not null and var city, null) => city,
        (null, not null and var state) => state,
        _ => "",
    };

}