using System;
using System.Collections.Generic;

#nullable disable

namespace NetBanking.DATA.Modelo
{
    public partial class Deposito
    {
        public Deposito()
        {
            Transacciones = new HashSet<Transaccione>();
        }

        public int DepositoId { get; set; }
        public int CuentaId { get; set; }
        public int UsuarioId { get; set; }
        public int EstadoId { get; set; }
        public string AlisCuenta { get; set; }
        public decimal Monto { get; set; }
        public DateTime? Fecha { get; set; }
        public string Detalles { get; set; }

        public virtual Cuenta Cuenta { get; set; }
        public virtual EstadoTrd Estado { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Transaccione> Transacciones { get; set; }
    }
}
