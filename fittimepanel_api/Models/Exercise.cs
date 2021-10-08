using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FittimePanelApi.Models
{
    public class Exercise : BaseEntity
    {
        public Exercise()
        {
            ExerciseMetas = new HashSet<ExerciseMeta>();
            ExerciseDownloads = new HashSet<ExerciseDownload>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public virtual User UserStudent { get; set; }
        public virtual User UserInstructor { get; set; }
        public virtual ExerciseType ExerciseType { get; set; }
        public virtual ICollection<ExerciseMeta> ExerciseMetas { get; set; }
        public virtual ExerciseStatus ExerciseStatus { get; set; }
        public virtual ICollection<ExerciseDownload> ExerciseDownloads { get; set; }
    }

    public class ExerciseType : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
    public class ExerciseMeta : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual User User { get; set; }
        public virtual Exercise Exercise { get; set; }
    }

    public class ExerciseStatus : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ExerciseDownload : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public virtual Exercise Exercise { get; set; }
        public string Uri { get; set; }
    }
}
