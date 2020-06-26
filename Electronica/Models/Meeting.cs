using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Meeting
    {
        [Key]
        public int meeting_Id { get; set; }

        [ForeignKey("contact")]
        public int ClientId { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public virtual Device Device { get; set; }


    }
}