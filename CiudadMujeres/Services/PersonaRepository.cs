using CiudadMujeres.Data;
using CiudadMujeres.Entities;
using CiudadMujeres.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Services
{
    public class PersonaRepository : IPersonaRepository, IDisposable
    {
        #region Variables Globales
        //esta variable almacena los valores principales dle contexto
        private ApplicationContext _contexto;
        #endregion

        #region Constructor
        /// <summary>
        /// este es el contructor d ela clase y se inyecta la entidad 
        /// </summary>
        /// <param name="contexto">la entidad o clase que se encarga de la conexion a la base de datos</param>
        public PersonaRepository(ApplicationContext contexto)
        {
            _contexto = contexto;
        }

        #endregion

        #region Consultar resgistros de la entidad
        /// <summary>
        /// Este metodo se encarga de consultar todas las entidades de persona
        /// </summary>
        /// <returns>una tarea o una lista de personas</returns>
        public async Task<List<Persona>> GetPersonasAsync()
        {
            return await _contexto.Personas.ToListAsync();
        }

        /// <summary>
        /// Este metodo se encarga de consultar a una entidad del tipo persona por id
        /// </summary>
        /// <param name="id">el identificador de la entidad persona</param>
        /// <returns>regresa una entidad del tipo persona</returns>
        public async Task<Persona> GetPersonaAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await _contexto.Personas.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Este metodo se encarga de verificar si existe una persona por id antes de realizar una accion
        /// </summary>
        /// <param name="id">el identificador  de la entidad</param>
        /// <returns>una tarea con un valor boolean</returns>
        public async Task<bool> PersonaExistsAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await _contexto.Personas.AnyAsync(c => c.Id == id);
        }

       
        /// <summary>
        /// Este metodo se encarga de realizar una busqueda ascendente por nombre de la entidad persona
        /// </summary>
        /// <param name="Nombre">el nombre de la entidad persona</param>
        /// <returns>una lista o coleccion de personas, tarea que contiene una lista de personas</returns>
        public async Task<List<Persona>> GetPersonasAsync_Asc(string Nombre)
        {
            return await _contexto.Personas.OrderBy(c => c.Nombre).ToListAsync();
        }
        /// <summary>
        /// Este metodo se encarga de realizar una busqueda descendente por nombre de la entidad persona
        /// </summary>
        /// <param name="Nombre">el nombre de la entidad persona</param>
        /// <returns>una lista o coleccion de personas, tarea que contiene una lista de personas</returns>
        public async Task<List<Persona>> GetPersonasAsync_Desc(string Nombre)
        {
            return await _contexto.Personas.OrderByDescending(c => c.Nombre).ToListAsync();
        }

        /// <summary>
        /// Este metodo se encarga de realizar la consulta por identificador de la entidad persona
        /// </summary>
        /// <param name="id">el identificador</param>
        /// <returns>una tarea con la entidad persona</returns>
        public async Task<Persona> GetPersonasAsync(int id)
        {
            return await _contexto.Personas.FirstOrDefaultAsync(c => c.Id == id);
        }
        #endregion

        #region Insercion de entidad

        /// <summary>
        /// Este metodo se encarga de realizar el agregado temporal de la entidad
        /// </summary>
        /// <param name="persona">entidad persona cargada de datos</param>
        /// <returns>una tarea  </returns>
        public async Task AddPersonaAsync(Persona persona)
        {
            if (persona == null)
            {
                throw new ArgumentNullException(nameof(persona));
            }
            await _contexto.AddAsync(persona);
        }
        /// <summary>
        /// Este metodo se encarga de realizar el agregado temporal de la entidad
        /// </summary>
        /// <param name="persona">entidad persona cargada de datos</param>
        public void AddPersona(Persona persona)
        {
            if (persona == null)
            {
                throw new ArgumentNullException(nameof(persona));
            }
            _contexto.Add(persona);
        }

        /// <summary>
        /// Este metodo se encarga de actualizar una entidad del tipo persona
        /// la actualización se realiza cambiando el estado de la entidad
        /// </summary>
        /// <param name="persona">la entidad persona</param>
        public void UpdatePersona(Persona persona)
        {
            if (persona == null)
            {
                throw new ArgumentNullException(nameof(persona));
            }

            var person = _contexto.Personas.Where(c => c.Id == persona.Id).FirstOrDefault();
            person.Id = persona.Id;
            person.Nombre = persona.Nombre;
            person.ApellidoPaterno = persona.ApellidoPaterno;
            person.ApellidoMaterno = persona.ApellidoMaterno;
            _contexto.SaveChangesAsync();
            
        }

        /// <summary>
        /// Este metodo se encarga de eliminar la entidad persona
        /// </summary>
        /// <param name="persona">la entidad persona a eliminar</param>
        public void DeletePersona(Persona persona)
        {
            if (persona == null)
            {
                throw new ArgumentNullException(nameof(persona));
            }
            _contexto.Personas.Remove(persona);
        }

        /// <summary>
        /// Este metodo se encarga de realizar la confirmacion de la insercion
        /// </summary>
        /// <returns>una tarea con un valor booleano</returns>
        public async Task<bool> SaveChangesAsync()
        {
            //regresamos verdadero si 1 o mas entidades  tuvieron un cambio en el contexto.
            return (await _contexto.SaveChangesAsync()>0);
        }

    #endregion

        #region Eliminacion de registros
    public void DeletePersona(int id)
        {
            var persona = _contexto.Personas.Where(p => p.Id == id).FirstOrDefault();
            _contexto.Remove(persona);
        }
        #endregion

        #region  Dipose y Liberacion de recursos
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_contexto != null)
                {
                    _contexto.Dispose();
                    _contexto = null;
                }
            }
        }

        #endregion
    }
}
