using Branef.Empresas.Domain.Models;
using FluentValidation;

namespace Branef.Empresas.Domain.Validators
{
    public class InsertOrUpdateCompanyCommandValidator : AbstractValidator<InsertOrUpdateCompanyCommand>
    {
        public InsertOrUpdateCompanyCommandValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        }
    }
}
