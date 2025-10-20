uusing System;
using System.Drawing;
using System.Windows.Forms;

namespace MechanicalClockApp
{
    public partial class Form1 : Form
    {
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Механические часы";
            this.BackColor = Color.White;
            this.ClientSize = new Size(400, 400);
            this.DoubleBuffered = true;

            this.Paint += Form1_Paint;

            timer = new Timer();
            timer.Interval = 1000; // Обновление каждую секунду
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate(); // Обновляем экран (вызывает Paint)
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;
            int clockRadius = 150;

            // Циферблат
            g.DrawEllipse(Pens.Black, centerX - clockRadius, centerY - clockRadius, clockRadius * 2, clockRadius * 2);

            // Центр
            g.FillEllipse(Brushes.Black, centerX - 4, centerY - 4, 8, 8);

            // Деления по кругу
            for (int i = 0; i < 12; i++)
            {
                double angle = i * Math.PI / 6;
                int x1 = centerX + (int)((clockRadius - 10) * Math.Sin(angle));
                int y1 = centerY - (int)((clockRadius - 10) * Math.Cos(angle));
                int x2 = centerX + (int)(clockRadius * Math.Sin(angle));
                int y2 = centerY - (int)(clockRadius * Math.Cos(angle));
                g.DrawLine(Pens.Black, x1, y1, x2, y2);
            }

            DateTime now = DateTime.Now;

            // Углы стрелок
            double secondAngle = now.Second * 6;
            double minuteAngle = (now.Minute + now.Second / 60.0) * 6;
            double hourAngle = (now.Hour % 12 + now.Minute / 60.0) * 30;

            // Отрисовка стрелок
            DrawHand(g, centerX, centerY, hourAngle, clockRadius * 0.5f, 6, Color.DarkBlue);
            DrawHand(g, centerX, centerY, minuteAngle, clockRadius * 0.75f, 4, Color.DarkGreen);
            DrawHand(g, centerX, centerY, secondAngle, clockRadius * 0.9f, 2, Color.Red);
        }

        private void DrawHand(Graphics g, int cx, int cy, double angleDeg, float length, float thickness, Color color)
        {
            double angleRad = Math.PI * angleDeg / 180;
            int x = cx + (int)(length * Math.Sin(angleRad));
            int y = cy - (int)(length * Math.Cos(angleRad));
            Pen pen = new Pen(color, thickness)
            {
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            g.DrawLine(pen, cx, cy, x, y);
        }
    }
}
