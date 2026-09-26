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
        private bool isSelecting = false;
        private bool isFormLoaded = false;

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

        // Сохранить ID выделенных строк
        private List<int> GetSelectedRowIds(DataGridView dgv, string idColumnName)
        {
            List<int> ids = new List<int>();
            foreach (DataGridViewRow row in dgv.SelectedRows)
            {
                object value = row.Cells[idColumnName].Value;
                if (value != null && value != DBNull.Value)
                {
                    ids.Add(Convert.ToInt32(value));
                }
            }
            return ids;
        }

        // Восстановить выделение по списку ID
        private void RestoreSelectionByIds(DataGridView dgv, string idColumnName, List<int> ids)
        {
            if (ids.Count == 0) return;

            dgv.ClearSelection();
            bool firstFound = false;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                object value = row.Cells[idColumnName].Value;
                if (value == null || value == DBNull.Value) continue;

                int rowId = Convert.ToInt32(value);
                if (ids.Contains(rowId))
                {
                    row.Selected = true;

                    if (!firstFound)
                    {
                        dgv.CurrentCell = row.Cells[0];
                        dgv.FirstDisplayedScrollingRowIndex = row.Index;
                        firstFound = true;
                    }
                }
            }
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


            // 2. Принудительно выделяем первую строку в таблице работников (чтобы было от чего отталкиваться)
            if (workerDataGridView.Rows.Count > 0)
            {
                workerDataGridView.ClearSelection();
                workerDataGridView.Rows[0].Selected = true;
                workerDataGridView.CurrentCell = workerDataGridView.Rows[0].Cells[0];
            }

            // фильтруем записи
            if (workerDataGridView.SelectedRows.Count > 0)
                workLogBindingSource.Filter = $"WorkerID = {workerDataGridView.SelectedRows[0].Cells["Worker_ID"].Value}";

            // 3. Только теперь говорим программе, что форма полностью загружена
            isFormLoaded = true;

            // 4. Вызываем функцию выделения записей для текущего (первого) работника
            if (workerDataGridView.SelectedRows.Count > 0)
            {
                // ВНИМАНИЕ: В вашем Designer.cs столбец называется "WorkerId" (без подчеркивания)
                object value = workerDataGridView.SelectedRows[0].Cells["Worker_ID"].Value;

                if (value != null && value != DBNull.Value)
                {
                    int initialWorkerId = Convert.ToInt32(value);
                    SelectRecordsById(initialWorkerId);
                }
            }
        }


        private void workerDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void addRecordButtonClick(object sender, EventArgs e)
        {
            //добавить запись
            AddEditRecordForm addEditRecordForm = new AddEditRecordForm();
            addEditRecordForm.setDataSet(dataSet1);
            addEditRecordForm.setSelectedWorker(Convert.ToInt32(workerDataGridView.SelectedRows[0].Cells["Worker_ID"].Value));
            addEditRecordForm.ShowDialog();
            List<int> selectedWorkerIds = GetSelectedRowIds(workerDataGridView, "Worker_ID");
            List<int> selectedRecordIds = GetSelectedRowIds(workLogDataGridView, "RecordId");
            UpdateButtonsState();
            SaveToXml();
            FillWorkerNames();
            sortByWorkerID();
            RestoreSelectionByIds(workerDataGridView, "Worker_ID", selectedWorkerIds);
            RestoreSelectionByIds(workLogDataGridView, "RecordId", selectedRecordIds);
        }

        private void addWorkerButtonClick(object sender, EventArgs e)
        {
            //добавить работника
            AddEditWorkerForm addEditWorkerForm = new AddEditWorkerForm();
            addEditWorkerForm.setDataSet(dataSet1);
            addEditWorkerForm.ShowDialog();
            List<int> selectedWorkerIds = GetSelectedRowIds(workerDataGridView, "Worker_ID");
            List<int> selectedRecordIds = GetSelectedRowIds(workLogDataGridView, "RecordId");
            UpdateButtonsState();
            SaveToXml();
            FillWorkerNames();
            sortByWorkerID();
            RestoreSelectionByIds(workerDataGridView, "Worker_ID", selectedWorkerIds);
            RestoreSelectionByIds(workLogDataGridView, "RecordId", selectedRecordIds);
        }

        private void workLogDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void deleteWorkerButtonClick(object sender, EventArgs e)
        {
            //удалить работника
            DataGridViewRow currentRow = workerDataGridView.SelectedRows[0];
            if (currentRow == null) return;
            
            DialogResult confirmation = MessageBox.Show($"Удалить работника {currentRow.Cells["Worker_FIO"].Value?.ToString()}?", 
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes) 
            {
                List<int> selectedWorkerIds = GetSelectedRowIds(workerDataGridView, "Worker_ID");
                List<int> selectedRecordIds = GetSelectedRowIds(workLogDataGridView, "RecordId");
                workerDataGridView.Rows.Remove(currentRow);
                UpdateButtonsState();
                SaveToXml();
                FillWorkerNames();
                sortByWorkerID();
                RestoreSelectionByIds(workerDataGridView, "Worker_ID", selectedWorkerIds);
                RestoreSelectionByIds(workLogDataGridView, "RecordId", selectedRecordIds);
            }
        }

        private void editWorkerButtonClick(object sender, EventArgs e)
        {
            //редактировать работника
            DataGridViewRow currentRow = workerDataGridView.SelectedRows[0];
            if (currentRow == null) return;

            AddEditWorkerForm addEditWorkerForm = new AddEditWorkerForm();
            addEditWorkerForm.setDataSet(dataSet1);
            addEditWorkerForm.setCurrentRow(currentRow);
            addEditWorkerForm.ShowDialog();
            List<int> selectedWorkerIds = GetSelectedRowIds(workerDataGridView, "Worker_ID");
            List<int> selectedRecordIds = GetSelectedRowIds(workLogDataGridView, "RecordId");
            UpdateButtonsState();
            SaveToXml();
            FillWorkerNames();
            sortByWorkerID();
            RestoreSelectionByIds(workerDataGridView, "Worker_ID", selectedWorkerIds);
            RestoreSelectionByIds(workLogDataGridView, "RecordId", selectedRecordIds);

        }

        private void deleteRecordButtonClick(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = workLogDataGridView.SelectedRows[0];
            if (currentRow == null) return;

            DialogResult confirmation = MessageBox.Show($"Удалить запись?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                List<int> selectedWorkerIds = GetSelectedRowIds(workerDataGridView, "Worker_ID");
                List<int> selectedRecordIds = GetSelectedRowIds(workLogDataGridView, "RecordId");
                workLogDataGridView.Rows.Remove(currentRow);
                UpdateButtonsState();
                SaveToXml();
                FillWorkerNames();
                sortByWorkerID();
                RestoreSelectionByIds(workerDataGridView, "Worker_ID", selectedWorkerIds);
                RestoreSelectionByIds(workLogDataGridView, "RecordId", selectedRecordIds);
            }
        }

        private void editRecordButtonClick(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = workLogDataGridView.SelectedRows[0];
            if (currentRow == null) return;

            AddEditRecordForm addEditRecordForm = new AddEditRecordForm();
            addEditRecordForm.setDataSet(dataSet1);
            addEditRecordForm.setCurrentRow(currentRow);
            addEditRecordForm.ShowDialog();
            List<int> selectedWorkerIds = GetSelectedRowIds(workerDataGridView, "Worker_ID");
            List<int> selectedRecordIds = GetSelectedRowIds(workLogDataGridView, "RecordId");
            UpdateButtonsState();
            SaveToXml();
            FillWorkerNames();
            sortByWorkerID();
            RestoreSelectionByIds(workerDataGridView, "Worker_ID", selectedWorkerIds);
            RestoreSelectionByIds(workLogDataGridView, "RecordId", selectedRecordIds);
        }

        private void workLogDataGridView_Sorted(object sender, EventArgs e)
        {
            FillWorkerNames();
        }

        private void workerDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Игнорируем, если идет программное выделение или форма еще не загружена
            if (isSelecting || !isFormLoaded) return;
            if (workerDataGridView.SelectedRows.Count == 0) return;

            DataGridViewRow row = workerDataGridView.SelectedRows[0];
            // ВНИМАНИЕ: В вашем Designer.cs столбец называется "WorkerId", а не "Worker_ID"
            object value = row.Cells["Worker_ID"].Value;

            if (value == null || value == DBNull.Value) return;

            int workerId = Convert.ToInt32(value);
            //SelectRecordsById(workerId);
            workLogBindingSource.Filter = $"WorkerID = {workerId}";
        }

        private void SelectRecordsById(int workerId)
        {
            if (isSelecting) return;

            try
            {
                isSelecting = true;
                workLogDataGridView.ClearSelection(); // Очищаем ДО цикла

                bool firstMatchFound = false;

                foreach (DataGridViewRow row in workLogDataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    object value = row.Cells["WorkerID"].Value; // Исправлено имя столбца
                    if (value == null || value == DBNull.Value) continue;

                    if (Convert.ToInt32(value) == workerId)
                    {
                        row.Selected = true;

                        if (!firstMatchFound)
                        {
                            workLogDataGridView.FirstDisplayedScrollingRowIndex = row.Index;
                            firstMatchFound = true;
                        }
                    }
                }
            }
            finally
            {
                isSelecting = false;
            }
        }

        private void selectWorkerByID(int workerId)
        {
            if (isSelecting || !isFormLoaded) return;

            try
            {
                isSelecting = true;
                workerDataGridView.ClearSelection(); // ВЫНЕСТИ ИЗ ЦИКЛА! (Было внутри)

                foreach (DataGridViewRow row in workerDataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    object value = row.Cells["Worker_ID"].Value; // Исправлено: был int, который не может быть null
                    if (value == null || value == DBNull.Value) continue;

                    if (Convert.ToInt32(value) == workerId)
                    {
                        row.Selected = true;
                        workerDataGridView.FirstDisplayedScrollingRowIndex = row.Index;
                        // Убрали return, чтобы код был симметричным и безопасным
                    }
                }
            }
            finally
            {
                isSelecting = false;
            }
        }

        private void workLogDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (isSelecting || !isFormLoaded) return;
            if (workLogDataGridView.SelectedRows.Count == 0) return;

            DataGridViewRow row = workLogDataGridView.SelectedRows[0];
            object value = row.Cells["WorkerID"].Value;

            if (value == null || value == DBNull.Value) return;

            int workerId = Convert.ToInt32(value);
            selectWorkerByID(workerId);
        }

    }


}
