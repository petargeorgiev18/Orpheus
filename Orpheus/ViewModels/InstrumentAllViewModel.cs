namespace Orpheus.ViewModels
{
    public class InstrumentAllViewModel
    {
        public IEnumerable<InstrumentViewModel> Instruments { get; set; } = new List<InstrumentViewModel>();
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public string? PriceRange { get; set; }
    }
}
