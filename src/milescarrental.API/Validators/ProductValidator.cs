using System;
using FluentValidation;
using milescarrental.Application.Models;

namespace milescarrental.API.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(m => m.Code).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(x => x.PriceList).Must(x => x.Count >= 1)
                .WithMessage("There must be at least one price!")
                .When(x => x.PriceList != null);
            RuleForEach(x => x.PriceList)
                .SetValidator(new PriceValidator())
                .When(x => x.PriceList != null);
        }
    }
}
