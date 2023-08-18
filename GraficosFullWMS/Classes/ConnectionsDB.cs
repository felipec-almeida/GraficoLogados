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

        //public ConnectionsDB(List<DateTime> dataHora, List<double> logados, List<double> colaboradores, List<double> totalLogados)
        //{

        //    this.DataHoraTemp = dataHora;
        //    this.LogadosTemp = logados;
        //    this.ColaboradoresTemp = colaboradores;
        //    this.TotalLogadosTemp = totalLogados;

        //}

    }
}
