using Microsoft.EntityFrameworkCore;
using MultasTransito2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MultasTransito2.Repositories
{
    public class MultasRepository
    {
        RegistroTransitoContext Context = new();
        public IEnumerable<Multa> GetMultas()
        {
            return Context.Multa.Where(x => x.Eliminado == false).OrderBy(x => x.Fecha);

        }

        public Multa GetMultaById(int id)
        {
            return Context.Multa.Where(x => x.Id == id).First();
        }

        public void InsertarMulta(Multa m)
        {                               
            string x = m.Fecha.ToString("yyyy-MM-dd");
            Context.Database.ExecuteSqlRaw($"Call RegistrarMulta('{m.IdCiudadano}','{x}','{m.Motivo}','{m.SancionPecuniaria}')");

        }

        public bool Validar(Multa m, out string errores)
        {
            List<string> lista = new();
            if (string.IsNullOrWhiteSpace(m.Motivo))
            {
                lista.Add("Indique el motivo de la multa");
            }
            if (!Context.Ciudadano.Any(x => x.NumeroLicenca == m.IdCiudadano))
            {
                lista.Add("El numero de licencia no existe");
            }
            if (m.IdCiudadano == null || !Regex.IsMatch(m.IdCiudadano, "^[A-Z]{3}-[0-9]{5}$"))
            {
                lista.Add("Indique un numero de licencia válido");
            }
            if (m.Fecha > DateOnly.FromDateTime(DateTime.Today))
            {
                lista.Add("Indique una fecha válida");
            }
            if (!Regex.IsMatch(m.SancionPecuniaria.ToString(), @"^\d{1,10}(\.\d{1,2})?$"))
            {

            }

            errores = string.Join(", \n", lista);
            return string.IsNullOrWhiteSpace(errores);
        }



        public void EditarMulta(Multa m)
        {
            Context.Multa.Update(m);
            Context.SaveChanges();
        }

        public void EliminarMulta(Multa m)
        {
            m.Eliminado = true;
            Context.Multa.Update(m);
            Context.SaveChanges();
        }

    }
}
