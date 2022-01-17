using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWcf.Infrastructure
{
    public interface IRepository
    {
        List<Model.DTO.State> GetStates(Guid? taskUid=null);
        List<Model.DTO.Task> GetTasks(Guid? taskUid=null, string taskName = null);
        List<Model.DTO.User> GetUsers();
        List<Guid> GetTaskExecutors(Guid taskUid);
        bool ValidateUser(Guid userUid); 
        Guid CreateTask(Model.DTO.Task task, List<Guid> Executors = null);
        bool UpdateTask(Model.DTO.Task task, List<Guid> Executors = null ); 
        bool DeleteTask(Guid taskUid);

        //Проверка возможности смены вышестоящей задачи
        bool CheckParentTaskChange(Guid taskUid, Guid parentUid);

        //Проверка наличия подзадач
        bool? CheckChildren(Guid taskUid);

        Guid UserAuthentication(string login, string password);
    }
}
