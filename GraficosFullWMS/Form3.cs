using System;
using System.Windows.Forms;

namespace GraficosFullWMS
{
    public partial class Form3 : Form
    {

        public string DataInicial { get; private set; }
        public string DataFinal { get; private set; }
        public int TipoRetorno { get; private set; }

        public Form3()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            DateTime? dataInicialTemp = DataInicio.Value;
            DateTime? dataFinalTemp = DataFim.Value;

            //Verificação de Entrada das Datas
            if (dataInicialTemp.HasValue && dataFinalTemp.HasValue)
            {

                DataInicial = dataInicialTemp.Value.ToString("dd/MM/yyyy");
                DataFinal = dataFinalTemp.Value.ToString("dd/MM/yyyy");

            }
            else if (dataInicialTemp.HasValue && !dataFinalTemp.HasValue)
            {

                DataInicial = dataInicialTemp.Value.ToString("dd/MM/yyyy");
                DataFinal = DateTime.Now.ToString("dd/MM/yyyy");

            }
            else
            {

                MessageBox.Show("Erro - Data Inicial não pode estar nula.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            try
            {

                string tipoRetorno = Tipo.SelectedItem.ToString();

                if (int.Parse(tipoRetorno) > 3 && int.Parse(tipoRetorno) == 0)
                {

                    MessageBox.Show($"Erro - Valor selecionado não permitido, tente novamente.", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                else if (tipoRetorno == null)
                {

                    throw new NullReferenceException(Tipo.SelectedItem.ToString());

                }

                TipoRetorno = int.Parse(tipoRetorno);

            }
            catch (NullReferenceException error)
            {

                MessageBox.Show($"Erro - O tipo do retorno não pode estar nulo! {error.Message}", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

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
