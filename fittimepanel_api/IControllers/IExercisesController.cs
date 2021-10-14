using FittimePanelApi.Data;
using FittimePanelApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IControllers
{
    public interface IExercisesController : IController<Exercise>
    {
        public Task<IActionResult> New(CreateExerciseDTO createExerciseDTO);

    }
}
