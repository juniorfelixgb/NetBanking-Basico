using System;
using System.Collections.Generic;

#nullable disable

namespace NetBanking.DATA.Modelo
{
    public partial class Usuario
    {
        public Usuario()
        {
            Cuenta = new HashSet<Cuenta>();
            Depositos = new HashSet<Deposito>();
            Retiros = new HashSet<Retiro>();
        }

        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string Cedula { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime? FechaRegitro { get; set; }

        public virtual ICollection<Cuenta> Cuenta { get; set; }
        public virtual ICollection<Deposito> Depositos { get; set; }
        public virtual ICollection<Retiro> Retiros { get; set; }
    }
}
