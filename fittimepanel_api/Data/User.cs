using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FittimePanelApi.Data
{
    public enum Gender : int
    {
        Male,
        Female,
    }

    public class User : IdentityUser
    {
        public User()
        {
            UserMetas = new HashSet<UserMeta>();
            UserBlobs = new HashSet<UserBlob>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { 
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public virtual ICollection<UserMeta> UserMetas { get; set; }
        public virtual ICollection<UserBlob> UserBlobs { get; set; }
        [PersonalData]
        public DateTime LastLoginTime { get; set; }
        [PersonalData]
        public DateTime RegistrationDate { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public List<Exercise> Exercises { get; set; }
        public List<Ticket> Tickets { get; set; }

    }

    public class UserMeta : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }
    }

    public class UserBlob : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public byte[] Value { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }
    }

}
