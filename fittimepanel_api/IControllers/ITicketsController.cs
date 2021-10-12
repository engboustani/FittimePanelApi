﻿using FittimePanelApi.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IControllers
{
    public interface ITicketsController
    {
        public Task<ActionResult> ReadAll();
        public Task<ActionResult> ReadById(Guid id);

    }
}
