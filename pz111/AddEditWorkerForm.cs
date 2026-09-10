using System;
using System.Windows.Forms;

namespace pz111
{
    public partial class AddEditWorkerForm : Form
    {
        public DataSet1 TheDataSet;
        public bool IsAddMode;

        // ИСПОЛЬЗУЕМ СТРОГО ТИПИЗИРОВАННУЮ СТРОКУ
        public DataSet1.WorkerRow WorkerRowToEdit;

        public AddEditWorkerForm()
        {
            InitializeComponent();
        }

        private void AddEditWorkerForm_Load(object sender, EventArgs e)
        {
            if (!IsAddMode && WorkerRowToEdit != null)
            {
                // Обращаемся к свойствам напрямую, без ["WorkerId"]
                txtWorkerId.Text = WorkerRowToEdit.WorkerId.ToString();
                txtWorkerId.ReadOnly = true;
                txtFullName.Text = WorkerRowToEdit.FullName;
                txtPosition.Text = WorkerRowToEdit.Position;
                txtInn.Text = WorkerRowToEdit.Inn;
            }
            else
            {
                txtWorkerId.Text = "Авто";
                txtWorkerId.ReadOnly = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtPosition.Text))
            {
                MessageBox.Show("Заполните ФИО и Должность!");
                return;
            }

            try
            {
                if (IsAddMode)
                {
                    // Вызываем сгенерированный метод из твоего DataSet1.Designer.cs
                    DataSet1.WorkerRow newRow = TheDataSet.Worker.NewWorkerRow();
                    newRow.FullName = txtFullName.Text;
                    newRow.Position = txtPosition.Text;
                    newRow.Inn = txtInn.Text;
                    TheDataSet.Worker.AddWorkerRow(newRow);
                }
                else
                {
                    WorkerRowToEdit.FullName = txtFullName.Text;
                    WorkerRowToEdit.Position = txtPosition.Text;
                    WorkerRowToEdit.Inn = txtInn.Text;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void Form2_Load(object sender, EventArgs e) { }
    }
}