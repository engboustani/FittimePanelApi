using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IControllers
{
    public interface IBlobController
    {
        [Authorize, DisableRequestSizeLimit, HttpPost("Avatar"), ProducesResponseType(400), ProducesResponseType(200), ProducesResponseType(500)]
        Task<IActionResult> UploadAvatar(int? chunk, string name);
    }
}
