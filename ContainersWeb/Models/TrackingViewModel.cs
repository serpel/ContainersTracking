using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainersWeb.Models
{
    public class TrackingViewModel
    {
        public Int32 ContainerTrackingId { get; set; }
        public TrackingType TrackingType { get; set; }
        public Type Type { get; set; }
        public ContaninerStatus ContainerStatus { get; set; }
        public DocStatus DocStatus { get; set; }
        public Int32? GateId { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerLicensePlate { get; set; }
        public string ContainerLabel { get; set; }
        public string ChasisNumber { get; set; }
        public string DUA { get; set; }
        public string DocNumber { get; set; }
        public string CorrelAduana { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Observations { get; set; }
        public bool IsInternalMove { get; set; }
        public bool IsConsolidate { get; set; }

        public Int32? SecuritySupervisorId { get; set; }
        public Int32? DriverId { get; set; }
        public IEnumerable<int> CompanyOriginId { get; set; }
        public IEnumerable<int> CompanyDestinationId { get; set; }


        public MultiSelectList Origins { get; set; }
        public MultiSelectList Destinations { get; set; }

        public static explicit operator TrackingViewModel(ContainerTracking v)
        {
            return new TrackingViewModel()
            {
                ChasisNumber = v.ChasisNumber,
                TrackingType = v.TrackingType,
                ContainerLabel = v.ContainerLabel, 
                DUA = v.DUA,
                DocNumber = v.DocNumber,
                DocStatus = v.DocStatus,
                CorrelAduana = v.CorrelAduana,
                ContainerNumber = v.ContainerNumber,
                InsertedAt = v.InsertedAt,
                InsertedBy = v.InsertedBy,
                UpdatedAt = v.UpdatedAt,
                UpdatedBy = v.UpdatedBy,
                ContainerStatus = v.ContainerStatus,
                Type = v.Type,
                Observations = v.Observations,
                SecuritySupervisorId = v.SecuritySupervisorId,
                ContainerLicensePlate = v.ContainerLicensePlate,
                ContainerTrackingId = v.ContainerTrackingId,
                GateId = v.GateId,
                DriverId = v.DriverId,
                IsInternalMove = v.IsInternalMove,
                IsConsolidate = v.IsConsolidate           
            };
        }
    }
}