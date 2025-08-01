namespace Orpheus.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ItemViewModel> FeaturedInstruments { get; set; } = new List<ItemViewModel>();
        public IEnumerable<ItemViewModel> FeaturedAlbums { get; set; } = new List<ItemViewModel>();
    }
}