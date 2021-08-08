using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoStudioApp.Models
{
    [Table("StudioSubscription_Tbl")]
    public class StudioSubscription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudioSubscriptionId { get; set; }
        public string subscriptionDate { get; set; }
        public int subscriptionPeriod { get; set; }
        public int subscriptionAmount { get; set; }
    }
}