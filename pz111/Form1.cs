using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace pz111
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("data.xml"))
            {
                this.dataSet1.ReadXml("data.xml");
            }
        }

        // Добавить запись
        private void button1_Click(object sender, EventArgs e)
        {
            AddEditRecordForm form = new AddEditRecordForm();
            form.TheDataSet = this.dataSet1;
            form.IsAddMode = true;

            // Берем ID из BindingSource, а не из DataGridView (это надежнее)
            DataRowView currentWorker = (DataRowView)this.workerBindingSource.Current;
            if (currentWorker != null)
            {
                form.DefaultWorkerId = Convert.ToInt32(currentWorker["WorkerId"]);
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                this.workLogBindingSource.ResetBindings(false);
            }
        }

        // Редактировать запись
        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            if (this.workLogDataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись для редактирования!");
                return;
            }

            // ИСПОЛЬЗУЕМ СТРОГО ТИПИЗИРОВАННУЮ СТРОКУ ИЗ DataSet1
            DataSet1.WorkLogRow rowToEdit = (DataSet1.WorkLogRow)((DataRowView)this.workLogBindingSource.Current).Row;

            AddEditRecordForm form = new AddEditRecordForm();
            form.TheDataSet = this.dataSet1;
            form.IsAddMode = false;
            form.WorkLogRowToEdit = rowToEdit; // Передаем строго типизированную строку

            if (form.ShowDialog() == DialogResult.OK)
            {
                this.workLogBindingSource.ResetBindings(false);
            }
        }

        // Удалить запись
        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            if (this.workLogDataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись для удаления!");
                return;
            }

            if (MessageBox.Show("Удалить эту запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.workLogBindingSource.RemoveCurrent();
            }
        }

        // Добавить работника
        private void button5_Click(object sender, EventArgs e)
        {
            AddEditWorkerForm form = new AddEditWorkerForm();
            form.TheDataSet = this.dataSet1;
            form.IsAddMode = true;

            if (form.ShowDialog() == DialogResult.OK)
            {
                this.workerBindingSource.ResetBindings(false);
            }
        }

        // Редактировать работника
        private void btnEditWorker_Click(object sender, EventArgs e)
        {
            if (this.workerDataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите работника для редактирования!");
                return;
            }

            // ИСПОЛЬЗУЕМ СТРОГО ТИПИЗИРОВАННУЮ СТРОКУ ИЗ DataSet1
            DataSet1.WorkerRow rowToEdit = (DataSet1.WorkerRow)((DataRowView)this.workerBindingSource.Current).Row;

            AddEditWorkerForm form = new AddEditWorkerForm();
            form.TheDataSet = this.dataSet1;
            form.IsAddMode = false;
            form.WorkerRowToEdit = rowToEdit; // Передаем строго типизированную строку

            if (form.ShowDialog() == DialogResult.OK)
            {
                this.workerBindingSource.ResetBindings(false);
            }
        }

        // Удалить работника
        private void btnDeleteWorker_Click(object sender, EventArgs e)
        {
            if (this.workerDataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите работника для удаления!");
                return;
            }

            if (MessageBox.Show("Удалить работника?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.workerBindingSource.RemoveCurrent();
            }
        }

        private void btnSaveXml_Click(object sender, EventArgs e)
        {
            this.dataSet1.WriteXml("data.xml");
            MessageBox.Show("Данные сохранены!");
        }

        private void btnLoadXml_Click(object sender, EventArgs e)
        {
            if (File.Exists("data.xml"))
            {
                this.dataSet1.Clear();
                this.dataSet1.ReadXml("data.xml");
                MessageBox.Show("Данные загружены!");
            }
            else
            {
                MessageBox.Show("Файл не найден!");
            }
        }

        private void btnSaveJson_Click(object sender, EventArgs e) { MessageBox.Show("XML достаточно"); }
        private void btnLoadJson_Click(object sender, EventArgs e) { MessageBox.Show("XML достаточно"); }
        private void workerDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void workLogDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}