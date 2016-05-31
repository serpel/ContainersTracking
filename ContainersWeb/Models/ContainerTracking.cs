using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public enum Type { In = 1, Out = 0 }
    public enum ContaninerStatus { Full = 1, Empty = 0 }

    public class ContainerTracking
    {
        [Key]
        public Int32 TrackingId { get; set; }
        [Required]
        public Type Type { get; set; }
        public Int32? CompanyOriginId { get; set; }
        public Int32? CompanyDestinationId { get; set; }
        [Required]
        public ContaninerStatus DocStatus { get; set; }
        [Required]
        public string ContainerNumber { get; set; }
        public string ContainerLicensePlate { get; set; }
        public string ContainerLabel { get; set; }
        public string ChasisNumber { get; set; }
        public string DuaNumber { get; set; }
        //public string NoCabezal { get; set; }
        public Int32 DriverId { get; set; }
        public Int32 SecuritySupervisorId { get; set; }

        public Int32 InsertedAt { get; set; }
        public Int32 UpdatedAt { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual SecuritySupervisor SecuritySupervisor { get; set;}
        [ForeignKey("CompanyOriginId"), Column(Order = 0)]
        public virtual Company CompanyOrigin { get; set; }
        [ForeignKey("CompanyDestinationId"), Column(Order = 1)]
        public virtual Company CompanyDestination { get; set; }
    }
}