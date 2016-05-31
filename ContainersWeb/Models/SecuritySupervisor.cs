using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContainersWeb.Models
{
    public class SecuritySupervisor
    {
        [Key]
        public Int32 SecuritySupervisorId { get; set; }
        [Required]
        public string Name { get; set; }
        public string CardId { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ContainerTracking> ContainerTracking { get; set; }
    }
}