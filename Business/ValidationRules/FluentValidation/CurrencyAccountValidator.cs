using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CurrencyAccountValidator :AbstractValidator<CurrencyAccount>
    {
        public CurrencyAccountValidator() {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Firma Adı Boş Olamaz");
            RuleFor(p => p.Name).MinimumLength(4).WithMessage("Firma Adı En Az 4 Karakter Olmalıdır");

            RuleFor(p => p.Address).NotEmpty().WithMessage("Firma Adresi Boş Olamaz");
            RuleFor(p => p.Address).MinimumLength(4).WithMessage("Firma Adresi En Az 4 Karakter Olmalıdır");

        }
    }
}
