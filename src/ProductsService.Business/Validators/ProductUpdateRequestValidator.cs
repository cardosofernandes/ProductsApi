namespace ProductsService.Business.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(MaxLength.C64);

        RuleFor(x => x.Description)
            .MaximumLength(MaxLength.C256);
        
        RuleFor(x => x.Price)
            .GreaterThan(0m)
            .WithMessage("Price must be greater than zero.")
            .LessThanOrEqualTo(9999999999999999.99m)
            .PrecisionScale(18, 2, ignoreTrailingZeros: true);
        
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than zero.");
    }
}