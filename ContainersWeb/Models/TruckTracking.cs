using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class TruckTracking
    {
        [Key]
        public Int32 TruckTrackingTrackingId { get; set; }
        [Required]
        public Type Type { get; set; }
        public Int32? CompanyOriginId { get; set; }
        public Int32? CompanyDestinationId { get; set; }
        [Required]
        public ContaninerStatus ContainerStatus { get; set; }
        [Required]
        public DocStatus DocStatus { get; set; }
        [Required]
        public string ContainerNumber { get; set; }
        public string ContainerLicensePlate { get; set; }
        public string ContainerLabel { get; set; }

        public string DocNumber { get; set; }
        public string CorrelAduana { get; set; }

        public Int32? DriverId { get; set; }
        public Int32? SecuritySupervisorId { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual SecuritySupervisor SecuritySupervisor { get; set; }
        [ForeignKey("CompanyOriginId"), Column(Order = 0)]
        public virtual Company CompanyOrigin { get; set; }
        [ForeignKey("CompanyDestinationId"), Column(Order = 1)]
        public virtual Company CompanyDestination { get; set; }
    }
}