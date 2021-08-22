using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CiudadMujeres.Common
{
    public static class RouteEntity
    {

        //controlador persona que obtiene todas las personas
        public const string PersonaFind = "api/find/data/persona/todo";
        //ruta para la insercion de una entidad el tipo persona
        public const string PersonaInsert = "api/insert/data/persona/registro";
        // ruta para la actualización de una entidad del tipo persona
        public const string PersonaUpdate = "api/update/data/persona/registro/{id}";
        //ruta para la insercion de una cuenta de usuario normal dentro del sistema
        public const string AccountInsert = "api/insert/data/account/registro";
        //ruta para el endpoint de insercion de un administrador en el sistema
        public const string AccountInsertAdmin = "api/admin/insert/data/account/registro";
        //ruta para el endpoint de eliminacion de un administrador del sistema
        public const string AccountEliminarAdmin = "api/admin/delete/data/account/registro";
        //ruta para el endpoint de inicio de sesión
        public const string AccountLogin = "api/security/account/credentials/login";
        //ruta para refrescar la ruta de acceso
        public const string AccountRefresh = "api/security/account/credentials/refresh";
        //ruta para el endpoint de cierre de sesión
        public const string AccountLogout = "api/security/account/credentials/logout";
        // ruta para la insercion del libro
        public const string LibroInsert = "api/insert/data/libro/registro";
        public const string LibroFind = "api/insert/data/libro/todo";
        public const string LibroUpdate = "api/update/data/libro/registro/{id}";
        public const string LibroDelete = "api/delete/data/libro/registro/{id}";
        // ruta para la insercion del comentario 
        public const string ComentarioInsert = "api/insert/data/libros/{libroId:int}/comentarios";
        
    }
}
