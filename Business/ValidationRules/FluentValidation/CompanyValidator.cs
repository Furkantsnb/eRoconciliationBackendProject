using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator() 
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Şirket Adı Boş Olamaz");
            RuleFor(p => p.Name).MinimumLength(4).WithMessage("Şirket Adı En Az 4 Karakter Olmalıdır");
            RuleFor(p => p.Address).NotEmpty().WithMessage("Şirket Adresi Boş Olamaz");

        }
    }
}
