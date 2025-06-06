using System;
using System.Collections.Generic;

namespace MultasTransito2.Models;

public partial class Multasporciudadanoview
{
    public int IdMulta { get; set; }

    public string NombreCiudadano { get; set; } = null!;

    public string NumeroLicenca { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public string Motivo { get; set; } = null!;

    public decimal SancionPecuniaria { get; set; }
}
