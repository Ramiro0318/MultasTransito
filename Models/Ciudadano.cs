using System;
using System.Collections.Generic;

namespace MultasTransito2.Models;

public partial class Ciudadano
{
    public string NumeroLicenca { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public int MultasAcargo { get; set; }

    public bool Eliminado { get; set; }

    public virtual ICollection<Multa> Multa { get; set; } = new List<Multa>();
}
