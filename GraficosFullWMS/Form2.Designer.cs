namespace GraficosFullWMS
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxConnections = new GraficosFullWMS.Custom.CustomComboBox();
            this.Senha = new GraficosFullWMS.Custom.CustomTextBox();
            this.NomeUsuario = new GraficosFullWMS.Custom.CustomTextBox();
            this.portaConexao = new GraficosFullWMS.Custom.CustomTextBox();
            this.NomeDataBase = new GraficosFullWMS.Custom.CustomTextBox();
            this.NomeServidor = new GraficosFullWMS.Custom.CustomTextBox();
            this.button1 = new GraficosFullWMS.Custom.CustomButtons();
            this.button2 = new GraficosFullWMS.Custom.CustomButtons();
            this.button3 = new GraficosFullWMS.Custom.CustomButtons();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Digite o Host:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 171);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Digite a DataBase:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 397);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Digite o Usuário:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 247);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Digite a Senha:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 319);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 18);
            this.label5.TabIndex = 12;
            this.label5.Text = "Digite a Porta:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 20);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "Conexões Salvas:";
            // 
            // comboBoxConnections
            // 
            this.comboBoxConnections.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxConnections.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.comboBoxConnections.BorderSize = 2;
            this.comboBoxConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConnections.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.comboBoxConnections.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxConnections.IconColor = System.Drawing.Color.CornflowerBlue;
            this.comboBoxConnections.ListBackColor = System.Drawing.Color.White;
            this.comboBoxConnections.ListTextColor = System.Drawing.Color.DimGray;
            this.comboBoxConnections.Location = new System.Drawing.Point(16, 41);
            this.comboBoxConnections.MinimumSize = new System.Drawing.Size(200, 30);
            this.comboBoxConnections.Name = "comboBoxConnections";
            this.comboBoxConnections.Padding = new System.Windows.Forms.Padding(2);
            this.comboBoxConnections.Size = new System.Drawing.Size(251, 33);
            this.comboBoxConnections.TabIndex = 21;
            this.comboBoxConnections.Texts = "";
            this.comboBoxConnections.OnSelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Senha
            // 
            this.Senha.BackColor = System.Drawing.SystemColors.Window;
            this.Senha.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.Senha.BorderColorFocus = System.Drawing.Color.LightSkyBlue;
            this.Senha.BorderSize = 2;
            this.Senha.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Senha.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Senha.Location = new System.Drawing.Point(16, 419);
            this.Senha.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Senha.Name = "Senha";
            this.Senha.Padding = new System.Windows.Forms.Padding(7);
            this.Senha.PasswordChar = true;
            this.Senha.Size = new System.Drawing.Size(251, 35);
            this.Senha.TabIndex = 20;
            this.Senha.UnderlinedStyle = true;
            // 
            // NomeUsuario
            // 
            this.NomeUsuario.BackColor = System.Drawing.SystemColors.Window;
            this.NomeUsuario.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.NomeUsuario.BorderColorFocus = System.Drawing.Color.LightSkyBlue;
            this.NomeUsuario.BorderSize = 2;
            this.NomeUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NomeUsuario.ForeColor = System.Drawing.SystemColors.GrayText;
            this.NomeUsuario.Location = new System.Drawing.Point(16, 341);
            this.NomeUsuario.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NomeUsuario.Name = "NomeUsuario";
            this.NomeUsuario.Padding = new System.Windows.Forms.Padding(7);
            this.NomeUsuario.PasswordChar = false;
            this.NomeUsuario.Size = new System.Drawing.Size(251, 35);
            this.NomeUsuario.TabIndex = 19;
            this.NomeUsuario.UnderlinedStyle = true;
            // 
            // portaConexao
            // 
            this.portaConexao.BackColor = System.Drawing.SystemColors.Window;
            this.portaConexao.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.portaConexao.BorderColorFocus = System.Drawing.Color.LightSkyBlue;
            this.portaConexao.BorderSize = 2;
            this.portaConexao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portaConexao.ForeColor = System.Drawing.SystemColors.GrayText;
            this.portaConexao.Location = new System.Drawing.Point(16, 193);
            this.portaConexao.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.portaConexao.Name = "portaConexao";
            this.portaConexao.Padding = new System.Windows.Forms.Padding(7);
            this.portaConexao.PasswordChar = false;
            this.portaConexao.Size = new System.Drawing.Size(251, 35);
            this.portaConexao.TabIndex = 18;
            this.portaConexao.UnderlinedStyle = true;
            // 
            // NomeDataBase
            // 
            this.NomeDataBase.BackColor = System.Drawing.SystemColors.Window;
            this.NomeDataBase.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.NomeDataBase.BorderColorFocus = System.Drawing.Color.LightSkyBlue;
            this.NomeDataBase.BorderSize = 2;
            this.NomeDataBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NomeDataBase.ForeColor = System.Drawing.SystemColors.GrayText;
            this.NomeDataBase.Location = new System.Drawing.Point(16, 269);
            this.NomeDataBase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NomeDataBase.Name = "NomeDataBase";
            this.NomeDataBase.Padding = new System.Windows.Forms.Padding(7);
            this.NomeDataBase.PasswordChar = false;
            this.NomeDataBase.Size = new System.Drawing.Size(251, 35);
            this.NomeDataBase.TabIndex = 17;
            this.NomeDataBase.UnderlinedStyle = true;
            // 
            // NomeServidor
            // 
            this.NomeServidor.BackColor = System.Drawing.SystemColors.Window;
            this.NomeServidor.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.NomeServidor.BorderColorFocus = System.Drawing.Color.LightSkyBlue;
            this.NomeServidor.BorderSize = 2;
            this.NomeServidor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NomeServidor.ForeColor = System.Drawing.SystemColors.GrayText;
            this.NomeServidor.Location = new System.Drawing.Point(16, 113);
            this.NomeServidor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NomeServidor.Name = "NomeServidor";
            this.NomeServidor.Padding = new System.Windows.Forms.Padding(7);
            this.NomeServidor.PasswordChar = false;
            this.NomeServidor.Size = new System.Drawing.Size(251, 35);
            this.NomeServidor.TabIndex = 16;
            this.NomeServidor.UnderlinedStyle = true;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.button1.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.button1.BorderColor = System.Drawing.Color.AliceBlue;
            this.button1.BorderRadius = 20;
            this.button1.BorderSize = 0;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(5, 476);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 52);
            this.button1.TabIndex = 22;
            this.button1.Text = "Conectar";
            this.button1.TextColor = System.Drawing.Color.White;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.ConnectionDataBase);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.CornflowerBlue;
            this.button2.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.button2.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.button2.BorderRadius = 20;
            this.button2.BorderSize = 0;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(148, 476);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 52);
            this.button2.TabIndex = 23;
            this.button2.Text = "Salvar e Conectar";
            this.button2.TextColor = System.Drawing.Color.White;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.ConnectionSaveDataBase);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.CornflowerBlue;
            this.button3.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.button3.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.button3.BorderRadius = 20;
            this.button3.BorderSize = 0;
            this.button3.Enabled = false;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(68, 534);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(150, 50);
            this.button3.TabIndex = 24;
            this.button3.Text = "Remover Conexão Salva";
            this.button3.TextColor = System.Drawing.Color.White;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.RemoverBase);
            // 
            // Form2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(285, 586);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxConnections);
            this.Controls.Add(this.Senha);
            this.Controls.Add(this.NomeUsuario);
            this.Controls.Add(this.portaConexao);
            this.Controls.Add(this.NomeDataBase);
            this.Controls.Add(this.NomeServidor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Conexão";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Custom.CustomTextBox NomeServidor;
        private Custom.CustomTextBox NomeDataBase;
        private Custom.CustomTextBox portaConexao;
        private Custom.CustomTextBox NomeUsuario;
        private Custom.CustomTextBox Senha;
        private Custom.CustomComboBox comboBoxConnections;
        private Custom.CustomButtons button1;
        private Custom.CustomButtons button2;
        private Custom.CustomButtons button3;
    }
}