namespace Pr05
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                int index = listBox1.SelectedIndex;

              
                if (index == -1)
                {
                    label1.Text = "Выберите строку из списка.";
                    return;
                }

            
                string str = (string)listBox1.Items[index];

                string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

           
                label1.Text = "Количество слов: " + words.Length.ToString();
            }
            catch (Exception ex)
            {
                label1.Text = "Ошибка: " + ex.Message;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

