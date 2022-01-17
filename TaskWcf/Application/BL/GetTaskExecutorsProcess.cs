using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;

namespace TaskWcf.Application.BL
{
    public class GetTaskExecutorsProcess : DataProcess<ContractResult<List<Guid>>>
    {
        Guid _taskUid;

        public GetTaskExecutorsProcess(Guid taskUid, IRepository repository)
            : base(repository)
        {
            _taskUid = taskUid;
        }
        public override ContractResult<List<Guid>> Validate()
        {
            return new ContractResult<List<Guid>>() { Code = CodeResult.Success };
        }

        public override ContractResult<List<Guid>> WorkOn()
        {
            var result = new ContractResult<List<Guid>>() { Code = CodeResult.SystemError };
            var executors = Repository.GetTaskExecutors(_taskUid);

            if (executors != null)
            {
                result.Code = CodeResult.Success;
                result.Data = executors;
            }
            return result;
        }
    }
}