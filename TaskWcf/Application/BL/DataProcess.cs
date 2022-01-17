using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;

namespace TaskWcf.Application.BL
{
    public abstract class DataProcess<T>
        where T : ContractResult, new()
    {        
        IRepository _repository;        

        public IRepository Repository { get { return _repository; } }

        public DataProcess(IRepository repository)
        {            
            _repository = repository;
        }

        public abstract T Validate();
        public abstract T WorkOn();
    }
}