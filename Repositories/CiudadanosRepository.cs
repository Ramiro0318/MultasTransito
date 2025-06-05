using Microsoft.EntityFrameworkCore;
using MultasTransito2.Models;
using MultasTransito2.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MultasTransito2.Repositories
{
    public class CiudadanosRepository
    {
        RegistrotransitoContext Context = new();

        public IEnumerable<MultasPorCiudadanoDTO> GetMultasPorCiudadanoView()
        {
            //var multasAgrupadas = Context.Multasporciudadanoview
            //.GroupBy(m => new { m.NumeroLicenca, m.NombreCiudadano })
            //.Select(grupo => new MultasPorCiudadanoDTO
            //{

            //    NumeroLicenca = grupo.Key.NumeroLicenca,
            //    NombreCiudadano = grupo.Key.NombreCiudadano,
            //    Multas = grupo.Select(m => new MultasDTO
            //    {
            //        IdMulta = m.IdMulta,
            //        Fecha = m.Fecha,
            //        Motivo = m.Motivo,
            //        SancionPecuniaria = m.SancionPecuniaria
            //    }).ToList()
            //})
            //.ToList();

            //return multasAgrupadas;

            var multasAgrupadas = Context.Ciudadano.Include(x => x.Multa).Select( c => new MultasPorCiudadanoDTO
            {
                NumeroLicenca = c.NumeroLicenca,
                NombreCiudadano = c.Nombre,
                Multas = c.Multa.Select(m => new MultasDTO
                {
                    IdMulta = m.Id,
                    Fecha = m.Fecha,
                    Motivo = m.Motivo,
                    SancionPecuniaria = m.SancionPecuniaria
                }).ToList()
            }
            );
            return multasAgrupadas;
        }

        public IEnumerable<Ciudadano> GetAll()
        {
            return Context.Ciudadano;
        }
        public IEnumerable<Ciudadano> Filtrar(string filtro)
        {
            return Context.Ciudadano.Where(x => x.NumeroLicenca.Contains(filtro) || x.Nombre.Contains(filtro));
        }



        public Ciudadano GetCiudadanoByNumero(string licencia)
        {
            return Context.Ciudadano.Where(x => x.NumeroLicenca == licencia).First();
        }

        public void InsertarCiudadano(Ciudadano c)
        {
            Context.Ciudadano.Add(c);
            Context.SaveChanges();
        }

        public void EditarCiudadano(Ciudadano c)
        {
            Context.Ciudadano.Update(c);
            Context.SaveChanges();
        }

        public void EliminarCiudadano(Ciudadano c)
        {
            Context.Ciudadano.Remove(c);
            Context.SaveChanges();
        }

        public bool Validar(Ciudadano c, out string errores)
        {
            List<string> lista = new();
            if (string.IsNullOrWhiteSpace(c.NumeroLicenca) || string.IsNullOrWhiteSpace(c.Nombre) || c.Edad < 18)
            {
                lista.Add("Indique adecuadamente los datos");
            }
            if (Context.Ciudadano.Any(x => x.NumeroLicenca == c.NumeroLicenca))
            {
                lista.Add("El numero de licencia ya existe");
            }

            if (string.IsNullOrWhiteSpace(c.NumeroLicenca) || !Regex.IsMatch(c.NumeroLicenca, "^[A-Z]{3}-[0-9]{5}$"))
            {
                lista.Add("El numero de licencia no es válido");
            }

            errores = string.Join(",\n", lista);
            return string.IsNullOrWhiteSpace(errores);
        }

        public bool ValidarEditar(Ciudadano c, out string errores)
        {
            List<string> lista = new();
            if (string.IsNullOrWhiteSpace(c.Nombre) || c.Edad < 18)
            {
                lista.Add("Indique adecuadamente los datos");
            }
            if (string.IsNullOrWhiteSpace(c.NumeroLicenca) || !Regex.IsMatch(c.NumeroLicenca, "^[A-Z]{3}-[0-9]{5}$"))
            {
                lista.Add("El numero de licencia no es válido");
            }

            errores = string.Join(",\n", lista);
            return string.IsNullOrWhiteSpace(errores);
        }

        public bool ValidarEliminar(Ciudadano c, out string errores)
        {
            List<string> lista = new();
            if (c.MultasAcargo > 0)
            {
                lista.Add("No se puede eliminar un ciudadano con multas");
            }

            errores = string.Join("", lista);
            return string.IsNullOrWhiteSpace(errores);
        }

    }
}
