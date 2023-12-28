using Core.DataAccsess.EntityFramework;
using DataAccsess.Abstract;
using DataAccsess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsess.Concrete.EntityFramework
{
    public class EfBaBsReconciliationDal : EfEntityRepositoryBase<BaBsReconciliation, ContextDb>, IBaBsReconciliationDal
    {
        public List<BaBsReconciliationDto> GetAllDto(int companyId)
        {
            using (var context = new ContextDb())
            {
                var result = from recoinciliation in context.BaBsReconciliations.Where(p => p.CompanyId == companyId)
                             join company in context.Companies on recoinciliation.CompanyId equals company.Id
                             join account in context.CurrencyAccounts on recoinciliation.CurrencuAccountId equals account.Id
                             select new BaBsReconciliationDto
                             {
                                 CompanyId = companyId,
                                 CurrencuAccountId = account.Id,
                                 AccountIdentityNumber = account.IdentiyNumber,
                                 AccountName = account.Name,
                                 AccountTaxDepartment = account.TaxDepatment,
                                 AccountTaxIdNumber = company.TaxIdNumber,
                                 Total = recoinciliation.Total,                            
                                 EmailReadDate = recoinciliation.EmailReadDate,
                                 Guid = recoinciliation.Guid,
                                 Id = recoinciliation.Id,
                                 IsEmailRead = recoinciliation.IsEmailRead,
                                 IsResultSucceed = recoinciliation.IsResultSucceed,
                                 IsSendEmail = recoinciliation.IsSendEmail,
                                 ResultDate = recoinciliation.ResultDate,
                                 ResultNote = recoinciliation.ResultNote,
                                 SendEmailDate = recoinciliation.SendEmailDate,
                                 CurrencyCode = "TL",
                                 AccountEmail = account.Email,
                                 Mounth =recoinciliation.Mounth,
                                 Type=recoinciliation.Type,
                                 Quantity=recoinciliation.Quantity,
                                 Year=recoinciliation.Year,
                                



                             };
                return result.ToList();
            }
        }
    }
}
