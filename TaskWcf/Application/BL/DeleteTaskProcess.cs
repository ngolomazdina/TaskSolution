using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;

namespace TaskWcf.Application.BL
{
    public class DeleteTaskProcess : DataProcess<ContractResult>
    {
        Guid _taskUid;

        public DeleteTaskProcess(Guid taskUid, IRepository repository)
            : base(repository)
        {
            _taskUid=taskUid;
        }

        public override ContractResult Validate()
        {
            var result = new ContractResult() { Code = CodeResult.SystemError };

            var hasChildren = Repository.CheckChildren(_taskUid);

            if (hasChildren == null)
                return result;

            if(hasChildren==true)
            {
                result.Code = CodeResult.Error;
                result.Message = "Задача имеет дочерние подзадачи. Удаление невозможно";

                return result;
            }

            result.Code = CodeResult.Success;

            return result;
        }

        public override ContractResult WorkOn()
        {
            var result = new ContractResult() { Code = CodeResult.Error, Message="Удаление задачи невозможно"};

            if (Repository.DeleteTask(_taskUid))
            {
                result.Code = CodeResult.Success;
                result.Message = "Задача удалена";
            }

            return result;
        }
    }
}