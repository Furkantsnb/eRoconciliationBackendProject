using Core.DataAccsess.EntityFramework;
using DataAccsess.Abstract;
using DataAccsess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsess.Concrete
{
    public class EfMailTemplateDal : EfEntityRepositoryBase<MailTemplate,ContextDb>,ImailTemplateDal
    {
    }
}
