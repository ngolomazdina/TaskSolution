using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;
using TaskWcf.Model.DTO;

namespace TaskWcf.Application.BL
{
    public class GetTasksProcess : DataProcess<ContractResult<List<Task>>>
    {
        Guid? _taskUid;
        string _taskName;

        public GetTasksProcess(Guid? taskUid, string taskName, IRepository repository)
            : base(repository)
        {
            _taskUid = taskUid;
            _taskName = taskName;
        }

        public override ContractResult<List<Task>> Validate()
        {
            return new ContractResult<List<Task>>() { Code = CodeResult.Success };
        }

        public override ContractResult<List<Task>> WorkOn()
        {
            var result = new ContractResult<List<Task>>() { Code = CodeResult.SystemError };
            var tasks = Repository.GetTasks(_taskUid, _taskName);

            if (tasks != null)
            {
                result.Code = CodeResult.Success;
                result.Data = tasks;
            }
            return result;
        }
    }
}