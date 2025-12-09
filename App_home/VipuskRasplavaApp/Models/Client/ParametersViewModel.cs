namespace RaspredeleniyeDutyaApp.Models.Client
{
    public class ParametersViewModel
    {
        public int VariantId { get; set; }
        public string? VariantName { get; set; }
        public int NRabFurm { get; set; }
        public bool ShowResults { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
