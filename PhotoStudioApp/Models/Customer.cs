using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoStudioApp.Models
{
    [Table("Customer_Photo")]
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int customerId { get; set; }
        [Required(ErrorMessage ="Required")]
        public string customerName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Contact { get; set; }
        public string Address { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string emailId { get; set; }
        public string ImageUrl { get; set; }
        public int studioId { get; set; }
    }
}