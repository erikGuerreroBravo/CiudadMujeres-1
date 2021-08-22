using CiudadMujeres.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Entities
{
	[Table("Comentarios")]
	public class Comentario : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required(ErrorMessage = "El campo Contenido es obligatorio")]
		public string Contenido { get; set; }

		[ForeignKey("LibroId")]
		public int LibroId { get; set; }
		public Libro Libro { get; set; }
	}
}
