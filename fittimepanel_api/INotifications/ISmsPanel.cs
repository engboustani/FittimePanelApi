using FittimePanelApi.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.INotifications
{
    public interface ISmsPanel : INotifications
    {
        public Task<IResponseSmsDTO> SendSimpleSMS(SendSimpleSmsDTO sendSimpleSmsDTO);
        public Task<IResponseSmsDTO> SendSMS(SendSmsDTO sendSmsDTO);

    }
}
