using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainersWeb.Models
{
    public class SecuritySupervisor
    {
        [Key]
        public Int32 SecuritySupervisorId { get; set; }
        [Required]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 0)]
        //[Index("CardIdIndex", IsUnique = true)]
        public string CardId { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ContainerTracking> ContainerTracking { get; set; }
    }
}