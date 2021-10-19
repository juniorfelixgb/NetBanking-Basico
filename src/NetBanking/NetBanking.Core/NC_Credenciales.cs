using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core
{
    public class NC_Credenciales
    {

        [Required(ErrorMessage = "Campo Obligatorio.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Recordarme { get; set; }
        public string NombreApellido { get; set; }
    }
}
