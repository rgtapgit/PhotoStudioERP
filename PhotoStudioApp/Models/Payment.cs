using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoStudioApp.Models
{
    [Table("Payment_Photo")]
    public class Payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int paymentId { get; set; }
        public int orderId { get; set; }
        public string paymentDetail { get; set; }
        public int downPayment { get; set; }
        public int balanceAmount { get; set; }
        public string payMode { get; set; }
        public string paymentDate { get; set; }
        public string receiptNo { get; set; }
        public int studioId { get; set; }
    }
    public class PaymentViewModel
    {
        public int paymentId { get; set; }
        public int orderId { get; set; }
        public int customerId { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public int OrderNo { get; set; }
        //public string OrderDetails { get; set; }
        //public string orderDate { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public int totalCost { get; set; }
        public string paymentDetail { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public int downPayment { get; set; }
        public int balanceAmount { get; set; }
        public string payMode { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public string paymentDate { get; set; }
        [Required(ErrorMessage = "Mandetory Field")]
        public string receiptNo { get; set; }
        public int studioId { get; set; }
    }
}
