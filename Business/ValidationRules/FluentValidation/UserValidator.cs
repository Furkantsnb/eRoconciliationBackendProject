using Core.Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class UserValidator :AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Kullanıcı Adı Boş Olamaz");
            RuleFor(p => p.Name).MinimumLength(4).WithMessage("Kullanıcı Adı En Az 4 Karakter Olmalıdır");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Mail Boş Olamaz");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Geçerli Bir Mail Adresi Yazınız");


        }
    }
}
