namespace Orpheus.ViewModels
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string BrandName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public List<string> Images { get; set; } = new();
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
    }
}