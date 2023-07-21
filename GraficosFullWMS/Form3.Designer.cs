namespace GraficosFullWMS
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.CodUsuario = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.DataInicial = new Syncfusion.WinForms.Input.SfDateTimeEdit();
            this.DataFinal = new Syncfusion.WinForms.Input.SfDateTimeEdit();
            this.CodEmpresa = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CodUsuario
            // 
            this.CodUsuario.Location = new System.Drawing.Point(12, 180);
            this.CodUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.CodUsuario.Name = "CodUsuario";
            this.CodUsuario.Size = new System.Drawing.Size(304, 22);
            this.CodUsuario.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 160);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Retorne o Tipo de Retorno do Relatório:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 87);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Digite a Data Final: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Digite a Data Inicial:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(89, 256);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 49);
            this.button1.TabIndex = 6;
            this.button1.Text = "Enviar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DataInicial
            // 
            this.DataInicial.Cursor = System.Windows.Forms.Cursors.Default;
            this.DataInicial.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask;
            this.DataInicial.Location = new System.Drawing.Point(12, 37);
            this.DataInicial.Name = "DataInicial";
            this.DataInicial.Size = new System.Drawing.Size(229, 35);
            this.DataInicial.TabIndex = 7;
            this.DataInicial.ToolTipText = "";
            // 
            // DataFinal
            // 
            this.DataFinal.Cursor = System.Windows.Forms.Cursors.Default;
            this.DataFinal.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask;
            this.DataFinal.Location = new System.Drawing.Point(12, 109);
            this.DataFinal.Name = "DataFinal";
            this.DataFinal.Size = new System.Drawing.Size(229, 35);
            this.DataFinal.TabIndex = 8;
            this.DataFinal.ToolTipText = "";
            // 
            // CodEmpresa
            // 
            this.CodEmpresa.Location = new System.Drawing.Point(12, 226);
            this.CodEmpresa.Margin = new System.Windows.Forms.Padding(4);
            this.CodEmpresa.Name = "CodEmpresa";
            this.CodEmpresa.Size = new System.Drawing.Size(304, 22);
            this.CodEmpresa.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 206);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Digite o Código da Empresa: ";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 317);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CodEmpresa);
            this.Controls.Add(this.DataFinal);
            this.Controls.Add(this.DataInicial);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CodUsuario);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form3";
            this.Text = "Gerar Gráfico";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox CodUsuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private Syncfusion.WinForms.Input.SfDateTimeEdit DataInicial;
        private Syncfusion.WinForms.Input.SfDateTimeEdit DataFinal;
        private System.Windows.Forms.TextBox CodEmpresa;
        private System.Windows.Forms.Label label4;
    }
}