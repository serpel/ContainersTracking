using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContainersWeb.Models
{
    public class Region
    {
        [Key]
        public Int32 RegionId { get; set; }
        [Required]
        public string Name { get; set; }  

        public virtual ICollection<Company> Companies { get; set; }
    }
}