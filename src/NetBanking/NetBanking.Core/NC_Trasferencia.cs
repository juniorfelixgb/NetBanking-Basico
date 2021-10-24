using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core
{
    public class NC_Trasferencia
    {
        public string UsuarioNombre { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MinLength(10, ErrorMessage = "Numero de cuenta invalido.")]
        public string NumeroCuentaRetiro { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio.")]
        [MinLength(10, ErrorMessage ="Numero de cuenta invalido.")]
        public string NumeroCuentaDeposito { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio.")]
       // [RegularExpression(@"^[0-9]*(\.[0-9]+)?$", ErrorMessage ="")]
        public decimal MontoParaDeposito { get; set; }
        public string Detalles { get; set; }
        public bool Confirmada { get; set; }
    }
}
