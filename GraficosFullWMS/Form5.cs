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
using CarlosAg.ExcelXmlWriter;
using System.IO;
using LiveChartsCore.Kernel;
using System.ComponentModel.Composition.Primitives;
using System.Runtime.Remoting.Messaging;
using System.Globalization;

namespace GraficosFullWMS
{
    public partial class Form5 : Form
    {
        private DataTable resultados = new DataTable();
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

            switch (tipo)
            {
                case 1:
                    commandString = $@"
select gl.dthr,
       l.dthr_saida,
       gl.tipo,
       gl.empresa,
       gl.codigo
  from ger_logados gl
   join ger_usuarios_logados l
 on l.ger_usuariologado_id = gl.id_login
 where to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') between gl.dthr and nvl(l.dthr_saida - 1 / 24 / 60 / 60, sysdate + 1)
and gl.dthr >= to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') - 0.5 -- BOMBANA 
and gl.tipo = 'U'
order by 1";
                    break;
                case 2:
                    commandString = $@"
select gl.dthr,
       c.dthr_saida,
       gl.tipo,
       gl.empresa,
       gl.codigo
  from ger_logados gl
   join wms_colaboradores_logados c
 on c.colog_id = gl.id_login
 where to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') between gl.dthr and nvl(c.dthr_saida - 1 / 24 / 60 / 60, sysdate + 1)
and gl.dthr >= to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') - 0.5 -- BOMBANA 
and gl.tipo = 'C'
order by 1";
                    break;
                case 3:
                    commandString = $@"
select gl.dthr,
       (case
          when gl.tipo = 'U' then
           l.dthr_saida
          else
           c.dthr_saida
       end) as dthr_saida,
       gl.tipo,
       gl.empresa,
       gl.codigo,
       gl.logado,
       gl.total
  from ger_logados gl
  left join ger_usuarios_logados l
    on l.ger_usuariologado_id = gl.id_login
  left join wms_colaboradores_logados c
    on c.colog_id = gl.id_login
 where to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') between gl.dthr and
       nvl((case
              when gl.tipo = 'U' then
               l.dthr_saida
              else
               c.dthr_saida
           end) - 1 / 24 / 60 / 60, sysdate + 1)
  and gl.dthr >= to_date('{p_data}', 'DD/MM/YYYY HH24:MI:SS') - 0.5 -- BOMBANA
 order by 1
";
                    break;
                default:
                    MessageBox.Show("Houve um erro ao tentar executar o select, tente novamente.", "Erro Inesperado!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
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
