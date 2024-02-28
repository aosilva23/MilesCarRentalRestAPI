using System;
using FluentValidation;
using milescarrental.Application.Models;

namespace milescarrental.API.Validators
{
    public class PriceValidator : AbstractValidator<Price>
    {
        public PriceValidator()
        {
            RuleFor(m => m.Type).NotEmpty().WithMessage("{PropertyName} should be not empty. NEVER!");
            RuleFor(m => m.Value).NotEmpty();
        }
    }
}
