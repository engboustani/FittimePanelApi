using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IControllers
{
    public interface IController <T> where T : class
    {
        public Task<ActionResult<IEnumerable<T>>> ReadAll();
        public Task<ActionResult<T>> ReadById(Guid id);
        public Task<IActionResult> Update(Guid id, object newData);
        public Task<ActionResult<T>> New(object data);
        public Task<ActionResult<T>> DeleteById(Guid id);
        public bool Exist(Guid id);

    }
}
