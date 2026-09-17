using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace pz111
{
    public partial class Form1 : Form
    {
        //DataSet1 dataSet1 = new DataSet1();
        const string FILE_PATH = "DataSet.xml";

        private void SaveToXml()
        {
            dataSet1.Worker.AcceptChanges();
            dataSet1.WorkLog.AcceptChanges();
            dataSet1.WriteXml(FILE_PATH);
            Console.WriteLine("saved to xml");
        }

        private void LoadFromXml()
        {
            dataSet1.Clear();
            dataSet1.ReadXml(FILE_PATH);
        }

        //функция включения/выключения кнопок
        private void UpdateButtonsState()
        {
            // Кнопки записей
            bool hasRecords = workLogDataGridView.Rows.Count > 0;
            EditRecordButton.Enabled = hasRecords;
            DeleteRecordButton.Enabled = hasRecords;

            // Кнопки работников
            bool hasWorkers = workerDataGridView.Rows.Count > 0;
            AddRecordButton.Enabled = hasWorkers;
            EditWorkerButton.Enabled = hasWorkers;
            DeleteWorkerButton.Enabled = hasWorkers;
        }

        //функция, чтобы заполнить ФИО работников в таблице журнала
        private void FillWorkerNames()
        {
            foreach (DataGridViewRow row in workLogDataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                // Получаем WorkerId из скрытой колонки
                int workerId = Convert.ToInt32(row.Cells["WorkerID"].Value);
                var worker = dataSet1.Worker.FirstOrDefault(w => w.WorkerId == workerId);

                // Заполняем ФИО
                if (worker != null)
                {
                    row.Cells["FIO"].Value = worker.FullName;
                }
                else
                {
                    row.Cells["FIO"].Value = "None";
                }
            }

        }

        void sortByWorkerID() {
            workLogDataGridView.Sort(workLogDataGridView.Columns["WorkerID"], ListSortDirection.Ascending);
        }


        public Form1()
        {
            InitializeComponent();
        }

        // Form1_Load выполняется после инициализации (Form1())
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(FILE_PATH))
            {
                LoadFromXml();
            }
            UpdateButtonsState();
            FillWorkerNames();
            sortByWorkerID();



        }


        private void workerDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void addRecordButtonClick(object sender, EventArgs e)
        {
            //добавить запись
            AddEditRecordForm addEditRecordForm = new AddEditRecordForm();
            addEditRecordForm.setDataSet(dataSet1);
            addEditRecordForm.ShowDialog();
            UpdateButtonsState();
            SaveToXml();
            FillWorkerNames();
            sortByWorkerID();
        }

        private void addWorkerButtonClick(object sender, EventArgs e)
        {
            //добавить работника
            AddEditWorkerForm addEditWorkerForm = new AddEditWorkerForm();
            addEditWorkerForm.setDataSet(dataSet1);
            addEditWorkerForm.ShowDialog();
            UpdateButtonsState();
            SaveToXml();
            FillWorkerNames();
            sortByWorkerID();
        }

        private void workLogDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void deleteWorkerButtonClick(object sender, EventArgs e)
        {
            //удалить работника
            DataGridViewRow currentRow = workerDataGridView.CurrentRow;
            if (currentRow == null) return;
            
            DialogResult confirmation = MessageBox.Show($"Удалить работника {currentRow.Cells["Worker_FIO"].Value?.ToString()}?", 
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes) 
            {
                workerDataGridView.Rows.Remove(workerDataGridView.CurrentRow);
                UpdateButtonsState();
                SaveToXml();
                FillWorkerNames();
                sortByWorkerID();
            }
        }

        private void editWorkerButtonClick(object sender, EventArgs e)
        {
            //редактировать работника
            DataGridViewRow currentRow = workerDataGridView.CurrentRow;
            if (currentRow == null) return;

            AddEditWorkerForm addEditWorkerForm = new AddEditWorkerForm();
            addEditWorkerForm.setDataSet(dataSet1);
            addEditWorkerForm.setCurrentRow(currentRow);
            addEditWorkerForm.ShowDialog();
            UpdateButtonsState();
            SaveToXml();
            FillWorkerNames();
            sortByWorkerID();

        }

        private void deleteRecordButtonClick(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = workLogDataGridView.CurrentRow;
            if (currentRow == null) return;

            DialogResult confirmation = MessageBox.Show($"Удалить запись?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                workLogDataGridView.Rows.Remove(workLogDataGridView.CurrentRow);
                UpdateButtonsState();
                SaveToXml();
                FillWorkerNames();
                sortByWorkerID();
            }
        }

        private void editRecordButtonClick(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = workLogDataGridView.CurrentRow;
            if (currentRow == null) return;

            AddEditRecordForm addEditRecordForm = new AddEditRecordForm();
            addEditRecordForm.setDataSet(dataSet1);
            addEditRecordForm.setCurrentRow(currentRow);
            addEditRecordForm.ShowDialog();
            UpdateButtonsState();
            SaveToXml();
            FillWorkerNames();
            sortByWorkerID();
        }

        private void workLogDataGridView_Sorted(object sender, EventArgs e)
        {
            FillWorkerNames();
        }
    }
}
