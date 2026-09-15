using System;
using System.Windows.Forms;

namespace pz111
{
    public partial class AddEditWorkerForm : Form
    {
        public DataSet1 TheDataSet;
        public bool IsAddMode;
        public DataSet1.WorkerRow WorkerRowToEdit;

        public AddEditWorkerForm()
        {
            InitializeComponent();

          

            // Подписываемся на события KeyPress
            txtInn.KeyPress += TxtInn_KeyPress;
            txtFullName.KeyPress += TxtFullName_KeyPress;
            txtPosition.KeyPress += TxtPosition_KeyPress;
        }

        private void AddEditWorkerForm_Load(object sender, EventArgs e)
        {
            if (!IsAddMode && WorkerRowToEdit != null)
            {
                // Заполняем только видимые поля
                txtFullName.Text = WorkerRowToEdit.FullName;
                txtPosition.Text = WorkerRowToEdit.Position;
                txtInn.Text = WorkerRowToEdit.Inn;
            }
            // Блок else удален, так как при добавлении поле и так скрыто и данные не нужны
        }

        // ========== ОБРАБОТЧИКИ KEYPRESS ==========

        private void TxtInn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            TextBox tb = sender as TextBox;
            if (tb.TextLength >= 12 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar)
                && e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void TxtPosition_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar)
                && e.KeyChar != ' ' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        // ========== СОХРАНЕНИЕ ==========

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtPosition.Text))
            {
                MessageBox.Show("Заполните ФИО и Должность!");
                return;
            }

            if (txtInn.Text.Length != 12)
            {
                MessageBox.Show("ИНН должен содержать ровно 12 цифр!");
                return;
            }

            try
            {
                if (IsAddMode)
                {
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

        // Заглушки для Designer
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void Form2_Load(object sender, EventArgs e) { }
    }
}