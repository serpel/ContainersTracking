using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class Destination
    {
        [Key]
        public Int32 DestinationId { get; set; }
        public Int32 ContainerTrackingId { get; set; }
        public Int32 CompanyDestinationId { get; set; }

        public virtual ContainerTracking ContainerTracking { get; set; }
        [ForeignKey("CompanyDestinationId"), Column(Order = 0)]
        public virtual Company CompanyDestination { get; set; }
    }
}