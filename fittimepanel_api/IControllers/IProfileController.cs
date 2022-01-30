using FittimePanelApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IControllers
{
    public interface IProfileController
    {
        public Task<IActionResult> GetProfile();
        public Task<IActionResult> UpdateProfile(UserProfileDTO userProfileDTO);
        public Task<IActionResult> AddMeta();
        public Task<IActionResult> AddOrUpdateBlob(UserBlobDTO userBlobDTO);
        public Task<IActionResult> ReadBlob(string key);
    }
}
