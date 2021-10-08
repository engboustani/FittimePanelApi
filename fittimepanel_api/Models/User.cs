using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fittimepanel_api.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            UserMetas = new HashSet<UserMeta>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public virtual ICollection<UserMeta> UserMetas { get; set; }
        public virtual UserGroup UserGroup { get; set; }
        //public bool MobileVerified { get; set; }
    }

    public class UserGroup : BaseEntity
    {
        public UserGroup()
        {
            Users = new HashSet<User>();
            UserRules = new HashSet<UserRule>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserRule> UserRules { get; set; }
    }

    public class UserRule : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }

    public class UserMeta : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual User User { get; set; }
    }

    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserRegister
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
    }

}
