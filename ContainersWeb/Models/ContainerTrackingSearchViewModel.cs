using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class ContainerTrackingSearchViewModel
    {
        public Int32 ContainerTrackingId { get; set; }
        public string TrackingType { get; set; }
        public string Type { get; set; }
        public string ContainerStatus { get; set; }
        public string DocStatus { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerLicensePlate { get; set; }
        public string InsertedAt { get; set; }
        public string UpdatedAt { get; set; } 
    }
}