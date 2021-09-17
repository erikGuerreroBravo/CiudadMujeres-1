using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Entities
{
    [Table("catEstadosCivil")]
    public class EstadoCivil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {1} y {2} caracteres")]
        public string StrValor { get; set; }
    }
}
