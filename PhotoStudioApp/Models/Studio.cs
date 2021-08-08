using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoStudioApp.Models
{
    [Table("Studio_Tbl")]
    public class Studio
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int studioId { get; set; }
        [Required(ErrorMessage ="Required")]
        public string studioName { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }
        public string ProfilePhoto { get; set; }
    }
}