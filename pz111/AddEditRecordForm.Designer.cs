namespace pz111
{
    partial class AddEditRecordForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRecordID = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxRecordEndDate = new System.Windows.Forms.TextBox();
            this.textBoxRecordStartDate = new System.Windows.Forms.TextBox();
            this.textBoxRecordWorkerID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxRecordDescription = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Код записи:";
            // 
            // textBoxRecordID
            // 
            this.textBoxRecordID.Location = new System.Drawing.Point(142, 27);
            this.textBoxRecordID.Name = "textBoxRecordID";
            this.textBoxRecordID.Size = new System.Drawing.Size(95, 20);
            this.textBoxRecordID.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(69, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Отмена";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(174, 216);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Сохранить";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBoxRecordEndDate
            // 
            this.textBoxRecordEndDate.Location = new System.Drawing.Point(142, 133);
            this.textBoxRecordEndDate.Name = "textBoxRecordEndDate";
            this.textBoxRecordEndDate.Size = new System.Drawing.Size(95, 20);
            this.textBoxRecordEndDate.TabIndex = 4;
            // 
            // textBoxRecordStartDate
            // 
            this.textBoxRecordStartDate.Location = new System.Drawing.Point(142, 98);
            this.textBoxRecordStartDate.Name = "textBoxRecordStartDate";
            this.textBoxRecordStartDate.Size = new System.Drawing.Size(95, 20);
            this.textBoxRecordStartDate.TabIndex = 5;
            // 
            // textBoxRecordWorkerID
            // 
            this.textBoxRecordWorkerID.Location = new System.Drawing.Point(142, 63);
            this.textBoxRecordWorkerID.Name = "textBoxRecordWorkerID";
            this.textBoxRecordWorkerID.Size = new System.Drawing.Size(95, 20);
            this.textBoxRecordWorkerID.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Код работника:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Дата начала:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Дата окончания:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Описание работы:";
            // 
            // textBoxRecordDescription
            // 
            this.textBoxRecordDescription.Location = new System.Drawing.Point(142, 169);
            this.textBoxRecordDescription.Name = "textBoxRecordDescription";
            this.textBoxRecordDescription.Size = new System.Drawing.Size(183, 20);
            this.textBoxRecordDescription.TabIndex = 10;
            // 
            // AddEditRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 259);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxRecordDescription);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxRecordWorkerID);
            this.Controls.Add(this.textBoxRecordStartDate);
            this.Controls.Add(this.textBoxRecordEndDate);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxRecordID);
            this.Controls.Add(this.label1);
            this.Name = "AddEditRecordForm";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRecordID;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxRecordEndDate;
        private System.Windows.Forms.TextBox textBoxRecordStartDate;
        private System.Windows.Forms.TextBox textBoxRecordWorkerID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxRecordDescription;
    }
}