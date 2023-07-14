using System;
using System.Windows.Forms;

namespace GraficosFullWMS
{
    public partial class Form3 : Form
    {

        public string DataInicio { get; private set; }
        public string DataFim { get; private set; }
        public int UsuarioId { get; private set; }

        public Form3()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            DateTime? dataInicialTemp = DataInicial.Value;
            DateTime? dataFinalTemp = DataFinal.Value;


            //Verificação de Entrada das Datas
            if (dataInicialTemp.HasValue && dataFinalTemp.HasValue)
            {

                DataInicio = dataInicialTemp.Value.ToString("dd/MM/yyyy");
                DataFim = dataFinalTemp.Value.ToString("dd/MM/yyyy");

            }
            else if (dataInicialTemp.HasValue && !dataFinalTemp.HasValue)
            {

                DataInicio = dataInicialTemp.Value.ToString("dd/MM/yyyy");
                DataFim = DateTime.Now.ToString("dd/MM/yyyy");

            }
            else
            {

                MessageBox.Show("Erro - Data Inicial não pode estar nula.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            string idUsuario = CodUsuario.Text;

            if (idUsuario != null)
            {

                UsuarioId = int.Parse(idUsuario);

            }

            if (DataInicio != null && DataFim != null)
            {

                this.Close();

            }
            else
            {

                MessageBox.Show("Erro ao gerar o gráfico, tente novamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

        }
    }
}
