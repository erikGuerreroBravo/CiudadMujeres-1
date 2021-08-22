using CiudadMujeres.Common;
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
    /// Este filtro se encarga de validar las entidades e indetificar si vienen nulas o vienen invalidas en la entidad
    /// </summary>
    public class ValidacionFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //buscamos la entidad por el parametro establecido en este caso es la interfaz IEntity
            var parametro = context.ActionArguments.SingleOrDefault(p => p.Value is IEntity);

            if (parametro.Value == null)
            {
                context.Result = new BadRequestObjectResult(ErrorLogController.BadRequestResult);
                return;
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
