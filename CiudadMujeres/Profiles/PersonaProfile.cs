using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Profiles
{
    public class PersonaProfile :Profile
    {
        public PersonaProfile()
        {
            CreateMap<Entities.Persona, Models.Persona.Crear>().ReverseMap();
            CreateMap<Entities.Persona, Models.Persona.Consultar>().ReverseMap();
            CreateMap<Entities.Persona, Models.Persona.Actualizar>().ReverseMap();
        }
    }
}
