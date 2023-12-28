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
    public class MailParameterManager : IMailParameterService
    {
        private readonly IMailParameterDal _mailParameterDal;

        public MailParameterManager(IMailParameterDal mailParameterDal)
        {
            _mailParameterDal = mailParameterDal;
        }
        [CacheAspect(60)]
        public IDataResult<MailParameter> Get(int companyId)
        {
            return new SuccesDataResult<MailParameter>(_mailParameterDal.Get(m => m.CompanyId == companyId));

        }
        [PerformanceAspect(2)]
        [SecuredOperation("MailParameter.Update,Admin")]
        [CacheRemoveAspect("IMailParameterService.Get")]
        public IResult Update(MailParameter mailParameter)
        {
            var result = Get(mailParameter.CompanyId);
            if(result.Data == null)
            {
                _mailParameterDal.Add(mailParameter);
            }
            else
            {
                result.Data.SMTP = mailParameter.SMTP;
                result.Data.Port = mailParameter.Port;
                result.Data.SSL = mailParameter.SSL;
                result.Data.Email = mailParameter.Email;
                result.Data.Password = mailParameter.Password;
                _mailParameterDal.Update(result.Data);

            }
            return new SuccessResult(Messages.MailParameterAdded);
        }
    }
}
