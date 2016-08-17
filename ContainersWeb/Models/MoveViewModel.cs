using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class MoveViewModel
    {
        [Required]
        public string Number { get; set; }
        public Int32? CompanyOriginId { get; set; }
        public Int32? CompanyDestinationId { get; set; }
        public Int32? GateIn { get; set; }
        public Int32? GateOut { get; set; }
        public bool IsInternalMove { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string User { get; set; }
    }
}