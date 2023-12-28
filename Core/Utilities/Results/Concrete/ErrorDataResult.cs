using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concrete
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data) : base(data, true)
        {
        }

        public ErrorDataResult(T data,  string message) : base(data, true, message)
        {
        }
        public ErrorDataResult():base(default,false)
        {

        }

        public ErrorDataResult(string message) : base(default, false,message)
        {

        }
    }
}
