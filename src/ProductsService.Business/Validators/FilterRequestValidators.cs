namespace ProductsService.Business.Validators;

public class ProductSearchRequestValidator : AbstractValidator<ProductSearchRequest>
{
    public ProductSearchRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(MaxLength.C64);
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageNumber)
            .Must((request, number) => ((long)number - 1) * request.PageSize <= int.MaxValue)
            .WithMessage("Page offset exceeds the supported range.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, PagedRequest.MaxPageSize);
    }
}

public class StockRangeRequestValidator : AbstractValidator<StockRangeRequest>
{
    public StockRangeRequestValidator()
    {
        RuleFor(x => x.MinStock).NotNull().GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxStock).NotNull().GreaterThanOrEqualTo(0);
        RuleFor(x => x).Must(x => x.MinStock <= x.MaxStock).WithMessage("Minimum stock must not exceed maximum stock.");
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageNumber)
            .Must((request, number) => ((long)number - 1) * request.PageSize <= int.MaxValue)
            .WithMessage("Page offset exceeds the supported range.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, PagedRequest.MaxPageSize);
    }
}
