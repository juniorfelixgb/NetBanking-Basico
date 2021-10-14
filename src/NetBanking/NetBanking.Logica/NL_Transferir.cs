using Microsoft.EntityFrameworkCore;
using NetBanking.Core;
using NetBanking.DATA.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Logica
{
    public class NL_Transferir
    {
        public bool Transfiere(NC_Trasferencia trasferencia)
        {
            bool resultado = false;
            trasferencia.NumeroCuentaDeposito = trasferencia.NumeroCuentaDeposito.Split(" ")[trasferencia.NumeroCuentaDeposito.Split(" ").Length - 1];

            try
            {
                using (var db=new netbankingContext())
                {
                    
                    var result=db.Database.ExecuteSqlInterpolated($@"EXEC	 [dbo].[Transferir]
                                                       @UsuarioID = {db.Usuarios.FirstOrDefault(p=>p.NombreUsuario == trasferencia.UsuarioNombre).UsuarioId},
                                                       @NumeroCuentaRetiro = {trasferencia.NumeroCuentaRetiro},
                                                       @NumeroCuentaDeposito = {trasferencia.NumeroCuentaDeposito},
                                                       @MontoParaDeposito = {trasferencia.MontoParaDeposito},
                                                       @Detalles = {trasferencia.Detalles}, 
                                                       @Confirmada = 1");
                    if (result > 7)
                        resultado = true;
                }
            }
            catch (Exception ex)
            {            }

            trasferencia.Confirmada = resultado;
            return resultado;
        }
    }
}
