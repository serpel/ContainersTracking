using System;
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
    public enum TrackingType { Contenedor = 0, Camion = 1, Rastra = 3, Vehiculo = 4, Moto = 5, Courier = 2 }

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
        public string Observations { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsInternalMove { get; set; }
        public bool IsConsolidate { get; set; }

        public virtual Region Gate { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual SecuritySupervisor SecuritySupervisor { get; set; }

        public virtual ICollection<Origin> Origins { get; set; }
        public virtual ICollection<Destination> Destinations { get; set; }

        public ContainerTracking()
        {
            IsInternalMove = false;
            IsConsolidate = false;
        }

        public static explicit operator ContainerTracking(TrackingViewModel v)
        {
            return new ContainerTracking()
            {
                ContainerTrackingId = v.ContainerTrackingId,
                ChasisNumber = v.ChasisNumber,
                ContainerLabel = v.ContainerLabel,
                ContainerNumber = v.ContainerNumber,
                DUA = v.DUA,
                CorrelAduana = v.CorrelAduana,
                DocNumber = v.DocNumber,
                DocStatus = v.DocStatus,
                Type = v.Type,
                InsertedAt = v.InsertedAt,
                UpdatedAt = v.UpdatedAt,
                InsertedBy = v.InsertedBy,
                UpdatedBy = v.UpdatedBy,
                ContainerLicensePlate = v.ContainerLicensePlate,
                ContainerStatus = v.ContainerStatus,
                DriverId = v.DriverId,
                Observations = v.Observations,
                SecuritySupervisorId = v.SecuritySupervisorId,
                TrackingType = v.TrackingType,
                GateId = v.GateId,
                IsInternalMove = v.IsInternalMove,
                IsConsolidate = v.IsConsolidate                     
            };
        }
    }
}