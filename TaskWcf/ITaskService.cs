using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaskWcf.Model.DTO;

namespace TaskWcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ITaskService
    {

        [OperationContract]
        ContractResult<Guid> CreateTask(Task task, List<Guid> executors);

        [OperationContract]
        ContractResult UpdateTask(Task task, List<Guid> executors);

        [OperationContract]
        ContractResult DeleteTask(Guid taskUid);

        [OperationContract]
        ContractResult<List<State>> GetStates(Guid? taskUid = null);

        [OperationContract]
        ContractResult<List<Task>> GetTasks(Guid? taskUid = null, string taskName = null);

        [OperationContract]
        ContractResult<List<User>> GetUsers();

        [OperationContract]
        ContractResult<List<Guid>> GetExecutors(Guid taskUid);

        [OperationContract]
        ContractResult<Guid> UserAuthentication(string login, string password);

    }

    [DataContract]
    public class ContractResult
    {
        [DataMember]
        public CodeResult Code { get; set; }

        [DataMember]
        public string Message { get; set; }

    }
    /// <summary>
    /// Результат выполнения OperationContract
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract(Name = "ContractResult{0}")]
    public class ContractResult<T> : ContractResult
    {
        [DataMember]
        public T Data { get; set; }

    }

    public enum CodeResult
    {
        [EnumDisplay("Системные ошибки")]
        SystemError = -1,

        [EnumDisplay("Успешно")]
        Success = 0,

        [EnumDisplay("Ошибка")]
        Error = 1
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayAttribute : Attribute
    {
        private string _displayAttribute;

        public EnumDisplayAttribute(string displayAttribute)
        {
            _displayAttribute = displayAttribute;
        }

        public string DisplayAttribute
        {
            get { return _displayAttribute; }
        }
    }

    public static class EnumExtensions
    {
        public static string ToDisplayString(this Enum value)
        {
            var attribute = (EnumDisplayAttribute)
                Attribute.GetCustomAttribute(
                                value.GetType().GetField(value.ToString()),
                                typeof(EnumDisplayAttribute)
                                            );

            if (attribute == null) return value.ToString();
            else return attribute.DisplayAttribute;
        }
    }

}
