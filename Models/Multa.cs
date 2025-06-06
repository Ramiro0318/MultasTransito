using System;
using System.Collections.Generic;

namespace MultasTransito2.Models;

public partial class Multa
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public string Motivo { get; set; } = null!;

    public decimal SancionPecuniaria { get; set; }

    public string IdCiudadano { get; set; } = null!;

    public bool Eliminado { get; set; }

    public virtual Ciudadano IdCiudadanoNavigation { get; set; } = null!;
}
