using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraficosFullWMS.Classes
{
    public class BarraProgresso
    {

        private int valorMaximo;
        private IProgress<int> progressoAtual;

        public int ValorMaximo { get => valorMaximo; set => valorMaximo = value; }
        public IProgress<int> ProgressoAtual { get => progressoAtual; set => progressoAtual = value; }

        public BarraProgresso()
        {
        }

        public async Task ExibirBarraProgresso(int valorMaximo, IProgress<int> progressoAtual, ProgressBar progressBar1)
        {

            progressBar1.Maximum = valorMaximo;
            progressBar1.Value = 0;
            progressBar1.Visible = true;

            for (int i = 0; i <= valorMaximo; i++)
            {

                progressBar1.Value++;
                progressoAtual.Report(i);

                await Task.Delay(50);

            }

            progressBar1.Visible = false;

        }

    }
}
