using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoStudioApp.Models
{
    [Table("Order_Photo")]
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderId { get; set;}
        public int customerId { get; set; }
        public int OrderNo { get; set; }
        public string OrderDetails { get; set; }
        public string orderDate { get; set; }
        public int totalCost { get; set; }
        public int studioId { get; set; }
    }

    public enum orderSequence
    {
        Index,
        OrderNo,
        OrderDetails,
        orderDate,
        totalCost
    }

    public class OrderViewModel
    {
        public int orderId { get; set; }
        public int customerId { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public string customerName { get; set; }
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mandetory Field")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Contact { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public string emailId { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public int OrderNo { get; set; }
        public string OrderDetails { get; set; }
        public string orderDate { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public int totalCost { get; set; }
        public int studioId { get; set; }
    }
}