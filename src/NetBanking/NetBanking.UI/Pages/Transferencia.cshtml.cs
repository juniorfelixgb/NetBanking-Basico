using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBanking.Core;
using NetBanking.DATA.Modelo;
using NetBanking.Logica;

namespace NetBanking.UI.Pages
{
    [Authorize]
    public class TransferenciaModel : PageModel
    {
        [BindProperty]
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


            _SelectCuentas = new SelectList(_Cuentas, nameof(NC_Cuentas.NumeroCuenta), nameof(NC_Cuentas.NumeroCuentaMontoDisponible), "", nameof(NC_Cuentas.MonedaSimbolo));
            
            
            
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
        public IActionResult OnPost()
        {
            _Trasferencia.UsuarioNombre = User.Identity.Name;
            if (new NL_Transferir().Transfiere(_Trasferencia))
            {
                return RedirectToPage("TransacionExito");
            }
            return RedirectToPage("/Transferencia");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OnPostCuentaDeposito([FromBody]NC_Trasferencia trasferencia)
        {
            try
            {
                using (var db = new netbankingContext())
                {
                    var cuentaRetiro = db.Cuentas.SingleOrDefault(p => p.NumeroCuenta == trasferencia.NumeroCuentaRetiro);
                    var cuentaDeposito = db.Cuentas.Where(p => p.NumeroCuenta == trasferencia.NumeroCuentaDeposito).Select(p=>new { p.MonedaId, p.Moneda, p.Usuario}).FirstOrDefault();

                    NC_Cuentas nc_cuentas ;
                    if ((cuentaDeposito != null))
                    {
                       var cambioMoneda = (cuentaRetiro.MonedaId == cuentaDeposito.MonedaId)?new MonedaCambio { Valor=(decimal)1 } 
                       :db.MonedaCambios.First(p => p.MonedaIddesde == cuentaRetiro.MonedaId && p.MonedaIdhacia == cuentaDeposito.MonedaId);
                        nc_cuentas = new NC_Cuentas
                        {
                            NumeroCuentaMontoDisponible =  cambioMoneda.Detalles,
                            ValorCambioMoneda = cambioMoneda.Valor,
                            MonedaSimbolo = cuentaDeposito.Moneda.Simbolo,
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
