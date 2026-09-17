using pz_11;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pz111
{
    public partial class AddEditRecordForm : Form
    {
        private DataSet1 dataSet1;
        private DataGridViewRow currentRow;

        public void setDataSet(DataSet1 _dataSet)
        {
            dataSet1 = _dataSet;
        }
        public void setCurrentRow(DataGridViewRow _currentRow)
        {
            currentRow = _currentRow;
        }



        public AddEditRecordForm()
        {
            InitializeComponent();
        }

        private void AddEditRecordForm_Load(object sender, EventArgs e)
        {
            if (currentRow != null) { 
                textBoxRecordWorkerID.Text = currentRow.Cells["WorkerID"].Value?.ToString();
                startDateTimePicker.Value = Convert.ToDateTime(currentRow.Cells["StartDate"].Value);
                endDateTimePicker2.Value = Convert.ToDateTime(currentRow.Cells["EndDate"].Value);
                textBoxRecordDescription.Text = currentRow.Cells["Description"].Value?.ToString();
            }
        }

        ///KEYPRESSES and textChanged
        ///
        /// ID (только int, length 9)
        ///
        private void keyPressID(object sender, KeyPressEventArgs e)
        {
            // 1. Всегда разрешаем управляющие клавиши (Backspace, Delete, Ctrl+C)
            if (char.IsControl(e.KeyChar)) return;

            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
            if (textBoxRecordWorkerID.Text.Length >= 9)
                e.Handled = true;

        }

        private void textChangedID(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            string cleaned = new string(tb.Text.Where(c => char.IsDigit(c)).ToArray());

            if (cleaned.Length > 9)
                cleaned = cleaned.Substring(0, 9);

            if (tb.Text != cleaned)
            {
                int pos = tb.SelectionStart;
                tb.Text = cleaned;
                tb.SelectionStart = pos;
            }
        }

        ///
        /// Description (length 150) 
        /// сделано через свойство

        private void AddEntry()
        {
            DataRow newEntry = dataSet1.WorkLog.NewRow();
            newEntry["WorkerId"] = textBoxRecordWorkerID.Text;
            newEntry["StartDate"] = startDateTimePicker.Value;
            newEntry["EndDate"] = endDateTimePicker2.Value;
            newEntry["Description"] = textBoxRecordDescription.Text;
            dataSet1.WorkLog.Rows.Add(newEntry);
            Console.WriteLine("worklog added");
        }

        private void EditEntry(Int64 entryID)
        {
            DataRow selectedRow = dataSet1.WorkLog.Select($"RecordId = {entryID}")[0]; //возвращает массив с 1 элементом, сразу берем только его
            selectedRow["WorkerId"] = textBoxRecordWorkerID.Text;
            selectedRow["StartDate"] = startDateTimePicker.Value;
            selectedRow["EndDate"] = endDateTimePicker2.Value;
            selectedRow["Description"] = textBoxRecordDescription.Text;
            //dataSet1.AcceptChanges();

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            //Проверка на пустоту
            if (String.IsNullOrWhiteSpace(textBoxRecordWorkerID.Text) ||
                String.IsNullOrWhiteSpace(textBoxRecordDescription.Text)) 
            { 
                MessageBox.Show("Все поля должны быть заполнены!", "Неверный формат данных", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            //Существование WorkerID
            if (!dataSet1.Worker.Any(w => w.WorkerId == Convert.ToInt32(textBoxRecordWorkerID.Text))) {
                MessageBox.Show($"Работника с ID = {textBoxRecordWorkerID.Text} не найдено!", "Неверный формат данных", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            //Сравнение дат
            if (startDateTimePicker.Value > endDateTimePicker2.Value) { 
                MessageBox.Show("Дата начала не может быть позже даты окончания!", "Неверный формат данных", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            if (currentRow != null) {
                EditEntry(Convert.ToInt64(currentRow.Cells[0].Value));
            }
            else {
                AddEntry();
            }

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
