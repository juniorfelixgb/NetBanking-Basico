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
                var usuario = db.Usuarios.FirstOrDefault(p => p.NombreUsuario.ToLower() == credenciales.Usuario.Trim().ToLower()
                && p.Contrasena == credenciales.Password.Trim());
                resultado = (usuario != null);
                if (resultado)
                    credenciales.NombreApellido = $"{usuario.Nombres.Split(" ")[0]} {usuario.Apellidos.Split(" ")[0]}";
            }
            return resultado;
        }

        public bool RegistroNewUsuario(newUsuario usuario)
        {
            bool registrado = false;
            try
            {
                using (var db = new netbankingContext())
                {
                    var usu = new Usuario
                    {
                        Nombres = usuario.Nombres,
                        Apellidos = usuario.Apellidos,
                        Cedula = usuario.Cedula,
                        NombreUsuario = usuario.NombreUsuario,
                        Email = usuario.Email,
                        Contrasena = usuario.Password,
                        FechaNacimiento = usuario.FechaNacimiento
                    };
                    db.Usuarios.Add(usu);
                    db.SaveChanges();
                    registrado = true;
                }

            }
            catch (Exception ex)
            { }
            return registrado;
        }
    }
}
