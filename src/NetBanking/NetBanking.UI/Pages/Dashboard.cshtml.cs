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


    public class DashboardModel : PageModel
    {

        public SelectList _SelectCuentas { get; set; }
        public List<NC_Cuentas> _Cuentas { get; set; }
        [BindProperty]
        public Cuenta _Cuenta { get; set; }

        public List<NC_Historico> Resumen;
        public void OnGet()
        { cargar(); }
        public void cargar()
        {
            NL_BuscarCuentas cuentas = new NL_BuscarCuentas();
            _Cuentas = cuentas.CuentasDisponibles(User.Identity.Name);
            NC_PropHistorico _propHistorico = new NC_PropHistorico { FechaDesde = DateTime.Now.AddMonths(-1), FechaHasta = DateTime.Now };
            Resumen = new List<NC_Historico>();

            foreach (var cuenta in _Cuentas)
            {
                _propHistorico.NumeroCuenta = cuenta.NumeroCuenta;
                Resumen = Resumen.Union(new NL_Historico().GetHistorico(_propHistorico)).ToList();
            }
            Resumen = Resumen.OrderByDescending(p => p.Fecha).Take(6).ToList();

            using (var db = new netbankingContext())
            {
                List<Moneda> monedas = db.Monedas.ToList();
                _SelectCuentas = new SelectList(monedas, nameof(Moneda.MonedaId), nameof(Moneda.MonedaNombre));
            }




        }
        public void OnPost()
        {
            try
            {
                using (var db = new netbankingContext())
                {
                    var cuent = db.Cuentas.OrderByDescending(p => p.CuentaId).FirstOrDefault();
                    _Cuenta.UsuarioId = db.Usuarios.FirstOrDefault(p => p.NombreUsuario == User.Identity.Name).UsuarioId;
                    _Cuenta.MontoTrancito = 0;
                    _Cuenta.NumeroCuenta = "RD" + (Convert.ToInt64(cuent.NumeroCuenta.Substring(2)) + 1);

                    db.Add(_Cuenta);
                    db.SaveChanges();
                    _Cuenta = new Cuenta();
                }

            }
            catch (Exception)
            {

            }
            cargar();
        }
    }
}
