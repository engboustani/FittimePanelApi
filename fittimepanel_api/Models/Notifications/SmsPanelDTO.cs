using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models.Notifications
{
    public class SmsPanelDTO
    {
    }

    public class SendSmsDTO
    {
        public string[] To { get; set; }
        public string Text { get; set; }
    }

    public class SendSimpleSmsDTO : SendSmsDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public bool IsFlush { get; set; }
    }

    public class ResponseSmsDTO : IResponseSmsDTO
    {
        public ReturnValue ReturnValue { get; set; }
    }

    public class ResponseErrorSmsDTO : IResponseErrorSmsDTO
    {
        public ResponseErrorSmsDTO(Exception exception, string error)
        {
            Exception = exception;
            Error = error;
        }
        public ReturnValue ReturnValue { get; set; }

        public Exception Exception { get; set; }
        public string Error { get; set; }
    }

}
