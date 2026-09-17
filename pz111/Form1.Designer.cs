namespace pz111
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.workerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new pz111.DataSet1();
            this.workerDataGridView = new System.Windows.Forms.DataGridView();
            this.Worker_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Worker_FIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Worker_Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Worker_INN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workLogDataGridView = new System.Windows.Forms.DataGridView();
            this.workLogBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AddRecordButton = new System.Windows.Forms.Button();
            this.EditRecordButton = new System.Windows.Forms.Button();
            this.DeleteRecordButton = new System.Windows.Forms.Button();
            this.EditWorkerButton = new System.Windows.Forms.Button();
            this.AddWorkerButton = new System.Windows.Forms.Button();
            this.DeleteWorkerButton = new System.Windows.Forms.Button();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.workerBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.workerBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RecordId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.workerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workLogDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workLogBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerBindingNavigator)).BeginInit();
            this.workerBindingNavigator.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // workerBindingSource
            // 
            this.workerBindingSource.DataMember = "Worker";
            this.workerBindingSource.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // workerDataGridView
            // 
            this.workerDataGridView.AllowUserToAddRows = false;
            this.workerDataGridView.AllowUserToDeleteRows = false;
            this.workerDataGridView.AllowUserToResizeColumns = false;
            this.workerDataGridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.workerDataGridView, "workerDataGridView");
            this.workerDataGridView.AutoGenerateColumns = false;
            this.workerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.workerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Worker_ID,
            this.Worker_FIO,
            this.Worker_Position,
            this.Worker_INN});
            this.workerDataGridView.DataSource = this.workerBindingSource;
            this.workerDataGridView.MultiSelect = false;
            this.workerDataGridView.Name = "workerDataGridView";
            this.workerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.workerDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.workerDataGridView_CellContentClick);
            // 
            // Worker_ID
            // 
            this.Worker_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Worker_ID.DataPropertyName = "WorkerId";
            resources.ApplyResources(this.Worker_ID, "Worker_ID");
            this.Worker_ID.Name = "Worker_ID";
            this.Worker_ID.ReadOnly = true;
            // 
            // Worker_FIO
            // 
            this.Worker_FIO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Worker_FIO.DataPropertyName = "FullName";
            this.Worker_FIO.FillWeight = 300F;
            resources.ApplyResources(this.Worker_FIO, "Worker_FIO");
            this.Worker_FIO.Name = "Worker_FIO";
            this.Worker_FIO.ReadOnly = true;
            // 
            // Worker_Position
            // 
            this.Worker_Position.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Worker_Position.DataPropertyName = "Position";
            this.Worker_Position.FillWeight = 200F;
            resources.ApplyResources(this.Worker_Position, "Worker_Position");
            this.Worker_Position.Name = "Worker_Position";
            this.Worker_Position.ReadOnly = true;
            // 
            // Worker_INN
            // 
            this.Worker_INN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Worker_INN.DataPropertyName = "Inn";
            this.Worker_INN.FillWeight = 200F;
            resources.ApplyResources(this.Worker_INN, "Worker_INN");
            this.Worker_INN.Name = "Worker_INN";
            this.Worker_INN.ReadOnly = true;
            // 
            // workLogDataGridView
            // 
            this.workLogDataGridView.AllowUserToAddRows = false;
            this.workLogDataGridView.AllowUserToDeleteRows = false;
            this.workLogDataGridView.AllowUserToResizeColumns = false;
            this.workLogDataGridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.workLogDataGridView, "workLogDataGridView");
            this.workLogDataGridView.AutoGenerateColumns = false;
            this.workLogDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.workLogDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordId,
            this.FIO,
            this.StartDate,
            this.EndDate,
            this.Description,
            this.WorkerID});
            this.workLogDataGridView.DataSource = this.workLogBindingSource;
            this.workLogDataGridView.MultiSelect = false;
            this.workLogDataGridView.Name = "workLogDataGridView";
            this.workLogDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.workLogDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.workLogDataGridView_CellContentClick);
            this.workLogDataGridView.Sorted += new System.EventHandler(this.workLogDataGridView_Sorted);
            // 
            // workLogBindingSource
            // 
            this.workLogBindingSource.DataMember = "WorkLog";
            this.workLogBindingSource.DataSource = this.dataSet1;
            // 
            // AddRecordButton
            // 
            resources.ApplyResources(this.AddRecordButton, "AddRecordButton");
            this.AddRecordButton.Name = "AddRecordButton";
            this.AddRecordButton.UseVisualStyleBackColor = true;
            this.AddRecordButton.Click += new System.EventHandler(this.addRecordButtonClick);
            // 
            // EditRecordButton
            // 
            resources.ApplyResources(this.EditRecordButton, "EditRecordButton");
            this.EditRecordButton.Name = "EditRecordButton";
            this.EditRecordButton.UseVisualStyleBackColor = true;
            this.EditRecordButton.Click += new System.EventHandler(this.editRecordButtonClick);
            // 
            // DeleteRecordButton
            // 
            resources.ApplyResources(this.DeleteRecordButton, "DeleteRecordButton");
            this.DeleteRecordButton.Name = "DeleteRecordButton";
            this.DeleteRecordButton.UseVisualStyleBackColor = true;
            this.DeleteRecordButton.Click += new System.EventHandler(this.deleteRecordButtonClick);
            // 
            // EditWorkerButton
            // 
            resources.ApplyResources(this.EditWorkerButton, "EditWorkerButton");
            this.EditWorkerButton.Name = "EditWorkerButton";
            this.EditWorkerButton.UseVisualStyleBackColor = true;
            this.EditWorkerButton.Click += new System.EventHandler(this.editWorkerButtonClick);
            // 
            // AddWorkerButton
            // 
            resources.ApplyResources(this.AddWorkerButton, "AddWorkerButton");
            this.AddWorkerButton.Name = "AddWorkerButton";
            this.AddWorkerButton.UseVisualStyleBackColor = true;
            this.AddWorkerButton.Click += new System.EventHandler(this.addWorkerButtonClick);
            // 
            // DeleteWorkerButton
            // 
            resources.ApplyResources(this.DeleteWorkerButton, "DeleteWorkerButton");
            this.DeleteWorkerButton.Name = "DeleteWorkerButton";
            this.DeleteWorkerButton.UseVisualStyleBackColor = true;
            this.DeleteWorkerButton.Click += new System.EventHandler(this.deleteWorkerButtonClick);
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveFirstItem, "bindingNavigatorMoveFirstItem");
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMovePreviousItem, "bindingNavigatorMovePreviousItem");
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            resources.ApplyResources(this.bindingNavigatorSeparator, "bindingNavigatorSeparator");
            // 
            // bindingNavigatorPositionItem
            // 
            resources.ApplyResources(this.bindingNavigatorPositionItem, "bindingNavigatorPositionItem");
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            resources.ApplyResources(this.bindingNavigatorCountItem, "bindingNavigatorCountItem");
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            resources.ApplyResources(this.bindingNavigatorSeparator1, "bindingNavigatorSeparator1");
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveNextItem, "bindingNavigatorMoveNextItem");
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveLastItem, "bindingNavigatorMoveLastItem");
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            resources.ApplyResources(this.bindingNavigatorSeparator2, "bindingNavigatorSeparator2");
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorAddNewItem, "bindingNavigatorAddNewItem");
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorDeleteItem, "bindingNavigatorDeleteItem");
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            // 
            // workerBindingNavigatorSaveItem
            // 
            this.workerBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.workerBindingNavigatorSaveItem, "workerBindingNavigatorSaveItem");
            this.workerBindingNavigatorSaveItem.Name = "workerBindingNavigatorSaveItem";
            // 
            // workerBindingNavigator
            // 
            this.workerBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.workerBindingNavigator.BindingSource = this.workerBindingSource;
            this.workerBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.workerBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.workerBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.workerBindingNavigatorSaveItem});
            resources.ApplyResources(this.workerBindingNavigator, "workerBindingNavigator");
            this.workerBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.workerBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.workerBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.workerBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.workerBindingNavigator.Name = "workerBindingNavigator";
            this.workerBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.workerDataGridView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.workLogDataGridView, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // RecordId
            // 
            this.RecordId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RecordId.DataPropertyName = "RecordId";
            resources.ApplyResources(this.RecordId, "RecordId");
            this.RecordId.Name = "RecordId";
            this.RecordId.ReadOnly = true;
            // 
            // FIO
            // 
            this.FIO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FIO.FillWeight = 300F;
            resources.ApplyResources(this.FIO, "FIO");
            this.FIO.Name = "FIO";
            this.FIO.ReadOnly = true;
            // 
            // StartDate
            // 
            this.StartDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StartDate.DataPropertyName = "StartDate";
            this.StartDate.FillWeight = 130F;
            resources.ApplyResources(this.StartDate, "StartDate");
            this.StartDate.Name = "StartDate";
            this.StartDate.ReadOnly = true;
            // 
            // EndDate
            // 
            this.EndDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EndDate.DataPropertyName = "EndDate";
            this.EndDate.FillWeight = 130F;
            resources.ApplyResources(this.EndDate, "EndDate");
            this.EndDate.Name = "EndDate";
            this.EndDate.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.DataPropertyName = "Description";
            this.Description.FillWeight = 200F;
            resources.ApplyResources(this.Description, "Description");
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // WorkerID
            // 
            this.WorkerID.DataPropertyName = "WorkerId";
            resources.ApplyResources(this.WorkerID, "WorkerID");
            this.WorkerID.Name = "WorkerID";
            this.WorkerID.ReadOnly = true;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.DeleteWorkerButton);
            this.Controls.Add(this.AddWorkerButton);
            this.Controls.Add(this.EditWorkerButton);
            this.Controls.Add(this.DeleteRecordButton);
            this.Controls.Add(this.EditRecordButton);
            this.Controls.Add(this.AddRecordButton);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.workerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workLogDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workLogBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerBindingNavigator)).EndInit();
            this.workerBindingNavigator.ResumeLayout(false);
            this.workerBindingNavigator.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataSet1 dataSet1;
        private System.Windows.Forms.BindingSource workerBindingSource;
        private System.Windows.Forms.DataGridView workerDataGridView;
        private System.Windows.Forms.BindingSource workLogBindingSource;
        private System.Windows.Forms.DataGridView workLogDataGridView;
        private System.Windows.Forms.Button AddRecordButton;
        private System.Windows.Forms.Button EditRecordButton;
        private System.Windows.Forms.Button DeleteRecordButton;
        private System.Windows.Forms.Button EditWorkerButton;
        private System.Windows.Forms.Button AddWorkerButton;
        private System.Windows.Forms.Button DeleteWorkerButton;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton workerBindingNavigatorSaveItem;
        private System.Windows.Forms.BindingNavigator workerBindingNavigator;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Worker_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Worker_FIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn Worker_Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Worker_INN;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordId;
        private System.Windows.Forms.DataGridViewTextBoxColumn FIO;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkerID;
    }
}

