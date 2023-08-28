using GraficosFullWMS.Classes;
using GraficosFullWMS.Dominio.File;
using GraficosFullWMS.Properties;
using LiveCharts.Definitions.Charts;
using LiveCharts.Helpers;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace GraficosFullWMS
{
    public partial class Form1 : Form
    {
        private string connectionString;
        private bool isAdded = false;
        private string labelTemp;
        private string dataInicio;
        private string dataFim;
        private string tipoRetorno;
        private int p_codemp;
        private Grafico grafico;
        private ConnectionToDB connectionToDB;

        public List<ConnectionsDB> connectionsDB = new List<ConnectionsDB>();
        private readonly List<DateTime> DataHora = new List<DateTime>(100000);
        private readonly List<double> Logados = new List<double>(100000);
        private readonly List<double> Colaboradores = new List<double>(100000);
        private readonly List<double> TotalLogados = new List<double>(100000);
        private readonly List<string> connectionsString = new List<string>(100000);
        private readonly FileOperations<List<ConnectionSave>> fileOperations;

        //Lista Junta Gráficos
        private readonly List<double> JuntaGraficosLogados = new List<double>(100000);
        private readonly List<double> JuntaGraficosColaboradores = new List<double>(100000);
        private readonly List<double> JuntaGraficosTotalLogados = new List<double>(100000);

        private bool juntaGrafico;

        public Form1()
        {
            InitializeComponent();
            fileOperations = new FileOperations<List<ConnectionSave>>(Path.Combine(Directory.GetCurrentDirectory(), "Files", "stringConnection.json"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            ClearData(true);
            Form2 form2 = new Form2();
            DialogResult result = form2.ShowDialog();

            if (result.Equals(DialogResult.Cancel))
            {
                form2.Close();
                return;
            }

            this.connectionString = form2.ConnectionStringResult;
            this.connectionsString.Add(form2.ConnectionName);
            label1.Text = form2.MensagemLabel;
            labelTemp = form2.MensagemLabel;
            cartesianChart1.Visible = false;
        }

        private async void OpenModalButton_Click(object sender, EventArgs e)
        {
            cartesianChart1.Visible = false;
            label2.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;

            // Validação para ver se a conexão está OK
            if (!string.IsNullOrEmpty(this.connectionString))
            {
                // Formulário para gerar o Gráfico
                Form3 form3 = new Form3();
                DialogResult result = form3.ShowDialog();

                if (result.Equals(DialogResult.Cancel))
                {
                    ExecuteComponenets(false);
                    return;
                }

                this.dataInicio = form3.DataInicial;
                this.dataFim = form3.DataFinal;
                this.tipoRetorno = form3.TipoRetorno;
                this.p_codemp = form3.EmpCodemp;
                this.juntaGrafico = form3.juntaDadosGraficos;

                if (juntaGrafico.Equals(true))
                {
                    connectionsString.AddRange(form3.connectionsToDB);
                    labelTemp = "Conectado às Bases: ";
                    foreach (var item in connectionsString)
                        labelTemp += $"{item}, ";

                    string labelTemp2 = Regex.Replace(labelTemp, ",(?=[^,]*,[^,]*$)", " e");
                    label1.Text = Regex.Replace(labelTemp2, ",(?=[^,]*$)", ".");
                    string json = fileOperations.Load();
                    var connectionObjects = JsonConvert.DeserializeObject<List<ConnectionSave>>(json);

                    for (int i = 1; i <= connectionsString.Count; i++)
                    {
                        var selectedConnection = connectionObjects.SingleOrDefault(con => con.nomeConexao.Equals(connectionsString[i - 1]));
                        this.connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={selectedConnection.server})(PORT={selectedConnection.porta}))(CONNECT_DATA=(SERVICE_NAME={selectedConnection.dataBase})));User Id={selectedConnection.usuario};Password={selectedConnection.senha};";

                        if (connectionsString[i - 1].Equals(connectionsString.Last()))
                        {
                            isAdded = true;
                        }
                        await Task.WhenAny(ConnectionToDB("5 - Junta Gráficos"));
                    }
                }
                else
                {
                    await Task.WhenAny(ConnectionToDB(this.tipoRetorno));
                }
            }
            else
            {
                MessageBox.Show("Você precisa primeiro se conectar ao banco de dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label1.Text = "Conecte-se a uma Base para gerar o Gráfico";
                return;
            }
        }
        public async Task ConnectionToDB(string Tipo)
        {
            try
            {
                ExecuteComponenets();

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Tipo.Equals("1 - Usuários Logados"))
                    {
                        connectionToDB = new ConnectionToDB(connectionString, dataInicio, dataFim, Tipo, p_codemp, DataHora, Logados, Colaboradores, TotalLogados);
                        await Task.WhenAny(connectionToDB.Connect());
                        grafico = new Grafico(cartesianChart1, "1 - Usuários Logados", DataHora, Logados);
                        await Task.WhenAny(CriaGrafico());
                    }
                    else if (Tipo.Equals("2 - Colaboradores Logados"))
                    {
                        connectionToDB = new ConnectionToDB(connectionString, dataInicio, dataFim, Tipo, p_codemp, DataHora, Logados, Colaboradores, TotalLogados);
                        await Task.WhenAny(connectionToDB.Connect());
                        grafico = new Grafico(cartesianChart1, "2 - Colaboradores Logados", DataHora, Colaboradores);
                        await Task.WhenAny(CriaGrafico());
                    }
                    else if (Tipo.Equals("3 - Total Logados"))
                    {
                        connectionToDB = new ConnectionToDB(connectionString, dataInicio, dataFim, Tipo, p_codemp, DataHora, Logados, Colaboradores, TotalLogados);
                        await Task.WhenAny(connectionToDB.Connect());
                        grafico = new Grafico(cartesianChart1, "3 - Total Logados", DataHora, TotalLogados);
                        await Task.WhenAny(CriaGrafico());
                    }
                    else if (Tipo.Equals("4 - Usuários/Colaboradores"))
                    {
                        connectionToDB = new ConnectionToDB(connectionString, dataInicio, dataFim, Tipo, p_codemp, DataHora, Logados, Colaboradores, TotalLogados);
                        await Task.WhenAny(connectionToDB.Connect());
                        grafico = new Grafico(cartesianChart1, "4 - Usuários/Colaboradores", DataHora, Logados, Colaboradores, TotalLogados);
                        await Task.WhenAny(CriaGrafico());
                    }
                    else if (Tipo.Equals("5 - Junta Gráficos"))
                    {
                        connectionToDB = new ConnectionToDB(connectionString, dataInicio, dataFim, Tipo, p_codemp, DataHora, Logados, Colaboradores, TotalLogados, JuntaGraficosLogados, JuntaGraficosColaboradores, JuntaGraficosTotalLogados, connectionsDB, isAdded);
                        await Task.WhenAny(connectionToDB.Connect());

                        if (isAdded.Equals(true))
                        {
                            grafico = new Grafico(cartesianChart1, "5 - Junta Gráficos", DataHora, JuntaGraficosLogados, JuntaGraficosColaboradores, JuntaGraficosTotalLogados);
                            await Task.WhenAny(CriaGrafico());
                            isAdded = false;
                            ClearData(true);
                            return;
                        }
                    }
                    ExecuteComponenets(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar o gráfico, tente novamente! {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExecuteComponenets(false);
                return;
            }
        }

        public async Task CriaGrafico()
        {
            var progressoAtual = new Progress<int>(valorProgresso => progressBar1.Value = valorProgresso);
            BarraProgresso progressBar = new BarraProgresso();
            await progressBar.ExibirBarraProgresso(100, progressoAtual, progressBar1);
            grafico.GeraGrafico();
            ExecuteComponenets(false);
            label2.Visible = false;
            progressBar1.Visible = false;
            Controls.Add(cartesianChart1);
            ClearData(false);
        }
        private void ImportaConfigs(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.connectionString))
            {
                // Classe que irá Gerar ou Remover as Querys
                ImportOrRemoveQuery IRQ = new ImportOrRemoveQuery(this.connectionString);
                MessageBoxManager.Yes = "Importar";
                MessageBoxManager.No = "Remover";
                MessageBoxManager.Cancel = "Cancelar";
                MessageBoxManager.Register();
                DialogResult result = MessageBox.Show("Deseja importar ou remover as querys da Base Conectada?", "Importante!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                MessageBoxManager.Unregister();

                if (result.Equals(DialogResult.Yes))
                {
                    IRQ.ImportQuery();
                }
                else if (result.Equals(DialogResult.Cancel))
                {
                    return;
                }
                else
                {
                    IRQ.RemoveQuery();
                }
            }
            else
            {
                MessageBox.Show("Você precisa primeiro se conectar ao banco de dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ExecuteComponenets(bool lockComponenets = true)
        {
            this.button1.Enabled = !lockComponenets;
            this.button2.Enabled = !lockComponenets;
            this.button3.Enabled = !lockComponenets;
        }

        private void ClearData(bool clearConnections = false)
        {
            if (clearConnections.Equals(true))
            {
                this.connectionString = "";
                connectionsString.Clear();
            }

            connectionsDB.Clear();
        }

#pragma warning disable RCS1163
        private void OnPointerDown(LiveChartsCore.Kernel.Sketches.IChartView chart, IEnumerable<ChartPoint> points)
        {
            if (tipoRetorno != "4 - Usuários/Colaboradores" && tipoRetorno != "5 - Junta Gráficos")
            {
                var Axis = cartesianChart1.XAxes.FirstOrDefault();

                if (points.FirstOrDefault() == null)
                {
                    MessageBox.Show("Necessário selecionar um ponto válido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var dataClick = Axis.Labels[points.FirstOrDefault().Index];
                var ConnectionString = this.connectionString;
                int tipo = 3;
                double total = points.FirstOrDefault().Coordinate.PrimaryValue;

                switch (this.tipoRetorno)
                {
                    case "1 - Usuários Logados":
                        tipo = 1;
                        break;
                    case "2 - Colaboradores Logados":
                        tipo = 2;
                        break;
                    case "3 - Total Logados":
                        tipo = 3;
                        break;
                }

                Form5 form5 = new Form5(dataClick, ConnectionString, tipo, total);
                DialogResult result = form5.ShowDialog();

                if (result.Equals(DialogResult.Cancel))
                {
                    form5.Close();
                    return;
                }
            }
            else
            {
                return;
            }
        }
#pragma warning restore RCS1163

        private void CustomButtons1_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] PDF = Resources.Manual_FullWMS;
                MemoryStream memoryStream = new MemoryStream(PDF);
                FileStream fileStream = new FileStream("Manual_FullWMS.pdf", FileMode.OpenOrCreate);
                memoryStream.WriteTo(fileStream);
                fileStream.Close();
                memoryStream.Close();
                Process.Start("Manual_FullWMS.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro ao abrir Documentação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
