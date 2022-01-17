using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;
using TaskWcf.Model.DTO;

namespace TaskWcf.Application.BL
{
    public class CreateTaskProcess : DataProcess<ContractResult<Guid>>
    {
        Task _task;
        List<Guid> _executors;

        public CreateTaskProcess(Task task, List<Guid> executors, IRepository repository)
            : base(repository)
        {
            _task = task;
            _executors = executors;
        }

        public override ContractResult<Guid> Validate()
        {
            return new ContractResult<Guid>() { Code = CodeResult.Success };
        }

        public override ContractResult<Guid> WorkOn()
        {
            var result = new ContractResult<Guid>() { Code= CodeResult.SystemError };
            var newTaskGuid = Repository.CreateTask(_task, _executors);

            if (newTaskGuid!=Guid.Empty)
            {
                result.Code = CodeResult.Success;
                result.Data = newTaskGuid;
            }

            return result;
        }
    }
}