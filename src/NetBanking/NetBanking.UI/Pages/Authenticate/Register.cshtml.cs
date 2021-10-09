using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetBanking.Core;
using NetBanking.DATA.Modelo;
using NetBanking.Logica;

namespace NetBanking.UI.Pages.Authenticate
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public newUsuario _Usuario { get; set; }
        public void OnGet()
        {
            _Usuario = new newUsuario();
        }
        public IActionResult OnPost()
        {
            if (new Login().RegistroNewUsuario(_Usuario))
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
