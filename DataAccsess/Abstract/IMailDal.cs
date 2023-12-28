using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsess.Abstract
{
    public interface IMailDal
    {
        void SendMail(SendMailDto sendMailDto);    
    }
}
