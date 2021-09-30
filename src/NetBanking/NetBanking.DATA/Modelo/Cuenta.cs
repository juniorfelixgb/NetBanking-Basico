using System;
using System.Collections.Generic;

#nullable disable

namespace NetBanking.DATA.Modelo
{
    public partial class Cuenta
    {
        public Cuenta()
        {
            Depositos = new HashSet<Deposito>();
            Retiros = new HashSet<Retiro>();
        }

        public int CuentaId { get; set; }
        public int MonedaId { get; set; }
        public int UsuarioId { get; set; }
        public string AlisCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal MontoDisponible { get; set; }
        public decimal MontoTrancito { get; set; }

        public virtual Moneda Moneda { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Deposito> Depositos { get; set; }
        public virtual ICollection<Retiro> Retiros { get; set; }
    }
}
