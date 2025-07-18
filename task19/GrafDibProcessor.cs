using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

[assembly: SupportedOSPlatform("windows")]

namespace task19
{
    public class GrafDibProcessor
    {
        private readonly List<TestCommand> _commands;

        public GrafDibProcessor(List<TestCommand> commands)
        {
            _commands = commands;
        }

        public void GenerateStatisticsAndChart()
        {
            GenerateTextReport();
            GeneratePngChart();
            Console.WriteLine("Файлы сохранены в папке с программой:");
            Console.WriteLine($"- statistics.txt");
            Console.WriteLine($"- chart.png");
        }

        private void GenerateTextReport()
        {
            string reportPath = "statistics.txt";
            using (StreamWriter writer = new StreamWriter(reportPath))
            {
                writer.WriteLine("Статистика выполнения команд:");
                writer.WriteLine("-----------------------------");
                foreach (var cmd in _commands.OrderBy(c => c.Id))
                {
                    writer.WriteLine($"Команда {cmd.Id}: выполнена {cmd.Counter} раз(а)");
                }
            }
        }

        private void GeneratePngChart()
        {
            string chartPath = "chart.png";
            int width = 650;
            int height = 450;
            int margin = 70;

            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
               
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                
                g.Clear(Color.White);

               
                var axisFont = new Font("Arial", 10);
                var titleFont = new Font("Arial", 12, FontStyle.Bold);
                var labelFont = new Font("Arial", 9);

  
                var titleSize = g.MeasureString("Статистика выполнения команд", titleFont);
                g.DrawString("Статистика выполнения команд", titleFont, Brushes.Black, 
                    new PointF((width - titleSize.Width) / 2, 20));

  
                int chartBottom = height - margin;
                int chartLeft = margin;
                int chartRight = width - margin;
                int chartTop = margin;

          
                g.DrawLine(Pens.Black, chartLeft, chartBottom, chartRight, chartBottom);
          
                g.DrawLine(Pens.Black, chartLeft, chartBottom, chartLeft, chartTop);

      
                StringFormat verticalFormat = new StringFormat();
                verticalFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                string yLabel = "Количество вызовов";
                var yLabelSize = g.MeasureString(yLabel, axisFont);
                g.DrawString(yLabel, axisFont, Brushes.Black, 
                    new PointF(chartLeft - yLabelSize.Height - 10, (height - yLabelSize.Width) / 2), 
                    verticalFormat);

        
                string xLabel = "Команда";
                var xLabelSize = g.MeasureString(xLabel, axisFont);
                g.DrawString(xLabel, axisFont, Brushes.Black, 
                    new PointF((width - xLabelSize.Width) / 2, chartBottom + 20));

          
                int maxValue = _commands.Max(c => c.Counter);
                int barWidth = 40;
                int spacing = 25;
                int startX = chartLeft + spacing;

                for (int i = 0; i < _commands.Count; i++)
                {
                    var cmd = _commands[i];
                    int barHeight = (int)((cmd.Counter / (float)maxValue) * (chartBottom - chartTop - 20));
                    int x = startX + i * (barWidth + spacing);
                    int y = chartBottom - barHeight;

                    
                    g.FillRectangle(Brushes.SteelBlue, x, y, barWidth, barHeight);
                    g.DrawRectangle(Pens.DarkBlue, x, y, barWidth, barHeight);

                   
                    string idLabel = $"ID {cmd.Id}";
                    var idLabelSize = g.MeasureString(idLabel, labelFont);
                    g.DrawString(idLabel, labelFont, Brushes.Black, 
                        new PointF(x + (barWidth - idLabelSize.Width) / 2, chartBottom + 5));

                   
                    string valueLabel = cmd.Counter.ToString();
                    var valueLabelSize = g.MeasureString(valueLabel, labelFont);
                    g.DrawString(valueLabel, labelFont, Brushes.Black, 
                        new PointF(x + (barWidth - valueLabelSize.Width) / 2, y - 20));
                }

                bitmap.Save(chartPath, ImageFormat.Png);
            }
        }
    }
}