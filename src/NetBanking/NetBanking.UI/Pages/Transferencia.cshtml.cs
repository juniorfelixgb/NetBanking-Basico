using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBanking.Core;
using NetBanking.DATA.Modelo;
using NetBanking.Logica;

namespace NetBanking.UI.Pages
{
    public class TransferenciaModel : PageModel
    {
        public NC_Trasferencia _Trasferencia { get; set; }
        public SelectList _SelectCuentas { get; set; }
        public void OnGet()
        {
            List<NC_Cuentas> _Cuentas;
            NL_BuscarCuentas cuentas = new NL_BuscarCuentas();
            _Cuentas = cuentas.CuentasDisponibles(User.Identity.Name)
                .Select(p => new NC_Cuentas
                {
                    NumeroCuenta = p.NumeroCuenta,
                    MontoDisponible = p.MontoDisponible,
                    MonedaSimbolo = p.MonedaSimbolo,
                    NumeroCuentaMontoDisponible = $"{p.NumeroCuenta} {p.MonedaSimbolo} {p.MontoDisponible}"
                }).ToList();



            _SelectCuentas = new SelectList(_Cuentas, nameof(NC_Cuentas.NumeroCuenta), nameof(NC_Cuentas.NumeroCuentaMontoDisponible), "RD40225178843", nameof(NC_Cuentas.MonedaSimbolo));
            //var Sales = new SelectListGroup { Name = "Sales" };
            //var Admin = new SelectListGroup { Name = "Admin" };
            //var IT = new SelectListGroup { Name = "IT" };
            //Staff = new List<SelectListItem>
            //        {
            //            new SelectListItem{ Value = "1", Text = "Mike", Group = IT},
            //            new SelectListItem{ Value = "2", Text = "Pete", Group = Sales},
            //            new SelectListItem{ Value = "3", Text = "Katy", Group = Admin},
            //            new SelectListItem{ Value = "4", Text = "Dean", Group = Sales}
            //        };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OnPostCuentaDeposito([FromBody]NC_Trasferencia trasferencia)
        {
            try
            {
                using (var db = new netbankingContext())
                {
                    var cuentaRetiro = db.Cuentas.FirstOrDefault(p => p.NumeroCuenta == trasferencia.NumeroCuentaRetiro);
                    var cuentaDeposito = db.Cuentas.FirstOrDefault(p => p.NumeroCuenta == trasferencia.NumeroCuentaDeposito);

                    NC_Cuentas nc_cuentas ;
                    if ((cuentaDeposito != null))
                    {
                       decimal valorCambioMoneda = db.MonedaCambios.First
                            (p => p.MonedaIddesde == cuentaRetiro.MonedaId && p.MonedaIdhacia == cuentaDeposito.MonedaId).Valor;
                        nc_cuentas = new NC_Cuentas()
                        {
                            ValorCambioMoneda = valorCambioMoneda,
                            MonedaSimbolo = cuentaDeposito.Moneda.Simbolo,
                            NumeroCuenta = cuentaDeposito.NumeroCuenta,
                            NombreApellido = $"{cuentaDeposito.Usuario.Nombres.Split(" ")[0]} {cuentaDeposito.Usuario.Apellidos.Split(" ")[0]}"
                        };
                    }
                    else
                    {
                        nc_cuentas = null;
                    }
                    return new JsonResult(new { Resultado = nc_cuentas, Mensaje = ((nc_cuentas == null) ? "Esta cuenta no existe." : "OK") });
                }

            }
            catch (Exception)
            { }


            return new JsonResult(new { Mensaje = "Esta cuenta no existe." });
        }
    }
}
