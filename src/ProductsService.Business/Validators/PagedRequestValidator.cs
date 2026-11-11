namespace ProductsService.Business.Validators;

public class PagedRequestValidator : AbstractValidator<ProductRequest>
{
    public PagedRequestValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than or equal to 1");
        RuleFor(x => x.PageNumber)
            .Must((request, number) => ((long)number - 1) * request.PageSize <= int.MaxValue)
            .WithMessage("Page offset exceeds the supported range.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, PagedRequest.MaxPageSize).WithMessage($"Page size must be between 1 and {PagedRequest.MaxPageSize}");
    }
}