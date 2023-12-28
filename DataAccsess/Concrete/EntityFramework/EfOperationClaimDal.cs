using Core.DataAccsess.EntityFramework;
using Core.Entities.Concrete;
using DataAccsess.Abstract;
using DataAccsess.Concrete.EntityFramework.Context;

namespace DataAccsess.Concrete.EntityFramework
{
    public class EfOperationClaimDal :EfEntityRepositoryBase<OperationClaim, ContextDb>, IOperationClaimDal
    {
    }
}
