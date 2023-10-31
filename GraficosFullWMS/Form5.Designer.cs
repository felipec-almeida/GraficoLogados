namespace GraficosFullWMS
{
    partial class Form5
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            this.DataGridLogados = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.customButtons1 = new GraficosFullWMS.Custom.CustomButtons();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridLogados)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridLogados
            // 
            this.DataGridLogados.AllowUserToAddRows = false;
            this.DataGridLogados.AllowUserToDeleteRows = false;
            this.DataGridLogados.AllowUserToOrderColumns = true;
            this.DataGridLogados.AllowUserToResizeColumns = false;
            this.DataGridLogados.AllowUserToResizeRows = false;
            this.DataGridLogados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridLogados.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridLogados.BackgroundColor = System.Drawing.Color.White;
            this.DataGridLogados.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DataGridLogados.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridLogados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridLogados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridLogados.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.DataGridLogados.Location = new System.Drawing.Point(-3, 36);
            this.DataGridLogados.Name = "DataGridLogados";
            this.DataGridLogados.ReadOnly = true;
            this.DataGridLogados.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.DataGridLogados.RowHeadersWidth = 51;
            this.DataGridLogados.RowTemplate.Height = 24;
            this.DataGridLogados.ShowEditingIcon = false;
            this.DataGridLogados.Size = new System.Drawing.Size(797, 404);
            this.DataGridLogados.TabIndex = 0;
            this.DataGridLogados.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(-3, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(797, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "Detalhes de Logados";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(-3, 438);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(797, 54);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total";
            // 
            // customButtons1
            // 
            this.customButtons1.BackColor = System.Drawing.Color.White;
            this.customButtons1.BackgroundColor = System.Drawing.Color.White;
            this.customButtons1.BorderColor = System.Drawing.Color.Black;
            this.customButtons1.BorderRadius = 20;
            this.customButtons1.BorderSize = 0;
            this.customButtons1.FlatAppearance.BorderSize = 0;
            this.customButtons1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButtons1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.customButtons1.Location = new System.Drawing.Point(638, 446);
            this.customButtons1.Name = "customButtons1";
            this.customButtons1.Size = new System.Drawing.Size(143, 37);
            this.customButtons1.TabIndex = 3;
            this.customButtons1.Text = "Exportar para Excel";
            this.customButtons1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.customButtons1.UseVisualStyleBackColor = false;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.ClientSize = new System.Drawing.Size(793, 490);
            this.Controls.Add(this.customButtons1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DataGridLogados);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalhes de Logados";
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridLogados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridLogados;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Custom.CustomButtons customButtons1;
    }
}