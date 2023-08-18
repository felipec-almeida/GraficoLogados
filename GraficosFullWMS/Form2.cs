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

        // Default Properties
        public string ConnectionStringResult { get; private set; }
        public string ConnectionName { get; private set; }
        public string mensagemLabel { get; private set; }

        private Connections connectionsSave = new Connections();
        private FileOperations<List<ConnectionSave>> fileOperations;

        public Form2()
        {

            InitializeComponent();
            portaConexao.Text = "Porta padrão: 1521";
            portaConexao.ForeColor = Color.Gray;
            fileOperations = new FileOperations<List<ConnectionSave>>(Path.Combine(Directory.GetCurrentDirectory(), "Files", "stringConnection.json"));
            JsonReaderFile();

            if (comboBoxConnections.SelectedIndex.Equals(-1))
            {
                comboBoxConnections.SelectedText = "Selecione uma Conexão";
            }

        }

        private void ConnectionDataBase(object sender, EventArgs e)
        {

            string porta;

            if (portaConexao.Text.Equals(null) && portaConexao.Text.Equals("Porta padrão: 1521".Trim()))
            {
                MessageBox.Show("Importante - Nenhuma porta inserida, setando porta padrão: 1521", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information);
                portaConexao.Text = "1521";
                porta = portaConexao.Text;
            }
            else
            {
                porta = portaConexao.Text;
            }

            string NomeConexao;

            if (nomeConexao.Text != null)
            {
                NomeConexao = nomeConexao.Text;
            }
            else
            {
                NomeConexao = NomeUsuario.Text;
            }

            string server = NomeServidor.Text;
            string dataBase = NomeDataBase.Text;
            string usuario = NomeUsuario.Text;
            string senha = Senha.Text;
            this.ConnectionName = nomeConexao.Text;
            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={server})(PORT={porta}))(CONNECT_DATA=(SERVICE_NAME={dataBase})));User Id={usuario};Password={senha};";

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Conexão feita com sucesso!");
                    ConnectionStringResult = connectionString;
                    connection.Close();
                    mensagemLabel = $"Conectado a Base: {NomeConexao}";
                    this.DialogResult = DialogResult.OK;
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

            if (Porta.Equals("Porta padrão: 1521"))
            {
                MessageBox.Show("Importante - Nenhuma porta inserida, setando porta padrão: 1521", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information);
                portaConexao.Text = "1521";
                Porta = portaConexao.Text;
            }

            string NomeConexao;

            if (nomeConexao.Text != null)
            {
                NomeConexao = nomeConexao.Text;
            }
            else
            {
                NomeConexao = NomeUsuario.Text;
            }

            string Server = NomeServidor.Text;
            string DataBase = NomeDataBase.Text;
            string Usuario = NomeUsuario.Text;
            string UsuarioSenha = Senha.Text;
            this.ConnectionName = nomeConexao.Text;

            try
            {

                string jsonTemp;

                if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Files", "stringConnection.json")))
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    jsonTemp = fileOperations.Load();
                }

                var connectionObject = JsonConvert.DeserializeObject<List<ConnectionSave>>(jsonTemp);

                if (connectionObject.Find(x => x.nomeConexao.Equals(NomeConexao)) != null)
                {

                    DialogResult result = MessageBox.Show("Aviso - Já existe uma base salva com este mesmo nome, deseja continuar mesmo assim?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result.Equals(DialogResult.Yes))
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
                                mensagemLabel = $"Conectado a Base: {NomeConexao.ToUpper()}";
                                Close();
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message} | {ex.InnerException}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        JsonReaderFile();
                        AtualizarBase();
                        this.DialogResult = DialogResult.OK;

                        return;
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    throw new ArgumentNullException();
                }

            }
            catch (ArgumentNullException)
            {
                MessageBox.Show($"Salvando Conexão {NomeConexao}...", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ConnectionSave connectionJSON = new ConnectionSave(NomeConexao, Server, Porta, DataBase, Usuario, UsuarioSenha);
            this.connectionsSave.connections.Add(connectionJSON);
            string json = JsonConvert.SerializeObject(this.connectionsSave.connections, Formatting.Indented);
            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Server})(PORT={Porta}))(CONNECT_DATA=(SERVICE_NAME={DataBase})));User Id={Usuario};Password={UsuarioSenha};";

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Conexão feita com sucesso!");
                    ConnectionStringResult = connectionString;
                    connection.Close();
                    mensagemLabel = $"Conectado a Base: {NomeConexao}";
                    this.DialogResult = DialogResult.OK;
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

            if (portaConexao.Text.Equals("Porta padrão: 1521"))
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

                if (connectionsObject.Equals(null))
                {
                    throw new Exception();
                }

                if (connectionsObject != null && connectionsObject.Count > 0)
                {
                    foreach (var connection in connectionsObject)
                    {
                        this.connectionsSave.connections.Add(connection);
                        comboBoxConnections.Items.Add(connection.nomeConexao);
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
                var connection = this.connectionsSave.connections.Find(x => x.nomeConexao.Equals(comboBoxConnections.SelectedItem.ToString()));
                button3.Enabled = true;
                this.nomeConexao.Text = connection.nomeConexao;
                this.NomeServidor.Text = connection.server;
                this.portaConexao.Text = connection.porta;
                this.NomeUsuario.Text = connection.usuario;
                this.NomeDataBase.Text = connection.dataBase;
                this.Senha.Text = connection.senha;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Não foi possível encontrar uma base {nomeConexao.Text}! Erro: {error.Message}", "Erro de conexão!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

        }

        private void AtualizarBase()
        {
            string json = fileOperations.Load();
            var connectionObjects = JsonConvert.DeserializeObject<List<ConnectionSave>>(json);

            try
            {
                var selectedConnection = connectionObjects.Find(con => con.nomeConexao.Equals(comboBoxConnections.SelectedItem.ToString()));
                int indexOfConnection = connectionObjects.IndexOf(selectedConnection);
                DialogResult result = MessageBox.Show($"Tem certeza que deseja atualizar a conexão {selectedConnection.nomeConexao}?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result.Equals(DialogResult.Yes))
                {
                    // Atualiza os dados da conexão
                    selectedConnection.nomeConexao = this.nomeConexao.Text;
                    selectedConnection.usuario = this.NomeUsuario.Text;
                    selectedConnection.dataBase = this.NomeDataBase.Text;
                    selectedConnection.porta = this.portaConexao.Text;
                    selectedConnection.server = this.NomeServidor.Text;
                    selectedConnection.senha = this.Senha.Text;

                    // Sobrescreve a conexão existente com os dados atualizados
                    connectionObjects[indexOfConnection].nomeConexao = selectedConnection.nomeConexao;
                    connectionObjects[indexOfConnection].usuario = selectedConnection.usuario;
                    connectionObjects[indexOfConnection].dataBase = selectedConnection.dataBase;
                    connectionObjects[indexOfConnection].porta = selectedConnection.porta;
                    connectionObjects[indexOfConnection].server = selectedConnection.server;
                    connectionObjects[indexOfConnection].senha = selectedConnection.senha;

                    // Sobrescreve o arquivo JSON com os dados atualizados
                    string newConnectionObject = JsonConvert.SerializeObject(connectionObjects, Formatting.Indented);
                    fileOperations.Override(newConnectionObject);
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show($"Conexão {selectedConnection.nomeConexao} foi atualizada com sucesso!", "Atualização feita com sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void RemoverDuplicarBase(object sender, EventArgs e)
        {

            string NomeConexao = nomeConexao.Text;
            string json = fileOperations.Load();
            var connectionObject = JsonConvert.DeserializeObject<List<ConnectionSave>>(json);
            MessageBoxManager.Yes = "Remover";
            MessageBoxManager.No = "Duplicar";
            MessageBoxManager.Register();
            DialogResult result1 = MessageBox.Show("Deseja Duplicar ou Remover a Conexão selecionada?", "Remover ou Duplicar Conexão", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            MessageBoxManager.Unregister();

            if (result1.Equals(DialogResult.Yes))
            {

                // Remove Conexão
                for (int i = 0; i < connectionObject.Count; i++)
                {

                    if (comboBoxConnections.SelectedItem.ToString().Equals(connectionObject[i].nomeConexao) && connectionsSave.connections.Find(x => x.usuario.Equals(connectionObject[i].usuario)) != null)
                    {

                        DialogResult result = MessageBox.Show($"Tem certeza que deseja remover a conexão {connectionObject[i--].nomeConexao}?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result.Equals(DialogResult.Yes))
                        {

                            MessageBox.Show($"Conexão {connectionObject[i + 1].nomeConexao} foi removida com sucesso!", "Remoção feita com sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                    comboBoxConnections.Items.Add(connection.nomeConexao);
                                }

                                comboBoxConnections.SelectedIndex = 0;

                            }

                            this.DialogResult = DialogResult.OK;
                            return;

                        }
                        else
                        {
                            return;
                        }

                    }

                }

            }
            else if (result1.Equals(DialogResult.No))
            {

                var connection = this.connectionsSave.connections.Find(x => x.nomeConexao.Equals(NomeConexao));
                try
                {

                    if (connection.Equals(null))
                    {
                        throw new NullReferenceException();
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Erro - Não é possível Duplicar uma Conexão com o mesmo nome, deseja atualiza-la?", "Importante!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.Yes))
                        {
                            AtualizarBase();
                        }
                    }

                }
                catch (NullReferenceException)
                {

                    DialogResult result = MessageBox.Show($"Tem certeza que deseja duplicar a conexão {NomeConexao}?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result.Equals(DialogResult.Yes))
                    {

                        string Porta = portaConexao.Text;
                        string Server = NomeServidor.Text;
                        string DataBase = NomeDataBase.Text;
                        string Usuario = NomeUsuario.Text;
                        string UsuarioSenha = Senha.Text;
                        ConnectionSave connectionJSON = new ConnectionSave(NomeConexao, Server, Porta, DataBase, Usuario, UsuarioSenha);
                        this.connectionsSave.connections.Add(connectionJSON);
                        string json2 = JsonConvert.SerializeObject(this.connectionsSave.connections, Formatting.Indented);
                        string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Server})(PORT={Porta}))(CONNECT_DATA=(SERVICE_NAME={DataBase})));User Id={Usuario};Password={UsuarioSenha};";

                        try
                        {

                            using (OracleConnection connection2 = new OracleConnection(connectionString))
                            {
                                connection2.Open();
                                MessageBox.Show("Conexão duplicada com sucesso!");
                                ConnectionStringResult = connectionString;
                                connection2.Close();
                                mensagemLabel = $"Conectado a Base: {NomeConexao.ToUpper()}";
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
                        fileOperations.Save(json2);

                    }

                }

            }


        }

    }
}
