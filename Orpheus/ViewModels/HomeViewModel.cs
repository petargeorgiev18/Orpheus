namespace Orpheus.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<InstrumentViewModel> FeaturedInstruments { get; set; } = new List<InstrumentViewModel>();
        public IEnumerable<InstrumentViewModel> FeaturedAlbums { get; set; } = new List<InstrumentViewModel>();
    }
}