using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskWcf.Infrastructure;

namespace TaskWcf.Application.BL
{
    public class UserAuthenticationProcess : DataProcess<ContractResult<Guid>>
    {
        string _login;
        string _password;

        public UserAuthenticationProcess(string login, string password, IRepository repository)
            : base(repository)
        {
            _login = login;
            _password = password;
        }

        public override ContractResult<Guid> Validate()
        {
            return new ContractResult<Guid>() { Code = CodeResult.Success };
        }

        public override ContractResult<Guid> WorkOn()
        {
            var result = new ContractResult<Guid>() { Code = CodeResult.SystemError, Message=$"Пользватель {_login} с паролем {_password} не прошёл аутентификацию" };
            var newTaskGuid = Repository.UserAuthentication(_login, _password);

            if (newTaskGuid != Guid.Empty)
            {
                result.Code = CodeResult.Success;
                result.Data = newTaskGuid;
            }

            return result;
        }
    }
}