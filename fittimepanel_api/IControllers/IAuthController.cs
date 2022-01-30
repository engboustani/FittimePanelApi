using FittimePanelApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IControllers
{
    public interface IAuthController
    {
        public Task<IActionResult> Register(UserDTO userDTO);
        public Task<IActionResult> Login(LoginUserDTO userDTO);
        public Task<IActionResult> UserRoles();

    }
}
