using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core
{
    public class NC_Trasferencia
    {
        public int UsuarioID { get; set; }
        public string NumeroCuentaRetiro { get; set; }
        public string NumeroCuentaDeposito { get; set; }
        public decimal MontoParaDeposito { get; set; }
        public string Detalles { get; set; }
        public bool MontoInsuficiente { get; set; }
        public bool Confirmada { get; set; }
    }
}
