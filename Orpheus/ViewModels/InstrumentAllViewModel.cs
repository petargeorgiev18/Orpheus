namespace Orpheus.ViewModels
{
    public class InstrumentAllViewModel
    {
        public IEnumerable<InstrumentItemViewModel> Instruments { get; set; } = new List<InstrumentItemViewModel>();
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public string? PriceRange { get; set; }
    }
}
