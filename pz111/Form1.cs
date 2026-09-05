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
    public partial class Form1 : Form
    {
        //DataSet1 dataSet1 = new DataSet1();
        const string FILE_PATH = "DataSet.xml";

        private void SaveToXml()
        {
            dataSet1.Worker.AcceptChanges();
            dataSet1.WorkLog.AcceptChanges();
            dataSet1.WriteXml(FILE_PATH);
        }

        private void LoadFromXml()
        {
            dataSet1.Clear();
            dataSet1.ReadXml(FILE_PATH);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void workerDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //добавить запись
            AddEditRecordForm addEditRecordForm = new AddEditRecordForm();
            addEditRecordForm.ShowDialog();
            SaveToXml();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //добавить работника
            AddEditWorkerForm addEditWorkerForm = new AddEditWorkerForm();
            addEditWorkerForm.ShowDialog();
            SaveToXml();
        }

        private void workLogDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
