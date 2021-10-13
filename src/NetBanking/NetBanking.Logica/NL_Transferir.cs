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
        public void Transfiere(NC_Trasferencia trasferencia)
        {
            trasferencia.NumeroCuentaDeposito = trasferencia.NumeroCuentaDeposito.Split(" ")[trasferencia.NumeroCuentaDeposito.Split(" ").Length - 1];

            try
            {
                using (var db=new netbankingContext())
                {
                    
                    var resultado=db.Database.ExecuteSqlInterpolated($@"EXEC	 [dbo].[Transferir]
                                                       @UsuarioID = {trasferencia.UsuarioID},
                                                       @NumeroCuentaRetiro = {trasferencia.NumeroCuentaRetiro},
                                                       @NumeroCuentaDeposito = {trasferencia.NumeroCuentaDeposito},
                                                       @MontoParaDeposito = {trasferencia.MontoParaDeposito},
                                                       @MontoInsuficiente = {trasferencia.MontoInsuficiente} OUTPUT,
                                                       @Detalles = {trasferencia.Detalles}, 
                                                       @Confirmada =1");
                }
            }
            catch (Exception)
            {            }

        }
    }
}
