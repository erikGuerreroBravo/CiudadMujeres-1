using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Models.Comentario
{
	public class Actualizar : IEntity
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El campo Contenido es obligatorio")]
		public string Contenido { get; set; }

	}
}
