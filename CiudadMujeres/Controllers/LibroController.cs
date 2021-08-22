using AutoMapper;
using CiudadMujeres.Common;
using CiudadMujeres.Filters;
using CiudadMujeres.Infraestructure;
using CiudadMujeres.Models.Libro;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Controllers
{
	//[Route("api/[controller]")]
	[ApiController]
	public class LibroController : ControllerBase
	{
		private readonly ILogger<LibroController> _logger;
		private readonly IMapper _mapper;
		private readonly ILibroRepository _libroRepository;

		public LibroController(IMapper mapper, ILibroRepository libroRepository,
			ILogger<LibroController> logger)
		{
			_logger = logger;
			_libroRepository = libroRepository ?? throw new ArgumentNullException(nameof(libroRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		#region Consultar EndPoint
		/// <summary>
		/// Este metodo se encarga de consultar todos los libros dentro del sistema
		/// </summary>
		/// <returns>un action result con la lista de libros</returns>
		[HttpGet] //cabecera de acceso para invocar el endpoint
		[Route(RouteEntity.LibroFind)] //ruta de acceso al endpoint
		public async Task<IActionResult> GetAllLibros()
		{
			try
			{
				var libros = await _libroRepository.GetLibrosAsync();
				var model = _mapper.Map<List<Models.Libro.Consultar>>(libros);
				return Ok(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ErrorLogController.ErrorNameController + ex.Message);
				return StatusCode(500, ErrorLogController.ServerError);
			}
		}
		#endregion

		#region HttpPost Crear Recurso
		/// <summary>
		/// Este metodo se encarga de insertar mediante post una entidad del tipo libro en el repositorio de la base de datos
		/// </summary>
		/// <param name="crearLibro">la entidad crearlibro</param>
		/// <returns>una accion o una tarea</returns>
		[HttpPost] //esta es la cabecera para realizar la operacion a traves del endpoint
		[Route(RouteEntity.LibroInsert)] //esta es la ruta del controlador
		[ServiceFilter(typeof(ValidacionFilterAttribute))]//este filtro se encarga de validar la entidad solo se aplica en insercion, eliminacion,actualizacion
		public async Task<IActionResult> Post([FromBody]Models.Libro.Crear crearLibro)
		{
			try
			{
                if (crearLibro.AutoresId == null)
                {
					return BadRequest(ErrorLogController.BadRequestListNull);
                }

                //1.- validar que existan los autores del libro que se envian
                if (!await _libroRepository.ValidarAutoresId( crearLibro.AutoresId))
                {
					return BadRequest(ErrorLogController.BadRequestIds);
                }
				//Realizamos el mapeo de la entidad
				var libro = _mapper.Map<Entities.Libro>(crearLibro);
				await _libroRepository.AddLibroAsync(libro);
				await _libroRepository.SaveChangesAsync();
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ErrorLogController.ErrorNameController + ex.Message);
				return StatusCode(500, ErrorLogController.ServerError);
			}
		}

		#endregion

		#region Actualizar
		/// <summary>
		/// Este endpoint consiste la entidad libro
		/// </summary>
		/// <param name="id">el identificador del libro</param>
		/// <param name="actualizarLibro">la entidad a actualizar</param>
		/// <returns>un codigo de respuesta</returns>
		[HttpPut]
		[Route(RouteEntity.LibroUpdate)]
		[ServiceFilter(typeof(ValidacionFilterAttribute))]
		[ServiceFilter(typeof(ValidateEntityExistsAttribute<Entities.Libro>))]
		public IActionResult Put(int id, [FromBody] Models.Libro.Actualizar actualizarLibro)
		{

			var libro = _mapper.Map<Entities.Libro>(actualizarLibro);
			_libroRepository.UpdateLibro(libro);
			return NoContent();
		}
		#endregion

		#region Eliminar
		/// <summary>
		/// Este metodo se encarga de eliminar fisicamente una entidad del tipo libro
		/// </summary>
		/// <param name="id">el identificador de la entidad</param>
		/// <returns>un resultado como tarea</returns>
		[HttpDelete]
		[Route(RouteEntity.LibroDelete)]
		public async Task<IActionResult> Delete(int id)
		{
			if (!_libroRepository.LibroExistsAsync(id).Result)
			{
				return NotFound(ErrorLogController.NotFoundEntity);
			}
			var libro = await _libroRepository.GetLibroAsync(id);
			_libroRepository.DeleteLibro(libro);
			await _libroRepository.SaveChangesAsync();
			return Ok();
		}
		#endregion
	}
}
