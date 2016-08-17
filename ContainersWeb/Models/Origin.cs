using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class Origin
    {
        [Key]
        public Int32 OriginId { get; set; }
        public Int32 ContainerTrackingId { get; set; }
        public Int32 CompanyOriginId { get; set; }

        public virtual ContainerTracking ContainerTracking { get; set; }
        [ForeignKey("CompanyOriginId"), Column(Order = 0)]
        public virtual Company CompanyOrigin { get; set; }
    }
}