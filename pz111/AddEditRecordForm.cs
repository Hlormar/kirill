using System;
using System.Windows.Forms;

namespace pz111
{
    public partial class AddEditRecordForm : Form
    {
        public DataSet1 TheDataSet;
        public bool IsAddMode;
        public DataSet1.WorkLogRow WorkLogRowToEdit;
        public int DefaultWorkerId;

        public AddEditRecordForm()
        {
            InitializeComponent();

            // Подписываемся на события KeyPress
            
            txtStartDate.KeyPress += TxtDate_KeyPress;
            txtEndDate.KeyPress += TxtDate_KeyPress;
        }

        private void AddEditRecordForm_Load(object sender, EventArgs e)
        {
            if (!IsAddMode && WorkLogRowToEdit != null)
            {
                // Заполняем только видимые поля
               
                txtStartDate.Text = WorkLogRowToEdit.StartDate.ToString("dd.MM.yyyy");
                txtEndDate.Text = WorkLogRowToEdit.EndDate.ToString("dd.MM.yyyy");
                txtDescription.Text = WorkLogRowToEdit.Description;
            }
            else
            {
                // При добавлении КодЗаписи скрыт, а КодРаботника берем из главной формы
            
                txtStartDate.Text = DateTime.Today.ToString("dd.MM.yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(7).ToString("dd.MM.yyyy");
            }
        }

        // ========== ОБРАБОТЧИКИ KEYPRESS ==========



        private void TxtDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == '.' && tb.Text.Contains("."))
            {
                int dotCount = 0;
                foreach (char c in tb.Text)
                {
                    if (c == '.') dotCount++;
                }
                if (dotCount >= 2)
                {
                    e.Handled = true;
                }
            }
        }

        // ========== СОХРАНЕНИЕ ==========

        private void btnSave_Click(object sender, EventArgs e)
        {


            if (!DateTime.TryParseExact(txtStartDate.Text, "dd.MM.yyyy",
                null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                MessageBox.Show("Неверный формат даты начала! Используйте дд.мм.гггг");
                return;
            }

            if (!DateTime.TryParseExact(txtEndDate.Text, "dd.MM.yyyy",
                null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
            {
                MessageBox.Show("Неверный формат даты окончания! Используйте дд.мм.гггг");
                return;
            }

            if (endDate < startDate)
            {
                MessageBox.Show("Дата окончания не может быть раньше даты начала!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Заполните описание работы!");
                return;
            }

            try
            {
                if (IsAddMode)
                {
                    DataSet1.WorkLogRow newRow = TheDataSet.WorkLog.NewWorkLogRow();
                    newRow.StartDate = startDate;
                    newRow.EndDate = endDate;
                    newRow.Description = txtDescription.Text;
                    TheDataSet.WorkLog.AddWorkLogRow(newRow);
                }
                else
                {
       
                    WorkLogRowToEdit.StartDate = startDate;
                    WorkLogRowToEdit.EndDate = endDate;
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

        // Заглушки для Designer
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
    }
}