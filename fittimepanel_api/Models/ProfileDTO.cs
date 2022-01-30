using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models
{
    public class UserBlobDTO
    {
        [Required]
        public string Key { get; set; }
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; }
    }

    public class UserMetaDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class UserBlobResponseDTO
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string File { get; set; }
    }

    public class UserProfileDTO
    {
        public UserProfileDTO()
        {
            this.UserMetas = new HashSet<UserMetaDTO>();
        }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserMetaDTO> UserMetas { get; set; }
    }
}
