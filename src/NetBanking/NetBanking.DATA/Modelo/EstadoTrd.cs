using System;
using System.Collections.Generic;

#nullable disable

namespace NetBanking.DATA.Modelo
{
    public partial class EstadoTrd
    {
        public EstadoTrd()
        {
            Depositos = new HashSet<Deposito>();
            Retiros = new HashSet<Retiro>();
            Transacciones = new HashSet<Transaccione>();
        }

        public int EstadoId { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Deposito> Depositos { get; set; }
        public virtual ICollection<Retiro> Retiros { get; set; }
        public virtual ICollection<Transaccione> Transacciones { get; set; }
    }
}
