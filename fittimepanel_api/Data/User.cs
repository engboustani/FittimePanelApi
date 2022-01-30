using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FittimePanelApi.Data
{
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
    }

    public class UserMeta : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual User User { get; set; }
    }

    public class UserBlob : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public byte[] Value { get; set; }
        public virtual User User { get; set; }
    }

}
