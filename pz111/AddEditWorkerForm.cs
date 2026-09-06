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

        public AddEditWorkerForm(DataSet1 _dataSet1, DataGridViewRow _currentRow = null)
        {
            InitializeComponent();
            //чтобы получить ту самую переменную dataSet1, созданную ранее в Form1, передаем ее как аргумент конструктора 
            dataSet1 = _dataSet1;
            currentRow = _currentRow; 

            //если редактируем работника, то получаем значения текущей строки
            if (currentRow != null ) 
            { 
                textBoxWorkerID.Text = currentRow.Cells[0].Value?.ToString();
                textBoxWorkerID.ReadOnly = true;
                textBoxWorkerFIO.Text = currentRow.Cells[1].Value?.ToString();
                textBoxWorkerPosition.Text = currentRow.Cells[2].Value?.ToString();
                textBoxWorkerINN.Text = currentRow.Cells[3].Value?.ToString();
            }
        }

        //check if textBox data is valid
        private bool isDataValid()
        {
            bool isValid = true;
            string errorsMessage = string.Empty;
            TextBox[] textBoxes = { textBoxWorkerID, textBoxWorkerFIO, textBoxWorkerPosition, textBoxWorkerINN };
            //TODO: delete workerId field
            foreach (TextBox textBox in textBoxes)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    isValid = false;
                    errorsMessage += $"Поле {textBox.Name} должно быть заполнено\n";
                }
            }
            if (!textBoxWorkerINN.Text.All(char.IsDigit)) 
            {
                isValid = false;
                errorsMessage += $"Поле {textBoxWorkerINN.Name} должно содержать только числа\n";
            }

            if (!isValid) 
            {
                MessageBox.Show(errorsMessage, "Неверный формат данных", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            return isValid;

        }

        private void AddWorker() 
        { 
            DataRow newWorker = dataSet1.Worker.NewRow();
            newWorker["FullName"] = textBoxWorkerFIO.Text;
            newWorker["Position"] = textBoxWorkerPosition.Text;
            newWorker["Inn"] = textBoxWorkerINN.Text;
            dataSet1.Worker.Rows.Add(newWorker);
            dataSet1.Worker.AcceptChanges();
            Console.WriteLine("worker added");
        }

        private void EditWorker(Int64 workerId) {
            DataRow selectedRow = dataSet1.Worker.Select($"WorkerId = {workerId}")[0]; //возвращает массив с 1 элементом, сразу берем только его
            selectedRow["FullName"] = textBoxWorkerFIO.Text;
            selectedRow["Position"] = textBoxWorkerPosition.Text;
            selectedRow["Inn"] = textBoxWorkerINN.Text;
            dataSet1.AcceptChanges();

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //save
            if (isDataValid()) {
                //если передали выбранную строку, значит мы редактировали
                if (currentRow != null) {
                    EditWorker(Convert.ToInt64(textBoxWorkerID.Text));
                } else //иначе добавляем нового работника
                {
                    AddWorker();
                }
               
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //otmena
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
