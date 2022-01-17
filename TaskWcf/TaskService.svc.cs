using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaskWcf.Application.BL;
using TaskCommon.Helpers;
using TaskWcf.Infrastructure;
using TaskWcf.Model.DTO;
using TaskWcf.Model.Mappings;

namespace TaskWcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class TaskService : ITaskService
    {
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        //IRepository repository = new Repository();

        public IRepository Repository { get; private set; }

        public TaskService(IRepository repository)
        {
            Repository = repository;
        }

        public ContractResult<Guid> CreateTask(Task task, List<Guid> executors)
        {            
            return DataProcessing(new CreateTaskProcess(task, executors, Repository));
        }

        public ContractResult DeleteTask(Guid taskUid)
        {
            return DataProcessing(new DeleteTaskProcess(taskUid, Repository));
        }

        public ContractResult<List<State>> GetStates(Guid? taskUid=null)
        {
            return DataProcessing(new GetStatesProcess(taskUid, Repository));
        }

        public ContractResult<List<Task>> GetTasks(Guid? taskUid = null, string taskName = null)
        {
            return DataProcessing(new GetTasksProcess(taskUid, taskName, Repository));
        }

        public ContractResult<List<User>> GetUsers()
        {
            return DataProcessing(new GetUsersProcess(Repository));
        }

        public ContractResult UpdateTask(Task task, List<Guid> executors)
        {
            return DataProcessing(new UpdateTaskProcess(task, executors, Repository));
        }


        public ContractResult<List<Guid>> GetExecutors(Guid taskUid)
        {
            return DataProcessing(new GetTaskExecutorsProcess(taskUid, Repository));
        }

        public ContractResult<Guid> UserAuthentication(string login, string password)
        {
            return DataProcessing(new UserAuthenticationProcess(login, password, Repository));
        }

        T DataProcessing<T>(DataProcess<T> dataProcess)
            where T : ContractResult, new()
        {
            T result = null;

            try
            {
                
                result = dataProcess.Validate();
                if (result.Code == CodeResult.Success)
                    result = dataProcess.WorkOn();

            }
            catch (Exception ex)
            {
                result = new T() { Code = CodeResult.SystemError, Message = CodeResult.SystemError.ToDisplayString() };
                _log.Error(LogHelper.GetErrorMessage(string.Empty, ex));

            }
            return result;
        }

        
    }
}
