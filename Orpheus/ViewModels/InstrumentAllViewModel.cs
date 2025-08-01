namespace Orpheus.ViewModels
{
    public class InstrumentAllViewModel
    {
        public IEnumerable<ItemViewModel> Instruments { get; set; } = new List<ItemViewModel>();
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public string? PriceRange { get; set; }
    }
}
