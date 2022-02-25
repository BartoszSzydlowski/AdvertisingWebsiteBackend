using Application.Dtos.CategoryDtos;
using FluentValidation;

namespace Application.Validators.Categories
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Category name can't be empty.")
                .Length(3, 100)
                .WithMessage("Category title must have at least 3 characters and max 100 characters.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Category description can't be empty.")
                .Length(3, 4096)
                .WithMessage("Category description must have at least 3 characters and max 4096 characters.");
        }
    }
}