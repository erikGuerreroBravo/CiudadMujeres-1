using CiudadMujeres.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Entities
{
	[Table("Libros")]
	public class Libro : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		
		[Required(ErrorMessage = "El título es obligatorio")]
		[StringLength(maximumLength:250, MinimumLength =3)]
		public string Titulo { get; set; }

		public List<Comentario> Comentarios { get; set; }
		public List<AutorLibro> AutoresLibros { get; set; }

	}
}
