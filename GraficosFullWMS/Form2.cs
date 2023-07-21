using GraficosFullWMS.Classes;
using GraficosFullWMS.Dominio.File;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GraficosFullWMS
{
    public partial class Form2 : Form
    {

        public string ConnectionStringResult { get; private set; }

        private Connections connectionsSave = new Connections();
        private FileOperations<List<ConnectionSave>> fileOperations;

        public Form2()
        {

            InitializeComponent();
            Senha.UseSystemPasswordChar = true;

            //Texto padrão.
            portaConexao.Text = "Porta padrão: 1521";
            portaConexao.ForeColor = Color.Gray;
            fileOperations = new FileOperations<List<ConnectionSave>>(Path.Combine(Directory.GetCurrentDirectory(), "Files", "stringConnection.json"));

            JsonReaderFile();

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

            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={server})(PORT={porta}))(CONNECT_DATA=(SERVICE_NAME={dataBase})));User Id={usuario};Password={senha};";

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    connection.Open();
                    MessageBox.Show("Conexão feita com sucesso!");
                    ConnectionStringResult = connectionString;
                    connection.Close();


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

        private void ConnectionSaveDataBase(object sender, EventArgs e)
        {

            string Porta = portaConexao.Text;

            if (Porta == "Porta padrão: 1521")
            {

                MessageBox.Show("Importante - Nenhuma porta inserida, setando porta padrão: 1521", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information);
                portaConexao.Text = "1521";
                Porta = portaConexao.Text;

            }

            string Server = NomeServidor.Text;
            string DataBase = NomeDataBase.Text;
            string Usuario = NomeUsuario.Text;
            string UsuarioSenha = Senha.Text;

            if (comboBoxConnections.SelectedIndex >= 0 && this.connectionsSave.connections.FirstOrDefault(x => x.usuario == comboBoxConnections.SelectedItem.ToString()) != null)
            {

                MessageBox.Show("Erro - Já existe uma base salva com este mesmo nome, tente novamente!", "Erro ao Conectar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            ConnectionSave connectionJSON = new ConnectionSave(Server, Porta, DataBase, Usuario, UsuarioSenha);
            this.connectionsSave.connections.Add(connectionJSON);

            string json = JsonConvert.SerializeObject(this.connectionsSave.connections, Formatting.Indented);

            fileOperations.Save(json);
            // Abre arquivo JSON

            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Server})(PORT={Porta}))(CONNECT_DATA=(SERVICE_NAME={DataBase})));User Id={Usuario};Password={UsuarioSenha};";

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    connection.Open();
                    MessageBox.Show("Conexão feita com sucesso!");
                    ConnectionStringResult = connectionString;
                    connection.Close();


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            JsonReaderFile();

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

        private void JsonReaderFile()
        {

            try
            {

                string json = fileOperations.Load();

                var connectionsObject = JsonConvert.DeserializeObject<List<ConnectionSave>>(json);

                if (connectionsObject != null && connectionsObject.Count > 0)
                {
                    foreach (var connection in connectionsObject)
                    {
                        this.connectionsSave.connections.Add(connection);
                        comboBoxConnections.Items.Add(connection.usuario);

                    }
                }

            }
            catch (Exception)
            {

                //MessageBox.Show("Erro ao carregar as Conexões!", "Erro - Conexões", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBoxConnections.Text = "Nenhuma Conexão Salva";

            }


        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {

                var connection = this.connectionsSave.connections.FirstOrDefault(x => x.usuario == comboBoxConnections.SelectedItem.ToString());
                button3.Visible = true;

                this.NomeServidor.Text = connection.server;
                this.portaConexao.Text = connection.porta;
                this.NomeUsuario.Text = connection.usuario;
                this.NomeDataBase.Text = connection.dataBase;
                this.Senha.Text = connection.senha;

            }
            catch (Exception error)
            {

                MessageBox.Show($"Não foi possível encontrar uma base {NomeUsuario.Text}! Erro: {error.Message}", "Erro de conexão!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            };

        }

        private void RemoverBase(object sender, EventArgs e)
        {

            string json = fileOperations.Load();

            var connectionObject = JsonConvert.DeserializeObject<List<ConnectionSave>>(json);

        }
    }
}
