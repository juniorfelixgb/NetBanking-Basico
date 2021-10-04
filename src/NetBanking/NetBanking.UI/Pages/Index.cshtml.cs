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
        public Credenciales _Credenciales { get; set; } 

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _Credenciales = new Credenciales();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Login login = new Login();
                if (login.LoginIN(_Credenciales))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _Credenciales.Usuario),
                        new Claim("Departamento","RRHH")
                    };
                    var identity = new ClaimsIdentity(claims, "AUT");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("AUT", claimsPrincipal);

                    return RedirectToPage("/Privacy");
                }
            }
            return Page();
        }

    }
}
