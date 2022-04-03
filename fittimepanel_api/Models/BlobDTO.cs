using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models
{
    public class ExercisePdfBlobDTO
    {
        public string Name { get; set; }
        public Guid ExerciseId { get; set; }
        public IFormFile File { get; set; }
    }
}
