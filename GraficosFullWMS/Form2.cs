using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace GraficosFullWMS
{
    public partial class Form2 : Form
    {

        public string ConnectionStringResult { get; private set; }
        public Form2()
        {
            InitializeComponent();
            Senha.UseSystemPasswordChar = true;

            //Texto padrão.
            portaConexao.Text = "Porta padrão: 1521";
            portaConexao.ForeColor = Color.Gray;

        }

        private void ConnectionDataBase(object sender, EventArgs e)
        {

            string porta;

            if (portaConexao.Text == null && portaConexao.Text == "Porta padrão: 1521")
            {

                MessageBox.Show("Importante - Nenhuma porta inserida, setando porta padrão: 1521", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information);
                portaConexao.Text = "1521";
                porta = portaConexao.Text;

            }
            else
            {

                porta = portaConexao.Text;

            }

            string server = NomeServidor.Text;
            string dataBase = NomeDataBase.Text;
            string usuario = NomeUsuario.Text;
            string senha = Senha.Text;

            //string server = "172.25.100.247";
            //string dataBase = "full11g";
            //string usuario = "r22sp11";
            //string senha = "r22sp11";

            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={server})(PORT={porta}))(CONNECT_DATA=(SERVICE_NAME={dataBase})));User Id={usuario};Password={senha};";

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    MessageBox.Show("Conexão feita com sucesso!");
                    ConnectionStringResult = connectionString;
                    this.Close();


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string porta;

            if (portaConexao.Text == null && portaConexao.Text == "Porta padrão: 1521")
            {

                MessageBox.Show("Importante - Nenhuma porta inserida, setando porta padrão: 1521", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information);
                portaConexao.Text = "1521";
                porta = portaConexao.Text;

            }
            else
            {

                porta = portaConexao.Text;

            }

            string server = NomeServidor.Text;
            string dataBase = NomeDataBase.Text;
            string usuario = NomeUsuario.Text;
            string senha = Senha.Text;

            ConnectionSave conexao = new ConnectionSave
            {

                server = server,
                porta = porta,
                dataBase = dataBase,
                usuario = usuario,
                senha = senha

            };

            //string server = "172.25.100.247";
            //string dataBase = "full11g";
            //string usuario = "r22sp11";
            //string senha = "r22sp11";

            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={server})(PORT={porta}))(CONNECT_DATA=(SERVICE_NAME={dataBase})));User Id={usuario};Password={senha};";

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    MessageBox.Show("Conexão feita com sucesso!");
                    ConnectionStringResult = connectionString;
                    this.Close();


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        private void portaConexao_Enter(object sender, EventArgs e)
        {

            if (portaConexao.Text == "Porta padrão: 1521")
            {

                portaConexao.Text = string.Empty;
                portaConexao.ForeColor = Color.Black;

            }

        }

        private void portaConexao_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(portaConexao.Text))
            {
                portaConexao.Text = "Porta padrão: 1521";
                portaConexao.ForeColor = Color.Gray;
            }

        }
    }

    public class ListConnectionSave
    {

        List<ConnectionSave> connections { get; set; }

    }
    public class ConnectionSave
    {

        public string server { get; set; }
        public string porta { get; set; }
        public string dataBase { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }

    }
}
