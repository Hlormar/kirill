using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace pz111
{
    public partial class AddEditWorkerForm : Form
    {
        
        private DataSet1 dataSet1;
        private DataGridViewRow currentRow;

        //инкапсуляция йоу
        public void setDataSet(DataSet1 _dataSet) {
            dataSet1 = _dataSet;
        }
        public void setCurrentRow(DataGridViewRow _currentRow) {
            currentRow = _currentRow;
        }

        public AddEditWorkerForm()
        {
            InitializeComponent();

        }

        /// 
        /// KeyPress и textchanged listeners
        /// ФИО (только буквы и пробел, 80 length)
        /// 
        private void keyPressFIO(object sender, KeyPressEventArgs e)
        {
            // 1. Всегда разрешаем управляющие клавиши (Backspace, Delete, Ctrl+C)
            if (char.IsControl(e.KeyChar)) return;

            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
            if (textBoxWorkerFIO.Text.Length >= 80)
                e.Handled = true;
        }

        private void textChangedFIO(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            string cleaned = new string(tb.Text.Where(c => char.IsLetter(c) || c == ' ').ToArray());

            if (cleaned.Length > 80)
                cleaned = cleaned.Substring(0, 80);

            if (tb.Text != cleaned)
            {
                int pos = tb.SelectionStart;
                tb.Text = cleaned;
                tb.SelectionStart = pos;
            }
        }
        /// 
        /// Должность (буквы и цифры), 30 length
        /// 
        private void keyPressPosition(object sender, KeyPressEventArgs e)
        {
            // 1. Всегда разрешаем управляющие клавиши (Backspace, Delete, Ctrl+C)
            if (char.IsControl(e.KeyChar)) return;

            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
            if (textBoxWorkerPosition.Text.Length >= 30)
                e.Handled = true;
        }

        private void textChangedPosition(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            string cleaned = new string(tb.Text.Where(c => char.IsLetterOrDigit(c) || c == ' ').ToArray());

            if (cleaned.Length > 30)
                cleaned = cleaned.Substring(0, 30);

            if (tb.Text != cleaned)
            {
                int pos = tb.SelectionStart;
                tb.Text = cleaned;
                tb.SelectionStart = pos;
            }
        }
        ///
        /// ИНН (только 12 цифр)
        ///
        private void keyPressINN(object sender, KeyPressEventArgs e)
        {
            // 1. Всегда разрешаем управляющие клавиши (Backspace, Delete, Ctrl+C)
            if (char.IsControl(e.KeyChar)) return;

            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
            if (textBoxWorkerINN.Text.Length >= 12)
                e.Handled = true;

        }

        private void textChangedINN(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            string cleaned = new string(tb.Text.Where(c => char.IsDigit(c)).ToArray());

            if (cleaned.Length > 12)
                cleaned = cleaned.Substring(0, 12);

            if (tb.Text != cleaned)
            {
                int pos = tb.SelectionStart;
                tb.Text = cleaned;
                tb.SelectionStart = pos;
            }
        }



        private void AddWorker() 
        { 
            DataRow newWorker = dataSet1.Worker.NewRow();
            newWorker["FullName"] = textBoxWorkerFIO.Text;
            newWorker["Position"] = textBoxWorkerPosition.Text;
            newWorker["Inn"] = textBoxWorkerINN.Text;
            dataSet1.Worker.Rows.Add(newWorker);
            //dataSet1.Worker.AcceptChanges();
            Console.WriteLine("worker added");
        }

        private void EditWorker(Int64 workerId) {
            DataRow selectedRow = dataSet1.Worker.Select($"WorkerId = {workerId}")[0]; //возвращает массив с 1 элементом, сразу берем только его
            selectedRow["FullName"] = textBoxWorkerFIO.Text;
            selectedRow["Position"] = textBoxWorkerPosition.Text;
            selectedRow["Inn"] = textBoxWorkerINN.Text;
            //dataSet1.AcceptChanges();

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            //если мы передали строку, значит мы редактируем работника.
            //получаем значения текущей строки
            if (currentRow != null)
            {
                textBoxWorkerFIO.Text = currentRow.Cells["Worker_FIO"].Value?.ToString();
                textBoxWorkerPosition.Text = currentRow.Cells["Worker_Position"].Value?.ToString();
                textBoxWorkerINN.Text = currentRow.Cells["Worker_INN"].Value?.ToString();
            }
        }

        private void saveButtonClick(object sender, EventArgs e)
        {
            // 1. Проверка на пустоту
            if (string.IsNullOrWhiteSpace(textBoxWorkerFIO.Text) ||
                string.IsNullOrWhiteSpace(textBoxWorkerPosition.Text) ||
                string.IsNullOrWhiteSpace(textBoxWorkerINN.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Неверный формат данных", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            // 2. Проверка длины ИНН (TextChanged фильтрует цифры, но не гарантирует ровно 12)
            if (textBoxWorkerINN.Text.Length != 12)
            {
                MessageBox.Show("ИНН должен состоять ровно из 12 цифр!", "Неверный формат данных", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (currentRow != null) {
                EditWorker(Convert.ToInt64(currentRow.Cells["Worker_ID"].Value));
            } else //иначе добавляем нового работника
            {
                AddWorker();
            }
               
            this.Close();
            
        }

        private void cancelButtonClick(object sender, EventArgs e)
        {
            //otmena
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
