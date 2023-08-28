﻿using System.Threading.Tasks;
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(402, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Conecte-se ao banco de dados para gerar o Gráfico";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(556, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Progresso Atual:";
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cartesianChart1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cartesianChart1.Location = new System.Drawing.Point(9, 90);
            this.cartesianChart1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(1342, 638);
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
            this.customButtons1.Location = new System.Drawing.Point(1315, 49);
            this.customButtons1.Margin = new System.Windows.Forms.Padding(2);
            this.customButtons1.Name = "customButtons1";
            this.customButtons1.Size = new System.Drawing.Size(36, 37);
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
            this.progressBar1.Location = new System.Drawing.Point(559, 37);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.ShowMaximun = false;
            this.progressBar1.ShowValue = GraficosFullWMS.Custom.TextPosition.Sliding;
            this.progressBar1.Size = new System.Drawing.Size(257, 49);
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
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(374, 37);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(181, 49);
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
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(188, 37);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(181, 49);
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
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(9, 37);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 49);
            this.button1.TabIndex = 9;
            this.button1.Text = "Conectar ao Banco";
            this.button1.TextColor = System.Drawing.Color.White;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 739);
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
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
    }
}

