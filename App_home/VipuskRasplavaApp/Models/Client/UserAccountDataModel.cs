namespace RaspredeleniyeDutyaApp.Models.Client
{
    public record UserAccountDataModel(int Id,
                                       string Email,
                                       string LastName,
                                       string FirstName,
                                       string MiddleName,
                                       bool IsAdmin,
                                       string PasswordHash,
                                       List<VariantDataModel> Variants)
    {
        public string GetFullName()
        {
            if (MiddleName.Length > 0)
                return $"{LastName} {FirstName} {MiddleName}";
            else
                return $"{LastName} {FirstName}";
        }
    }
}
