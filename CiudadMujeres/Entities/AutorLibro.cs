using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Entities
{
    [Table("AutoresLibros")]
    public class AutorLibro
    {
        public int LibroId { get; set; }
        public int AutorId { get; set; }
        public int Orden { get; set; }        public Libro Libro { get; set; }        public Autor Autor { get; set; }    }
}
