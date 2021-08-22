using AutoMapper;
using CiudadMujeres.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Profiles
{
	public class ComentarioProfile : Profile
	{
		public ComentarioProfile()
		{
			CreateMap<Comentario, Models.Comentario.Consultar>().ReverseMap();
			CreateMap<Comentario, Models.Comentario.Crear>().ReverseMap();
			CreateMap<Comentario, Models.Comentario.Actualizar>().ReverseMap();
		}
	}
}
