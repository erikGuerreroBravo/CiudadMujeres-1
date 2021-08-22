using AutoMapper;
using CiudadMujeres.Entities;
using CiudadMujeres.Models.Autor;
using CiudadMujeres.Models.Libro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Profiles
{
	public class LibroProfile : Profile
	{
		public LibroProfile()
		{
			CreateMap<Libro, Models.Libro.Crear>();
			CreateMap<Models.Libro.Crear, Entities.Libro>()
				.ForMember(l => l.AutoresLibros, opcion => opcion.MapFrom(MapAutoresLibros));
			CreateMap<Libro, Models.Libro.Consultar>()
				.ForMember(a => a.Autores, opcion => opcion.MapFrom(MapLibroAutor));
			CreateMap<Models.Libro.Consultar, Entities.Libro>();
			CreateMap<Libro, Models.Libro.Actualizar>().ReverseMap();
			
		}

		private List<ConsultarAutor> MapLibroAutor(Entities.Libro libro, Models.Libro.Consultar consultar)
        {
			var resultado = new List<ConsultarAutor>();
            if (libro.AutoresLibros == null)
            {
				return resultado;
            }
            foreach (var autorLibro in libro.AutoresLibros)
            {
				resultado.Add(new ConsultarAutor()
				{
					Id = autorLibro.AutorId,
					Nombre = autorLibro.Autor.Nombre
                });
            }
			return resultado;
        }

		private List<AutorLibro> MapAutoresLibros(Models.Libro.Crear crear, Libro libro)
        {
			var resultado = new List<AutorLibro>();
            if (crear.AutoresId == null)
            {
				return resultado;
            }
            foreach ( var autorId in crear.AutoresId)
            {
				resultado.Add(new AutorLibro() { AutorId = autorId });
            }
			return resultado;
        }
	}
}
