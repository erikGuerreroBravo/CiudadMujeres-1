using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiudadMujeres.Data;
using CiudadMujeres.Entities;
using CiudadMujeres.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace CiudadMujeres.Services
{
	public class ComentarioRepository : IComentarioRepository, IDisposable
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
        public ComentarioRepository(ApplicationContext contexto)
        {
            _contexto = contexto;
        }

        #endregion

        #region Consultar resgistros de la entidad
        /// <summary>
        /// Este metodo se encarga de consultar todas las entidades de comentario
        /// </summary>
        /// <returns>una tarea o una lista de comentarios</returns>
        public async Task<List<Comentario>> GetComentariosAsync()
        {
            return await _contexto.Comentarios.ToListAsync();
        }

        /// <summary>
        /// Este metodo se encarga de verificar si existe un comentario por id antes de realizar una accion
        /// </summary>
        /// <param name="id">el identificador de la entidad</param>
        /// <returns>una tarea con un valor boolean</returns>
        public async Task<bool> ComentarioExistsAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await _contexto.Comentarios.AnyAsync(c => c.Id == id);
        }

        /// <summary>
        /// Este metodo se encarga de realizar la consulta por identificador de la entidad comentario
        /// </summary>
        /// <param name="id">el identificador</param>
        /// <returns>una tarea con la entidad comentario</returns>
        public async Task<Comentario> GetComentarioAsync(int id)
        {
            return await _contexto.Comentarios.FirstOrDefaultAsync(c => c.Id == id);
        }
        #endregion

        #region Actualizacion de entidad
        /// <summary>
        /// Este metodo se encarga de actualizar una entidad del tipo comentario
        /// la actualización se realiza cambiando el estado de la entidad
        /// </summary>
        /// <param name="comentario">la entidad comentario</param>
        public void UpdateLibro(Comentario comentario)
        {
            if (comentario == null)
            {
                throw new ArgumentNullException(nameof(comentario));
            }

            var com = _contexto.Comentarios.Where(c => c.Id == comentario.Id).FirstOrDefault();
            com.Id = comentario.Id;
            com.Contenido = comentario.Contenido;
            com.LibroId = comentario.LibroId;

            _contexto.SaveChanges();

        }
        #endregion

        #region Insercion de entidad

        /// <summary>
        /// Este metodo se encarga de realizar el agregado temporal de la entidad
        /// </summary>
        /// <param name="comentario">entidad comentario cargada de datos</param>
        /// <returns>una tarea  </returns>
        public async Task AddComentarioAsync(Comentario comentario)
        {
            if (comentario == null)
            {
                throw new ArgumentNullException(nameof(comentario));
            }
            await _contexto.AddAsync(comentario);
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
        /// Este metodo se encarga de eliminar la entidad comentario
        /// </summary>
        /// <param name="comentario">la entidad comentario a eliminar</param>
        public void DeleteLibro(Comentario comentario)
        {
            if (comentario == null)
            {
                throw new ArgumentNullException(nameof(comentario));
            }
            _contexto.Comentarios.Remove(comentario);
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
