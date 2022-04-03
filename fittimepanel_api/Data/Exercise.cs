using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FittimePanelApi.Repository;

namespace FittimePanelApi.Data
{
    public class Exercise : BaseEntity
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

        public Exercise()
        {
            ExerciseMetas = new HashSet<ExerciseMeta>();
            ExerciseBlobs = new HashSet<ExerciseBlob>();
            ExerciseDownloads = new HashSet<ExerciseDownload>();
            Payments = new HashSet<Payment>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public virtual User UserStudent { get; set; }
        public virtual User UserInstructor { get; set; }
        public int ExerciseTypeId { get; set; }
        public virtual ExerciseType ExerciseType { get; set; }
        public virtual ICollection<ExerciseMeta> ExerciseMetas { get; set; }
        public virtual ICollection<ExerciseBlob> ExerciseBlobs { get; set; }
        public virtual ExerciseStatus Status { get; set; }
        public virtual ICollection<ExerciseDownload> ExerciseDownloads { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }

    public class ExerciseType : BaseEntity
    {
        public ExerciseType()
        {
            Exercises = new HashSet<Exercise>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public uint Price { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
    public class ExerciseMeta : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual User User { get; set; }
        public virtual Exercise Exercise { get; set; }
        public string? Description { get; set; }
    }

    public class ExerciseBlob : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Key { get; set; }
        public byte[] Value { get; set; }
        public virtual User User { get; set; }
        public virtual Exercise Exercise { get; set; }
        public string? Description { get; set; }
    }

    public class ExerciseDownload : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public virtual Guid ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }
        public string? Uri { get; set; }
        public byte[] Value { get; set; }
        public string? Description { get; set; }
    }
}
