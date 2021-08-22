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
	public class LibroRepository: ILibroRepository, IDisposable
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
        public LibroRepository(ApplicationContext contexto)
        {
            _contexto = contexto;
        }

        #endregion

        #region Consultar resgistros de la entidad
        /// <summary>
        /// Este metodo se encarga de consultar todas las entidades de libro
        /// </summary>
        /// <returns>una tarea o una lista de libros</returns>
        public async Task<List<Libro>> GetLibrosAsync()
        {
            return await _contexto.Libros.Include(x => x.AutoresLibros)
                .ThenInclude(al => al.Autor).ToListAsync();
        }

        /// <summary>
        /// Este metodo se encarga de verificar si existe un libro por id antes de realizar una accion
        /// </summary>
        /// <param name="id">el identificador  de la entidad</param>
        /// <returns>una tarea con un valor boolean</returns>
        public async Task<bool> LibroExistsAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await _contexto.Libros.AnyAsync(c => c.Id == id);
        }

        /// <summary>
        /// Este metodo se encarga de realizar una validacion de los ID de la tabla autores con los id que le envia el endpoint
        /// </summary>
        /// <param name="IdAutores">los id que vienen del enpoint qu probablemente no esten almacenados en la base de datos</param>
        /// <returns>un valor boolean</returns>
        public async Task<bool> ValidarAutoresId(List<int> IdAutores)
        {
            var autores = await _contexto.Autores.Where(a => IdAutores.Contains(a.Id))
                .Select(p => p.Id).ToListAsync();
            if (IdAutores.Count != autores.Count)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Este metodo se encarga de realizar la consulta por identificador de la entidad libro
        /// </summary>
        /// <param name="id">el identificador</param>
        /// <returns>una tarea con la entidad libro</returns>
        public async Task<Libro> GetLibroAsync(int id)
        {
            return await _contexto.Libros.FirstOrDefaultAsync(c => c.Id == id);
        }
        #endregion

        #region Actualizacion de entidad
        /// <summary>
        /// Este metodo se encarga de actualizar una entidad del tipo libro
        /// la actualización se realiza cambiando el estado de la entidad
        /// </summary>
        /// <param name="libro">la entidad libro</param>
        public void UpdateLibro(Libro libro)
        {
            if (libro == null)
            {
                throw new ArgumentNullException(nameof(libro));
            }

            var libr = _contexto.Libros.Where(c => c.Id == libro.Id).FirstOrDefault();
            libr.Id = libro.Id;
            libr.Titulo = libro.Titulo;

            _contexto.SaveChanges();

        }
        #endregion

        #region Insercion de entidad

        /// <summary>
        /// Este metodo se encarga de realizar el agregado temporal de la entidad
        /// </summary>
        /// <param name="libro">entidad persona cargada de datos</param>
        /// <returns>una tarea  </returns>
        public async Task AddLibroAsync(Libro libro)
        {
            if (libro == null)
            {
                throw new ArgumentNullException(nameof(libro));
            }
            await _contexto.AddAsync(libro);
        }

        /// <summary>
        /// Este metodo se encarga de realizar la confirmacion de la insercion
        /// </summary>
        /// <returns>una tarea con un valor booleano</returns>
        public async Task<bool> SaveChangesAsync()
        {
            //regresamos verdadero si 1 o mas entidades  tuvieron un cambio en el contexto.
            return (await _contexto.SaveChangesAsync() > 0);
        }

        #endregion

        #region Eliminacion de registros
        /// <summary>
        /// Este metodo se encarga de eliminar la entidad libro
        /// </summary>
        /// <param name="libro">la entidad libro a eliminar</param>
        public void DeleteLibro(Libro libro)
        {
            if (libro == null)
            {
                throw new ArgumentNullException(nameof(libro));
            }
            _contexto.Libros.Remove(libro);
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
