namespace RaspredeleniyeDutyaApp.Models.Server
{
    public record RegisterDataModel(
    string Email,
    string LastName,
    string FirstName,
    string MiddleName,
    string Password,
    string ConfirmPassword);
}
