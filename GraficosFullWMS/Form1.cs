using LiveCharts;
using LiveCharts.Wpf;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        private List<DateTime> DataHora = new List<DateTime>();
        private List<int> Logados = new List<int>();

        public Form1()
        {
            InitializeComponent();


            elementHost1.Visible = false;
            cartesianChart1.Zoom = ZoomingOptions.None;
            cartesianChart1.Pan = PanningOptions.None;
            cartesianChart1.DisableAnimations = false;

            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();


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

        private void CriaGrafico()
        {

            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();

            ExibirBarraProgresso(100, 50);

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

                ConnectionToDB(this.connectionString, this.dataInicio, this.dataFim, this.UsuarioId);

            }
            else
            {

                MessageBox.Show("Você precisa primeiro se conectar ao banco de dados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }

        public void ConnectionToDB(string connectionString, string dataInicio, string dataFim, int UsuarioId)
        {

            try
            {

                using (OracleConnection connection = new OracleConnection(connectionString))
                {

                    connection.Open();

                    OracleCommand command = new OracleCommand("prc_fullwms_licencas", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_tipo", OracleDbType.BinaryFloat, ParameterDirection.Input).Value = 1;
                    command.Parameters.Add("data_inicio", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataInicio;
                    command.Parameters.Add("data_fim", OracleDbType.Varchar2, ParameterDirection.Input).Value = dataFim;

                    OracleParameter cursorParameter = new OracleParameter("cursorParameter", OracleDbType.RefCursor);
                    cursorParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(cursorParameter);

                    command.ExecuteNonQuery();

                    using (OracleDataReader reader = ((OracleRefCursor)cursorParameter.Value).GetDataReader())
                    {

                        while (reader.Read())
                        {

                            DateTime coluna1 = reader.GetDateTime(0);
                            int coluna2 = reader.GetInt32(1);

                            DataHora.Add(coluna1);
                            Logados.Add(coluna2);

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

                                string procedureVerify2 = "select count(1) from user_procedures o where upper(o.object_type) = 'FUNCTION' and upper(o.object_name) = 'FNC_USU_LOG'";

                                OracleCommand commandVerify2 = new OracleCommand(procedureVerify2, connection);
                                commandVerify2.CommandType = CommandType.Text;

                                using (OracleDataReader reader2 = commandVerify2.ExecuteReader())
                                {

                                    if (reader2.Read())
                                    {

                                        int contador2 = reader2.GetInt32(0);

                                        if (contador2 < 1)
                                        {


                                            string fncString = "create or replace function fnc_usu_log(p_usu_id in number,\n" +
                                            "                                       p_colab  in number,\n" +
                                            "                                       p_data   in date) return number is\n" +
                                            "   v_aux number;\n" +
                                            "begin\n" +
                                            "   if p_usu_id is not null then\n" +
                                            "      select 1\n" +
                                            "        into v_aux\n" +
                                            "        from ger_usuarios_logados l\n" +
                                            "       where l.ger_usuario_id = p_usu_id\n" +
                                            "         and p_data between l.dthr and nvl(l.dthr_saida, sysdate)\n" +
                                            "         and rownum < 2;\n" +
                                            "      return(1);\n" +
                                            "   else\n" +
                                            "      select 1\n" +
                                            "        into v_aux\n" +
                                            "        from wms_colaboradores_logados l\n" +
                                            "       where l.colab_cod_colab = p_colab\n" +
                                            "         and p_data between l.dthr_ent and nvl(l.dthr_saida, sysdate)\n" +
                                            "         and rownum < 2;\n" +
                                            "      return(1);\n" +
                                            "   end if;\n" +
                                            "exception\n" +
                                            "   when no_data_found then\n" +
                                            "      return 0;\n" +
                                            "end;";

                                            OracleCommand commandFNC = new OracleCommand(fncString, connection);
                                            commandFNC.CommandType = CommandType.Text;

                                            commandFNC.ExecuteNonQuery();

                                        }

                                    }

                                }

                                // Cria a Procedure FullWMSLincenças

                                string prcString = "create or replace procedure prc_fullwms_licencas(p_tipo      in integer,\n" +
                                "                                                 data_inicio in varchar2,\n" +
                                "                                                 data_fim    in varchar2,\n" +
                                "                                                 p_retorno   out sys_refcursor) is\n" +
                                "begin\n" +
                                "   if p_tipo = 1 then\n" +
                                "      open p_retorno for /* Logados - Hora / Minuto */\n" +
                                "         select dt,\n" +
                                "                count(logado) as logados\n" +
                                "           from (with tab as (select (to_date(data_inicio, 'DD/MM/YYYY') + (level - 1) / 24 / 60) dt\n" +
                                "                                from dual\n" +
                                "                              connect by level <= (to_date(data_fim, 'DD/MM/YYYY') -\n" +
                                "                                         to_date(data_inicio, 'DD/MM/YYYY')) * 24 * 60 + 1)\n" +
                                "                   select dt,\n" +
                                "                          ger_usuario_id,\n" +
                                "                          colab,\n" +
                                "                          fnc_usu_log(ger_usuario_id, colab, dt) as logado\n" +
                                "                     from tab\n" +
                                "                    cross join (select distinct l.ger_usuario_id,\n" +
                                "                                                null as colab\n" +
                                "                                  from ger_usuarios_logados l\n" +
                                "                                 where l.dthr > to_date(data_inicio, 'DD/MM/YYYY')\n" +
                                "                                   and (l.dthr_saida is null or\n" +
                                "                                       l.dthr_saida < to_date(data_fim, 'DD/MM/YYYY'))) usu\n" +
                                "                    cross join (select distinct null,\n" +
                                "                                                c.colab_cod_colab\n" +
                                "                                  from wms_colaboradores_logados c\n" +
                                "                                 where c.dthr_ent > to_date(data_inicio, 'DD/MM/YYYY')\n" +
                                "                                   and (c.dthr_saida is null or\n" +
                                "                                       c.dthr_saida < to_date(data_fim, 'DD/MM/YYYY'))) colab\n" +
                                "                    where fnc_usu_log(ger_usuario_id, colab, dt) > 0\n" +
                                "                    group by dt,\n" +
                                "                             ger_usuario_id,\n" +
                                "                             colab\n" +
                                "                    order by 1,\n" +
                                "                             2)\n" +
                                "                    group by dt\n" +
                                "                    order by 1;\n" +
                                "\n" +
                                "\n" +
                                "   elsif p_tipo = 2 then\n" +
                                "      /* Logados e Colaboradores - Hora / Minuto */\n" +
                                "      open p_retorno for\n" +
                                "         with tab as\n" +
                                "          (select (to_date(data_inicio, 'DD/MM/YYYY') + (level - 1) / 24 / 60) dt\n" +
                                "             from dual\n" +
                                "           connect by level <=\n" +
                                "                      (to_date(data_fim, 'DD/MM/YYYY') - to_date(data_inicio, 'DD/MM/YYYY')) * 24 * 60 + 1)\n" +
                                "         select dt,\n" +
                                "                ger_usuario_id,\n" +
                                "                colab,\n" +
                                "                fnc_usu_log(ger_usuario_id, colab, dt) as logado\n" +
                                "           from tab\n" +
                                "          cross join (select distinct l.ger_usuario_id,\n" +
                                "                                      null as colab\n" +
                                "                        from ger_usuarios_logados l\n" +
                                "                       where l.dthr > to_date(data_inicio, 'DD/MM/YYYY')\n" +
                                "                         and (l.dthr_saida is null or l.dthr_saida < to_date(data_fim, 'DD/MM/YYYY'))) usu\n" +
                                "          cross join (select distinct null,\n" +
                                "                                      c.colab_cod_colab\n" +
                                "                        from wms_colaboradores_logados c\n" +
                                "                       where c.dthr_ent > to_date(data_inicio, 'DD/MM/YYYY')\n" +
                                "                         and (c.dthr_saida is null or c.dthr_saida < to_date(data_fim, 'DD/MM/YYYY'))) colab\n" +
                                "          where fnc_usu_log(ger_usuario_id, colab, dt) > 0\n" +
                                "          group by dt,\n" +
                                "                   ger_usuario_id,\n" +
                                "                   colab\n" +
                                "          order by 1,\n" +
                                "                   2;\n" +
                                "\n" +
                                "   elsif p_tipo = 3 then\n" +
                                "      /* Logados - Hora / Minuto (Mostra apenas o MAX de logados) */\n" +
                                "      open p_retorno for\n" +
                                "         select dt,\n" +
                                "                sum(logado) as logados\n" +
                                "           from (with tab as (select (to_date(data_inicio, 'DD/MM/YYYY') + (level - 1) / 24 / 60) dt\n" +
                                "                                from dual\n" +
                                "                              connect by level <= (to_date(data_fim, 'DD/MM/YYYY') -\n" +
                                "                                         to_date(data_inicio, 'DD/MM/YYYY')) * 24 * 60 + 1)\n" +
                                "                   select dt,\n" +
                                "                          ger_usuario_id,\n" +
                                "                          colab,\n" +
                                "                          fnc_usu_log(ger_usuario_id, colab, dt) as logado\n" +
                                "                     from tab\n" +
                                "                    cross join (select distinct l.ger_usuario_id,\n" +
                                "                                                null as colab\n" +
                                "                                  from ger_usuarios_logados l\n" +
                                "                                 where l.dthr > to_date(data_inicio, 'DD/MM/YYYY')\n" +
                                "                                   and (l.dthr_saida is null or\n" +
                                "                                       l.dthr_saida < to_date(data_fim, 'DD/MM/YYYY'))) usu\n" +
                                "                    cross join (select distinct null,\n" +
                                "                                                c.colab_cod_colab\n" +
                                "                                  from wms_colaboradores_logados c\n" +
                                "                                 where c.dthr_ent > to_date(data_inicio, 'DD/MM/YYYY')\n" +
                                "                                   and (c.dthr_saida is null or\n" +
                                "                                       c.dthr_saida < to_date(data_fim, 'DD/MM/YYYY'))) colab\n" +
                                "                    where fnc_usu_log(ger_usuario_id, colab, dt) > 0\n" +
                                "                    group by dt,\n" +
                                "                             ger_usuario_id,\n" +
                                "                             colab\n" +
                                "                    order by 1,\n" +
                                "                             2)\n" +
                                "                    group by dt\n" +
                                "                   having sum(logado) > 1\n" +
                                "                    order by 1;\n" +
                                "\n" +
                                "\n" +
                                "   elsif p_tipo = 4 then\n" +
                                "      /* Logados - Hora */\n" +
                                "      open p_retorno for\n" +
                                "         select dt,\n" +
                                "                sum(logado) as logados\n" +
                                "           from (with tab as (select (to_date('02/05/2023', 'DD/MM/YYYY') + (level - 1) / 24) dt\n" +
                                "                                from dual\n" +
                                "                              connect by level <= (to_date('10/05/2023', 'DD/MM/YYYY') -\n" +
                                "                                         to_date('02/05/2023', 'DD/MM/YYYY')) * 24 * 60 + 1)\n" +
                                "                   select dt,\n" +
                                "                          ger_usuario_id,\n" +
                                "                          colab,\n" +
                                "                          fnc_usu_log(ger_usuario_id, colab, dt) as logado\n" +
                                "                     from tab\n" +
                                "                    cross join (select distinct l.ger_usuario_id,\n" +
                                "                                                null as colab\n" +
                                "                                  from ger_usuarios_logados l\n" +
                                "                                 where l.dthr > to_date('02/05/2023', 'DD/MM/YYYY')\n" +
                                "                                   and (l.dthr_saida is null or\n" +
                                "                                       l.dthr_saida < to_date('10/05/2023', 'DD/MM/YYYY'))) usu\n" +
                                "                    cross join (select distinct null,\n" +
                                "                                                c.colab_cod_colab\n" +
                                "                                  from wms_colaboradores_logados c\n" +
                                "                                 where c.dthr_ent > to_date('02/05/2023', 'DD/MM/YYYY')\n" +
                                "                                   and (c.dthr_saida is null or\n" +
                                "                                       c.dthr_saida < to_date('10/05/2023', 'DD/MM/YYYY'))) colab\n" +
                                "                    where fnc_usu_log(ger_usuario_id, colab, dt) > 0\n" +
                                "                    group by dt,\n" +
                                "                             ger_usuario_id,\n" +
                                "                             colab\n" +
                                "                    order by 1,\n" +
                                "                             2)\n" +
                                "                    group by dt\n" +
                                "                    order by 1;\n" +
                                "\n" +
                                "\n" +
                                "   else\n" +
                                "      /* Logados - Hora / Minuto (Padrão, volta para o caso 1 caso não seja nennhuma das outras opções) */\n" +
                                "      open p_retorno for\n" +
                                "         select dt,\n" +
                                "                sum(logado) as logados\n" +
                                "           from (with tab as (select (to_date(data_inicio, 'DD/MM/YYYY') + (level - 1) / 24 / 60) dt\n" +
                                "                                from dual\n" +
                                "                              connect by level <= (to_date(data_fim, 'DD/MM/YYYY') -\n" +
                                "                                         to_date(data_inicio, 'DD/MM/YYYY')) * 24 * 60 + 1)\n" +
                                "                   select dt,\n" +
                                "                          ger_usuario_id,\n" +
                                "                          colab,\n" +
                                "                          fnc_usu_log(ger_usuario_id, colab, dt) as logado\n" +
                                "                     from tab\n" +
                                "                    cross join (select distinct l.ger_usuario_id,\n" +
                                "                                                null as colab\n" +
                                "                                  from ger_usuarios_logados l\n" +
                                "                                 where l.dthr > to_date(data_inicio, 'DD/MM/YYYY')\n" +
                                "                                   and (l.dthr_saida is null or\n" +
                                "                                       l.dthr_saida < to_date(data_fim, 'DD/MM/YYYY'))) usu\n" +
                                "                    cross join (select distinct null,\n" +
                                "                                                c.colab_cod_colab\n" +
                                "                                  from wms_colaboradores_logados c\n" +
                                "                                 where c.dthr_ent > to_date(data_inicio, 'DD/MM/YYYY')\n" +
                                "                                   and (c.dthr_saida is null or\n" +
                                "                                       c.dthr_saida < to_date(data_fim, 'DD/MM/YYYY'))) colab\n" +
                                "                    where fnc_usu_log(ger_usuario_id, colab, dt) > 0\n" +
                                "                    group by dt,\n" +
                                "                             ger_usuario_id,\n" +
                                "                             colab\n" +
                                "                    order by 1,\n" +
                                "                             2)\n" +
                                "                    group by dt\n" +
                                "                    order by 1;\n" +
                                "\n" +
                                "\n" +
                                "   end if;\n" +
                                "end;";

                                OracleCommand commandPRC = new OracleCommand(prcString, connection);
                                commandPRC.CommandType = CommandType.Text;

                                commandPRC.ExecuteNonQuery();

                            }

                        }

                    }

                    connection.Close();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Erro ao gerar o código, tente novamente! {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show($"");
                return;

            }

        }

        private void ExibirBarraProgresso(int valorMaximo, int velocidadeBarraProgresso)
        {

            ProgressBar progressBar = new ProgressBar();
            progressBar.Maximum = valorMaximo;
            progressBar.Value = 0;
            progressBar.Visible = true;
            progressBar.Style = ProgressBarStyle.Continuous;

            for (int i = 0; i <= valorMaximo; i++)
            {

                progressBar.Value = i;
                Application.DoEvents();

                System.Threading.Thread.Sleep(velocidadeBarraProgresso);

            }

            progressBar.Visible = false;

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
