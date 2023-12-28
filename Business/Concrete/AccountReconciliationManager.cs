using Business.Abstract;
using Business.BusinessAcpects;
using Business.Constans;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccsess.Abstract;
using DataAccsess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AccountReconciliationManager : IAccountReconciliationService
    {
        private readonly IAccountReconciliationDal _accountReconciliationDal;
        private readonly ICurrencyAccountService _currencyAccountService;
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMailParameterService _mailParameterService;

        public AccountReconciliationManager(IAccountReconciliationDal accountReconciliationDal, ICurrencyAccountService currencyAccountService, IMailService mailService, IMailTemplateService mailTemplateService, IMailParameterService mailParameterService)
        {
            _accountReconciliationDal = accountReconciliationDal;
            _currencyAccountService = currencyAccountService;
            _mailService=mailService;
            _mailTemplateService = mailTemplateService;
            _mailParameterService = mailParameterService;
        }
        [PerformanceAspect(2)]
        [SecuredOperation("AccountReconciliation.Add,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        public IResult Add(AccountReconciliation accountReconciliation)
        {
            string guid = Guid.NewGuid().ToString();
            accountReconciliation.Guid= guid;
          _accountReconciliationDal.Add(accountReconciliation);
            return new SuccessResult(Messages.AddedAccountReconciliation);
        }
        [PerformanceAspect(2)]
       [SecuredOperation("AccountReconciliation.Add,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath, int companyId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string code = reader.GetString(0);
                       


                        if (code != "Cari Kodu" &&code!=null)
                        {
                            DateTime stratingData = reader.GetDateTime(1);
                            DateTime endingDate = reader.GetDateTime(2);
                            double currencyId = reader.GetDouble(3);
                            double debit = reader.GetDouble(4);
                            double credit = reader.GetDouble(5);
                            int currencyAccountId= _currencyAccountService.GetByCode(code,companyId).Data.Id;

                            string guid = Guid.NewGuid().ToString();
                          

                            AccountReconciliation accountReconciliation = new AccountReconciliation()
                            {
                                CompanyId = companyId,
                                CurrencuAccountId = currencyAccountId,
                                CurrencyCredit=Convert.ToDecimal(credit),
                                CurrencyDebit= Convert.ToDecimal(debit),
                                CurrencyId =Convert.ToInt16(currencyId),
                                StartingDate=stratingData,
                                EndingDate=endingDate,
                                Guid = guid
                            };

                            _accountReconciliationDal.Add(accountReconciliation);
                        }
                    }
                }
            }
            File.Delete(filePath);
            return new SuccessResult(Messages.AddedAccountReconciliation);
        }
        [PerformanceAspect(2)]
        [SecuredOperation("AccountReconciliation.Delete,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        public IResult Delete(AccountReconciliation accountReconciliation)
        {
            _accountReconciliationDal.Delete(accountReconciliation);
            return new SuccessResult(Messages.DeletedAccountReconciliation);
        }
        [PerformanceAspect(2)]
        public IDataResult<AccountReconciliation> GetByCode(string code)
        {
            return new SuccesDataResult<AccountReconciliation>(_accountReconciliationDal.Get(p => p.Guid == code));
        }

        [PerformanceAspect(2)]
        [SecuredOperation("AccountReconciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<AccountReconciliation> GetById(int id)
        {
            return new SuccesDataResult<AccountReconciliation>(_accountReconciliationDal.Get(p => p.Id == id));
        }
        [PerformanceAspect(2)]
        [SecuredOperation("AccountReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<AccountReconciliation>> GetList(int companyId)
        {
            return new SuccesDataResult<List<AccountReconciliation>>(_accountReconciliationDal.GetList(p => p.CompanyId == companyId));

        }
        [PerformanceAspect(2)]
        [SecuredOperation("AccountReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<AccountReconciliationDto>> GetListDto(int companyId)
        {
            return new SuccesDataResult<List<AccountReconciliationDto>>(_accountReconciliationDal.GetAllDto(companyId));
        }

        [PerformanceAspect(2)]
        [SecuredOperation("AccountReconciliation.SendMail,Admin")]
        public IResult SendReconciliationMail(AccountReconciliationDto accountReconciliationDto)
        {
            string subject = "Kullanıcı Kayıt Onay Maili";
            string body = $" Bizim Şirket: {accountReconciliationDto.CompanyName}<br /> " +
               $"Şirket Vergi Dairesi: {accountReconciliationDto.CompanyTaxDepartment}<br />" +
               $"Şirket Vergi Numarası: {accountReconciliationDto.CompanyTaxIdNumber} -{accountReconciliationDto.CompanyIdentityNumber} <br /><hr />" +
               $" Sizin Şirket: {accountReconciliationDto.AccountName}<br /> " +
               $"Sizin Vergi Dairesi: {accountReconciliationDto.AccountTaxDepartment}<br />" +
               $"Sizin Vergi Numarası: {accountReconciliationDto.AccountTaxIdNumber} -{accountReconciliationDto.AccountIdentityNumber} <br /><hr />" +
               $" Borç: {accountReconciliationDto.CurrencyDebit} {accountReconciliationDto.CurrencyCode}<br /> " +
               $" Alacak: {accountReconciliationDto.CurrencyCredit} {accountReconciliationDto.CurrencyCode}<br /> ";

            string link = "https://localhost:7174/api/AccountReconciliations/GetByCode?code=" + accountReconciliationDto.Guid;
            string linkDescription = "Mutabakata Cevaplamak İçin Tıkla";
            var mailTemplate = _mailTemplateService.GetByTemplateName("Kayıt", 6);
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);
            //accountReconciliationDto.CompanyId
            var mailParameter = _mailParameterService.Get(6);
            Entities.Dtos.SendMailDto sendMailDto = new Entities.Dtos.SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = accountReconciliationDto.AccountEmail,
                subject = subject,
                body = templateBody,
            };
            _mailService.SendMail(sendMailDto);
            return new SuccessResult(Messages.MailSendSucessful);
            //string subject = "Mutabakat Maili";
            //string body = $"Şirket Adımız: {accountReconciliationDto.CompanyName} <br />" +
            //    $"Şirket Vergi Dairesi: {accountReconciliationDto.CompanyTaxDepartment} <br />" +
            //    $"Şirket Vergi Numarası: {accountReconciliationDto.CompanyTaxIdNumber} - {accountReconciliationDto.CompanyIdentityNumber} <br /><hr>" +
            //    $"Sizin Şirket: {accountReconciliationDto.AccountName}<br />" +
            //    $"Sizin Şirket Vergi Dairesi: {accountReconciliationDto.AccountTaxDepartment} <br />" +
            //    $"Sizin Şirket Vergi Numarası: {accountReconciliationDto.AccountTaxIdNumber} - {accountReconciliationDto.AccountIdentityNumber} <br /><hr>" +
            //    $"Borç: {accountReconciliationDto.CurrencyDebit} {accountReconciliationDto.CurrencyCode} <br />" +
            //    $"Alacak: {accountReconciliationDto.CurrencyCredit} {accountReconciliationDto.CurrencyCode} <br />";

            //string link = "https://localhost:7096/api/AccountReconciliations/GetByCode?code=" + accountReconciliationDto.Guid;
            //string linkDescription = "Mutabakatı Cevaplamak için Tıklayın";

            //var mailTemplate = _mailTemplateService.GetByTemplateName("Kayıt", 6);//burada null hatası veriyor
            //string templateBody = mailTemplate.Data.Value;

            //templateBody = templateBody.Replace("{{title}}", subject);
            //templateBody = templateBody.Replace("{{message}}", body);
            //templateBody = templateBody.Replace("{{link}}", link);
            //templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

            //var mailParameter = _mailParameterService.Get(5);
            //Entities.Dtos.SendMailDto sendMailDto = new Entities.Dtos.SendMailDto()
            //{
            //    mailParameter = mailParameter.Data,
            //    email = accountReconciliationDto.AccountEmail,
            //    subject = subject,
            //    body = templateBody
            //};

            //_mailService.SendMail(sendMailDto);

            //return new SuccessResult(Messages.MailSendSucessful);
        }

        [PerformanceAspect(2)]
        [SecuredOperation("AccountReconciliation.Update,Admin")]
        [CacheRemoveAspect("IAccountReconciliationService.Get")]
        public IResult Update(AccountReconciliation accountReconciliation)
        {
            _accountReconciliationDal.Update(accountReconciliation);
            return new SuccessResult(Messages.UpdatedAccountReconciliation);
        }
    }
}
