namespace Orpheus.ViewModels
{
    public class InstrumentItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string BrandName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}