using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraficosFullWMS
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cartesianChart1 = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            this.customButtons1 = new GraficosFullWMS.Custom.CustomButtons();
            this.progressBar1 = new GraficosFullWMS.Custom.CustomProgressBar();
            this.button2 = new GraficosFullWMS.Custom.CustomButtons();
            this.button3 = new GraficosFullWMS.Custom.CustomButtons();
            this.button1 = new GraficosFullWMS.Custom.CustomButtons();
            this.label3 = new System.Windows.Forms.Label();
            this.customButtons2 = new GraficosFullWMS.Custom.CustomButtons();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(457, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Conecte-se ao banco de dados para gerar o Gráfico";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(988, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 19);
            this.label2.TabIndex = 8;
            this.label2.Text = "Progresso Atual:";
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cartesianChart1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cartesianChart1.Location = new System.Drawing.Point(1, 126);
            this.cartesianChart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(1815, 783);
            this.cartesianChart1.TabIndex = 13;
            this.cartesianChart1.Visible = false;
            this.cartesianChart1.DataPointerDown += new LiveChartsCore.Kernel.Events.ChartPointsHandler(this.OnPointerDown);
            // 
            // customButtons1
            // 
            this.customButtons1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.customButtons1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.customButtons1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.customButtons1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.customButtons1.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.customButtons1.BorderRadius = 20;
            this.customButtons1.BorderSize = 0;
            this.customButtons1.FlatAppearance.BorderSize = 0;
            this.customButtons1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButtons1.ForeColor = System.Drawing.Color.White;
            this.customButtons1.Image = global::GraficosFullWMS.Properties.Resources.icons8_help_40;
            this.customButtons1.Location = new System.Drawing.Point(1753, 60);
            this.customButtons1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.customButtons1.Name = "customButtons1";
            this.customButtons1.Size = new System.Drawing.Size(48, 46);
            this.customButtons1.TabIndex = 14;
            this.customButtons1.TextColor = System.Drawing.Color.White;
            this.customButtons1.UseVisualStyleBackColor = false;
            this.customButtons1.Click += new System.EventHandler(this.CustomButtons1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.ChannelColor = System.Drawing.Color.AliceBlue;
            this.progressBar1.ChannelHeight = 35;
            this.progressBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressBar1.ForeBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.progressBar1.ForeColor = System.Drawing.Color.White;
            this.progressBar1.Location = new System.Drawing.Point(992, 46);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.ShowMaximun = false;
            this.progressBar1.ShowValue = GraficosFullWMS.Custom.TextPosition.Sliding;
            this.progressBar1.Size = new System.Drawing.Size(343, 60);
            this.progressBar1.SliderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.progressBar1.SliderHeight = 20;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.SymbolAfter = "";
            this.progressBar1.SymbolBefore = "";
            this.progressBar1.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button2.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.button2.BorderRadius = 20;
            this.button2.BorderSize = 0;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(745, 46);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(241, 60);
            this.button2.TabIndex = 11;
            this.button2.Text = "Gerar Gráfico";
            this.button2.TextColor = System.Drawing.Color.White;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.OpenModalButton_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button3.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.button3.BorderRadius = 20;
            this.button3.BorderSize = 0;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(251, 46);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(241, 60);
            this.button3.TabIndex = 10;
            this.button3.Text = "Importar ou Remover";
            this.button3.TextColor = System.Drawing.Color.White;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.ImportaConfigs);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button1.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.button1.BorderRadius = 20;
            this.button1.BorderSize = 0;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(12, 46);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(233, 60);
            this.button1.TabIndex = 9;
            this.button1.Text = "Conectar ao Banco";
            this.button1.TextColor = System.Drawing.Color.White;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.MenuBar;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(-27, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(6944, 3);
            this.label3.TabIndex = 15;
            // 
            // customButtons2
            // 
            this.customButtons2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.customButtons2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.customButtons2.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.customButtons2.BorderRadius = 20;
            this.customButtons2.BorderSize = 0;
            this.customButtons2.FlatAppearance.BorderSize = 0;
            this.customButtons2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButtons2.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButtons2.ForeColor = System.Drawing.Color.White;
            this.customButtons2.Location = new System.Drawing.Point(498, 46);
            this.customButtons2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.customButtons2.Name = "customButtons2";
            this.customButtons2.Size = new System.Drawing.Size(241, 60);
            this.customButtons2.TabIndex = 16;
            this.customButtons2.Text = "Importar Dados";
            this.customButtons2.TextColor = System.Drawing.Color.White;
            this.customButtons2.UseVisualStyleBackColor = false;
            this.customButtons2.Click += new System.EventHandler(this.ImportaDados);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1816, 910);
            this.Controls.Add(this.customButtons2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.customButtons1);
            this.Controls.Add(this.cartesianChart1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gerador de Gráficos FullWMS Licenças";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Custom.CustomButtons button1;
        private Custom.CustomButtons button3;
        private Custom.CustomButtons button2;
        private Custom.CustomProgressBar progressBar1;
        private Custom.CustomButtons customButtons1;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart cartesianChart1;
        private Label label3;
        private Custom.CustomButtons customButtons2;
    }
}

