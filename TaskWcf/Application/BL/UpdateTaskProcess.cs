using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;
using TaskWcf.Model.DTO;

namespace TaskWcf.Application.BL
{
    public class UpdateTaskProcess : DataProcess<ContractResult>
    {
        Task _task;
        List<Guid> _executors;

        public UpdateTaskProcess(Task task, List<Guid> executors, IRepository repository)
            : base(repository)
        {
            _task = task;
            _executors = executors;
        }

        public override ContractResult Validate()
        {
            if (_task.ParentUid != null && _task.ParentUid != Guid.Empty)
                if(!Repository.CheckParentTaskChange((Guid)_task.Uid,(Guid)_task.ParentUid))
                   return new ContractResult() { Code = CodeResult.Error, Message="Невозможно изменить вышестоящую задачу" };

            return new ContractResult() { Code = CodeResult.Success };
        }

        public override ContractResult WorkOn()
        {

            var result = new ContractResult() { Code = CodeResult.SystemError };
            var updateTaskResult = Repository.UpdateTask(_task, _executors);

            if (updateTaskResult)
                result.Code = CodeResult.Success;              
            

            return result;
        }
    }
}