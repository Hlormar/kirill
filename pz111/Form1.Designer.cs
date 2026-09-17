namespace pz111
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.workerDataGridView = new System.Windows.Forms.DataGridView();
            this.workLogDataGridView = new System.Windows.Forms.DataGridView();
            this.btnAddRecord = new System.Windows.Forms.Button();
            this.btnEditRecord = new System.Windows.Forms.Button();
            this.btnDeleteRecord = new System.Windows.Forms.Button();
            this.btnEditWorker = new System.Windows.Forms.Button();
            this.btnAddWorker = new System.Windows.Forms.Button();
            this.btnDeleteWorker = new System.Windows.Forms.Button();
            this.btnSaveXml = new System.Windows.Forms.Button();
            this.btnLoadXml = new System.Windows.Forms.Button();
            this.btnSaveJson = new System.Windows.Forms.Button();
            this.btnLoadJson = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workLogBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new pz111.DataSet1();
            this.workerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.innDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.workerDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workLogDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workLogBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // workerDataGridView
            // 
            this.workerDataGridView.AllowUserToAddRows = false;
            this.workerDataGridView.AllowUserToDeleteRows = false;
            this.workerDataGridView.AutoGenerateColumns = false;
            this.workerDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.workerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.workerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.workerIdDataGridViewTextBoxColumn,
            this.fullNameDataGridViewTextBoxColumn,
            this.positionDataGridViewTextBoxColumn,
            this.innDataGridViewTextBoxColumn});
            this.workerDataGridView.DataSource = this.workerBindingSource;
            this.workerDataGridView.Location = new System.Drawing.Point(24, 398);
            this.workerDataGridView.Margin = new System.Windows.Forms.Padding(6);
            this.workerDataGridView.Name = "workerDataGridView";
            this.workerDataGridView.RowHeadersWidth = 82;
            this.workerDataGridView.Size = new System.Drawing.Size(1084, 340);
            this.workerDataGridView.TabIndex = 1;
            // 
            // workLogDataGridView
            // 
            this.workLogDataGridView.AllowUserToAddRows = false;
            this.workLogDataGridView.AllowUserToDeleteRows = false;
            this.workLogDataGridView.AutoGenerateColumns = false;
            this.workLogDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.workLogDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.workLogDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            this.workLogDataGridView.DataSource = this.workLogBindingSource;
            this.workLogDataGridView.Location = new System.Drawing.Point(24, 23);
            this.workLogDataGridView.Margin = new System.Windows.Forms.Padding(6);
            this.workLogDataGridView.Name = "workLogDataGridView";
            this.workLogDataGridView.RowHeadersWidth = 82;
            this.workLogDataGridView.Size = new System.Drawing.Size(1084, 342);
            this.workLogDataGridView.TabIndex = 2;
            // 
            // btnAddRecord
            // 
            this.btnAddRecord.Location = new System.Drawing.Point(1120, 23);
            this.btnAddRecord.Margin = new System.Windows.Forms.Padding(6);
            this.btnAddRecord.Name = "btnAddRecord";
            this.btnAddRecord.Size = new System.Drawing.Size(336, 106);
            this.btnAddRecord.TabIndex = 3;
            this.btnAddRecord.Text = "Добавить запись";
            this.btnAddRecord.UseVisualStyleBackColor = true;
            this.btnAddRecord.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnEditRecord
            // 
            this.btnEditRecord.Location = new System.Drawing.Point(1120, 142);
            this.btnEditRecord.Margin = new System.Windows.Forms.Padding(6);
            this.btnEditRecord.Name = "btnEditRecord";
            this.btnEditRecord.Size = new System.Drawing.Size(336, 106);
            this.btnEditRecord.TabIndex = 4;
            this.btnEditRecord.Text = "Редактировать запись";
            this.btnEditRecord.UseVisualStyleBackColor = true;
            this.btnEditRecord.Click += new System.EventHandler(this.btnEditRecord_Click);
            // 
            // btnDeleteRecord
            // 
            this.btnDeleteRecord.Location = new System.Drawing.Point(1122, 260);
            this.btnDeleteRecord.Margin = new System.Windows.Forms.Padding(6);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(336, 106);
            this.btnDeleteRecord.TabIndex = 5;
            this.btnDeleteRecord.Text = "Удалить запись";
            this.btnDeleteRecord.UseVisualStyleBackColor = true;
            this.btnDeleteRecord.Click += new System.EventHandler(this.btnDeleteRecord_Click);
            // 
            // btnEditWorker
            // 
            this.btnEditWorker.Location = new System.Drawing.Point(1120, 515);
            this.btnEditWorker.Margin = new System.Windows.Forms.Padding(6);
            this.btnEditWorker.Name = "btnEditWorker";
            this.btnEditWorker.Size = new System.Drawing.Size(336, 106);
            this.btnEditWorker.TabIndex = 6;
            this.btnEditWorker.Text = "Редактировать работника";
            this.btnEditWorker.UseVisualStyleBackColor = true;
            this.btnEditWorker.Click += new System.EventHandler(this.btnEditWorker_Click);
            // 
            // btnAddWorker
            // 
            this.btnAddWorker.Location = new System.Drawing.Point(1120, 398);
            this.btnAddWorker.Margin = new System.Windows.Forms.Padding(6);
            this.btnAddWorker.Name = "btnAddWorker";
            this.btnAddWorker.Size = new System.Drawing.Size(336, 106);
            this.btnAddWorker.TabIndex = 7;
            this.btnAddWorker.Text = "Добавить работника";
            this.btnAddWorker.UseVisualStyleBackColor = true;
            this.btnAddWorker.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnDeleteWorker
            // 
            this.btnDeleteWorker.Location = new System.Drawing.Point(1120, 633);
            this.btnDeleteWorker.Margin = new System.Windows.Forms.Padding(6);
            this.btnDeleteWorker.Name = "btnDeleteWorker";
            this.btnDeleteWorker.Size = new System.Drawing.Size(336, 106);
            this.btnDeleteWorker.TabIndex = 8;
            this.btnDeleteWorker.Text = "Удалить работника";
            this.btnDeleteWorker.UseVisualStyleBackColor = true;
            this.btnDeleteWorker.Click += new System.EventHandler(this.btnDeleteWorker_Click);
            // 
            // btnSaveXml
            // 
            this.btnSaveXml.Location = new System.Drawing.Point(24, 750);
            this.btnSaveXml.Margin = new System.Windows.Forms.Padding(6);
            this.btnSaveXml.Name = "btnSaveXml";
            this.btnSaveXml.Size = new System.Drawing.Size(264, 63);
            this.btnSaveXml.TabIndex = 9;
            this.btnSaveXml.Text = "Сохранить XML";
            this.btnSaveXml.UseVisualStyleBackColor = true;
            this.btnSaveXml.Click += new System.EventHandler(this.btnSaveXml_Click);
            // 
            // btnLoadXml
            // 
            this.btnLoadXml.Location = new System.Drawing.Point(300, 750);
            this.btnLoadXml.Margin = new System.Windows.Forms.Padding(6);
            this.btnLoadXml.Name = "btnLoadXml";
            this.btnLoadXml.Size = new System.Drawing.Size(264, 63);
            this.btnLoadXml.TabIndex = 10;
            this.btnLoadXml.Text = "Загрузить XML";
            this.btnLoadXml.UseVisualStyleBackColor = true;
            this.btnLoadXml.Click += new System.EventHandler(this.btnLoadXml_Click);
            // 
            // btnSaveJson
            // 
            this.btnSaveJson.Location = new System.Drawing.Point(576, 750);
            this.btnSaveJson.Margin = new System.Windows.Forms.Padding(6);
            this.btnSaveJson.Name = "btnSaveJson";
            this.btnSaveJson.Size = new System.Drawing.Size(264, 63);
            this.btnSaveJson.TabIndex = 11;
            this.btnSaveJson.Text = "Сохранить JSON";
            this.btnSaveJson.UseVisualStyleBackColor = true;
            this.btnSaveJson.Click += new System.EventHandler(this.btnSaveJson_Click);
            // 
            // btnLoadJson
            // 
            this.btnLoadJson.Location = new System.Drawing.Point(852, 750);
            this.btnLoadJson.Margin = new System.Windows.Forms.Padding(6);
            this.btnLoadJson.Name = "btnLoadJson";
            this.btnLoadJson.Size = new System.Drawing.Size(256, 63);
            this.btnLoadJson.TabIndex = 12;
            this.btnLoadJson.Text = "Загрузить JSON";
            this.btnLoadJson.UseVisualStyleBackColor = true;
            this.btnLoadJson.Click += new System.EventHandler(this.btnLoadJson_Click);
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "RecordId";
            this.dataGridViewTextBoxColumn5.HeaderText = "RecordId";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "WorkerId";
            this.dataGridViewTextBoxColumn6.HeaderText = "WorkerId";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "StartDate";
            this.dataGridViewTextBoxColumn7.HeaderText = "StartDate";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "EndDate";
            this.dataGridViewTextBoxColumn8.HeaderText = "EndDate";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn9.HeaderText = "Description";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 10;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // workLogBindingSource
            // 
            this.workLogBindingSource.DataMember = "WorkLog";
            this.workLogBindingSource.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // workerIdDataGridViewTextBoxColumn
            // 
            this.workerIdDataGridViewTextBoxColumn.DataPropertyName = "WorkerId";
            this.workerIdDataGridViewTextBoxColumn.HeaderText = "WorkerId";
            this.workerIdDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.workerIdDataGridViewTextBoxColumn.Name = "workerIdDataGridViewTextBoxColumn";
            // 
            // fullNameDataGridViewTextBoxColumn
            // 
            this.fullNameDataGridViewTextBoxColumn.DataPropertyName = "FullName";
            this.fullNameDataGridViewTextBoxColumn.HeaderText = "FullName";
            this.fullNameDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.fullNameDataGridViewTextBoxColumn.Name = "fullNameDataGridViewTextBoxColumn";
            // 
            // positionDataGridViewTextBoxColumn
            // 
            this.positionDataGridViewTextBoxColumn.DataPropertyName = "Position";
            this.positionDataGridViewTextBoxColumn.HeaderText = "Position";
            this.positionDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.positionDataGridViewTextBoxColumn.Name = "positionDataGridViewTextBoxColumn";
            // 
            // innDataGridViewTextBoxColumn
            // 
            this.innDataGridViewTextBoxColumn.DataPropertyName = "Inn";
            this.innDataGridViewTextBoxColumn.HeaderText = "Inn";
            this.innDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.innDataGridViewTextBoxColumn.Name = "innDataGridViewTextBoxColumn";
            // 
            // workerBindingSource
            // 
            this.workerBindingSource.DataMember = "Worker";
            this.workerBindingSource.DataSource = this.dataSet1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1476, 829);
            this.Controls.Add(this.btnLoadJson);
            this.Controls.Add(this.btnSaveJson);
            this.Controls.Add(this.btnLoadXml);
            this.Controls.Add(this.btnSaveXml);
            this.Controls.Add(this.btnDeleteWorker);
            this.Controls.Add(this.btnAddWorker);
            this.Controls.Add(this.btnEditWorker);
            this.Controls.Add(this.btnDeleteRecord);
            this.Controls.Add(this.btnEditRecord);
            this.Controls.Add(this.btnAddRecord);
            this.Controls.Add(this.workLogDataGridView);
            this.Controls.Add(this.workerDataGridView);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сотрудники / Записи";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.workerDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workLogDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workLogBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataSet1 dataSet1;
        private System.Windows.Forms.BindingSource workerBindingSource;
        private System.Windows.Forms.DataGridView workerDataGridView;
        private System.Windows.Forms.BindingSource workLogBindingSource;
        private System.Windows.Forms.DataGridView workLogDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn workerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn innDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnAddRecord;
        private System.Windows.Forms.Button btnEditRecord;
        private System.Windows.Forms.Button btnDeleteRecord;
        private System.Windows.Forms.Button btnEditWorker;
        private System.Windows.Forms.Button btnAddWorker;
        private System.Windows.Forms.Button btnDeleteWorker;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.Button btnSaveXml;
        private System.Windows.Forms.Button btnLoadXml;
        private System.Windows.Forms.Button btnSaveJson;
        private System.Windows.Forms.Button btnLoadJson;
    }
}