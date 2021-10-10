using System;
using System.Collections.Generic;

#nullable disable

namespace NetBanking.DATA.Modelo
{
    public partial class Moneda
    {
        public Moneda()
        {
            Cuenta = new HashSet<Cuenta>();
            MonedaCambioMonedaIddesdeNavigations = new HashSet<MonedaCambio>();
            MonedaCambioMonedaIdhaciaNavigations = new HashSet<MonedaCambio>();
        }

        public int MonedaId { get; set; }
        public string MonedaNombre { get; set; }
        public string Simbolo { get; set; }
        public string Abreviado { get; set; }

        public virtual ICollection<Cuenta> Cuenta { get; set; }
        public virtual ICollection<MonedaCambio> MonedaCambioMonedaIddesdeNavigations { get; set; }
        public virtual ICollection<MonedaCambio> MonedaCambioMonedaIdhaciaNavigations { get; set; }
    }
}
