using CiudadMujeres.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Infraestructure
{
	public interface IComentarioRepository
	{
		/// <summary>
		/// Este metodo se encarga de consultar todas las entidades de comentario
		/// </summary>
		/// <returns>una tarea o una lista de comentarios</returns>
		Task<List<Comentario>> GetComentariosAsync();

		/// <summary>
		/// Este metodo se encarga de verificar si existe un comentario por id antes de realizar una accion
		/// </summary>
		/// <param name="id">el identificador de la entidad</param>
		/// <returns>una tarea con un valor boolean</returns>
		Task<bool> ComentarioExistsAsync(int id);

		/// <summary>
		/// Este metodo se encarga de realizar la consulta por identificador de la entidad comentario
		/// </summary>
		/// <param name="id">el identificador</param>
		/// <returns>una tarea con la entidad comentario</returns>
		Task<Comentario> GetComentarioAsync(int id);

		/// <summary>
		/// Este metodo se encarga de actualizar una entidad del tipo comentario
		/// la actualización se realiza cambiando el estado de la entidad
		/// </summary>
		/// <param name="comentario">la entidad comentario</param>
		void UpdateLibro(Comentario comentario);

		/// <summary>
		/// Este metodo se encarga de realizar el agregado temporal de la entidad
		/// </summary>
		/// <param name="comentario">entidad comentario cargada de datos</param>
		/// <returns>una tarea  </returns>
		Task AddComentarioAsync(Comentario comentario);

		/// <summary>
		/// Este metodo se encarga de realizar la confirmacion de la insercion
		/// </summary>
		/// <returns>una tarea con un valor booleano</returns>
		Task<bool> SaveChangesAsync();

		/// <summary>
		/// Este metodo se encarga de eliminar la entidad comentario
		/// </summary>
		/// <param name="comentario">la entidad comentario a eliminar</param>
		void DeleteLibro(Comentario comentario);


	}
}
