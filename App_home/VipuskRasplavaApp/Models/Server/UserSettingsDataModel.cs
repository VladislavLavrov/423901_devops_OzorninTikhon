namespace RaspredeleniyeDutyaApp.Models.Server
{
    public record UserSettingsDataModel(
        int Id,
        string LastName,
        string FirstName,
        string? MiddleName,
        string Email);
}
