using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetBanking.DATA.Modelo;

namespace NetBanking.UI.Pages.Authenticate
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Usuario _Usuario { get; set; }
        [BindProperty]
        public string PasswordCheck { get; set; }
        public void OnGet()
        {
            _Usuario = new Usuario();
        }
        public void OnPost()
        {
            using (var db = new netbankingContext())
            {
                db.Usuarios.Add(_Usuario);
                db.SaveChanges();
            }
        }
    }
}
