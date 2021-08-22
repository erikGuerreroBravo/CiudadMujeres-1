using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiudadMujeres.Models
{
    //Esta interfaz se implementara en todas las clases para poder procesar los filtros de accion y de excepcion
    public interface IEntity
    {
        public int Id { get; set; }
    }
}
