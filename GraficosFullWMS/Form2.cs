using GraficosFullWMS.Classes;
using GraficosFullWMS.Dominio.File;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GraficosFullWMS
{
    public partial class Form2 : Form
    {

        public string ConnectionStringResult { get; private set; }
        public string mensagemLabel { get; private set; }

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

                    mensagemLabel = $"Conectado a Base: {usuario.ToUpper()}";
                    Close();


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

                DialogResult result = MessageBox.Show("Aviso - Já existe uma base salva com este mesmo nome, deseja continuar mesmo assim?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {

                    string ConnectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Server})(PORT={Porta}))(CONNECT_DATA=(SERVICE_NAME={DataBase})));User Id={Usuario};Password={UsuarioSenha};";

                    try
                    {

                        using (OracleConnection connection = new OracleConnection(ConnectionString))
                        {

                            connection.Open();
                            MessageBox.Show("Conexão feita com sucesso!");
                            ConnectionStringResult = ConnectionString;
                            connection.Close();

                            mensagemLabel = $"Conectado a Base: {Usuario.ToUpper()}";
                            Close();

                        }

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"{ex.Message} | {ex.InnerException}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                    JsonReaderFile();

                    //Atualiza o .JSON
                    AtualizarBase();

                    return;

                }
                else
                {

                    return;

                }

            }

            ConnectionSave connectionJSON = new ConnectionSave(Server, Porta, DataBase, Usuario, UsuarioSenha);
            this.connectionsSave.connections.Add(connectionJSON);

            string json = JsonConvert.SerializeObject(this.connectionsSave.connections, Formatting.Indented);

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

                    mensagemLabel = $"Conectado a Base: {Usuario.ToUpper()}";
                    Close();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            //Lê primeiramente o ComboBox.
            JsonReaderFile();

            // Salva o .JSON normalmente
            fileOperations.Save(json);

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

                if (connectionsObject == null)
                {

                    throw new Exception();

                }

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

                comboBoxConnections.Text = "Nenhuma Conexão Selecionada";

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

        private void AtualizarBase()
        {
            string json = fileOperations.Load();
            var connectionObjects = JsonConvert.DeserializeObject<List<ConnectionSave>>(json);

            // Procura pela conexão selecionada

            try
            {

                var selectedConnection = connectionObjects.FirstOrDefault(con => con.usuario == comboBoxConnections.SelectedItem.ToString());

                // Obtém o índice da conexão encontrada
                int indexOfConnection = connectionObjects.IndexOf(selectedConnection);

                DialogResult result = MessageBox.Show($"Tem certeza que deseja atualizar a conexão {selectedConnection.usuario}?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Atualiza os dados da conexão
                    selectedConnection.usuario = this.NomeUsuario.Text;
                    selectedConnection.dataBase = this.NomeDataBase.Text;
                    selectedConnection.porta = this.portaConexao.Text;
                    selectedConnection.server = this.NomeServidor.Text;
                    selectedConnection.senha = this.Senha.Text;

                    // Sobrescreve a conexão existente com os dados atualizados
                    connectionObjects[indexOfConnection].usuario = selectedConnection.usuario;
                    connectionObjects[indexOfConnection].dataBase = selectedConnection.dataBase;
                    connectionObjects[indexOfConnection].porta = selectedConnection.porta;
                    connectionObjects[indexOfConnection].server = selectedConnection.server;
                    connectionObjects[indexOfConnection].senha = selectedConnection.senha;

                    // Sobrescreve o arquivo JSON com os dados atualizados
                    string newConnectionObject = JsonConvert.SerializeObject(connectionObjects, Formatting.Indented);
                    fileOperations.Override(newConnectionObject);

                    MessageBox.Show($"Conexão {selectedConnection.usuario} foi atualizada com sucesso!", "Atualização feita com sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Conexão selecionada não encontrada!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }


        private void RemoverBase(object sender, EventArgs e)
        {

            string json = fileOperations.Load();
            var connectionObject = JsonConvert.DeserializeObject<List<ConnectionSave>>(json);

            for (int i = 0; i < connectionObject.Count; i++)
            {

                if (comboBoxConnections.SelectedItem.ToString() == connectionObject[i].usuario && connectionsSave.connections.FirstOrDefault(x => x.usuario.Equals(connectionObject[i].usuario)) != null)
                {

                    DialogResult result = MessageBox.Show($"Tem certeza que deseja remover a conexão {connectionObject[i--].usuario}?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {

                        MessageBox.Show($"Conexão {connectionObject[i + 1].usuario} foi removida com sucesso!", "Remoção feita com sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connectionObject.RemoveAt(i + 1);

                        //Sobrescreve com o conteúdo novo.
                        string newConnectionObject = JsonConvert.SerializeObject(connectionObject, Formatting.Indented);
                        fileOperations.Override(newConnectionObject);

                        //Atualiza o comboBox
                        if (connectionObject != null && connectionObject.Count > 0)
                        {

                            //Remove todos os índices antigos.
                            comboBoxConnections.DataSource = null;
                            comboBoxConnections.Items.Clear();

                            //Adiciona os novos índices.
                            foreach (var connection in connectionObject)
                            {
                                comboBoxConnections.Items.Add(connection.usuario);
                            }

                            comboBoxConnections.SelectedIndex = 0;

                        }

                        return;

                    }
                    else
                    {

                        return;

                    }

                }

            }

        }

    }
}
