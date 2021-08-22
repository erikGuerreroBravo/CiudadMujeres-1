using AutoMapper;
using CiudadMujeres.Common;
using CiudadMujeres.Filters;
using CiudadMujeres.Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Controllers
{
    
    [ApiController]
    public class ComentarioController : ControllerBase
    {

        #region Variables
        private readonly ILogger<ComentarioController> _logger;
        private readonly IMapper _mapper;
        private readonly IComentarioRepository _repository;
        private readonly ILibroRepository _repositoryLibro;
        #endregion

        #region
         public ComentarioController(IMapper mapper, ILogger<ComentarioController> logger, 
             IComentarioRepository repository, ILibroRepository repositoryLibro)
        {
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _repositoryLibro = repositoryLibro ?? throw new ArgumentNullException(nameof(repositoryLibro));
        }

        #endregion

        #region Insertar Post
        /// <summary>
        /// Este endPoint se encarga de insertar un comentario relacionado a un libro 
        /// </summary>
        /// <param name="libroId"> el identificador del libro</param>
        /// <param name="crear">la entidad comentario a insertar</param>
        /// <returns>una tarea Task con un Resultado</returns>
        [HttpPost]
        [Route(RouteEntity.ComentarioInsert)]
        [ServiceFilter(typeof(ValidacionFilterAttribute))]//este filtro se encarga de validar la entidad solo se aplica en insercion, eliminacion,actualizacion

        public async Task<IActionResult> Post(int libroId, Models.Comentario.Crear crear)
        {
            // 1.- validar que el idn de la identidad exista
            var libro = await _repositoryLibro.LibroExistsAsync(libroId);
            if(!libro)
            {
                return NotFound();
            }
            var comentario = _mapper.Map<Entities.Comentario>(crear);
            comentario.LibroId = libroId; //establecemos el identificador del libro a insertar
            await _repository.AddComentarioAsync(comentario);
            await _repository.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}
