using AutoMapper;
using EBazar.API.Models.Domain;
using EBazar.API.Models.DTOs;

namespace EBazar.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Product, ProductDto>();

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                    src.Items.Sum(item => item.Quantity * item.Product.Price)));

            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Product.Slug))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product.Image))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Product.Discount))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Product.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Product.EndDate))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src =>
                    CalculateSubTotalWithDiscount(src)
                ));
        }

        private decimal CalculateSubTotalWithDiscount(CartItem cartItem)
        {
            var now = DateTime.UtcNow;
            var product = cartItem.Product;

            bool isDiscountActive =
                product.Discount.HasValue &&
                product.Discount.Value > 0 &&
                product.StartDate.HasValue &&
                product.EndDate.HasValue &&
                now >= product.StartDate.Value &&
                now <= product.EndDate.Value;

            decimal effectivePrice = product.Price;

            if (isDiscountActive)
            {
                var discountAmount = product.Price * (product.Discount.Value / 100);
                effectivePrice -= discountAmount;
            }

            return effectivePrice * cartItem.Quantity;
        }

    }
}
