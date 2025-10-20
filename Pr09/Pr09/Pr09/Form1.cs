namespace pr09
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Графические примитивы";
            this.BackColor = Color.White;
            this.ClientSize = new Size(800, 600);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen solidPen = new Pen(Color.Black, 2);
            g.DrawLine(solidPen, 20, 20, 200, 20);

            Pen dashPen = new Pen(Color.Blue, 2);
            dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawLine(dashPen, 20, 50, 200, 50);
            Pen dashDotPen = new Pen(Color.Red, 2);
            dashDotPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            g.DrawLine(dashDotPen, 20, 80, 200, 80);

            Pen rectPen = new Pen(Color.Green, 3);
            g.DrawRectangle(rectPen, 250, 20, 100, 60);
            SolidBrush rectBrush = new SolidBrush(Color.Orange);
            g.FillRectangle(rectBrush, 370, 20, 100, 60);

            g.DrawEllipse(Pens.DarkViolet, 250, 100, 100, 60);
            g.FillEllipse(Brushes.CadetBlue, 370, 100, 100, 60);
            Point[] triangle = new Point[]
            {
                new Point(500, 50),
                new Point(550, 100),
                new Point(450, 100)
            };
            g.DrawPolygon(Pens.Maroon, triangle);
            Point[] pentagon = new Point[]
            {
                new Point(500, 150),
                new Point(530, 180),
                new Point(515, 220),
                new Point(485, 220),
                new Point(470, 180)
            };
            g.FillPolygon(Brushes.MediumPurple, pentagon);
            g.DrawArc(Pens.SaddleBrown, 600, 20, 100, 100, 30, 120);
            g.FillPie(Brushes.Goldenrod, 600, 140, 100, 100, 45, 90);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(Form1_Paint);
        }
    }
}