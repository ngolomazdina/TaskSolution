using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskWebApp.Models.ErrorHandle
{
    public class Error
    {
        private string _message;

        public string Message {
            get { return _message; }
            private set { }
        }
        public Error(string message)
        {
            _message = message;
        }
        
    }
}