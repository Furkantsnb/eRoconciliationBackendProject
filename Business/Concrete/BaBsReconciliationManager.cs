using Business.Abstract;
using Business.BusinessAcpects;
using Business.Constans;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccsess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using ExcelDataReader;

namespace Business.Concrete
{
    public class BaBsReconciliationManager : IBaBsReconciliationService
    {
        private readonly IBaBsReconciliationDal _baBsReconciliationDal;
        private readonly ICurrencyAccountService _currencyAccountService;
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMailParameterService _mailParameterService;

        public BaBsReconciliationManager(IBaBsReconciliationDal baBsReconciliationDal, ICurrencyAccountService currencyAccountService, IMailService mailService, IMailTemplateService mailTemplateService, IMailParameterService mailParameterService)
        {
            _baBsReconciliationDal = baBsReconciliationDal;
            _currencyAccountService = currencyAccountService;
            _mailService = mailService;
            _mailTemplateService = mailTemplateService;
            _mailParameterService = mailParameterService;
        }

        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.Add,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Add(BaBsReconciliation baBsReconciliation)
        {
            string guid = Guid.NewGuid().ToString();
            baBsReconciliation.Guid = guid;
            _baBsReconciliationDal.Add(baBsReconciliation);
            return new SuccessResult(Messages.AddedBaBsReconciliation);
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.Add,Admin")]

        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
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
                        string code = reader.GetString(1);



                        if (code != "Cari Kodu" && code != null)
                        {
                            string type = reader.GetString(1);
                            double  mounth = reader.GetDouble(2);
                            double year = reader.GetDouble(3);
                            double quantit = reader.GetDouble(4);
                            double total = reader.GetDouble(5);
                            string guid = Guid.NewGuid().ToString();
                          

                            int currencyAccountId = _currencyAccountService.GetByCode(code, companyId).Data.Id;

                            BaBsReconciliation baBsReconciliation = new BaBsReconciliation()
                            {
                                CompanyId = companyId,
                                CurrencuAccountId = currencyAccountId,
                                Type = type,
                                Mounth = Convert.ToInt16(mounth),
                                Year = Convert.ToInt16(year),
                                Quantity = Convert.ToInt16(quantit),
                                Total = Convert.ToDecimal(total),
                                Guid=guid,
                                
                            };
                            _baBsReconciliationDal.Add(baBsReconciliation);
                        }
                    }
                }
            }
            File.Delete(filePath);
            return new SuccessResult(Messages.AddedBaBsReconciliation);
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.Delete,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Delete(BaBsReconciliation baBsReconciliation)
        {
            _baBsReconciliationDal.Delete(baBsReconciliation);
            return new SuccessResult(Messages.DeletedBaBsReconciliation);
        }

        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsReconciliation> GetByCode(string code)
        {
            return new SuccesDataResult<BaBsReconciliation>(_baBsReconciliationDal.Get(p => p.Guid == code));
        }

        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsReconciliation> GetById(int id)
        {
            return new SuccesDataResult<BaBsReconciliation>(_baBsReconciliationDal.Get(p => p.Id == id));
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliation>> GetList(int companyId)
        {
            return new SuccesDataResult<List<BaBsReconciliation>>(_baBsReconciliationDal.GetList(p => p.CompanyId == companyId));

        }

        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliationDto>> GetListDto(int companyId)
        {
            return new SuccesDataResult<List<BaBsReconciliationDto>>(_baBsReconciliationDal.GetAllDto(companyId));

        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.SendMail,Admin")]
        public IResult SendReconciliationMail(BaBsReconciliationDto baBsReconciliationDto)
        {
            string subject = "Kullanıcı Kayıt Onay Maili";
            string body = $" Bizim Şirket: {baBsReconciliationDto.CompanyName}<br /> " +
               $"Şirket Vergi Dairesi: {baBsReconciliationDto.CompanyTaxDepartment}<br />" +
               $"Şirket Vergi Numarası: {baBsReconciliationDto.CompanyTaxIdNumber} -{baBsReconciliationDto.CompanyIdentityNumber} <br /><hr />" +
               $" Sizin Şirket: {baBsReconciliationDto.AccountName}<br /> " +
               $"Sizin Vergi Dairesi: {baBsReconciliationDto.AccountTaxDepartment}<br />" +
               $"Sizin Vergi Numarası: {baBsReconciliationDto.AccountTaxIdNumber} -{baBsReconciliationDto.AccountIdentityNumber} <br /><hr />" +
               $" Ay / Yıl: {baBsReconciliationDto.Mounth} / {baBsReconciliationDto.Year}<br /> " +
           
               $" Adet: {baBsReconciliationDto.Quantity} <br /> " +
               $" Tutar: {baBsReconciliationDto.Total} {baBsReconciliationDto.CurrencyCode}<br /> ";

            string link = "https://localhost:7174/api/BaBsReconciliations/GetByCode?code=" + baBsReconciliationDto.Guid;
            string linkDescription = "Mutabakata Cevaplamak İçin Tıkla";
            var mailTemplate = _mailTemplateService.GetByTemplateName("Kayıt", 6);
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

            var mailParameter = _mailParameterService.Get(baBsReconciliationDto.CompanyId);
            Entities.Dtos.SendMailDto sendMailDto = new Entities.Dtos.SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = baBsReconciliationDto.AccountEmail,
                subject = subject,
                body = templateBody,
            };
            _mailService.SendMail(sendMailDto);
            return new SuccessResult(Messages.MailSendSucessful);
        }

        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliation.Update,Admin")]

        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Update(BaBsReconciliation baBsReconciliation)
        {
            _baBsReconciliationDal.Update(baBsReconciliation);
            return new SuccessResult(Messages.UpdatedBaBsReconciliation);
        }
    }
}
