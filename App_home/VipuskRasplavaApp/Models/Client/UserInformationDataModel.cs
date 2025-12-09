namespace RaspredeleniyeDutyaApp.Models.Client
{
    public record UserInformationDataModel(int? UserId,
                                           string? UserLastName,
                                           string? UserFirstName,
                                           string? UserMiddleName,
                                           string? UserEmail,
                                           bool? UserIsAdmin);
}
