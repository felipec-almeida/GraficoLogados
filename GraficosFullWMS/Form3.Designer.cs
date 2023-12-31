﻿namespace GraficosFullWMS
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Codemp = new GraficosFullWMS.Custom.CustomTextBox();
            this.button1 = new GraficosFullWMS.Custom.CustomButtons();
            this.DataFim = new GraficosFullWMS.Custom.CustomDatePicker();
            this.DataInicio = new GraficosFullWMS.Custom.CustomDatePicker();
            this.Tipo = new GraficosFullWMS.Custom.CustomComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 242);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Retorne o Tipo de Retorno do Relatório:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 97);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Digite a Data Final: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Digite a Data Inicial:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 166);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(202, 18);
            this.label4.TabIndex = 14;
            this.label4.Text = "Digite o Código da Empresa: ";
            // 
            // Codemp
            // 
            this.Codemp.BackColor = System.Drawing.SystemColors.Window;
            this.Codemp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.Codemp.BorderColorFocus = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(12)))), ((int)(((byte)(20)))));
            this.Codemp.BorderSize = 2;
            this.Codemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Codemp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Codemp.Location = new System.Drawing.Point(12, 193);
            this.Codemp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Codemp.Name = "Codemp";
            this.Codemp.Padding = new System.Windows.Forms.Padding(7);
            this.Codemp.PasswordChar = false;
            this.Codemp.Size = new System.Drawing.Size(326, 35);
            this.Codemp.TabIndex = 13;
            this.Codemp.UnderlinedStyle = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.button1.BorderRadius = 20;
            this.button1.BorderSize = 0;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(54, 316);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(241, 46);
            this.button1.TabIndex = 12;
            this.button1.Text = "Gerar";
            this.button1.TextColor = System.Drawing.Color.White;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // DataFim
            // 
            this.DataFim.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.DataFim.BorderSize = 2;
            this.DataFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.DataFim.Location = new System.Drawing.Point(12, 118);
            this.DataFim.MinimumSize = new System.Drawing.Size(4, 35);
            this.DataFim.Name = "DataFim";
            this.DataFim.Size = new System.Drawing.Size(326, 35);
            this.DataFim.SkinColor = System.Drawing.SystemColors.Window;
            this.DataFim.TabIndex = 11;
            this.DataFim.TextColor = System.Drawing.Color.DimGray;
            this.DataFim.Value = new System.DateTime(2023, 7, 31, 0, 0, 0, 0);
            // 
            // DataInicio
            // 
            this.DataInicio.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.DataInicio.BorderSize = 2;
            this.DataInicio.CalendarTitleBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.DataInicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.DataInicio.Location = new System.Drawing.Point(12, 49);
            this.DataInicio.MinimumSize = new System.Drawing.Size(4, 35);
            this.DataInicio.Name = "DataInicio";
            this.DataInicio.Size = new System.Drawing.Size(326, 35);
            this.DataInicio.SkinColor = System.Drawing.SystemColors.Window;
            this.DataInicio.TabIndex = 10;
            this.DataInicio.TextColor = System.Drawing.Color.DimGray;
            this.DataInicio.Value = new System.DateTime(2023, 7, 1, 0, 0, 0, 0);
            // 
            // Tipo
            // 
            this.Tipo.BackColor = System.Drawing.SystemColors.Window;
            this.Tipo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.Tipo.BorderSize = 2;
            this.Tipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Tipo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Tipo.ForeColor = System.Drawing.Color.DimGray;
            this.Tipo.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.Tipo.Items.AddRange(new object[] {
            "1 - Usuários Logados",
            "2 - Colaboradores Logados",
            "3 - Total Logados",
            "4 - Usuários/Colaboradores"});
            this.Tipo.ListBackColor = System.Drawing.Color.White;
            this.Tipo.ListTextColor = System.Drawing.Color.DimGray;
            this.Tipo.Location = new System.Drawing.Point(12, 263);
            this.Tipo.MinimumSize = new System.Drawing.Size(200, 30);
            this.Tipo.Name = "Tipo";
            this.Tipo.Padding = new System.Windows.Forms.Padding(2);
            this.Tipo.Size = new System.Drawing.Size(326, 35);
            this.Tipo.TabIndex = 9;
            this.Tipo.Texts = "";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(350, 374);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Codemp);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DataFim);
            this.Controls.Add(this.DataInicio);
            this.Controls.Add(this.Tipo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gerar Gráfico";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Custom.CustomComboBox Tipo;
        private Custom.CustomDatePicker DataInicio;
        private Custom.CustomDatePicker DataFim;
        private Custom.CustomButtons button1;
        private Custom.CustomTextBox Codemp;
        private System.Windows.Forms.Label label4;
    }
}