using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core
{
    public class NC_Cuentas
    {
        public string MonedaSimbolo { get; set; }
        public string NumeroCuenta { get; set; }
        public string NumeroCuentaMontoDisponible { get; set; }
        public decimal MontoDisponible { get; set; }
        public string NombreApellido { get; set; }
        public decimal ValorCambioMoneda { get; set; }
    }
}
