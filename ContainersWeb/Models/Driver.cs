using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class Driver
    {
        [Key]
        public Int32 DriverId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "StringExceeded")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "StringExceeded")]
        [Index("DriverCardIdIndex", IsUnique = true)]
        public string CardId { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ContainerTracking> ContainerTracking { get; set; }
    }
}