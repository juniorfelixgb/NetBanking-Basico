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
    
    [Authorize]
    public class DashboardModel : PageModel
    {
        public List<NC_Cuentas> _Cuentas { get; set; }
        public void OnGet()
        {
            NL_BuscarCuentas cuentas = new NL_BuscarCuentas();
            _Cuentas=cuentas.CuentasDisponibles(User.Identity.Name);
        }
    }
}
