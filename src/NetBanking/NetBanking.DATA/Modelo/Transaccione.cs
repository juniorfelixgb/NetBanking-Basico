using System;
using System.Collections.Generic;

#nullable disable

namespace NetBanking.DATA.Modelo
{
    public partial class Transaccione
    {
        public int TransaccionId { get; set; }
        public int RetiroId { get; set; }
        public int DepositoId { get; set; }
        public int EstadoId { get; set; }
        public decimal CostoTransferencia { get; set; }
        public int? MonedaCambioId { get; set; }

        public virtual Deposito Deposito { get; set; }
        public virtual EstadoTrd Estado { get; set; }
        public virtual MonedaCambio MonedaCambio { get; set; }
        public virtual Retiro Retiro { get; set; }
    }
}
