using System.Collections.Generic;

namespace GraficosFullWMS.Classes
{
    public class Connections
    {
#pragma warning disable IDE1006 // Estilos de Nomenclatura
        public List<ConnectionSave> connections { get; } = new List<ConnectionSave>();
#pragma warning restore IDE1006 // Estilos de Nomenclatura

        public Connections() { }
    }
}
