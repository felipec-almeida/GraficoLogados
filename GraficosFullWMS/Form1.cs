using GraficosFullWMS.Classes;
using GraficosFullWMS.Dominio.File;
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
using System.IO;
using System.Linq;
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

        public List<ConnectionsDB> connectionsDB = new List<ConnectionsDB>();
        private List<DateTime> DataHora = new List<DateTime>(100000);
        private List<double> Logados = new List<double>(100000);
        private List<double> Colaboradores = new List<double>(100000);
        private List<double> TotalLogados = new List<double>(100000);
        private List<string> connectionsString = new List<string>(100000);
        private FileOperations<List<ConnectionSave>> fileOperations;

        //Lista Junta Gráficos
        private List<double> JuntaGraficosLogados = new List<double>(100000);
        private List<double> JuntaGraficosColaboradores = new List<double>(100000);
        private List<double> JuntaGraficosTotalLogados = new List<double>(100000);

        private bool juntaGrafico;

        public Form1()
        {
            InitializeComponent();
            fileOperations = new FileOperations<List<ConnectionSave>>(Path.Combine(Directory.GetCurrentDirectory(), "Files", "stringConnection.json"));
        }

        private void ConfigGrafico()
        {

            cartesianChart1.Visible = false;
            label2.Visible = false;
            progressBar1.Visible = false;
            cartesianChart1.HorizontalScroll.Enabled = true;
            cartesianChart1.ZoomingSpeed = 0.5;
            cartesianChart1.AnimationsSpeed = TimeSpan.FromMilliseconds(750);
            cartesianChart1.ZoomMode = ZoomAndPanMode.ZoomX;
            cartesianChart1.EasingFunction = EasingFunctions.CubicOut;
            cartesianChart1.TooltipPosition = TooltipPosition.Top;
            cartesianChart1.TooltipTextSize = 11;
            cartesianChart1.TooltipBackgroundPaint = new SolidColorPaint(SKColors.WhiteSmoke);
            cartesianChart1.TooltipTextPaint = new SolidColorPaint() { Color = SKColors.Black, FontFamily = "Arial" };
            cartesianChart1.LegendPosition = LegendPosition.Bottom;
            cartesianChart1.LegendTextPaint = new SolidColorPaint() { Color = SKColors.Black, FontFamily = "Arial" };
            cartesianChart1.LegendTextSize = 13;
            cartesianChart1.LegendBackgroundPaint = new SolidColorPaint(SKColors.WhiteSmoke);
            cartesianChart1.TooltipFindingStrategy = TooltipFindingStrategy.Automatic;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            clearData(true);

            Form2 form2 = new Form2();
            DialogResult result = form2.ShowDialog();

            if (result.Equals(DialogResult.Cancel))
            {
                form2.Close();
                return;
            }

            this.connectionString = form2.ConnectionStringResult;
            this.connectionsString.Add(form2.ConnectionName);
            label1.Text = form2.mensagemLabel;
            labelTemp = form2.mensagemLabel;
            cartesianChart1.Visible = false;

        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
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

                    foreach (var item in form3.connectionsToDB)
                    {
                        connectionsString.Add(item);
                    }

                    labelTemp = "Conectado às Bases: ";
                    foreach (var item in connectionsString)
                        labelTemp += $"{item}, ";

                    string labelTemp2 = Regex.Replace(labelTemp, @",(?=[^,]*,[^,]*$)", " e");
                    label1.Text = Regex.Replace(labelTemp2, @",(?=[^,]*$)", ".");
                    string json = fileOperations.Load();
                    var connectionObjects = JsonConvert.DeserializeObject<List<ConnectionSave>>(json);

                    for (int i = 1; i <= connectionsString.Count(); i++)
                    {
                        var selectedConnection = connectionObjects.FirstOrDefault(con => con.nomeConexao.Equals(connectionsString[i - 1]));
                        this.connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={selectedConnection.server})(PORT={selectedConnection.porta}))(CONNECT_DATA=(SERVICE_NAME={selectedConnection.dataBase})));User Id={selectedConnection.usuario};Password={selectedConnection.senha};";

                        if (connectionsString[i - 1].Equals(connectionsString.Last()))
                        {
                            isAdded = true;
                        }
                        await Task.WhenAny(ConnectionToDB(this.connectionString, this.dataInicio, this.dataFim, "5 - Junta Gráficos", this.p_codemp));
                    }
                }
                else
                {
                    await Task.WhenAny(ConnectionToDB(this.connectionString, this.dataInicio, this.dataFim, this.tipoRetorno, this.p_codemp));
                }
            }
            else
            {
                MessageBox.Show("Você precisa primeiro se conectar ao banco de dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label1.Text = "Conecte-se a uma Base para gerar o Gráfico";
                return;
            }

        }
        public async Task ConnectionToDB(string connectionString, string dataInicio, string dataFim, string Tipo, int empr_codemp)
        {
            try
            {
                ExecuteComponenets();

                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    Cursor.Current = Cursors.WaitCursor;

                    if (Tipo.Equals("1 - Usuários Logados"))
                    {
                        // MessageBox.Show("O Gráfico está sendo gerado, esta ação pode demorar um pouco.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        connection.Open();

                        OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 1;
                        command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                        command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

                        if (empr_codemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = p_codemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter("cursorParameter", OracleDbType.RefCursor);
                        cursorParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(cursorParameter);

                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            Logados.Clear();

                            while (reader.Read())
                            {
                                // Retorna a DataInicio
                                DateTime coluna1 = reader.GetDateTime(0);

                                // Retorna quantidade de Logados
                                int coluna4 = reader.GetInt32(4);
                                DataHora.Add(coluna1);
                                Logados.Add(coluna4);
                            }
                        }

                        connection.Close();

                        await Task.WhenAny(CriaGrafico("1 - Usuários Logados"));

                    }
                    else if (Tipo.Equals("2 - Colaboradores Logados"))
                    {
                        // MessageBox.Show("O Gráfico está sendo gerado, esta ação pode demorar um pouco.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        connection.Open();

                        OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 2;
                        command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                        command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

                        if (empr_codemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = p_codemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter("cursorParameter", OracleDbType.RefCursor);
                        cursorParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(cursorParameter);

                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            Colaboradores.Clear();

                            while (reader.Read())
                            {
                                // Retorna a DataInicio
                                DateTime coluna1 = reader.GetDateTime(0);

                                // Retorna quantidade de Logados
                                int coluna4 = reader.GetInt32(4);
                                DataHora.Add(coluna1);
                                Colaboradores.Add(coluna4);
                            }
                        }

                        connection.Close();

                        await Task.WhenAny(CriaGrafico("2 - Colaboradores Logados"));

                    }
                    else if (Tipo.Equals("3 - Total Logados"))
                    {
                        // MessageBox.Show("O Gráfico está sendo gerado, esta ação pode demorar um pouco.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        connection.Open();

                        OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 3;
                        command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                        command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

                        if (empr_codemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = p_codemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter("cursorParameter", OracleDbType.RefCursor);
                        cursorParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(cursorParameter);

                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            TotalLogados.Clear();

                            while (reader.Read())
                            {
                                // Retorna a DataInicio
                                DateTime coluna1 = reader.GetDateTime(0);

                                // Retorna quantidade de Colaboradores
                                int coluna5 = reader.GetInt32(5);
                                DataHora.Add(coluna1);
                                TotalLogados.Add(coluna5);
                            }
                        }

                        connection.Close();

                        await Task.WhenAny(CriaGrafico("3 - Total Logados"));

                    }
                    else if (Tipo.Equals("4 - Usuários/Colaboradores"))
                    {
                        // MessageBox.Show("O Gráfico está sendo gerado, esta ação pode demorar um pouco.", "Importante!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        connection.Open();

                        OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 4;
                        command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                        command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

                        if (empr_codemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = p_codemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter("cursorParameter", OracleDbType.RefCursor);
                        cursorParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(cursorParameter);
                        command.ExecuteNonQuery();

                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            DataHora.Clear();
                            Logados.Clear();
                            Colaboradores.Clear();
                            TotalLogados.Clear();

                            while (reader.Read())
                            {
                                // Retorna todas as colunas
                                // (DataInicio, Colaboradores, Usuários)
                                DateTime coluna1 = reader.GetDateTime(0);
                                string coluna2 = reader.GetString(1);
                                string coluna3 = reader.GetString(2);
                                double tempColuna2 = Convert.ToDouble(coluna2);
                                double tempColuna3 = Convert.ToDouble(coluna3);
                                DataHora.Add(coluna1);
                                Logados.Add(tempColuna2);
                                Colaboradores.Add(tempColuna3);
                                TotalLogados.Add((tempColuna2 + tempColuna3));
                            }
                        }

                        connection.Close();

                        await Task.WhenAny(CriaGrafico("4 - Usuários/Colaboradores"));
                    }
                    else if (Tipo.Equals("5 - Junta Gráficos"))
                    {

                        //Abre Duas Conexões
                        connection.Open();
                        OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 4;
                        command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                        command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

                        if (empr_codemp.Equals(0))
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = null;
                        }
                        else
                        {
                            command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = p_codemp;
                        }

                        OracleParameter cursorParameter = new OracleParameter("cursorParameter", OracleDbType.RefCursor);
                        cursorParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(cursorParameter);
                        command.ExecuteNonQuery();
                        using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                        {
                            var temp = new ConnectionsDB();
                            while (reader.Read())
                            {
                                DateTime coluna1 = reader.GetDateTime(0);
                                string coluna2 = reader.GetString(1);
                                string coluna3 = reader.GetString(2);
                                double tempColuna2 = Convert.ToDouble(coluna2);
                                double tempColuna3 = Convert.ToDouble(coluna3);
                                temp.DataHoraTemp.Add(coluna1);
                                temp.LogadosTemp.Add(tempColuna2);
                                temp.ColaboradoresTemp.Add(tempColuna3);
                                temp.TotalLogadosTemp.Add(tempColuna2 + tempColuna3);

                                if (isAdded.Equals(false))
                                    DataHora.Add(coluna1);

                            }

                            connectionsDB.Add(temp);
                            if (isAdded.Equals(true))
                            {

                                //Usuarios
                                try
                                {
                                    for (int i = 1; i <= connectionsDB.Count(); i++)
                                    {
                                        if (!JuntaGraficosLogados.Any())
                                        {
                                            foreach (var item in connectionsDB[i - 1].LogadosTemp)
                                            {
                                                JuntaGraficosLogados.Add(item);
                                            }
                                        }
                                        else
                                        {
                                            if (JuntaGraficosLogados.Count().Equals(connectionsDB[i - 1].LogadosTemp.Count()))
                                            {
                                                for (int j = 0; j < JuntaGraficosLogados.Count(); j++)
                                                {
                                                    JuntaGraficosLogados[j] += connectionsDB[i - 1].LogadosTemp[j];
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    MessageBox.Show("Erro, índice de usuários fora do limite.", "Erro", MessageBoxButtons.OK);
                                }

                                // Colaboradores
                                try
                                {
                                    for (int i = 1; i <= connectionsDB.Count(); i++)
                                    {
                                        if (!JuntaGraficosColaboradores.Any())
                                        {
                                            foreach (var item in connectionsDB[i - 1].ColaboradoresTemp)
                                            {
                                                JuntaGraficosColaboradores.Add(item);
                                            }
                                        }
                                        else
                                        {
                                            if (JuntaGraficosColaboradores.Count().Equals(connectionsDB[i - 1].ColaboradoresTemp.Count()))
                                            {
                                                for (int j = 0; j < JuntaGraficosColaboradores.Count(); j++)
                                                {
                                                    JuntaGraficosColaboradores[j] += connectionsDB[i - 1].ColaboradoresTemp[j];
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    MessageBox.Show("Erro, índice de usuários fora do limite.", "Erro", MessageBoxButtons.OK);
                                }

                                // Linha Suave de Total de Logados

                                try
                                {
                                    for (int i = 1; i <= connectionsDB.Count(); i++)
                                    {

                                        if (!JuntaGraficosTotalLogados.Any())
                                        {
                                            foreach (var item in connectionsDB[i - 1].TotalLogadosTemp)
                                            {
                                                JuntaGraficosTotalLogados.Add(item);
                                            }
                                        }
                                        else
                                        {
                                            if (JuntaGraficosTotalLogados.Count().Equals(connectionsDB[i - 1].TotalLogadosTemp.Count()))
                                            {
                                                for (int j = 0; j < JuntaGraficosTotalLogados.Count(); j++)
                                                {
                                                    JuntaGraficosTotalLogados[j] += connectionsDB[i - 1].TotalLogadosTemp[j];
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    MessageBox.Show("Erro, índice de usuários fora do limite.", "Erro", MessageBoxButtons.OK);
                                }
                            }
                        }

                        connection.Close();

                        if (isAdded.Equals(true))
                        {
                            await Task.WhenAny(CriaGrafico("5 - Junta Gráficos"));
                            isAdded = false;
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

        public async Task CriaGrafico(string tipo)
        {

            ConfigGrafico();

            var progressoAtual = new Progress<int>(valorProgresso =>
            {
                progressBar1.Value = valorProgresso;
            });

            BarraProgresso progressBar = new BarraProgresso();

            await progressBar.ExibirBarraProgresso(100, progressoAtual, progressBar1);

            if (tipo.Equals(null))
            {
                MessageBox.Show("Houve um erro ao gerar o gráfico, verifique os campos preenchidos e tente novamente!", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tipo.Equals("1 - Usuários Logados"))
            {
                cartesianChart1.Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name = "Usuários Logados",
                        Values = Logados,
                        GeometryFill = null,
                        GeometryStroke = null,
                        Fill = new SolidColorPaint() { Color = SKColors.Maroon.WithAlpha(25), StrokeThickness = 1.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.Maroon },
                        LineSmoothness = 1
                    }
                };

                cartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm:ss")).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                DataHora.Clear();

                cartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Usuários Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                cartesianChart1.BackColor = System.Drawing.Color.White;
                cartesianChart1.Visible = true;
                label2.Visible = false;
                progressBar1.Visible = false;

                Controls.Add(cartesianChart1);
                clearData(false);
            }
            else if (tipo.Equals("2 - Colaboradores Logados"))
            {
                cartesianChart1.Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name = "Colaboradores Logados",
                        Values = Colaboradores,
                        GeometryFill = null,
                        GeometryStroke = null,
                        Fill = new SolidColorPaint() { Color = SKColors.Maroon.WithAlpha(25), StrokeThickness = 1.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.Maroon },
                        LineSmoothness = 1
                    }
                };

                cartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm:ss")).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                DataHora.Clear();

                cartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Colaboradores Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                cartesianChart1.BackColor = System.Drawing.Color.White;
                cartesianChart1.Visible = true;
                label2.Visible = false;
                progressBar1.Visible = false;

                Controls.Add(cartesianChart1);
                clearData(false);
            }
            else if (tipo.Equals("3 - Total Logados"))
            {
                cartesianChart1.Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name = "Total Logados",
                        Values = TotalLogados,
                        GeometryFill = null,
                        GeometryStroke = null,
                        Fill = new SolidColorPaint() { Color = SKColors.Maroon.WithAlpha(25), StrokeThickness = 1.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.Maroon },
                        LineSmoothness = 1,
                    }
                };

                cartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm:ss")).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                DataHora.Clear();

                cartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Total Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                cartesianChart1.BackColor = System.Drawing.Color.White;
                cartesianChart1.Visible = true;
                label2.Visible = false;
                progressBar1.Visible = false;

                Controls.Add(cartesianChart1);
                clearData(false);
            }
            else if (tipo.Equals("4 - Usuários/Colaboradores"))
            {
                cartesianChart1.ZoomMode = ZoomAndPanMode.X;

                cartesianChart1.Series = new ISeries[]
                {
                    new StackedColumnSeries<double>
                    {
                        Name = "Usuários",
                        Values = Logados,
                        Fill = new SolidColorPaint() { Color = SKColors.CornflowerBlue, StrokeThickness = 4.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.DodgerBlue },
                        MaxBarWidth = 95,
                        DataLabelsSize = 7.5,
                        Padding = 7.5,
                        IsHoverable = true
                    },
                    new StackedColumnSeries<double>
                    {
                        Name = "Colaboradores",
                        Values = Colaboradores,
                        Fill = new SolidColorPaint() { Color = SKColors.PaleVioletRed, StrokeThickness = 4.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.IndianRed },
                        Padding = 7.5,
                        DataLabelsSize = 7.5,
                        MaxBarWidth = 95,
                        IsHoverable = true
                    },
                    new LineSeries<double>
                    {
                        Name = "Total de Logados",
                        Values = TotalLogados,
                        GeometryFill = new SolidColorPaint() { Color = SKColors.WhiteSmoke },
                        GeometryStroke = new SolidColorPaint() { Color = SKColors.Black, StrokeThickness = 1F },
                        GeometrySize = 8.5,
                        Fill = null,
                        LineSmoothness = 0.5,
                        Stroke = new SolidColorPaint() { Color = SKColors.Black, StrokeThickness = 1.25F }
                    }
                };

                cartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy")).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                cartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Máximo de Usuários e Colaboradores Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                cartesianChart1.BackColor = System.Drawing.Color.White;
                cartesianChart1.Visible = true;
                label2.Visible = false;
                progressBar1.Visible = false;

                Controls.Add(cartesianChart1);
                clearData(false);
            }
            else if (tipo.Equals("5 - Junta Gráficos"))
            {
                cartesianChart1.ZoomMode = ZoomAndPanMode.X;

                cartesianChart1.Series = new ISeries[]
                {
                    new StackedColumnSeries<double>
                    {
                        Name = "Usuários",
                        Values = JuntaGraficosLogados,
                        Fill = new SolidColorPaint() { Color = SKColors.CornflowerBlue, StrokeThickness = 4.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.DodgerBlue },
                        MaxBarWidth = 95,
                        DataLabelsSize = 7.5,
                        Padding = 7.5,
                        IsHoverable = true
                    },
                    new StackedColumnSeries<double>
                    {
                        Name = "Colaboradores",
                        Values = JuntaGraficosColaboradores,
                        Fill = new SolidColorPaint() { Color = SKColors.PaleVioletRed, StrokeThickness = 4.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.IndianRed },
                        Padding = 7.5,
                        DataLabelsSize = 7.5,
                        MaxBarWidth = 95,
                        IsHoverable = true
                    },
                    new LineSeries<double>
                    {
                        Name = "Total de Logados",
                        Values = JuntaGraficosTotalLogados,
                        GeometryFill = new SolidColorPaint() { Color = SKColors.WhiteSmoke },
                        GeometryStroke = new SolidColorPaint() { Color = SKColors.Black, StrokeThickness = 1F },
                        GeometrySize = 8.5,
                        Fill = null,
                        LineSmoothness = 1,
                        Stroke = new SolidColorPaint() { Color = SKColors.Black, StrokeThickness = 1.25F }
                    }
                };

                cartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy")).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                cartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Máximo de Usuários e Colaboradores Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                cartesianChart1.BackColor = System.Drawing.Color.White;
                cartesianChart1.Visible = true;
                label2.Visible = false;
                progressBar1.Visible = false;

                Controls.Add(cartesianChart1);
                clearData(true);
            }

            await Task.Delay(50);
            ExecuteComponenets(false);

        }
        private void importaConfigs(object sender, EventArgs e)
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

        private void clearData(bool clearConnections = false)
        {

            if (clearConnections.Equals(true))
            {
                this.connectionString = "";
                connectionsString.Clear();
            }

            connectionsDB.Clear();
        }

        private void OnPointerDown(LiveChartsCore.Kernel.Sketches.IChartView chart, IEnumerable<ChartPoint> points)
        {

            if (tipoRetorno != "4 - Usuários/Colaboradores" && tipoRetorno != "5 - Junta Gráficos")
            {
                var Axis = cartesianChart1.XAxes.FirstOrDefault();

                if (points.FirstOrDefault().Equals(null))
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
    }
}
