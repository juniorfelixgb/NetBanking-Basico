using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core
{
    public class NC_Historico
    {
        public string NumeroCuenta { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public decimal Monto { get; set; }
        public DateTime? Fecha { get; set; }
        public string Detalles { get; set; }
        public string Moneda { get; set; }
        public string DepRet { get; set; }
    }
}
