using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class Driver
    {
        [Key]
        public Int32 DriverId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CardId { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ContainerTracking> ContainerTracking { get; set; }
    }
}