using FittimePanelApi.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual int ExerciseTypeId { get; set; }
        public virtual ICollection<CreateExerciseMetaDTO> ExerciseMetas { get; set; }
        public ICollection<ExerciseBlobResponseDTO> ExerciseBlobs { get; set; }
    }

    public class ExerciseDTO : CreateExerciseDTO
    {
        public Guid Id { get; set; }
        public virtual UserDTO UserStudent { get; set; }
        public virtual UserDTO UserInstructor { get; set; }
        public virtual ICollection<ExerciseDownloadDTO> ExerciseDownloads { get; set; }
        public virtual ICollection<ExerciseMetaDTO> ExerciseMetas { get; set; }
    }

    public class ExerciseDetailDTO : ExerciseDTO
    {
        public virtual ExerciseTypeDTO ExerciseType { get; set; }
        public ExerciseStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class ExerciseListItemDTO
    {
        public Guid Id { get; set; }
        public ExerciseStatus Status { get; set; }
        public CreateExerciseTypeDTO ExerciseType { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ExercisePageAllItemDTO
    {
        public PageInfo PageInfo { get; set; }
        public IList<ExerciseListAllItemDTO> ItemsList { get; set; }
    }

    public class ExerciseListAllItemDTO
    {
        public Guid Id { get; set; }
        public ExerciseStatus Status { get; set; }
        public CreateExerciseTypeDTO ExerciseType { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserShortListItemDTO UserStudent { get; set; }
    }

    public class CreateExerciseTypeDTO
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class ExerciseTypeDTO : CreateExerciseTypeDTO
    {
        public int Id { get; set; }
    }

    public class CreateExerciseMetaDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
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
        public string Description { get; set; }
    }

    public class ExerciseBlobDTO
    {
        [Required]
        public string Key { get; set; }
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }

    public class ExerciseBlobResponseDTO
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
    }
}
