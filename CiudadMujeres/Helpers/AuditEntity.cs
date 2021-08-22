using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Helpers
{
    public class AuditEntity
    {
        public DateTime CreateAt { get; set; }
        public string CreatedBy { get; set; }

        [ForeignKey("CreadoPor")]
        public IdentityUser CreatedUser { get; set; }

        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        [ForeignKey("ActualizadoPor")]
        public IdentityUser UpdatedUser { get; set; }

        public DateTime DeleteAt { get; set; }
        public string DeletedAt { get; set; }
        [ForeignKey("BorradoPor")]
        public IdentityUser DeletedUser { get; set; }
    }
}
