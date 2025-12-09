namespace RaspredeleniyeDutyaApp.Models.Client
{
    public record UserSettingsDataModel(UserAccountDataModel User,
                                        bool Success,
                                        string Message);
}
