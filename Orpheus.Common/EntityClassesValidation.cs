namespace Orpheus.Common
{
    public static class EntityClassesValidation
    {
        public static class Brand
        {
            public const int BrandNameMaxLength = 100;
            public const int BrandNameMinLength = 3;
            public const int BrandDescriptionMaxLength = 500;
            public const int BrandLogoUrlMaxLength = 200;
        }
        public static class Category
        {
            public const int CategoryNameMaxLength = 100;
            public const int CategoryNameMinLength = 3;
        }
        public static class Item
        {
            public const int ItemNameMaxLength = 100;
            public const int ItemDescriptionMaxLength = 500;
            public const double ItemPriceMinValue = 0.01;
            public const double ItemPriceMaxValue = 10000.00;
        }
        public static class ItemImage
        {
            public const int ImageUrlMaxLength = 2048;
        }
        public static class OrpheusAppUser
        {
            public const int UserNameMaxLength = 100;
            public const int UserNameMinLength = 3;
            public const int FullNameMaxLength = 100;
            public const int FullNameMinLength = 10;
            public const int EmailMaxLength = 256;
            public const int PasswordMaxLength = 256;
            public const int PhoneNumberMaxLength = 15;
            public const int PhoneNumberMinLength = 10;
            public const int AddressMaxLength = 80;
            public const int AddressMinLength = 10;
            public const int CityMaxLength = 50;
            public const int CityMinLength = 3;
            public const int PostalCodeMaxLength = 10;
            public const int PostalCodeMinLength = 4;
            public const int CountryMaxLength = 50;
            public const int ExpiryMonthMaxValue = 12;
            public const int ExpiryMonthMinValue = 1;
            public const int ExpiryYearMinValue = 2025;
            public const int ExpiryYearMaxValue = 2100;
        }
        public static class Review
        {
            public const int ReviewCommentMaxLength = 150;
            public const int ReviewRatingMinValue = 1;
            public const int ReviewRatingMaxValue = 5;
        }
        public static class Order
        {
            public const double OrderAmountMinValue = 0.01;
            public const double OrderAmountMaxValue = 10000.00;
        }
        public static class Cart
        {
            public const int CartItemCountMaxValue = 100;
            public const int CartItemCountMinValue = 1;
        }
        public static class CartItem
        {
            public const int CartItemQuantityMinValue = 1;
            public const int CartItemQuantityMaxValue = 100;
            public const double CartItemPriceMinValue = 0.01;
            public const double CartItemPriceMaxValue = 10000.00;
        }
    }
}
