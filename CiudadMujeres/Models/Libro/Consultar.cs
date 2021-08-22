using CiudadMujeres.Models.Autor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Models.Libro
{
	public class Consultar : IEntity
	{
		public int Id { get; set; }
		public string Titulo { get; set; }
		public List<ConsultarAutor> Autores { get; set; }
	}
}
