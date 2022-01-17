using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskCommon.Helpers;
using TaskWcf;
using TaskWebApp.Models;
using TaskWebApp.Models.ErrorHandle;

namespace TaskWebApp.Controllers
{
    public class TaskController : Controller
    {
        private static ITaskService srv = WcfServiceHelper.ServiceBootstrap.TaskWcfService;
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View("TaskTree");
        }

        public async Task<ActionResult> GetTaskTree()
        {
            var tasks = srv.GetTasks();
            if (tasks.Code != CodeResult.Success)
            {
                var er = new Error("При получении дерева задач возникла ошибка. Обратитесь к системному администратору");
                return View("Error", er);
            }

            var treeNodes = Models.Mappings.MappingTreeNode.LoadTreeNodes(tasks.Data);            
            return Json(treeNodes, JsonRequestBehavior.AllowGet);           
        }

        public ActionResult OpenTaskDetails(Guid? taskUid = null)
        {

            var states = srv.GetStates(taskUid);
            if (states.Code!= CodeResult.Success)
            {
                var er = new Error($"При получении списка доступных статусов для задачи {taskUid.ToString()} возникла ошибка {states.Message}. Обратитесь к системному администратору");
                return View("Error", er);
            }

            ViewData["AllAvailableStates"] = from state in states.Data
                                             select new SelectListItem { Text = state.Name, Value = state.Id.ToString() };

            var users = srv.GetUsers();
            if (users.Code != CodeResult.Success)
            {
                var er = new Error($"При получении списка пользователей для задачи {taskUid.ToString()} возникла ошибка {states.Message}. Обратитесь к системному администратору");
                return View("Error", er);
            }

            ViewData["AllUsers"] = from user in users.Data
                                   select new SelectListItem { Text = user.Name, Value = user.Uid.ToString() };

            var tasks = srv.GetTasks();
            if(tasks.Code != CodeResult.Success)
            {
                var er = new Error($"При получении списка задач для задачи {taskUid.ToString()} возникла ошибка {states.Message}. Обратитесь к системному администратору");
                return View("Error", er);
            }

            ViewData["AllTasks"] = from tsk in tasks.Data
                                   select new SelectListItem { Text = tsk.Name, Value = tsk.Uid.ToString() };
            var executorsData = new List<Guid>();

            if (taskUid != null)
            {
                var executors = srv.GetExecutors((Guid)taskUid);
                if (executors.Code != CodeResult.Success)
                {
                    var er = new Error($"При получении списка исполнителей для задачи {taskUid.ToString()} возникла ошибка {states.Message}. Обратитесь к системному администратору");
                    return View("Error", er);
                }
                executorsData = executors.Data;
            }

            if (taskUid == null)            
                return PartialView("Task", new Models.Task());
            
            var taskDto = (from t in tasks.Data where t.Uid == taskUid select t).FirstOrDefault();

            if (taskDto == null)
            {
                var er = new Error($"Задача с идентификатором {taskUid.ToString()} не найдена. Обратитесь к системному администратору");
                return View("Error", er);
            }

            Models.Task task=null;

            try
            {
                task = Models.Mappings.AutomapBootstrap.Mapper.Map<TaskWcf.Model.DTO.Task, Models.Task>(taskDto);
                
            }
            catch (Exception ex)
            {
                var erMsg = LogHelper.GetErrorMessage($"При преобразовании объекта DTO к объекту модели для задачи {taskDto.Uid.ToString()}", ex);
                _log.Error(erMsg);
                var er = new Error(erMsg);
                return View("Error", er);
            }

            task.Executors = executorsData;

            return PartialView("Task",task);
        }

        [HttpPost]
        public ActionResult Save(TaskWebApp.Models.Task task)
        {

            

            if (task == null)
            {
                var er = new Error("ошибка при сохранении задачи. task==null");
                return View("Error", er);
            }

            bool isNew = (task.Uid == null || task.Uid == Guid.Empty);

            TaskWcf.Model.DTO.Task taskDto = null;

            try
            {
                taskDto = Models.Mappings.AutomapBootstrap.Mapper.Map<Models.Task, TaskWcf.Model.DTO.Task>(task);

            }
            catch (Exception ex)
            {
                var erMsg = LogHelper.GetErrorMessage($"При создании задачи возникла ошибка на этапе преобразования объекта модели к объекту DTO для задачи", ex);
                _log.Error(erMsg);
                var er = new Error(erMsg);
                return View("Error", er);
            }

            var executors = task.Executors;

            ContractResult result = null;

            if (isNew)
            {
                result = srv.CreateTask(taskDto, executors);
            }
            else
            {
                result = srv.UpdateTask(taskDto, executors);
            }

            if (result.Code != CodeResult.Success)
            {
                var er = new Error($"При сохранении задачи возникла ошибка {result.Message}. Обратитесь к системному администратору");
                return View("Error", er);
            }

            return View("TaskTree");
        }

        public ActionResult Delete(Guid taskUid)
        {
            var task = srv.DeleteTask(taskUid);

            if (task.Code== CodeResult.Success)
                return RedirectToAction("Index");

            var er = new Error(task.Message);
            return View("Error", er);
        }
    }
}