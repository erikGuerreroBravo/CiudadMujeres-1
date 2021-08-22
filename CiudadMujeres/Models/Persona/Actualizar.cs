using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Models.Persona
{
    public class Actualizar : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo apellido paterno es requerido")]
        public string ApellidoPaterno { get; set; }
        
        public string ApellidoMaterno { get; set; }
    }
}
