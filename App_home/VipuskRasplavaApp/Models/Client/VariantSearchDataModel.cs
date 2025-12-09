namespace RaspredeleniyeDutyaApp.Models.Client
{
    public record VariantSearchDataModel(int? VariantId,
                                         string? VariantName,
                                         int? UserId,
                                         string? UserLastName,
                                         string? UserFirstName,
                                         string? UserMiddleName,
                                         string? UserEmail);
}
