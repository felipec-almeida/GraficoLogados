using GraficosFullWMS.Classes;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraficosFullWMS
{
    public partial class Form5 : Form
    {
        private readonly string p_data;
        private readonly string connectionString;
        private readonly int tipo;
        private readonly double total;

        public Form5(string p_data, string connectionString, int tipo, double total)
        {
            InitializeComponent();
            this.p_data = p_data;
            this.connectionString = connectionString;
            this.tipo = tipo;
            this.total = total;
            label1.Text += $": {p_data}";

            switch (this.tipo)
            {
                case 1:
                    label2.Text += $" de Usuários Logados: {this.total}";
                    break;
                case 2:
                    label2.Text += $" de Colaboradores Logados: {this.total}";
                    break;
                case 3:
                    label2.Text += $" de Logados: {this.total}";
                    break;
            }

            LoadData();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
        }

        private void LoadData()
        {
            string commandString = string.Empty;

            if (tipo.Equals(1))
            {
                commandString = $@"
select count(l.ger_usuariologado_id) over() as logados,
       l.ger_usuario_id as usuario_id,
       l.empresa as empresa,
       l.dthr as entrada,
       l.dthr_saida as saida
  from ger_usuarios_logados l
 where to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') between l.dthr and
       nvl(l.dthr_saida - 1 / 24 / 60 / 60, sysdate + 1)
order by entrada";
            }
            else if (tipo.Equals(2))
            {
                commandString = $@"
select count(c.colab_cod_colab) over() as logados,
       c.colab_cod_colab as colaborador_id,
       c.empr_codemp as empresa,
       c.dthr_ent as entrada,
       c.dthr_saida as saida
  from wms_colaboradores_logados c
 where to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') between c.dthr_ent and
       nvl(c.dthr_saida - 1 / 24 / 60 / 60, sysdate + 1)
 order by entrada";
            }
            else if (tipo.Equals(3))
            {
                commandString = $@"
select 'W' as tipo,
       count(l.ger_usuariologado_id) over() as logados,
       l.ger_usuario_id as id,
       l.empresa as empresa,
       l.dthr as entrada,
       l.dthr_saida as saida
  from ger_usuarios_logados l
 where to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') between l.dthr and
       nvl(l.dthr_saida - 1 / 24 / 60 / 60, sysdate + 1)
union all
select 'C' as tipo,
       count(c.colab_cod_colab) over() as logados,
       c.colab_cod_colab as id,
       c.empr_codemp as empresa,
       c.dthr_ent as entrada,
       c.dthr_saida as saida
  from wms_colaboradores_logados c
 where to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') between c.dthr_ent and
       nvl(c.dthr_saida - 1 / 24 / 60 / 60, sysdate + 1)
 order by entrada
";
            }

            DataTable resultados = new DataTable();
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    OracleCommand command = new OracleCommand
                    {
                        CommandText = commandString,
                        Connection = connection,
                        CommandType = CommandType.Text
                    };
                    connection.Open();
                    OracleDataAdapter adapter = new OracleDataAdapter(command);
                    adapter.Fill(resultados);
                    DataGridLogados.DataSource = resultados;
                    DataGridLogados.AllowUserToResizeColumns = true;
                    DataGridLogados.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridLogados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Houve um erro, tente novamente. Erro - {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Escape))
            {
                this.Close();
            }
        }
    }
}
