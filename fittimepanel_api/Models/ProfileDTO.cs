using FittimePanelApi.Repository;
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
        public string Description { get; set; }
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
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class UserProfileDetailDTO
    {
        public UserProfileDetailDTO()
        {
            this.UserMetas = new HashSet<UserMetaDTO>();
            this.Exercises = new HashSet<ExerciseListItemDTO>();
            this.Tickets = new HashSet<TicketListItemDTO>();
        }
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserMetaDTO> UserMetas { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public virtual ICollection<ExerciseListItemDTO> Exercises { get; set; }
        public virtual ICollection<TicketListItemDTO> Tickets { get; set; }
    }

    public class UserShortProfileDTO
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UserShortListItemDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
    }

    public class UserListItemDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }


    public class UserPageItemDTO
    {
        public PageInfo PageInfo { get; set; }
        public IList<UserListItemDTO> ItemsList { get; set; }
    }

}
