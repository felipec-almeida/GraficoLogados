using Elskom.Generic.Libs;
using GraficosFullWMS.Classes;
using LiveCharts;
using LiveCharts.Wpf;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Syncfusion.Windows.Forms.Interop;

namespace GraficosFullWMS
{
    public partial class Form1 : Form
    {
        private string connectionString;

        private string dataInicio;
        private string dataFim;
        private string tipoRetorno;

        private List<DateTime> DataHora = new List<DateTime>();
        private List<double> Logados = new List<double>();
        private List<double> Colaboradores = new List<double>();
        private List<int> TotalLogados = new List<int>();

        public Form1()
        {

            InitializeComponent();

            elementHost1.Visible = false;
            cartesianChart1.Zoom = ZoomingOptions.X;
            cartesianChart1.Pan = PanningOptions.X;
            cartesianChart1.DisableAnimations = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2();
            form2.ShowDialog();
            this.connectionString = form2.ConnectionStringResult;
            label1.Text = form2.mensagemLabel;

            elementHost1.Visible = false;
            label2.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;

            DataHora.Clear();
            Logados.Clear();
            Colaboradores.Clear();
            TotalLogados.Clear();

            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();

        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
        }

        private void OpenModalButton_Click(object sender, EventArgs e)
        {

            elementHost1.Visible = false;
            label2.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;

            DataHora.Clear();
            Logados.Clear();
            Colaboradores.Clear();
            TotalLogados.Clear();

            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();

            //Validação para ver se a conexão está OK
            if (!string.IsNullOrEmpty(this.connectionString))
            {

                //Formulário para gerar o Gráfico
                Form3 form3 = new Form3();
                form3.ShowDialog();

                this.dataInicio = form3.DataInicial;
                this.dataFim = form3.DataFinal;
                this.tipoRetorno = form3.TipoRetorno;

                ConnectionToDB(this.connectionString, this.dataInicio, this.dataFim, this.tipoRetorno);

            }
            else
            {

                MessageBox.Show("Você precisa primeiro se conectar ao banco de dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        private async void CriaGrafico(string tipo)
        {

            if (tipo == null)
            {

                MessageBox.Show("Houve um erro ao gerar o gráfico, verifique os campos preenchidos e tente novamente!", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            if (tipo == "1 - Usuários Logados")
            {

                var progressoAtual = new Progress<int>(valorProgresso =>
                {

                    progressBar1.Value = valorProgresso;

                });

                await ExibirBarraProgresso(100, progressoAtual);

                MessageBox.Show("Gráfico Gerado com Sucesso!", "Gráfico Gerado", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    StrokeThickness = 1.5
                };

                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Data de Entrada",
                    Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm")).ToList(),
                });

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Usuários Logados"
                });

                elementHost1.Visible = true;

                System.Windows.Media.Brush backgroundColor = new SolidColorBrush(Colors.White);
                cartesianChart1.Background = backgroundColor;

                cartesianChart1.Series.Add(series);

                Controls.Add(elementHost1);

            }
            else if (tipo == "2 - Colaboradores Logados")
            {

                var progressoAtual = new Progress<int>(valorProgresso =>
                {

                    progressBar1.Value = valorProgresso;

                });

                await ExibirBarraProgresso(100, progressoAtual);

                MessageBox.Show("Gráfico Gerado com Sucesso!", "Gráfico Gerado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                label2.Visible = false;
                progressBar1.Visible = false;

                var DataPoints = new ChartValues<double>(Colaboradores);

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
                    Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm")).ToList(),
                });

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Colaboradores Logados"
                });

                elementHost1.Visible = true;

                System.Windows.Media.Brush backgroundColor = new SolidColorBrush(Colors.White);
                cartesianChart1.Background = backgroundColor;

                cartesianChart1.Series.Add(series);

                Controls.Add(elementHost1);

            }
            else if (tipo == "3 - Total Logados")
            {

                var progressoAtual = new Progress<int>(valorProgresso =>
                {

                    progressBar1.Value = valorProgresso;

                });

                await ExibirBarraProgresso(100, progressoAtual);

                MessageBox.Show("Gráfico Gerado com Sucesso!", "Gráfico Gerado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                label2.Visible = false;
                progressBar1.Visible = false;

                var DataPoints = new ChartValues<int>(TotalLogados);

                LineSeries series = new LineSeries
                {
                    Title = "Total de Logados",
                    Values = DataPoints,
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Stroke = System.Windows.Media.Brushes.CornflowerBlue,
                    StrokeThickness = 1.5
                };

                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Data de Entrada",
                    Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm")).ToList(),
                });

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "Total de Logados"
                });

                elementHost1.Visible = true;

                System.Windows.Media.Brush backgroundColor = new SolidColorBrush(Colors.White);
                cartesianChart1.Background = backgroundColor;

                cartesianChart1.Series.Add(series);

                Controls.Add(elementHost1);

            }

        }

        public void ConnectionToDB(string connectionString, string dataInicio, string dataFim, string Tipo)
        {

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    if (Tipo == "1 - Usuários Logados")
                    {

                        connection.Open();

                        OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 1;
                        command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                        command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

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

                                //Retorna a DataInicio
                                DateTime coluna1 = reader.GetDateTime(0);

                                //Retorna quantidade de Logados
                                int coluna4 = reader.GetInt32(4);


                                DataHora.Add(coluna1);

                                Logados.Add(coluna4);

                            }

                            CriaGrafico("1 - Usuários Logados");

                        }

                        connection.Close();

                    }
                    else if (Tipo == "2 - Colaboradores Logados")
                    {

                        connection.Open();

                        OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 2;
                        command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                        command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

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

                                //Retorna a DataInicio
                                DateTime coluna1 = reader.GetDateTime(0);

                                //Retorna quantidade de Logados
                                int coluna4 = reader.GetInt32(4);

                                DataHora.Add(coluna1);

                                Colaboradores.Add(coluna4);

                            }

                            CriaGrafico("2 - Colaboradores Logados");

                        }

                        connection.Close();

                    }
                    else if (Tipo == "3 - Total Logados")
                    {

                        connection.Open();

                        OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 3;
                        command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                        command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

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

                                //Retorna a DataInicio
                                DateTime coluna1 = reader.GetDateTime(0);

                                //Retorna quantidade de Colaboradores
                                int coluna5 = reader.GetInt32(5);

                                DataHora.Add(coluna1);
                                TotalLogados.Add(coluna5);

                            }

                            CriaGrafico("3 - Total Logados");

                        }

                        connection.Close();

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Erro ao gerar o gráfico, tente novamente! {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        private async Task ExibirBarraProgresso(int valorMaximo, IProgress<int> progressoAtual)
        {

            progressBar1.Maximum = valorMaximo;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Blocks;

            for (int i = 0; i <= valorMaximo; i++)
            {

                progressBar1.Value++;
                progressoAtual.Report(i);

                await Task.Delay(50);

            }

            progressBar1.Visible = false;

        }

        private void importaConfigs(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(this.connectionString))
            {

                //Classe que irá Gerar ou Remover as Querys
                ImportOrRemoveQuery IRQ = new ImportOrRemoveQuery(this.connectionString);

                MessageBoxManager.Yes = "Importar";
                MessageBoxManager.No = "Remover";
                MessageBoxManager.Cancel = "Cancelar";
                MessageBoxManager.Register();

                DialogResult result = MessageBox.Show("Deseja importar ou remover as querys da Base Conectada?", "Importante!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                MessageBoxManager.Unregister();

                if (result == DialogResult.Yes)
                {

                    IRQ.ImportQuery();

                }
                else if (result == DialogResult.Cancel)
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

    }
}
