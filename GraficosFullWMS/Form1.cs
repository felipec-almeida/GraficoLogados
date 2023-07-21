using LiveCharts;
using LiveCharts.Wpf;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace GraficosFullWMS
{
    public partial class Form1 : Form
    {
        private string connectionString;

        private string dataInicio;
        private string dataFim;
        private int UsuarioId;
        private int CodEmpresa;

        private List<DateTime> DataHora = new List<DateTime>();
        private List<int> Logados = new List<int>();

        public Form1()
        {
            InitializeComponent();

            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();

            elementHost1.Visible = false;
            cartesianChart1.Zoom = ZoomingOptions.X;
            cartesianChart1.Pan = PanningOptions.X;
            cartesianChart1.DisableAnimations = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            this.connectionString = form2.ConnectionStringResult;
        }

        private async void CriaGrafico()
        {

            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();

            var progressoAtual = new Progress<int>(valorProgresso =>
            {

                progressBar1.Value = valorProgresso;

            });

            await ExibirBarraProgresso(100, progressoAtual);

            MessageBox.Show("Gráfico Gerado com Sucesso!", "Gráfico Gerado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            label2.Visible = false;
            progressBar1.Visible = false;

            var DataPoints = new ChartValues<int>(Logados);

            LineSeries series = new LineSeries
            {
                Title = "Logados",
                Values = DataPoints,
                PointGeometry = null,
                LineSmoothness = 1,
                Stroke = System.Windows.Media.Brushes.CornflowerBlue,
                StrokeThickness = 1.5,
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Data",
                Labels = DataHora.Select(data => data.ToString("dd/MM/yyyy HH:mm")).ToList(),
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Logados"
            });

            elementHost1.Visible = true;

            System.Windows.Media.Brush backgroundColor = new SolidColorBrush(Colors.White);
            cartesianChart1.Background = backgroundColor;

            cartesianChart1.Series.Add(series);

            Controls.Add(elementHost1);

        }
        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();

        }

        private void OpenModalButton_Click(object sender, EventArgs e)
        {

            //Validação para ver se a conexão está OK
            if (!string.IsNullOrEmpty(this.connectionString))
            {

                //Formulário para gerar o Gráfico
                //ConnectionToDB(this.connectionString, "02/05/2023", "06/05/2023", 1);
                Form3 form3 = new Form3();
                form3.ShowDialog();

                this.dataInicio = form3.DataInicio;
                this.dataFim = form3.DataFim;
                this.UsuarioId = form3.UsuarioId;
                this.CodEmpresa = form3.codEmpresa;

                ConnectionToDB(this.connectionString, this.dataInicio, this.dataFim, this.UsuarioId, this.CodEmpresa);

            }
            else
            {

                MessageBox.Show("Você precisa primeiro se conectar ao banco de dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        public void ConnectionToDB(string connectionString, string dataInicio, string dataFim, int UsuarioId, int CodEmpresa)
        {

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    connection.Open();

                    OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 1;
                    command.Parameters.Add("p_codemp", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = CodEmpresa;
                    command.Parameters.Add("p_data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                    command.Parameters.Add("p_data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

                    OracleParameter cursorParameter = new OracleParameter("cursorParameter", OracleDbType.RefCursor);
                    cursorParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(cursorParameter);

                    command.ExecuteNonQuery();

                    using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                    {

                        while (reader.Read())
                        {

                            //Retorna a DataInicio
                            DateTime coluna1 = reader.GetDateTime(0);

                            //Retorna quantidade de Logados
                            int coluna4 = reader.GetInt32(3);

                            DataHora.Add(coluna1);
                            Logados.Add(coluna4);

                        }

                        CriaGrafico();

                    }

                    connection.Close();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Erro ao gerar o gráfico, tente novamente! {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show($"");
                return;

            }

        }
        private void geraQuery()
        {

            try
            {

                using (OracleConnection connection = new OracleConnection(this.connectionString))
                {

                    connection.Open();

                    //Verifica se a Procedure prc_full_wms_licencas existe na Base Conectada.

                    string procedureVerify = "select count(1) from user_procedures o where upper(o.object_type) = 'PROCEDURE' and upper(o.object_name) = 'PRC_FULLWMS_LICENCAS'";

                    OracleCommand commandVerify = new OracleCommand(procedureVerify, connection);
                    commandVerify.CommandType = CommandType.Text;

                    using (OracleDataReader reader = commandVerify.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            int contador = reader.GetInt32(0);

                            if (contador < 1)
                            {

                                // Verifica a Existência da função fnc_usu_log

                                string procedureVerify2 = "select count(1) from user_procedures o where upper(o.object_type) = 'FUNCTION' and upper(o.object_name) = 'FNC_USU_LOG3'";

                                OracleCommand commandVerify2 = new OracleCommand(procedureVerify2, connection);
                                commandVerify2.CommandType = CommandType.Text;

                                using (OracleDataReader reader2 = commandVerify2.ExecuteReader())
                                {

                                    if (reader2.Read())
                                    {

                                        int contador2 = reader2.GetInt32(0);

                                        if (contador2 < 1)
                                        {

                                            string fncString = @"create or replace function fnc_usu_log3(p_tipo   in char,
                                        p_codemp in number,
                                        p_data   in date) return number is
                                       v_aux number := 0;
                                    begin
                                       if p_tipo in ('U', 'T') then
                                          select count(1) + v_aux
                                            into v_aux
                                            from ger_usuarios_logados l
                                           where l.empresa = p_codemp
                                             and p_data between l.dthr and nvl(l.dthr_saida, sysdate);
                                       end if;
                                       if p_tipo in ('C', 'T') then
                                          select count(1) + v_aux
                                            into v_aux
                                            from wms_colaboradores_logados l
                                           where l.empr_codemp = p_codemp
                                             and p_data between l.dthr_ent and nvl(l.dthr_saida, sysdate);
                                       end if;
                                       return v_aux;
                                    exception
                                       when no_data_found then
                                          return 0;
                                    end;
                                    ";

                                            OracleCommand commandFNC = new OracleCommand(fncString, connection);
                                            commandFNC.CommandType = CommandType.Text;

                                            commandFNC.ExecuteNonQuery();

                                        }

                                    }

                                }

                                // Cria a Procedure FullWMSLincenças

                                string prcString = @"create or replace procedure prc_fullwms_licencas(p_tipo        in number,
                                                 p_codemp      in number,
                                                 p_data_inicio in varchar2,
                                                 p_data_fim    in varchar2,
                                                 p_retorno     out sys_refcursor) is

                                   v_data_inicio date := to_date(p_data_inicio, 'DD/MM/YYYY');
                                   v_data_fim    date := to_date(p_data_fim, 'DD/MM/YYYY');

                                begin

                                   if p_tipo = 1 then
   
                                      open p_retorno for
                                         select l.dthr as data_entrada,
                                                l.dthr_saida as data_saida,
                                                l.ger_usuario_id as usuario_id,
                                                fnc_usu_log3('U', l.empresa, l.dthr) as usuarios_logados
                                           from ger_usuarios_logados l
                                          where l.empresa = p_codemp
                                            and l.dthr >= v_data_inicio
                                            and (l.dthr_saida is null or l.dthr < v_data_fim + 1)
                                          order by l.dthr asc;

                                   elsif p_tipo = 2 then
   
                                      open p_retorno for
      
                                         select c.dthr_ent as data_entrada,
                                                c.dthr_saida as data_saida,
                                                c.colab_cod_colab as colab_id,
                                                fnc_usu_log3('C', c.empr_codemp, c.dthr_ent) as colaboradores_logados
                                           from wms_colaboradores_logados c
                                          where c.empr_codemp = p_codemp
                                            and c.dthr_ent >= v_data_inicio
                                            and (c.dthr_saida is null or c.dthr_ent < v_data_fim + 1)
                                          order by c.dthr_ent;
          
                                   else
   
                                      open p_retorno for
      
                                         select x.*,
                                                to_char(max(x.total) over(partition by trunc(x.data_entrada))) || ' / ' ||
                                                to_char(max(x.total) over()) as max_diario
                                           from (select l.dthr           as data_entrada,
                                                        l.dthr_saida     as data_saida,
                                                        l.ger_usuario_id as usuario,
                                                        -- fnc_usu_log3('U', l.empresa, l.dthr) as usuarios_logados,
                                                        null as colab,
                                                        -- null as colabs_logados,
                                                        fnc_usu_log3('T', l.empresa, l.dthr) as total
                                                   from ger_usuarios_logados l
                                                  where l.empresa = p_codemp
                                                    and l.dthr >= v_data_inicio
                                                    and (l.dthr_saida is null or l.dthr < v_data_fim + 1)
                                                 union all
                                                 select c.dthr_ent,
                                                        c.dthr_saida,
                                                        null as usuario,
                                                        -- null              as usuarios_logados,
                                                        c.colab_cod_colab as colab_id,
                                                        -- fnc_usu_log3('C', c.empr_codemp, c.dthr_ent) as colaboradores_logados,
                                                        fnc_usu_log3('T', c.empr_codemp, c.dthr_ent) as total
                                                   from wms_colaboradores_logados c
                                                  where c.empr_codemp = p_codemp
                                                    and c.dthr_ent >= v_data_inicio
                                                    and (c.dthr_saida is null or c.dthr_ent < v_data_fim + 1)) x
                                          order by data_entrada asc;
   
                                   end if;

                                end;
                                ";

                                OracleCommand commandPRC = new OracleCommand(prcString, connection);
                                commandPRC.CommandType = CommandType.Text;

                                commandPRC.ExecuteNonQuery();

                            }
                            else
                            {

                                MessageBox.Show("Importante - Uma ou mais funções já estão importadas na base conectada!", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }

                        }

                    }

                    connection.Close();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Houve um erro ao gerar o código, tente novamente! {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        private async Task ExibirBarraProgresso(int valorMaximo, IProgress<int> progressoAtual)
        {

            progressBar1.Maximum = valorMaximo;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Continuous;

            for (int i = 0; i <= valorMaximo; i++)
            {

                progressBar1.Value = i;
                progressoAtual.Report(i);

                await Task.Delay(50);

            }

            progressBar1.Visible = false;

        }
        private void button3_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(this.connectionString))
            {

                geraQuery();
                MessageBox.Show("Procedure prc_fullwms_licencas e Function fnc_usu_log importadas na Base com sucesso!", "Importado!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {

                MessageBox.Show("Você precisa primeiro se conectar ao banco de dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }
    }
}
