using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models
{
    public enum ExerciseStatus : int
    {
        Requested = 0,
        GoesForPay = 1,
        Paid = 2,
        InProcess = 3,
        Completed = 4,
        ReProcess = 5
    }

    public class CreateExerciseDTO
    {
        public virtual ExerciseTypeDTO ExerciseType { get; set; }
        public virtual ICollection<CreateExerciseMetaDTO> ExerciseMetas { get; set; }
        public virtual ExerciseStatus Status { get; set; }
    }

    public class ExerciseDTO : CreateExerciseDTO
    {
        public Guid Id { get; set; }
        public virtual UserDTO UserStudent { get; set; }
        public virtual UserDTO UserInstructor { get; set; }
        public virtual ICollection<ExerciseDownloadDTO> ExerciseDownloads { get; set; }
    }

    public class CreateExerciseTypeDTO
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public class ExerciseTypeDTO : CreateExerciseTypeDTO
    {
        public int Id { get; set; }
    }

    public class CreateExerciseMetaDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ExerciseMetaDTO : CreateExerciseMetaDTO
    {
        public Guid Id { get; set; }
    }

    public class CreateExerciseDownloadDTO
    {
        public string Uri { get; set; }
    }

    public class ExerciseDownloadDTO : CreateExerciseDownloadDTO
    {
        public Guid Id { get; set; }
    }
}
