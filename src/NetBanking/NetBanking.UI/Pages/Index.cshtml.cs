using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetBanking.Core;
using NetBanking.Logica;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace NetBanking.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public NC_Credenciales _Credenciales { get; set; }
        public bool Incorreto { get; set; } 

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _Credenciales = new NC_Credenciales();
            Incorreto = false;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                NL_Login login = new NL_Login();
                if (login.LoginIN(_Credenciales))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _Credenciales.Usuario),
                        new Claim("NombreApellido",_Credenciales.NombreApellido)
                    };
                    var identity = new ClaimsIdentity(claims, "AUT");

                    var autPropiedades = new AuthenticationProperties() 
                    { IsPersistent = _Credenciales.Recordarme};

                    await HttpContext.SignInAsync("AUT", new ClaimsPrincipal(identity), autPropiedades);

                    return RedirectToPage("/Dashboard");
                }
                else
                {
                    Incorreto = true;
                }
            }
            return Page();
        }

    }
}
