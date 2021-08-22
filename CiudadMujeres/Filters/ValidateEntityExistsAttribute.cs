using CiudadMujeres.Data;
using CiudadMujeres.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Filters
{
    /// <summary>
    /// Esta entidad se encarga de validar una entidad  en base de datos a traves de la validacion de un atributo
    /// </summary>
    /// <typeparam name="T">una entidad del tipo entities que implementa la interfaz IEntity</typeparam>
    public class ValidateEntityExistsAttribute<T> : IActionFilter where T : class, IEntity
    {

        private readonly ApplicationContext _contexto; 

        public ValidateEntityExistsAttribute(ApplicationContext _context)
        {
            _contexto = _context;
        }

        /// <summary>
        /// Una vez que se ejecuta el controlador y se hace la llamada al endpoint  este es el metodo que se llama o invoca de forma inmediata
        /// </summary>
        /// <param name="context">el contexto de la aplicación para tener referencia con las entidades de base de datos</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // inicializamos el identificador en 0 
            int id = 0;

            if (context.ActionArguments.ContainsKey("id"))
            {
                //convertimos el identificador en integer para asegurarnos que el tipo de datos es entero
                id = (int)context.ActionArguments["id"];
            }
            else
            {
                context.Result = new BadRequestObjectResult(Common.ErrorLogController.BadParameterId);
                return;
            }
            var entity = _contexto.Set<T>().SingleOrDefault(x => x.Id.Equals(id));
            if (entity == null)
            {
                // el resultado no fue encontrado o la entidad de base de datos no fue encontrada
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("entity",entity);
            }
        }



        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        
    }
}
