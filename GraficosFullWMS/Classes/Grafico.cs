using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.WinForms;
using OpenTK.Graphics.ES11;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView;
using System.Windows.Forms;

namespace GraficosFullWMS.Classes
{
    public class Grafico
    {
        public CartesianChart CartesianChart1 { get; }
        public string Tipo { get; }
        public List<DateTime> DataHora { get; }
        public List<double> Logados { get; }
        public List<double> Colaboradores { get; }
        public List<double> TotalLogados { get; }

        private readonly string DateTimeFormat1 = "dd/MM/yyyy HH:mm:ss";
        private readonly string DateTimeFormat2 = "dd/MM/yyyy";

        //Construtores
        public Grafico(CartesianChart cartesianChart1, string tipo, List<DateTime> dataHora, List<double> logados)
        {
            this.CartesianChart1 = cartesianChart1;
            this.Tipo = tipo;
            this.DataHora = dataHora;
            this.Logados = logados;
        }
        public Grafico(CartesianChart cartesianChart1, string tipo, List<DateTime> dataHora, List<double> logados, List<double> colaboradores, List<double> totalLogados)
        {
            CartesianChart1 = cartesianChart1;
            this.Tipo = tipo;
            this.DataHora = dataHora;
            this.Logados = logados;
            this.Colaboradores = colaboradores;
            this.TotalLogados = totalLogados;
        }

        public void GeraGrafico()
        {
            ConfigGrafico();

            if (Tipo.Equals(null))
            {
                MessageBox.Show("Houve um erro ao gerar o gráfico, verifique os campos preenchidos e tente novamente!", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Tipo.Equals("1 - Usuários Logados"))
            {
                CartesianChart1.Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name = "Usuários Logados",
                        Values = Logados,
                        GeometryFill = null,
                        GeometryStroke = null,
                        Fill = new SolidColorPaint() { Color = SKColors.Maroon.WithAlpha(25), StrokeThickness = 1.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.Maroon },
                        LineSmoothness = 1
                    }
                };

                CartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora?.Select(data => data.ToString(DateTimeFormat1)).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                DataHora.Clear();

                CartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Usuários Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                CartesianChart1.BackColor = System.Drawing.Color.White;
                CartesianChart1.Visible = true;
            }
            else if (Tipo.Equals("2 - Colaboradores Logados"))
            {
                CartesianChart1.Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name = "Colaboradores Logados",
                        Values = Logados,
                        GeometryFill = null,
                        GeometryStroke = null,
                        Fill = new SolidColorPaint() { Color = SKColors.Maroon.WithAlpha(25), StrokeThickness = 1.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.Maroon },
                        LineSmoothness = 1
                    }
                };

                CartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora?.Select(data => data.ToString(DateTimeFormat1)).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                DataHora.Clear();

                CartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Colaboradores Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                CartesianChart1.BackColor = System.Drawing.Color.White;
                CartesianChart1.Visible = true;
            }
            else if (Tipo.Equals("3 - Total Logados"))
            {
                CartesianChart1.Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name = "Total Logados",
                        Values = Logados,
                        GeometryFill = null,
                        GeometryStroke = null,
                        Fill = new SolidColorPaint() { Color = SKColors.Maroon.WithAlpha(25), StrokeThickness = 1.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.Maroon },
                        LineSmoothness = 1,
                    }
                };

                CartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora?.Select(data => data.ToString(DateTimeFormat1)).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                DataHora.Clear();

                CartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Total Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                CartesianChart1.BackColor = System.Drawing.Color.White;
                CartesianChart1.Visible = true;
            }
            else if (Tipo.Equals("4 - Usuários/Colaboradores"))
            {
                CartesianChart1.ZoomMode = ZoomAndPanMode.X;
                CartesianChart1.Series = new ISeries[]
                {
                    new StackedColumnSeries<double>
                    {
                        Name = "Usuários",
                        Values = Logados,
                        Fill = new SolidColorPaint() { Color = SKColors.CornflowerBlue, StrokeThickness = 4.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.DodgerBlue },
                        MaxBarWidth = 95,
                        DataLabelsSize = 7.5,
                        Padding = 7.5,
                        IsHoverable = true
                    },
                    new StackedColumnSeries<double>
                    {
                        Name = "Colaboradores",
                        Values = Colaboradores,
                        Fill = new SolidColorPaint() { Color = SKColors.PaleVioletRed, StrokeThickness = 4.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.IndianRed },
                        Padding = 7.5,
                        DataLabelsSize = 7.5,
                        MaxBarWidth = 95,
                        IsHoverable = true
                    },
                    new LineSeries<double>
                    {
                        Name = "Total de Logados",
                        Values = TotalLogados,
                        GeometryFill = new SolidColorPaint() { Color = SKColors.WhiteSmoke },
                        GeometryStroke = new SolidColorPaint() { Color = SKColors.Black, StrokeThickness = 1F },
                        GeometrySize = 8.5,
                        Fill = null,
                        LineSmoothness = 0.5,
                        Stroke = new SolidColorPaint() { Color = SKColors.Black, StrokeThickness = 1.25F }
                    }
                };

                CartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora?.Select(data => data.ToString(DateTimeFormat2)).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                CartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Máximo de Usuários e Colaboradores Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                CartesianChart1.BackColor = System.Drawing.Color.White;
                CartesianChart1.Visible = true;
            }
            else if (Tipo.Equals("5 - Junta Gráficos"))
            {
                CartesianChart1.ZoomMode = ZoomAndPanMode.X;
                CartesianChart1.Series = new ISeries[]
                {
                    new StackedColumnSeries<double>
                    {
                        Name = "Usuários",
                        Values = Logados,
                        Fill = new SolidColorPaint() { Color = SKColors.CornflowerBlue, StrokeThickness = 4.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.DodgerBlue },
                        MaxBarWidth = 95,
                        DataLabelsSize = 7.5,
                        Padding = 7.5,
                        IsHoverable = true
                    },
                    new StackedColumnSeries<double>
                    {
                        Name = "Colaboradores",
                        Values = Colaboradores,
                        Fill = new SolidColorPaint() { Color = SKColors.PaleVioletRed, StrokeThickness = 4.5F },
                        Stroke = new SolidColorPaint() { Color = SKColors.IndianRed },
                        Padding = 7.5,
                        DataLabelsSize = 7.5,
                        MaxBarWidth = 95,
                        IsHoverable = true
                    },
                    new LineSeries<double>
                    {
                        Name = "Total de Logados",
                        Values = TotalLogados,
                        GeometryFill = new SolidColorPaint() { Color = SKColors.WhiteSmoke },
                        GeometryStroke = new SolidColorPaint() { Color = SKColors.Black, StrokeThickness = 1F },
                        GeometrySize = 8.5,
                        Fill = null,
                        LineSmoothness = 1,
                        Stroke = new SolidColorPaint() { Color = SKColors.Black, StrokeThickness = 1.25F }
                    }
                };

                CartesianChart1.XAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Data de Entrada",
                        NamePaint = new SolidColorPaint(SKColors.Gray),
                        Labels = DataHora?.Select(data => data.ToString(DateTimeFormat2)).ToList(),
                        TextSize = 8.5,
                        NameTextSize = 11,
                        LabelsRotation = 15
                    }
                };

                CartesianChart1.YAxes = new Axis[]
                {
                    new Axis
                    {
                        Name = "Máximo de Usuários e Colaboradores Logados",
                        NameTextSize = 10,
                        TextSize = 10.5,
                        NamePaint = new SolidColorPaint(SKColors.Gray)
                    }
                };

                CartesianChart1.BackColor = System.Drawing.Color.White;
                CartesianChart1.Visible = true;
            }
        }

        private void ConfigGrafico()
        {
            CartesianChart1.Visible = false;
            //label2.Visible = false;
            //progressBar1.Visible = false;
            CartesianChart1.HorizontalScroll.Enabled = true;
            CartesianChart1.ZoomingSpeed = 0.5;
            CartesianChart1.AnimationsSpeed = TimeSpan.FromMilliseconds(750);
            CartesianChart1.ZoomMode = ZoomAndPanMode.ZoomX;
            CartesianChart1.EasingFunction = EasingFunctions.CubicOut;
            CartesianChart1.TooltipPosition = TooltipPosition.Top;
            CartesianChart1.TooltipTextSize = 11;
            CartesianChart1.TooltipBackgroundPaint = new SolidColorPaint(SKColors.WhiteSmoke);
            CartesianChart1.TooltipTextPaint = new SolidColorPaint() { Color = SKColors.Black, FontFamily = "Arial" };
            CartesianChart1.LegendPosition = LegendPosition.Bottom;
            CartesianChart1.LegendTextPaint = new SolidColorPaint() { Color = SKColors.Black, FontFamily = "Arial" };
            CartesianChart1.LegendTextSize = 13;
            CartesianChart1.LegendBackgroundPaint = new SolidColorPaint(SKColors.WhiteSmoke);
            CartesianChart1.TooltipFindingStrategy = TooltipFindingStrategy.Automatic;
        }
    }
}
