using CiudadMujeres.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Infraestructure
{
	/// <summary>
	/// Esta interfaz permite llevar a cabo los procesos de LibroRepository
	/// </summary>
	public interface ILibroRepository
	{
		/// <summary>
		/// Este metodo se encarga de consultar todas las entidades de libro
		/// </summary>
		/// <returns>una tarea o una lista de libros</returns>
		Task<List<Libro>> GetLibrosAsync();

		/// <summary>
		/// Este metodo se encarga de verificar si existe un libro por id antes de realizar una accion
		/// </summary>
		/// <param name="id">el identificador  de la entidad</param>
		/// <returns>una tarea con un valor boolean</returns>
		Task<bool> LibroExistsAsync(int id);

		/// <summary>
		/// Este metodo se encarga de realizar la consulta por identificador de la entidad libro
		/// </summary>
		/// <param name="id">el identificador</param>
		/// <returns>una tarea con la entidad libro</returns>
		Task<Libro> GetLibroAsync(int id);

		/// <summary>
		/// Este metodo se encarga de realizar una validacion de los ID de la tabla autores con los id que le envia el endpoint
		/// </summary>
		/// <param name="IdAutores">los id que vienen del enpoint qu probablemente no esten almacenados en la base de datos</param>
		/// <returns>un valor boolean</returns>
		Task<bool> ValidarAutoresId(List<int> IdAutores);

		/// <summary>
		/// Este metodo se encarga de actualizar una entidad del tipo libro
		/// la actualización se realiza cambiando el estado de la entidad
		/// </summary>
		/// <param name="libro">la entidad libro</param>
		void UpdateLibro(Libro libro);

		/// <summary>
		/// Este metodo se encarga de realizar el agregado temporal de la entidad
		/// </summary>
		/// <param name="libro">entidad persona cargada de datos</param>
		/// <returns>una tarea  </returns>
		Task AddLibroAsync(Libro libro);

		/// <summary>
		/// Este metodo se encarga de realizar la confirmacion de la insercion
		/// </summary>
		/// <returns>una tarea con un valor booleano</returns>
		Task<bool> SaveChangesAsync();

		/// <summary>
		/// Este metodo se encarga de eliminar la entidad libro
		/// </summary>
		/// <param name="libro">la entidad libro a eliminar</param>
		void DeleteLibro(Libro libro);
	}
}
