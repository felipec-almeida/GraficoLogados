using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GraficosFullWMS.Custom
{
    public partial class CustomTextBox : UserControl
    {

        private Color borderColor = Color.CornflowerBlue;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private Color borderColorFocus = Color.LightSkyBlue;
        private bool isFocused = false;

        public CustomTextBox()
        {
            InitializeComponent();
        }

        [Category("Custom TextBox Configuration")]
        public Color BorderColor
        {

            get
            {

                return borderColor;

            }

            set
            {

                borderColor = value;
                this.Invalidate();

            }

        }

        [Category("Custom TextBox Configuration")]
        public int BorderSize
        {

            get
            {

                return borderSize;

            }

            set
            {

                borderSize = value;
                this.Invalidate();

            }

        }

        [Category("Custom TextBox Configuration")]
        public bool UnderlinedStyle
        {

            get
            {

                return underlinedStyle;

            }

            set
            {

                underlinedStyle = value;
                this.Invalidate();

            }

        }

        [Category("Custom TextBox Configuration")]
        public bool PasswordChar
        {

            get { return textBox1.UseSystemPasswordChar; }
            set { textBox1.UseSystemPasswordChar = value; }

        }

        [Category("Custom TextBox Configuration")]
        public override Color BackColor
        {

            get
            {

                return base.BackColor;

            }

            set
            {

                base.BackColor = value;
                textBox1.BackColor = value;

            }

        }

        [Category("Custom TextBox Configuration")]
        public override Color ForeColor
        {

            get
            {

                return base.ForeColor;

            }

            set
            {

                base.ForeColor = value;
                textBox1.ForeColor = value;

            }

        }

        [Category("Custom TextBox Configuration")]
        public override Font Font
        {

            get
            {

                return base.Font;

            }

            set
            {

                base.Font = value;
                textBox1.Font = value;
                if (this.DesignMode)
                    UpdateControlHeight();

            }

        }

        [Category("Custom TextBox Configuration")]
        public override string Text
        {

            get
            {

                return base.Text;

            }

            set
            {

                base.Text = value;
                textBox1.Text = value;

            }

        }

        [Category("Custom TextBox Configuration")]
        public Color BorderColorFocus
        {

            get { return borderColorFocus; }
            set { borderColorFocus = value; }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            using (Pen penBorder = new Pen(borderColor, borderSize))
            {

                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                if (isFocused) penBorder.Color = borderColorFocus;

                if (underlinedStyle)
                    graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                else
                    graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);

            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.DesignMode)
                UpdateControlHeight();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }

        private void UpdateControlHeight()
        {
            if (textBox1.Multiline == false)
            {

                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;

                textBox1.Multiline = true;

                textBox1.MinimumSize = new Size(0, txtHeight);

                textBox1.Multiline = false;

                this.Height = textBox1.Height + this.Padding.Top + this.Padding.Bottom;

            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

            isFocused = true;
            this.Invalidate();

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {

            isFocused = false;
            this.Invalidate();

        }
    }
}
