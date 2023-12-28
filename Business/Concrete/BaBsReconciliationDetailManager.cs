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
using ExcelDataReader;

namespace Business.Concrete
{
    public class BaBsReconciliationDetailManager : IBaBsReconciliationDetailService
    {
        private readonly IBaBsReconciliationDetailDal _babsReconciliationDetailDal;

        public BaBsReconciliationDetailManager(IBaBsReconciliationDetailDal babsReconciliationDetailDal)
        {
            _babsReconciliationDetailDal = babsReconciliationDetailDal;
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliationDetail.Add,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationDetailService.Get")]
        public IResult Add(BaBsReconciliationDetail babsReconciliationDetail)
        {
            _babsReconciliationDetailDal.Add(babsReconciliationDetail);
            return new SuccessResult(Messages.AddedBaBsReconciliationDetail);
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliationDetail.Add,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationDetailService.Get")]
        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath, int babsReconciliationId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string description = reader.GetString(1);



                        if (description != "Açıklama" && description != null)
                        {
                            DateTime date = reader.GetDateTime(0);
                            double amount = reader.GetDouble(2);

                            BaBsReconciliationDetail baBsReconciliationDetail = new BaBsReconciliationDetail()
                            {
                                BaBsReconciliationId = babsReconciliationId,
                                Date = date,
                                Description = description,
                                Amount = Convert.ToDecimal(amount)
                            };
                            _babsReconciliationDetailDal.Add(baBsReconciliationDetail );
                        }
                    }
                }
            }
            File.Delete(filePath);
            return new SuccessResult(Messages.AddedBaBsReconciliation);
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliationDetail.Delete,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationDetailService.Get")]
        public IResult Delete(BaBsReconciliationDetail babsReconciliationDetail)
        {
            _babsReconciliationDetailDal.Delete(babsReconciliationDetail);
            return new SuccessResult(Messages.DeletedBaBsReconciliationDetail);
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliationDetail.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsReconciliationDetail> GetById(int id)
        {
            return new SuccesDataResult<BaBsReconciliationDetail>(_babsReconciliationDetailDal.Get(p => p.Id == id));
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliationDetail.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliationDetail>> GetList(int babsReconciliationId)
        {
            return new SuccesDataResult<List<BaBsReconciliationDetail>>(_babsReconciliationDetailDal.GetList(p => p.BaBsReconciliationId == babsReconciliationId));
        }
        [PerformanceAspect(2)]
        [SecuredOperation("BaBsReconciliationDetail.Update,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationDetailService.Get")]
        public IResult Update(BaBsReconciliationDetail babsReconciliationDetail)
        {
            _babsReconciliationDetailDal.Update(babsReconciliationDetail);
            return new SuccessResult(Messages.UpdatedBaBsReconciliationDetail);
        }
    }
}
