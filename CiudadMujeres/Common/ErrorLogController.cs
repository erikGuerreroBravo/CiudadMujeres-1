using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Common
{
    public static class ErrorLogController
    {
        public const string ErrorNameController = "Ocurrio un error en el controlador llamado: ";
        public const string ServerError = "Error Interno del Servidor";
        public const string LoginDenegate = "Login incorrecto";
        public const string BadRequestResult = "El objeto es nulo";
        public const string BadParameterId = "Error en el parametro de tipo id";
        public const string NotFoundEntity = "No se encontro la entidad";
        public const string BadRequestIds = "No existen algunos identificadores o Id enviados";
        public const string BadRequestListNull = "No existen datos en la lista agregada";
    }
}
