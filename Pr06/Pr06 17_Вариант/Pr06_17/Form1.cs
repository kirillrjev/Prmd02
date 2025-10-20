namespace Pr06_17
{
    public partial class Form1 : Form
    {
        int[] Z = new int[35]; // Массив из 35 элементов

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            textBox1.Clear();

            for (int i = 0; i < Z.Length; i++)
            {
                Z[i] = rand.Next(-10, 10); // случайные числа от -10 до 9
                textBox1.AppendText($"Z[{i}] = {Z[i]}\r\n");
            }

            label1.Text = "Массив сгенерирован.";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int S = 0;
            int P = 1;
            bool foundOdd = false;

            for (int i = 0; i < Z.Length; i++)
            {
                if (Z[i] % 2 == 0 && Z[i] < 3)
                {
                    S += Z[i];
                }

                if (Z[i] % 2 != 0 && Z[i] > 1)
                {
                    P *= Z[i];
                    foundOdd = true;
                }
            }

            if (!foundOdd)
            {
                P = 0; // если нет подходящих нечётных чисел, произведение = 0
            }

            int R = S + P;

            label1.Text = $"S = {S}, P = {P}, R = S + P = {R}";
        }
    }
}
       

