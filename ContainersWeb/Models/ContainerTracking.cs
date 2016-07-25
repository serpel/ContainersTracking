﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public enum Type { Entrada = 1, Salida = 0 }
    public enum ContaninerStatus { Lleno = 1, Vacio = 0 }
    public enum DocStatus { Listo = 1, Pendiente = 0 }
    public enum TrackingType { Contenedor = 0, Camion = 1, Rastra = 3 }

    public class ContainerTracking
    {
        [Key]
        public Int32 ContainerTrackingId { get; set; }

        public TrackingType TrackingType { get; set; }
        [Required]
        public Type Type { get; set; }
        public Int32? CompanyOriginId { get; set; }
        public Int32? CompanyDestinationId { get; set; }
        public Int32? GateId { get; set; }
        [Required]
        public ContaninerStatus ContainerStatus { get; set; }
        [Required]
        public DocStatus DocStatus { get; set; }
        [Required]
        public string ContainerNumber { get; set; }
        public string ContainerLicensePlate { get; set; }
        public string ContainerLabel { get; set; }
        public string ChasisNumber { get; set; }
        //es el numero de DUA
        public string DUA { get; set; }
        public string DocNumber { get; set; }
        public string CorrelAduana { get; set; }

        //public string NoCabezal { get; set; }
        public Int32? DriverId { get; set; }
        public Int32? SecuritySupervisorId { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Region Gate { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual SecuritySupervisor SecuritySupervisor { get; set; }
        [ForeignKey("CompanyOriginId"), Column(Order = 0)]
        public virtual Company CompanyOrigin { get; set; }
        [ForeignKey("CompanyDestinationId"), Column(Order = 1)]
        public virtual Company CompanyDestination { get; set; }
    }
}