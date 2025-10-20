namespace Pr07
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int size = 15;
            int[,] a = new int[size, size];
            Random rand = new Random();

            dataGridView1.RowCount = size;
            dataGridView1.ColumnCount = size;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    a[i, j] = rand.Next(-100, 101); // от -100 до 100
                    dataGridView1.Rows[i].Cells[j].Value = a[i, j];
                }
            }
            int min = int.MaxValue;
            for (int i = 0; i < size; i++)
            {
                int value = a[i, size - 1 - i];
                if (value < min)
                    min = value;
            }

            textBox1.Text = "Минимум на доп. диагонали: " + min.ToString();
        }
    }
}

        
    

