using System;
using System.Windows.Forms;

namespace pz111
{
    public partial class AddEditRecordForm : Form
    {
        public DataSet1 TheDataSet;
        public bool IsAddMode;

        // ИСПОЛЬЗУЕМ СТРОГО ТИПИЗИРОВАННУЮ СТРОКУ
        public DataSet1.WorkLogRow WorkLogRowToEdit;

        public int DefaultWorkerId;

        public AddEditRecordForm()
        {
            InitializeComponent();
        }

        private void AddEditRecordForm_Load(object sender, EventArgs e)
        {
            if (!IsAddMode && WorkLogRowToEdit != null)
            {
                // Обращаемся к свойствам напрямую, без ["RecordId"]
                txtRecordId.Text = WorkLogRowToEdit.RecordId.ToString();
                txtRecordId.ReadOnly = true;
                txtWorkerId.Text = WorkLogRowToEdit.WorkerId.ToString();
                txtStartDate.Text = WorkLogRowToEdit.StartDate.ToString("dd.MM.yyyy");
                txtEndDate.Text = WorkLogRowToEdit.EndDate.ToString("dd.MM.yyyy");
                txtDescription.Text = WorkLogRowToEdit.Description;
            }
            else
            {
                txtRecordId.Text = "Авто";
                txtRecordId.ReadOnly = true;
                txtWorkerId.Text = DefaultWorkerId > 0 ? DefaultWorkerId.ToString() : "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWorkerId.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Заполните Код работника и Описание!");
                return;
            }

            try
            {
                int workerId = Convert.ToInt32(txtWorkerId.Text);
                DateTime start = Convert.ToDateTime(txtStartDate.Text);
                DateTime end = Convert.ToDateTime(txtEndDate.Text);

                if (IsAddMode)
                {
                    // Вызываем сгенерированный метод из твоего DataSet1.Designer.cs
                    DataSet1.WorkLogRow newRow = TheDataSet.WorkLog.NewWorkLogRow();
                    newRow.WorkerId = workerId;
                    newRow.StartDate = start;
                    newRow.EndDate = end;
                    newRow.Description = txtDescription.Text;
                    TheDataSet.WorkLog.AddWorkLogRow(newRow);
                }
                else
                {
                    WorkLogRowToEdit.WorkerId = workerId;
                    WorkLogRowToEdit.StartDate = start;
                    WorkLogRowToEdit.EndDate = end;
                    WorkLogRowToEdit.Description = txtDescription.Text;
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
        private void label5_Click(object sender, EventArgs e) { }
    }
}