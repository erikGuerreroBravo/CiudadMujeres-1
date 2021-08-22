using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Models.Libro
{
	public class Crear : IEntity
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El título es obligatorio")]
		[StringLength(maximumLength: 250, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 250 caracteres")]
		public string Titulo { get; set; }
		public List<int> AutoresId { get; set; }
		
	}
}
