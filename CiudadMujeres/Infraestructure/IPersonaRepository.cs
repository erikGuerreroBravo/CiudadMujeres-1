using CiudadMujeres.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Infraestructure
{
    public interface IPersonaRepository
    {
        /// <summary>
        /// Este metodo se encarga de consultar todas las entidades de persona
        /// </summary>
        /// <returns>una tarea o una lista de personas</returns>
        Task<List<Persona>> GetPersonasAsync();

        /// <summary>
        /// Este metodo se encarga de consultar a una entidad del tipo persona por id
        /// </summary>
        /// <param name="id">el identificador de la entidad persona</param>
        /// <returns>regresa una entidad del tipo persona</returns>
        Task<Persona> GetPersonaAsync(int id);

        /// <summary>
        /// Este metodo se encarga de verificar si existe una persona por id antes de realizar una accion
        /// </summary>
        /// <param name="id">el identificador  de la entidad</param>
        /// <returns>una tarea con un valor boolean</returns>
        Task<bool> PersonaExistsAsync(int id);



        /// <summary>
        /// Este metodo se encarga de realizar el agregado temporal de la entidad
        /// </summary>
        /// <param name="persona">entidad persona cargada de datos</param>
        /// <returns>una tarea  </returns>
        void AddPersona(Persona persona);


        /// <summary>
        /// Este metodo se encarga de actualizar una entidad del tipo persona
        /// la actualización se realiza cambiando el estado de la entidad
        /// </summary>
        /// <param name="persona">la entidad persona</param>
        void UpdatePersona(Persona persona);

        /// <summary>
        /// Este metodo se encarga de eliminar la entidad persona
        /// </summary>
        /// <param name="persona">la entidad persona a eliminar</param>
        void DeletePersona(Persona persona);

        /// <summary>
        /// Este metodo se encarga de realizar la confirmacion de la insercion
        /// </summary>
        /// <returns>una tarea con un valor booleano</returns>
        Task<bool> SaveChangesAsync();
    }
}
