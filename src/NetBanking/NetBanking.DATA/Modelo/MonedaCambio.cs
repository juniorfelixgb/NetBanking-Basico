using System;
using System.Collections.Generic;

#nullable disable

namespace NetBanking.DATA.Modelo
{
    public partial class MonedaCambio
    {
        public MonedaCambio()
        {
            Transacciones = new HashSet<Transaccione>();
        }

        public int MonedaCambioId { get; set; }
        public int MonedaIddesde { get; set; }
        public int MonedaIdhacia { get; set; }
        public decimal Valor { get; set; }
        public string Detalles { get; set; }

        public virtual Moneda MonedaIddesdeNavigation { get; set; }
        public virtual Moneda MonedaIdhaciaNavigation { get; set; }
        public virtual ICollection<Transaccione> Transacciones { get; set; }
    }
}
