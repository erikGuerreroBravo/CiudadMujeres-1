using CiudadMujeres.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Entities
{
    [Table("Autores")]
    public class Autor:IEntity
    {
        //llave principal de la tabla
        [Key]
        //especificacion de como se genera la llave
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo de Nombre es requerido")]
        [StringLength(maximumLength:120,ErrorMessage ="El campo {0} no debe de tener mas de {1} caracteres")]
        public string Nombre { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }
    }
}
