using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultasTransito2.Models.DTOs
{
    public class MultasDTO
    {
        public int IdMulta { get; set; }
        public DateOnly Fecha { get; set; }
        public string Motivo { get; set; } = null!;
        public decimal SancionPecuniaria { get; set; }
    }

    public class MultasPorCiudadanoDTO
    {
        public string NombreCiudadano { get; set; } = null!;
        public string NumeroLicenca { get; set; } = null!;
        public List<MultasDTO> Multas { get; set; } = new(); // Lista de multas
    }
}
