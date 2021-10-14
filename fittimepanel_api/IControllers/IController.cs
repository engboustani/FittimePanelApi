using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IControllers
{
    public interface IController <T> where T : class
    {
        public Task<IActionResult> ReadAll();
        public Task<IActionResult> ReadById(Guid id);
        public Task<IActionResult> DeleteById(Guid id);

    }
}
