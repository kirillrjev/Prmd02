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
                // Получаем индекс выбранной строки
                int index = listBox1.SelectedIndex;

                // Если ничего не выбрано
                if (index == -1)
                {
                    label1.Text = "Выберите строку из списка.";
                    return;
                }

                // Получаем строку
                string str = (string)listBox1.Items[index];

                // Удаляем лишние пробелы и разбиваем строку на слова
                string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Выводим результат
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
