using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBanking.DATA.Modelo;
using NetBanking.Core;

namespace NetBanking.Logica
{
    public class Login
    {
        public bool LoginIN(Credenciales credenciales)
        {
            bool resultado = false;
            using (var db = new netbankingContext())
            {
                resultado = db.Usuarios.Any(p => p.NombreUsuario.ToLower() == credenciales.Usuario.Trim().ToLower() 
                && p.Contrasena == credenciales.Password.Trim());
            }
            return resultado;
        }
    }
}
