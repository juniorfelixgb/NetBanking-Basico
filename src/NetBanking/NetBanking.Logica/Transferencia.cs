using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBanking.DATA.Modelo;
using NetBanking.Core;

namespace NetBanking.Logica
{
    class Transferencia
    {
        public int BuscarCuenta(int cuenta)
        {
            int encontrada;
            using(var db = new netbankingContext())
            {

                var objcuenta = (from a in db.Usuarios 
                                 where a.UsuarioId == cuenta 
                                 select a.UsuarioId).FirstOrDefault();
                //var Objcuenta = db.Usuarios.FirstOrDefault(item => item.UsuarioId == cuenta);
                encontrada = objcuenta;
            }
            return encontrada;
        }

        public double EnviarFondos(double fondos)
        {
            return 0;
        }
    }
}
