using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models.Notifications
{
    public enum ReturnValue : int
    {
        UserOrPassInvalid = 0,
        Successful = 1,
        NotEnoughCredit = 2,
        DailySendLimit = 3,
        VolumeSendLimit = 4,
        FromNumberInvalid = 5,
        PanelInUpdate = 6,
        FilterInText = 7,
        SendNotPossible = 9,
        UserIsNotActive = 10,
        NotSent = 11,
        UserPapers = 12,
    }

    public interface ISmsPanelDTO
    {
    }

    public interface IResponseSmsDTO
    {
        public ReturnValue ReturnValue { get; set; }

    }

    public interface IResponseErrorSmsDTO : IResponseSmsDTO
    {
        public Exception Exception { get; set; }
        public string Error { get; set; }
    }

}
