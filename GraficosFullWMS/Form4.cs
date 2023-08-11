using GraficosFullWMS.Classes;
using GraficosFullWMS.Dominio.File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraficosFullWMS
{
    public partial class Form4 : Form
    {

        private FileOperations<List<ConnectionSave>> fileOperations;
        public List<string> connectionsStrings = new List<string>();

        public Form4()
        {
            InitializeComponent();

            fileOperations = new FileOperations<List<ConnectionSave>>(Path.Combine(Directory.GetCurrentDirectory(), "Files", "stringConnection.json"));

            JsonReaderFile();


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

                        comboBoxConnections.Items.Add(connection.nomeConexao);

                    }
                }

            }
            catch (Exception)
            {

                comboBoxConnections.Text = "Nenhuma Conexão Selecionada";

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                string nameConnection = comboBoxConnections.SelectedItem.ToString();

                if (!nameConnection.Equals(null) || !nameConnection.Equals(""))
                {

                    connectionsStrings.Add(nameConnection);
                    MessageBox.Show($"Conexão {nameConnection} adicionada!", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {

                    throw new Exception();

                }

            }
            catch (Exception error)
            {

                MessageBox.Show($"Erro - Não é possível adicionar um valor nulo! {error.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }

        private void customButtons1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
