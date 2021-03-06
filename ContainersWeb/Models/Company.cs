﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class Company
    {
        [Key]
        public Int32 CompanyId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "StringExceeded")]
        [StringLength(150, MinimumLength = 1)]
        public string Name { get; set; }
        public int Code { get; set; }
        public Int32 RegionId { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<ContainerTracking> CompanyOrigin { get; set; }
        public virtual ICollection<ContainerTracking> CompanyDestination { get; set; }

    }
}