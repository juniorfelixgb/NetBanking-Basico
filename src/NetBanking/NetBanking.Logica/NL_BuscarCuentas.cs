using NetBanking.Core;
using NetBanking.DATA.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Logica
{
    public class NL_BuscarCuentas
    {
        public List<NC_Cuentas> CuentasDisponibles(string NombreUsuario)
        {
            List<NC_Cuentas> cuentas = null;
            try
            {
                using (var db= new netbankingContext())
                {
                    cuentas = db.Cuentas.Where(p => p.Usuario.NombreUsuario == NombreUsuario)
                        .Select(q=> new NC_Cuentas
                        { 
                            NumeroCuenta=q.NumeroCuenta,
                            MontoDisponible = q.MontoDisponible,
                            MonedaSimbolo= q.Moneda.Simbolo
                        }).ToList();
                }
            }
            catch (Exception)
            {            }


            return cuentas;
        }
    }
}
