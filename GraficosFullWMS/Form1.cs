using GraficosFullWMS.Classes;
using GraficosFullWMS.Dominio.File;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
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
            elementHost1.Visible = false;
            label2.Visible = false;
            progressBar1.Visible = false;
            cartesianChart1.Pan = PanningOptions.None;
            cartesianChart1.DisableAnimations = true;
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
            elementHost1.Visible = false;

        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
        }

        private async void OpenModalButton_Click(object sender, EventArgs e)
        {
            elementHost1.Visible = false;
            label2.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;

            // Validação para ver se a conexão está OK
            if (!string.IsNullOrEmpty(this.connectionString))
            {
                cartesianChart1.Series.Clear();
                cartesianChart1.AxisX.Clear();
                cartesianChart1.AxisY.Clear();

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

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();

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

                cartesianChart1.ChartLegend = null;
                label2.Visible = false;
                progressBar1.Visible = false;

                var DataPoints = new ChartValues<double>(Logados);

                LineSeries series = new LineSeries
                {
                    Title = "Usuários Logados",
                    Values = DataPoints,
                    PointGeometry = null,
                    LineSmoothness = 0.2,
                    Stroke = System.Windows.Media.Brushes.CornflowerBlue,
                    StrokeThickness = 1.5,
                };


                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Data de Entrada",
                    Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm:ss")).ToList(),
                });

                DataHora.Clear();

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Usuários Logados"
                });

                elementHost1.Visible = true;

                System.Windows.Media.Brush backgroundColor = new SolidColorBrush(Colors.White);
                cartesianChart1.Background = backgroundColor;

                cartesianChart1.Series.Add(series);

                Controls.Add(elementHost1);

                clearData();

                await Task.Delay(50);

            }
            else if (tipo.Equals("2 - Colaboradores Logados"))
            {

                cartesianChart1.ChartLegend = null;
                label2.Visible = false;
                progressBar1.Visible = false;

                var DataPoints = new ChartValues<double>(Colaboradores);

                Colaboradores.Clear();

                LineSeries series = new LineSeries
                {
                    Title = "Colaboradores Logados",
                    Values = DataPoints,
                    PointGeometry = null,
                    LineSmoothness = 0.2,
                    Stroke = System.Windows.Media.Brushes.CornflowerBlue,
                    StrokeThickness = 1
                };

                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Data de Entrada",
                    Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm:ss")).ToList(),
                });

                DataHora.Clear();

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Colaboradores Logados"
                });

                elementHost1.Visible = true;

                System.Windows.Media.Brush backgroundColor = new SolidColorBrush(Colors.White);
                cartesianChart1.Background = backgroundColor;

                cartesianChart1.Series.Add(series);

                Controls.Add(elementHost1);

                clearData();

                await Task.Delay(50);

            }
            else if (tipo.Equals("3 - Total Logados"))
            {

                cartesianChart1.ChartLegend = null;
                label2.Visible = false;
                progressBar1.Visible = false;
                var DataPoints = new ChartValues<double>(TotalLogados);
                TotalLogados.Clear();

                LineSeries series = new LineSeries
                {
                    Title = "Total de Logados",
                    Values = DataPoints,
                    PointGeometry = null, //,
                    Stroke = System.Windows.Media.Brushes.CornflowerBlue,
                    StrokeThickness = 1.5,
                    DataLabels = false
                };

                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Data de Entrada",
                    Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm:ss")).ToList(),
                });

                DataHora.Clear();

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Total de Logados"
                });

                elementHost1.Visible = true;

                System.Windows.Media.Brush backgroundColor = new SolidColorBrush(Colors.White);
                cartesianChart1.Background = backgroundColor;

                cartesianChart1.Series.Add(series);

                Controls.Add(elementHost1);

                clearData();

                await Task.Delay(50);

            }
            else if (tipo.Equals("4 - Usuários/Colaboradores"))
            {

                cartesianChart1.ChartLegend = null;
                label2.Visible = false;
                progressBar1.Visible = false;

                SeriesCollection seriesCollection = new SeriesCollection();

                // Usuários
                var DataPointsUsuarios = new ChartValues<double>(Logados);
                Logados.Clear();

                StackedColumnSeries series1 = new StackedColumnSeries
                {
                    Title = "Usuários",
                    Values = DataPointsUsuarios,
                    Stroke = System.Windows.Media.Brushes.CornflowerBlue,
                    DataContext = true,
                    StackMode = StackMode.Values
                };

                seriesCollection.Add(series1);

                // Colaboradores
                var DataPointsColaboradores = new ChartValues<double>(Colaboradores);

                Colaboradores.Clear();

                StackedColumnSeries series2 = new StackedColumnSeries
                {
                    Title = "Colaboradores",
                    Values = DataPointsColaboradores,
                    Stroke = System.Windows.Media.Brushes.DarkOrange,
                    DataContext = true,
                    StackMode = StackMode.Values,
                };

                seriesCollection.Add(series2);

                // Linha Suave de Total de Logados
                var DataPointsTotalLogados = new ChartValues<double>(TotalLogados);

                TotalLogados.Clear();

                LineSeries series3 = new LineSeries
                {
                    Title = "Total de Logados",
                    Values = DataPointsTotalLogados,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 10,
                    PointForeground = System.Windows.Media.Brushes.Black,
                    Stroke = System.Windows.Media.Brushes.Black,
                    StrokeThickness = 0,
                    Fill = Brushes.Transparent,
                    LineSmoothness = 0,
                    DataContext = true
                };

                seriesCollection.Add(series3);

                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Data de Entrada",
                    Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy")).ToList(),
                });

                DataHora.Clear();

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Máximo de Usuários e Colaboradores"
                });

                elementHost1.Visible = true;
                elementHost1.Enabled = true;

                cartesianChart1.Background = System.Windows.Media.Brushes.White;
                cartesianChart1.DisableAnimations = false;
                cartesianChart1.Series = seriesCollection;

                Controls.Add(elementHost1);

                clearData();

                await Task.Delay(50);

            }
            else if (tipo.Equals("5 - Junta Gráficos"))
            {

                cartesianChart1.ChartLegend = null;
                label2.Visible = false;
                progressBar1.Visible = false;

                SeriesCollection seriesCollection = new SeriesCollection();

                // Usuários

                var DataPointsUsuarios = new ChartValues<double>(JuntaGraficosLogados);

                StackedColumnSeries series1 = new StackedColumnSeries
                {
                    Title = "Usuários",
                    Values = DataPointsUsuarios,
                    Stroke = System.Windows.Media.Brushes.CornflowerBlue,
                    DataContext = true,
                    StackMode = StackMode.Values
                };

                seriesCollection.Add(series1);

                //Colaboradores

                var DataPointsColaboradores = new ChartValues<double>(JuntaGraficosColaboradores);
                StackedColumnSeries series2 = new StackedColumnSeries
                {
                    Title = "Colaboradores",
                    Values = DataPointsColaboradores,
                    Stroke = System.Windows.Media.Brushes.DarkOrange,
                    DataContext = true,
                    StackMode = StackMode.Values,
                };

                seriesCollection.Add(series2);

                // Linha Suave de Total de Logados

                var DataPointsTotalLogados = new ChartValues<double>(JuntaGraficosTotalLogados);

                LineSeries series3 = new LineSeries
                {
                    Title = "Total entre as Duas Bases",
                    Values = DataPointsTotalLogados,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 10,
                    PointForeground = System.Windows.Media.Brushes.Black,
                    Stroke = System.Windows.Media.Brushes.Black,
                    StrokeThickness = 0,
                    Fill = Brushes.Transparent,
                    LineSmoothness = 0,
                    DataContext = true
                };

                seriesCollection.Add(series3);

                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Data de Entrada",
                    Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy")).ToList(),
                });

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Máximo de Usuários e Colaboradores"
                });

                elementHost1.Visible = true;
                elementHost1.Enabled = true;
                cartesianChart1.Background = System.Windows.Media.Brushes.White;
                cartesianChart1.DisableAnimations = false;
                cartesianChart1.Series = seriesCollection;

                Controls.Add(elementHost1);

                clearData(true);
                await Task.Delay(50);

            }

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
            JuntaGraficosColaboradores.Clear();
            JuntaGraficosLogados.Clear();
            JuntaGraficosTotalLogados.Clear();
        }

        private void elementHost1_ChartOnDataClick(object sender, ChartPoint p)
        {

            if (tipoRetorno != "4 - Usuários/Colaboradores" && tipoRetorno != "5 - Junta Gráficos")
            {
                var dataClick = cartesianChart1.AxisX[0].Labels[(int)p.X];
                var ConnectionString = this.connectionString;
                int tipo = 3;

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

                Form5 form5 = new Form5(dataClick, ConnectionString, tipo);
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
