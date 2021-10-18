using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetBanking.Core;
using NetBanking.Logica;

namespace NetBanking.UI.Pages
{
    
    
    public class DashboardModel : PageModel
    {
        public List<NC_Cuentas> _Cuentas { get; set; }

        public List<NC_Historico> Resumen;
        public void OnGet()
        {
            NL_BuscarCuentas cuentas = new NL_BuscarCuentas();
            _Cuentas=cuentas.CuentasDisponibles(User.Identity.Name);
            NC_PropHistorico _propHistorico = new NC_PropHistorico { FechaDesde = DateTime.Now.AddMonths(-1), FechaHasta = DateTime.Now };
            Resumen = new List<NC_Historico>();

            foreach (var cuenta in _Cuentas)
            {
                _propHistorico.NumeroCuenta = cuenta.NumeroCuenta;
                Resumen = Resumen.Union(new NL_Historico().GetHistorico(_propHistorico)).ToList();
            }
            Resumen = Resumen.OrderByDescending(p => p.Fecha).Take(6).ToList();
        }
    }
}
