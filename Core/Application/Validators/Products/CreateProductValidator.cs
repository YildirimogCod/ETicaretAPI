using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using FluentValidation;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator:AbstractValidator<CreateProduct>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull()
                .WithMessage("Product name cannot be empty")
                .MinimumLength(2).WithMessage("Product name must be at least 2 characters long")
                .MaximumLength(100).WithMessage("Product name must be at most 100 characters long");
            RuleFor(x => x.Price).NotEmpty().NotNull()
                .WithMessage("Product price cannot be empty")
                .Must(x => x >= 0).WithMessage("Product price must be greater than 0");
            RuleFor(x => x.Stock)
                .Must(x => x >= 0).WithMessage("Product stock must be greater than 0");
        }
    }
}
