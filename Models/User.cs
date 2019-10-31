using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required]
        [Display(Name = "First Name")]
        [MinLength(2, ErrorMessage="Dawg, that's a letter not a name.")]
        public string FirstName {get;set;}

        [Required]
        [Display(Name = "Last Name")]
        [MinLength(2, ErrorMessage="Dawg, that's a letter not a name.")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        // Will not be mapped to your users table!
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Yo! That didn't match!")]
        public string Confirm {get;set;}

        public List<Relation> Weddings { get; set; }
    }

    public class LogUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password { get; set; }
    }
}