using Application.Dtos.AdvertDtos;
using FluentValidation;

namespace Application.Validators.Adverts
{
    public class UpdateAdvertValidator : AbstractValidator<UpdateAdvertDto>
    {
        public UpdateAdvertValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Advert title can't be empty.")
                .Length(3, 100)
                .WithMessage("Advert title must have at least 3 characters and max 100 characters.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Advert description can't be empty.")
                .Length(3, 4096)
                .WithMessage("Advert description must have at least 3 characters and max 4096 characters.");

            RuleFor(x => x.Price)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Price can't be empty.");

            RuleFor(x => x.CategoryId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Category can't be empty.");
        }
    }
}