namespace GraficosFullWMS.Classes
{
    public class ConnectionSave
    {
#pragma warning disable IDE1006
        public string nomeConexao { get; set; }
        public string server { get; set; }
        public string porta { get; set; }
        public string dataBase { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
#pragma warning restore IDE1006

        public ConnectionSave(string nomeConexao, string server, string porta, string dataBase, string usuario, string senha)
        {
            this.nomeConexao = nomeConexao;
            this.server = server;
            this.porta = porta;
            this.dataBase = dataBase;
            this.usuario = usuario;
            this.senha = senha;
        }
    }
}
