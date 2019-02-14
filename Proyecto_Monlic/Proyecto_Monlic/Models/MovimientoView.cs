using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Monlic.Models
{
    public class MovimientoView:MovimientoESA
    {
        public int Cantidad { get; set; }
        public int IdMaterial { get; set; }
    }
}