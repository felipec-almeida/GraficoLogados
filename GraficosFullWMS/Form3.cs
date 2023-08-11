using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GraficosFullWMS
{
    public partial class Form3 : Form
    {

        public string DataInicial { get; private set; }
        public string DataFinal { get; private set; }
        public string TipoRetorno { get; private set; }
        public int EmpCodemp { get; private set; }
        public bool juntaDadosGraficos { get; private set; }

        public List<string> connectionsToDB = new List<string>();

        public Form3()
        {
            InitializeComponent();

            var pastMonth = DateTime.Today.AddMonths(-1);
            var firstDayOfMonth = new DateTime(pastMonth.Year, pastMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            DataInicio.Value = firstDayOfMonth;
            DataFim.Value = lastDayOfMonth;

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

                if (tipoRetorno.Equals(""))
                {

                    MessageBox.Show($"{tipoRetorno} - Erro - Valor selecionado não permitido, tente novamente.", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                else if (tipoRetorno.Equals(null))
                {

                    throw new NullReferenceException(Tipo.SelectedItem.ToString());

                }

                TipoRetorno = tipoRetorno;

            }
            catch (NullReferenceException error)
            {

                MessageBox.Show($"Erro - O tipo do retorno não pode estar nulo! {error.Message}", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            try
            {

                if (Codemp.Text.Equals(null) || Codemp.Text.Equals(""))
                {

                    this.EmpCodemp = 0;

                }
                else
                {

                    int empr_codemp = Convert.ToInt32(Codemp.Text);

                    if (empr_codemp < 0)
                    {

                        throw new NullReferenceException(Tipo.SelectedItem.ToString());

                    }

                    this.EmpCodemp = empr_codemp;

                }



            }
            catch (NullReferenceException error)
            {

                MessageBox.Show($"Erro - O código da empresa não pode ser menor do que zero! {error.Message}", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (DataInicio != null && DataFim != null)
            {

                //Valor Padrão:
                juntaDadosGraficos = false;

                if (TipoRetorno.Equals("4 - Usuários/Colaboradores"))
                {

                    DialogResult result = MessageBox.Show("Deseja juntar os dados do relatório com o de outra base?", "Importante!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result.Equals(DialogResult.Yes))
                    {

                        juntaDadosGraficos = true;
                        Form4 form4 = new Form4();
                        form4.ShowDialog();

                        if (form4.DialogResult.Equals(DialogResult.Cancel))
                        {


                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                            return;

                        }

                        foreach (var item in form4.connectionsStrings)
                        {

                            connectionsToDB.Add(item);

                        }

                    }

                }

                this.DialogResult = DialogResult.OK;
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
