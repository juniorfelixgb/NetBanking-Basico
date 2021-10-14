using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBanking.Core;
using NetBanking.Logica;

namespace NetBanking.UI.Pages
{
    public class HistoricoModel : PageModel
    {
        public SelectList _SelectCuentas { get; set; }
        public void OnGet(string numeroCuenta="")
        {
            List<NC_Cuentas> _Cuentas;
            _Cuentas = new NL_BuscarCuentas().CuentasDisponibles(User.Identity.Name)
                .Select(p => new NC_Cuentas
                {
                    NumeroCuenta = p.NumeroCuenta,
                    MontoDisponible = p.MontoDisponible,
                    MonedaSimbolo = p.MonedaSimbolo,
                    NumeroCuentaMontoDisponible = $"{p.NumeroCuenta} {p.MonedaSimbolo} {p.MontoDisponible}"
                }).ToList();


            _SelectCuentas = new SelectList(_Cuentas, nameof(NC_Cuentas.NumeroCuenta), nameof(NC_Cuentas.NumeroCuenta), numeroCuenta, nameof(NC_Cuentas.MonedaSimbolo));

        }
    }
}
