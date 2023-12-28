using Business.Abstract;
using Business.BusinessAcpects;
using Business.Constans;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccsess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class MailTemplateManager : IMailTemplateService
    {
        private readonly ImailTemplateDal _mailTemplateDal;

        public MailTemplateManager(ImailTemplateDal mailTemplateDal)
        {
            _mailTemplateDal = mailTemplateDal;
        }

        [PerformanceAspect(2)]
        [SecuredOperation("MailTemplate.Add,Admin")]
        [CacheRemoveAspect("IMailTemplateService.Get")]
        public IResult Add(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Add(mailTemplate);
            return new SuccessResult(Messages.MailTemplateAdded);
        }
        [CacheRemoveAspect("IMailTemplateService.Get")]

        [PerformanceAspect(2)]
        [SecuredOperation("MailTemplate.Delete,Admin")]
        [CacheRemoveAspect("IMailTemplateService.Get")]
        public IResult Delete(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Delete(mailTemplate);
            return new SuccessResult(Messages.MailTemplateDeleted);
        }

        [PerformanceAspect(2)]
        [CacheAspect(60)]
        public IDataResult<MailTemplate> Get(int id)
        {
            return new SuccesDataResult<MailTemplate>(_mailTemplateDal.Get(m => m.Id == id));
        }

        [PerformanceAspect(2)]
        [SecuredOperation("MailTemplate.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<MailTemplate>> GetAll(int companyId)
        {
            return new SuccesDataResult<List<MailTemplate>>(_mailTemplateDal.GetList(m => m.CompanyId == companyId));

        }
        [CacheAspect(60)]
        public IDataResult<MailTemplate> GetByTemplateName(string name, int companyId)
        {
            return new SuccesDataResult<MailTemplate>(_mailTemplateDal.Get(m => m.Type == name && m.CompanyId ==companyId));
        }
        [PerformanceAspect(2)]
        [SecuredOperation("MailTemplate.Update,Admin")]
        [CacheRemoveAspect("IMailTemplateService.Get")]
        public IResult Update(MailTemplate mailTemplate)
        {
            _mailTemplateDal.Update(mailTemplate);
            return new SuccessResult(Messages.MailTemplateUpdated);
        }
    }
}
