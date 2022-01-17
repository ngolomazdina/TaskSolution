using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;
using TaskWcf.Model.DTO;

namespace TaskWcf.Application.BL
{
    public class GetUsersProcess: DataProcess<ContractResult<List<User>>>
    {
        
        public GetUsersProcess(IRepository repository)
            : base(repository)
        {
        }

        public override ContractResult<List<User>> Validate()
        {
            return new ContractResult<List<User>>() { Code = CodeResult.Success }; 
        }

        public override ContractResult<List<User>> WorkOn()
        {
            var result = new ContractResult<List<User>>() { Code = CodeResult.SystemError };
            var users = Repository.GetUsers();

            if (users != null)
            {
                result.Code = CodeResult.Success;
                result.Data = users;
            }
            return result;
        }
    }
}