using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class ContainerAddViewModel
    {
        [Required]
        public Type Type { get; set; }
        public Int32? CompanyOriginId { get; set; }
        public Int32? CompanyDestinationId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public ContaninerStatus DocStatus { get; set; }
        [Required]
        public string ContainerNumber { get; set; }
    }
}