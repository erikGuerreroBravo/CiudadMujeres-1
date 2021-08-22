using AutoMapper;
using CiudadMujeres.Common;
using CiudadMujeres.Filters;
using CiudadMujeres.Infraestructure;
using CiudadMujeres.Models.Persona;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CiudadMujeres.Controllers
{
    
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly ILogger<PersonaController> _logger;
        private readonly IMapper _mapper;
        private readonly IPersonaRepository _repositoryPersona;

        private readonly UserManager<IdentityUser> _userManager;

        public PersonaController(IMapper mapper, IPersonaRepository repositoryPersona, 
            ILogger<PersonaController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _repositoryPersona = repositoryPersona ?? throw new ArgumentNullException(nameof(repositoryPersona));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ??  throw new ArgumentNullException(nameof(userManager));
            
        }

        /// <summary>
        /// Este metodo se encarga de consultar a todas las personas dentro del sistema
        /// </summary>
        /// <returns>un action result con la lista de personas</returns>
        [HttpGet] //cabecera de acceso para invocar el endpoint
        [Route(RouteEntity.PersonaFind)] //ruta de acceso al endpoint
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")] //metodo de autenticacion del endpoint utilizando la politica admin
        public async Task<IActionResult> GetAllPersonas()
        {
            try
            {
                // en esta consulta al contexto obtenemos el claim de email guardado en el user de identity
                var emailClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
                // obtenemos el el valor del claim que es el email
                var email = emailClaim.Value;
                // consultamos el usuario por email para obtener datos del userIdentity
                var user = await _userManager.FindByEmailAsync(email);

                var personas = await _repositoryPersona.GetPersonasAsync();
                var model = _mapper.Map<List<Models.Persona.Consultar>>(personas);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorLogController.ErrorNameController +ex.Message);
                return StatusCode(500,ErrorLogController.ServerError);
            }
            
        }

        /// <summary>
        /// Este metodo se encarga de insertar mediante post una entidad del tipo crear  que hace referencia a una persona
        /// </summary>
        /// <param name="crearPersona">la entidad persona en su modalidad crear</param>
        /// <returns>una accion o una tarea</returns>
        [HttpPost]
        [Route(RouteEntity.PersonaInsert)] //ruta de acceso al endpoint
        [ServiceFilter(typeof(ValidacionFilterAttribute))]//este filtro se encarga de validar la entidad solo se aplica en insercion, eliminacion,actualizacion
        public async Task<IActionResult> Post([FromBody] Models.Persona.Crear crearPersona)
        {
            try 
            {
                var persona = _mapper.Map<Entities.Persona>(crearPersona);
                _repositoryPersona.AddPersona(persona);
                await _repositoryPersona.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ErrorLogController.ErrorNameController + ex.Message);
                return StatusCode(500, ErrorLogController.ServerError);
            }

        }




        /// <summary>
        /// Este metodo se encarga de actualizar una entidad dentro del sistema, la entidad a actualizar es actualizar y hace referencia
        /// a una persona
        /// </summary>
        /// <param name="id">el identificador de la entidad para realizar la actualización</param>
        /// <param name="actualizarPersona">la entidad del tipo actualizarpersona</param>
        /// <returns>una accion o una tarea</returns>
        [HttpPut("{id}",Name ="Actualizar")]
        [ServiceFilter(typeof(ValidacionFilterAttribute))]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Entities.Persona>))]
        public IActionResult Put(int id, [FromBody] Models.Persona.Actualizar actualizarPersona)
        {
            
            var persona = _mapper.Map<Entities.Persona>(actualizarPersona);
            _repositoryPersona.UpdatePersona(persona);
            return NoContent();
        }


        /// <summary>
        /// Este metodo se encarga de eliminar fisicamente una entidad del tipo persona
        /// </summary>
        /// <param name="id">el identificador de la entidad</param>
        /// <returns>un resultado como tarea</returns>
        [HttpDelete("{id}", Name = "Eliminar")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_repositoryPersona.PersonaExistsAsync(id).Result)
            {
                return NotFound(ErrorLogController.NotFoundEntity);
            }
            var person = await _repositoryPersona.GetPersonaAsync(id);
            _repositoryPersona.DeletePersona(person);
            await _repositoryPersona.SaveChangesAsync();
            return Ok();
        }

       

    }
}
