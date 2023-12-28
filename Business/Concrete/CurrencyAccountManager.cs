﻿using Business.Abstract;
using Business.BusinessAcpects;
using Business.Constans;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccsess.Abstract;
using Entities.Concrete;
using ExcelDataReader;

namespace Business.Concrete
{
    public class CurrencyAccountManager : ICurrencyAccountService
    {
        private readonly ICurrencyAccountDal _currencyAccountDal;

        public CurrencyAccountManager(ICurrencyAccountDal currencyAccountDal)
        {
            _currencyAccountDal = currencyAccountDal;
        }

        [PerformanceAspect(2)]
        [SecuredOperation("CurrencyAccountValidator.Add,Admin")]
        [CacheRemoveAspect("ICurrencyAccountService.Get")]
        [ValidationAspect(typeof(CurrencyAccountValidator))]
        public IResult Add(CurrencyAccount currencyAccount)
        {
          
            _currencyAccountDal.Add(currencyAccount);
            return new SuccessResult(Messages.AddedCurrencyAccount);
        }
        [PerformanceAspect(2)]
        [SecuredOperation("CurrencyAccountValidator.Add,Admin")]
        [CacheRemoveAspect("ICurrencyAccountService.Get")]
        [ValidationAspect(typeof(CurrencyAccountValidator))]
        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath,int companyId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
              using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while(reader.Read())
                    {
                        string code = reader.GetString(0);
                        string name = reader.GetString(1);
                        string address = reader.GetString(2);
                        string taxDepartment = reader.GetString(3);
                        string taxIdNumber = reader.GetString(4);
                        string identityNumber = reader.GetString(5);
                        string email = reader.GetString(6);
                        string authorized = reader.GetString(7);

                        if(code!="Cari Kodu")
                        {
                            CurrencyAccount currencyAccount = new CurrencyAccount()
                            {
                                Name = name,
                                Address = address,
                                TaxDepatment = taxDepartment,
                                TaxIdNumber = taxIdNumber,
                                IdentiyNumber = identityNumber,
                                Email = email,
                                Authorized = authorized,
                                AddedAt = DateTime.Now,
                                Code = code,
                                CompanyId = companyId,
                                IsActive=true
                            };
                            _currencyAccountDal.Add(currencyAccount);
                        }
                    }
              }
            }
            return new SuccessResult(Messages.AddedCurrencyAccount);
        }

        [PerformanceAspect(2)]
        [SecuredOperation("CurrencyAccountValidator.Delete,Admin")]
        [CacheRemoveAspect("ICurrencyAccountService.Get")]
        public IResult Delete(CurrencyAccount currencyAccount)
        {
            _currencyAccountDal.Delete(currencyAccount);
            return new SuccessResult(Messages.DeletedCurrencyAccount);
        }

        [PerformanceAspect(2)]
        [SecuredOperation("CurrencyAccountValidator.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<CurrencyAccount> Get(int id)
        {
            return new SuccesDataResult<CurrencyAccount>(_currencyAccountDal.Get(p => p.Id == id));
        }

        [PerformanceAspect(2)]
        [SecuredOperation("CurrencyAccountValidator.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<CurrencyAccount> GetByCode(string code, int companyId)
        {
            return new SuccesDataResult<CurrencyAccount>(_currencyAccountDal.Get(p => p.Code == code && p.CompanyId == companyId));
        }

        [PerformanceAspect(2)]
        [SecuredOperation("CurrencyAccountValidator.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<CurrencyAccount>> GetList(int companyId)
        {
            return new SuccesDataResult<List<CurrencyAccount>>(_currencyAccountDal.GetList(p => p.CompanyId == companyId));
        }

        [PerformanceAspect(2)]
        [SecuredOperation("CurrencyAccountValidator.Update,Admin")]
        [CacheRemoveAspect("ICurrencyAccountService.Get")]
        [ValidationAspect(typeof(CurrencyAccountValidator))]
        public IResult Update(CurrencyAccount currencyAccount)
        {
            _currencyAccountDal.Update(currencyAccount);
            return new SuccessResult(Messages.UpdatedCurrencyAccount);
        }
    }
}