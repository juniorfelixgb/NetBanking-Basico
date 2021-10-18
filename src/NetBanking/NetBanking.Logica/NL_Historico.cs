using NetBanking.Core;
using NetBanking.DATA.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Logica
{
    public class NL_Historico
    {
        public List<NC_Historico> GetHistorico(NC_PropHistorico propHistorico)
        {
            List<NC_Historico> resultado;
            try
            {
                using (var db = new netbankingContext())
                {
                    var retiros = db.Retiros.Where(p => p.Cuenta.NumeroCuenta == propHistorico.NumeroCuenta && p.Fecha >= propHistorico.FechaDesde && p.Fecha <= propHistorico.FechaHasta.AddDays(1))
                        .Select(j => new NC_Historico { NumeroCuenta = j.Transacciones.FirstOrDefault().TransaccionId.ToString(), Moneda = j.Cuenta.Moneda.Simbolo, Monto = (-1 * j.Monto), Fecha = j.Fecha, Detalles = j.Detalles, UsuarioNombre = j.Usuario.Nombres, UsuarioApellido = j.Usuario.Apellidos, DepRet = "Retiro" }).ToList();

                    var depositos = db.Depositos.Where(p => p.Cuenta.NumeroCuenta == propHistorico.NumeroCuenta && p.Fecha >= propHistorico.FechaDesde && p.Fecha <= propHistorico.FechaHasta.AddDays(1))
                        .Select(j => new NC_Historico { NumeroCuenta = j.Transacciones.FirstOrDefault().TransaccionId.ToString(), Moneda = j.Cuenta.Moneda.Simbolo, Monto = j.Monto, Fecha = j.Fecha, Detalles = j.Detalles, UsuarioNombre = j.Usuario.Nombres, UsuarioApellido = j.Usuario.Apellidos, DepRet = "Deposito" }).ToList();


                    resultado = retiros.Union(depositos).OrderByDescending(p => p.Fecha).ToList();
                }
            }
            catch (Exception ex)
            { resultado = null; }
            return resultado;
        }
        public List<NC_Historico> GetHistoricoResumen(NC_PropHistorico propHistorico)
        {
            List<NC_Historico> resultado;
            try
            {
                using (var db = new netbankingContext())
                {
                    var retiros = db.Retiros.Where(p => p.Cuenta.NumeroCuenta == propHistorico.NumeroCuenta && p.Fecha >= propHistorico.FechaDesde && p.Fecha <= propHistorico.FechaHasta.AddDays(1))
                        .Select(j => new NC_Historico { NumeroCuenta = j.Transacciones.FirstOrDefault().TransaccionId.ToString(), Moneda=j.Cuenta.Moneda.Simbolo, Monto = -1 * j.Monto, Fecha = j.Fecha, Detalles = j.Detalles, UsuarioNombre = j.Usuario.Nombres, UsuarioApellido = j.Usuario.Apellidos, DepRet = "Retiro" }).ToList();

                    var depositos = db.Depositos.Where(p => p.Cuenta.NumeroCuenta == propHistorico.NumeroCuenta && p.Fecha >= propHistorico.FechaDesde && p.Fecha <= propHistorico.FechaHasta.AddDays(1))
                        .Select(j => new NC_Historico { NumeroCuenta = j.Transacciones.FirstOrDefault().TransaccionId.ToString(), Moneda = j.Cuenta.Moneda.Simbolo, Monto = j.Monto, Fecha = j.Fecha, Detalles = j.Detalles, UsuarioNombre = j.Usuario.Nombres, UsuarioApellido = j.Usuario.Apellidos, DepRet = "Deposito" }).ToList();


                    resultado = retiros.Union(depositos).OrderByDescending(p => p.Fecha).ToList();
                }
            }
            catch (Exception ex)
            { resultado = null; }
            return resultado;
        }
    }
}
