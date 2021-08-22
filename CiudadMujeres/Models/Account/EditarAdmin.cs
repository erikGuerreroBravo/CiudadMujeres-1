using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Models.Account
{
    /// <summary>
    /// Esta clase representa una entidad del tipo crear administrador, su funcionalidad consiste en 
    /// registrar un usuario admnistrador en el sistema
    /// </summary>
    public class EditarAdmin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
