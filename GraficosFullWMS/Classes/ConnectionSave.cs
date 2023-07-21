using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficosFullWMS.Classes
{
    public class ConnectionSave
    {

        public string server { get; set; }
        public string porta { get; set; }
        public string dataBase { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }

        public ConnectionSave(string server, string porta, string dataBase, string usuario, string senha)
        {

            this.server = server;
            this.porta = porta;
            this.dataBase = dataBase;
            this.usuario = usuario;
            this.senha = senha;

        }

    }

}
