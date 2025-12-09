namespace RaspredeleniyeDutyaApp.Models.Client
{
    public record VariantDataModel(int Id,
                                   string Name,
                                   int OwnerId,
                                   UserAccountDataModel Owner);
}
