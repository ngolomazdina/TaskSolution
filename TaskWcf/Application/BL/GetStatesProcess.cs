using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;
using TaskWcf.Model.DTO;

namespace TaskWcf.Application.BL
{
    public class GetStatesProcess : DataProcess<ContractResult<List<State>>>
    {
        Guid? _taskUid;

        public GetStatesProcess(Guid? taskUid, IRepository repository)
            : base(repository)
        {
            _taskUid = taskUid;
        }

        public override ContractResult<List<State>> Validate()
        {
            return new ContractResult<List<State>>() { Code = CodeResult.Success };
        }

        public override ContractResult<List<State>> WorkOn()
        {
            var result = new ContractResult<List<State>>() { Code = CodeResult.SystemError };
            var states=Repository.GetStates(_taskUid);

            if(states!=null)
            {
                result.Code = CodeResult.Success;
                result.Data = states;
            }                
            return result;
        }
    }
}