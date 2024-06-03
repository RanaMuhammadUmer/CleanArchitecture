using CleanArchitecture.Application.Dto;
using FluentValidation;

namespace CleanArchitecture.Application.Validators
{
    public class CreateEmployeeRequestParametersValidator:AbstractValidator<CreateEmployeeRequestParameters>
    {
        public CreateEmployeeRequestParametersValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();

        }
    }
}
