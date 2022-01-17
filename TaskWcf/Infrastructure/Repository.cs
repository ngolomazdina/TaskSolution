using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using TaskCommon.Helpers;

namespace TaskWcf.Infrastructure
{
    public class Repository : IRepository
    {
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();

        public bool CheckParentTaskChange(Guid taskUid, Guid parentUid)
        {
            try
            {

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    return _context.Database.SqlQuery<bool>($"SELECT [dbo].[fn_CheckParentTaskChange] ('{taskUid.ToString()}', '{parentUid.ToString()}')").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage($"taskUid: {taskUid.ToString()}, parentUid: {parentUid.ToString()}", ex));
            }

            return false;
        }

        public Guid CreateTask(Model.DTO.Task task, List<Guid> executors = null )
        {
            var result = Guid.Empty;
            try
            {

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    using (var dbContextTransaction = _context.Database.BeginTransaction())
                    {
                        var taskDb = Model.Mappings.AutomapBootstrap.Mapper.Map<Model.DTO.Task, Model.DB.Task>(task);
                        taskDb.Uid = Guid.NewGuid();
                        taskDb.CreateDate = DateTime.Now;
                        taskDb.State = _context.State.Where(s => s.Id == taskDb.StateId).First();
                        _context.Task.Add(taskDb);
                        
                        if (executors != null)
                            foreach (Guid user in executors)
                                _context.TaskExecutor.Add(new Model.DB.TaskExecutor() { TaskUid = taskDb.Uid, ExecutorUid = user });

                        _context.SaveChanges();

                        dbContextTransaction.Commit();

                        result = taskDb.Uid;
                    }
                }

                SetParentTaskFactTime(result);
                SetParentTaskPlanTime(result);
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage("При создании задачи возникла ошибка", ex));
            }

            return result;
        }

        public bool UpdateTask(Model.DTO.Task task, List<Guid> executors = null)
        {
            var result = false;
            try
            {

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    using (var dbContextTransaction = _context.Database.BeginTransaction())
                    {
                        var taskDb = _context.Task.Where(t => t.Uid == task.Uid).First();
                        taskDb.ModifiedDate = DateTime.Now;
                        
                        try
                        {
                            foreach (PropertyInfo propertyInfo in task.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                                                        .Where(p => p.CanRead && p.CanWrite && p.Name != "CreateDate"))
                            {
                                PropertyInfo propertyInfo1 = taskDb.GetType().GetProperty(propertyInfo.Name);

                                if (propertyInfo1 != null && propertyInfo.GetValue(task) != null)
                                {
                                    propertyInfo1.SetValue(taskDb, propertyInfo.GetValue(task));
                                }
                            };
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidCastException(string.Format("переназначение свойств задачи при обновлении завершилось ошибкой: {0}", ex.Message));
                        }


                        if (executors != null)
                        {
                            _context.TaskExecutor.RemoveRange(_context.TaskExecutor.Where(t => t.TaskUid == task.Uid));

                            foreach (Guid user in executors)
                                _context.TaskExecutor.Add(new Model.DB.TaskExecutor() { TaskUid = taskDb.Uid, ExecutorUid = user });
                        }

                        _context.sp_ChangeChildState(task.Uid, task.StateId);

                        _context.SaveChanges();
                        dbContextTransaction.Commit();                        
                    }
                    
                }

                SetParentTaskFactTime(task.Uid);
                SetParentTaskPlanTime(task.Uid);

                result = true;
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage($"При сохранении задачи {task.Uid.ToString()} возникла ошибка", ex));
            }
            return result;
        }

        public List<Model.DTO.State> GetStates(Guid? taskUid=null)
        {
            List<Model.DTO.State> states = null;

            try
            {
                
                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    var DBstates = _context.sp_GetAvailableStates(taskUid).ToList();
                    states = Model.Mappings.AutomapBootstrap.Mapper.Map<List<Model.DB.sp_GetAvailableStates_Result>, List<Model.DTO.State>>(DBstates);
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage(taskUid.ToString(), ex));
            }

            return states;
        }

        public List<Model.DTO.Task> GetTasks(Guid? taskUid=null, string taskName=null)
        {
            List<Model.DTO.Task> tasks = null;

            try
            {

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    var DBTasks = _context.sp_GetTaskList(taskName, taskUid).ToList();
                    tasks = Model.Mappings.AutomapBootstrap.Mapper.Map<List<Model.DB.sp_GetTaskList_Result>, List<Model.DTO.Task>>(DBTasks);
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage(taskUid.ToString(), ex));
            }

            return tasks;
        }

        public bool DeleteTask(Guid taskUid)
        {
            var result = false;
            try
            {
                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    var task = _context.Task.Where(t => t.Uid == taskUid).First();
                    task.IsArchive = true;
                    _context.SaveChanges();
                    
                }

                SetParentTaskFactTime(taskUid);
                SetParentTaskPlanTime(taskUid);

                result = true;
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage(taskUid.ToString(), ex));
            }
            return result;
        }

        public bool? CheckChildren(Guid taskUid)
        {
            bool? result = null;
            try
            {

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    var childrenCount = _context.Task.Where(t => t.ParentUid == taskUid && t.IsArchive == false).Count();
                    result = childrenCount == 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage(taskUid.ToString(), ex));
            }

            return result;
        }

        public List<Model.DTO.User> GetUsers()
        {
            List<Model.DTO.User> users = null;

            try
            {

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    var DBusers = _context.User.Where(u=> u.IsArchive==false).ToList();
                    users = Model.Mappings.AutomapBootstrap.Mapper.Map<List<Model.DB.User>, List<Model.DTO.User>>(DBusers);
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage("При получении пользователей возникла ошибка", ex));
            }

            return users;
        }
        public List<Guid> GetTaskExecutors(Guid taskUid)
        {
            List<Guid> executors = null;

            try
            {

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    executors = (from te in _context.TaskExecutor where te.TaskUid == taskUid select (Guid)te.ExecutorUid).ToList();                      
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage($"При получении исполнителей по задаче {taskUid.ToString()} возникла ошибка", ex));
            }

            return executors;
        }

        public bool ValidateUser(Guid userUid)
        {
            bool result = false;
            try
            {

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    result = _context.UserRole.Where(ur=>ur.UserUid==userUid).Count()>0;
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage($"При проверке пользователя {userUid.ToString()} возникла ошибка", ex));
            }
            return result;
        }

        public Guid UserAuthentication(string login, string password)
        {
            var userUid = Guid.Empty;

            try
            {
                byte[] bytes = Encoding.GetEncoding(1252).GetBytes(password);

                var sha1 = SHA512.Create();
                byte[] hashBytes = sha1.ComputeHash(bytes);

                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    userUid = _context.User.Where(u => u.Login == login && u.Password == hashBytes).FirstOrDefault().Uid;
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage($"При аутентификации пользователя login {login}, password {password} возникла ошибка", ex));
            }

            return userUid;
        }

        internal void SetParentTaskFactTime(Guid? taskUid)
        {
            if (taskUid == null)
                return;

            try
            {
                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {                   
                    while(taskUid != null)
                    {                        
                        _context.sp_SetParentTaskFactTime(taskUid);
                        taskUid = _context.Task.Where(t => t.Uid == taskUid).First().ParentUid;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage(taskUid.ToString(), ex));
            }


        }

        internal void SetParentTaskPlanTime(Guid? taskUid)
        {
            if (taskUid == null)
                return;

            try
            {
                using (Model.DB.TaskDBEntities _context = new Model.DB.TaskDBEntities())
                {
                    while (taskUid != null)
                    {
                        _context.sp_SetParentTaskPlanTime(taskUid);
                        taskUid = _context.Task.Where(t => t.Uid == taskUid).First().ParentUid;
                    }

                }
            }
            catch (Exception ex)
            {
                _log.Error(LogHelper.GetErrorMessage(taskUid.ToString(), ex));
            }


        }

        
    }
}