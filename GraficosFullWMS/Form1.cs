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
        private readonly List<double> JuntaGraficosLogados = new List<double>(100000);
        private readonly List<double> JuntaGraficosColaboradores = new List<double>(100000);
        private readonly List<double> JuntaGraficosTotalLogados = new List<double>(100000);
        private bool juntaGrafico;

        public Form1()
        {
            InitializeComponent();
            fileOperations = new FileOperations<List<ConnectionSave>>(
                Path.Combine(Directory.GetCurrentDirectory(), "Files", "stringConnection.json")
            );
        }

        private void Form1_Load(object sender, EventArgs e) { }

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

            connectionString = form2.ConnectionStringResult;
            connectionsString.Add(form2.ConnectionName);
            label1.Text = form2.MensagemLabel;
            labelTemp = form2.MensagemLabel;
            cartesianChart1.Visible = false;
        }

        private async void OpenModalButton_Click(object sender, EventArgs e)
        {
            string tempOriginalMessage = label1.Text;
            cartesianChart1.Visible = false;
            label2.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;

            // Validação para ver se a conexão está OK
            if (!string.IsNullOrEmpty(connectionString))
            {
                // Formulário para gerar o Gráfico
                Form3 form3 = new Form3();
                DialogResult result = form3.ShowDialog();

                if (result.Equals(DialogResult.Cancel))
                {
                    ExecuteComponenets(false);
                    return;
                }

                dataInicio = form3.DataInicial;
                dataFim = form3.DataFinal;
                tipoRetorno = form3.TipoRetorno;
                p_codemp = form3.EmpCodemp;
                juntaGrafico = form3.JuntaDadosGraficos;

                if (juntaGrafico.Equals(true))
                {
                    connectionsString.AddRange(form3.connectionsToDB);
                    labelTemp = "Conectado às Bases: ";
                    foreach (var item in connectionsString)
                        labelTemp += $"{item}, ";

                    string labelTemp2 = Regex.Replace(labelTemp, ",(?=[^,]*,[^,]*$)", " e");
                    label1.Text = Regex.Replace(labelTemp2, ",(?=[^,]*$)", ".");
                    string json = fileOperations.Load();
                    var connectionObjects = JsonConvert.DeserializeObject<List<ConnectionSave>>(
                        json
                    );

                    for (int i = 1; i <= connectionsString.Count; i++)
                    {
                        var selectedConnection = connectionObjects.SingleOrDefault(
                            con => con.nomeConexao.Equals(connectionsString[i - 1])
                        );
                        connectionString =
                            $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={selectedConnection.server})(PORT={selectedConnection.porta}))(CONNECT_DATA=(SERVICE_NAME={selectedConnection.dataBase})));User Id={selectedConnection.usuario};Password={selectedConnection.senha};";

                        if (connectionsString[i - 1].Equals(connectionsString.Last()))
                        {
                            isAdded = true;
                        }
                        label1.Text = "Aguarde, o gráfico está sendo gerado...";
                        await Task.WhenAny(ConnectionToDB("5 - Junta Gráficos"));
                    }
                }
                else
                {
                    label1.Text = "Aguarde, o gráfico está sendo gerado...";
                    await Task.Delay(25);
                    await Task.WhenAny(ConnectionToDB(tipoRetorno));
                }

                label1.Text = tempOriginalMessage;
            }
            else
            {
                MessageBox.Show(
                    "Você precisa primeiro se conectar ao banco de dados!",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                label1.Text = "Conecte-se a uma Base para gerar o Gráfico";
                return;
            }
        }

        public async Task ConnectionToDB(string Tipo)
        {
            try
            {
                ExecuteComponenets();

                if (Tipo.Equals("1 - Usuários Logados"))
                {
                    connectionToDB = new ConnectionToDB(
                        connectionString,
                        dataInicio,
                        dataFim,
                        Tipo,
                        p_codemp,
                        DataHora,
                        Logados,
                        Colaboradores,
                        TotalLogados
                    );
                    grafico = new Grafico(
                        cartesianChart1,
                        "1 - Usuários Logados",
                        DataHora,
                        Logados
                    );
                    await connectionToDB.Connect();
                    await CriaGrafico();
                }
                else if (Tipo.Equals("2 - Colaboradores Logados"))
                {
                    connectionToDB = new ConnectionToDB(
                        connectionString,
                        dataInicio,
                        dataFim,
                        Tipo,
                        p_codemp,
                        DataHora,
                        Logados,
                        Colaboradores,
                        TotalLogados
                    );
                    grafico = new Grafico(
                        cartesianChart1,
                        "2 - Colaboradores Logados",
                        DataHora,
                        Colaboradores
                    );
                    await connectionToDB.Connect();
                    await CriaGrafico();
                }
                else if (Tipo.Equals("3 - Total Logados"))
                {
                    connectionToDB = new ConnectionToDB(
                        connectionString,
                        dataInicio,
                        dataFim,
                        Tipo,
                        p_codemp,
                        DataHora,
                        Logados,
                        Colaboradores,
                        TotalLogados
                    );
                    grafico = new Grafico(
                        cartesianChart1,
                        "3 - Total Logados",
                        DataHora,
                        TotalLogados
                    );
                    await connectionToDB.Connect();
                    await CriaGrafico();
                }
                else if (Tipo.Equals("4 - Usuários/Colaboradores"))
                {
                    connectionToDB = new ConnectionToDB(
                        connectionString,
                        dataInicio,
                        dataFim,
                        Tipo,
                        p_codemp,
                        DataHora,
                        Logados,
                        Colaboradores,
                        TotalLogados
                    );
                    grafico = new Grafico(
                        cartesianChart1,
                        "4 - Usuários/Colaboradores",
                        DataHora,
                        Logados,
                        Colaboradores,
                        TotalLogados
                    );
                    await connectionToDB.Connect();
                    await CriaGrafico();
                }
                else if (Tipo.Equals("5 - Junta Gráficos"))
                {
                    connectionToDB = new ConnectionToDB(
                        connectionString,
                        dataInicio,
                        dataFim,
                        Tipo,
                        p_codemp,
                        DataHora,
                        Logados,
                        Colaboradores,
                        TotalLogados,
                        JuntaGraficosLogados,
                        JuntaGraficosColaboradores,
                        JuntaGraficosTotalLogados,
                        connectionsDB,
                        isAdded
                    );
                    await connectionToDB.Connect();

                    if (isAdded.Equals(true))
                    {
                        grafico = new Grafico(
                            cartesianChart1,
                            "5 - Junta Gráficos",
                            DataHora,
                            JuntaGraficosLogados,
                            JuntaGraficosColaboradores,
                            JuntaGraficosTotalLogados
                        );
                        await CriaGrafico();
                        isAdded = false;
                        ClearData(true);
                        return;
                    }
                }
                ExecuteComponenets(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao gerar o gráfico, tente novamente! {ex.Message} - {ex.InnerException} - {ex.StackTrace}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                ExecuteComponenets(false);
                return;
            }
        }

        public async Task CriaGrafico()
        {
            var progressoAtual = new Progress<int>(
                valorProgresso => progressBar1.Value = valorProgresso
            );
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
            if (!string.IsNullOrEmpty(connectionString))
            {
                ImportOrRemoveQuery IRQ = new ImportOrRemoveQuery(connectionString);
                MessageBoxManager.Yes = "Importar";
                MessageBoxManager.No = "Remover";
                MessageBoxManager.Cancel = "Cancelar";
                MessageBoxManager.Register();
                DialogResult result = MessageBox.Show(
                    "Deseja importar ou remover as querys da Base Conectada?",
                    "Importante!",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Information
                );
                MessageBoxManager.Unregister();

                if (result.Equals(DialogResult.Yes))
                    IRQ.ImportQuery();
                else if (result.Equals(DialogResult.Cancel))
                    return;
                else
                    IRQ.RemoveQuery();
            }
            else
            {
                MessageBox.Show(
                    "Você precisa primeiro se conectar ao banco de dados!",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
        }

        private void ImportaDados(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception();
                }

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    DialogResult result = MessageBox.Show(
                        $"Deseja copiar o Comando para Executar em uma Aba Teste na base {connection.InstanceName}?",
                        "Importante!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (result.Equals(DialogResult.Yes))
                    {
                        string commandCopy =
                            @"
pkg_wms_full_lic.prc_aud_ger_logados(p_tipo => 'T',
                                     p_data_inicio => null);
";
                        Clipboard.SetText(commandCopy);
                        MessageBox.Show("Comando copiado com sucesso!", "Comando Copiado!");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    $"Houve um Erro ao tentar se conectar a base, tente novamente!",
                    "Erro Inesperado!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
        }

        private void ExecuteComponenets(bool lockComponenets = true)
        {
            button1.Enabled = !lockComponenets;
            button2.Enabled = !lockComponenets;
            button3.Enabled = !lockComponenets;
            customButtons2.Enabled = !lockComponenets;
        }

        private void ClearData(bool clearConnections = false)
        {
            if (clearConnections.Equals(true))
            {
                connectionString = "";
                connectionsString.Clear();
            }

            connectionsDB.Clear();
        }

#pragma warning disable RCS1163
        private void OnPointerDown(
            LiveChartsCore.Kernel.Sketches.IChartView chart,
            IEnumerable<ChartPoint> points
        )
        {
            if (tipoRetorno != "4 - Usuários/Colaboradores" && tipoRetorno != "5 - Junta Gráficos")
            {
                var Axis = cartesianChart1.XAxes.FirstOrDefault();

                if (points.FirstOrDefault() == null)
                {
                    MessageBox.Show(
                        "Necessário selecionar um ponto válido",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                    );
                    return;
                }

                var dataClick = Axis.Labels[points.FirstOrDefault().Index];
                var ConnectionString = connectionString;
                int tipo = 3;
                double total = points.FirstOrDefault().Coordinate.PrimaryValue;

                switch (tipoRetorno)
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
                MessageBox.Show(
                    $"{ex.Message}!",
                    "Erro ao abrir Documentação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
        }
    }
}
