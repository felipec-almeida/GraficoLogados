using System;
using System.Collections.Generic;

namespace GraficosFullWMS.Classes
{
    public class ConnectionsDB
    {
        public List<DateTime> DataHoraTemp { get; } = new List<DateTime>();
        public List<double> LogadosTemp { get; } = new List<double>();
        public List<double> ColaboradoresTemp { get; } = new List<double>();
        public List<double> TotalLogadosTemp { get; } = new List<double>();

        public ConnectionsDB() { }
    }
}
