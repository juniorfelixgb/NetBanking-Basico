using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core
{
    public class newUsuario
    {

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [StringLength(50,ErrorMessage ="Usuario demasiado largo.")]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Correo inválido.")]
        [StringLength(150, ErrorMessage = "Correo demasiado largo.")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [StringLength(50, ErrorMessage = "Contraseña demasiado larga.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [Compare("Password",ErrorMessage = "No coinciden las contraseñas.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string CheckPassword { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [RegularExpression(@"^[0-9]{11}$",ErrorMessage = "Solo los 11 digitos sin guiones.")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [StringLength(200, ErrorMessage = "Nombres demasiado largos.")]
        [DataType(DataType.Text)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [StringLength(200, ErrorMessage = "Apellidos demasiado largos.")]
        [DataType(DataType.Text)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

    }
}
