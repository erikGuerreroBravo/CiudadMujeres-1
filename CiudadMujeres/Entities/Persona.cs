using CiudadMujeres.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Entities
{
    [Table("Personas")]
    public class Persona :  IEntity
    {
        //llave principal de la tabla
        [Key]
        //espcificacion de como se genera la llave
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo apellido paterno es requerido")]
        public string  ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }
    }
}
