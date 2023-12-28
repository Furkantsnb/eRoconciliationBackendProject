using Core.DataAccsess;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsess.Abstract
{
    public interface IAccountReconciliationDal :IEntityRepository<AccountReconciliation>
    {
        List<AccountReconciliationDto> GetAllDto(int companyId);
    }
}
