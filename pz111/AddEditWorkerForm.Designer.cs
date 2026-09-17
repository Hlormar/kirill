namespace pz111
{
    partial class AddEditWorkerForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxWorkerFIO = new System.Windows.Forms.TextBox();
            this.textBoxWorkerINN = new System.Windows.Forms.TextBox();
            this.textBoxWorkerPosition = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ИНН:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ФИО:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Должность:";
            // 
            // textBoxWorkerFIO
            // 
            this.textBoxWorkerFIO.Location = new System.Drawing.Point(129, 31);
            this.textBoxWorkerFIO.Name = "textBoxWorkerFIO";
            this.textBoxWorkerFIO.Size = new System.Drawing.Size(292, 20);
            this.textBoxWorkerFIO.TabIndex = 1;
            this.textBoxWorkerFIO.TextChanged += new System.EventHandler(this.textChangedFIO);
            this.textBoxWorkerFIO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPressFIO);
            // 
            // textBoxWorkerINN
            // 
            this.textBoxWorkerINN.Location = new System.Drawing.Point(129, 102);
            this.textBoxWorkerINN.Name = "textBoxWorkerINN";
            this.textBoxWorkerINN.Size = new System.Drawing.Size(292, 20);
            this.textBoxWorkerINN.TabIndex = 3;
            this.textBoxWorkerINN.TextChanged += new System.EventHandler(this.textChangedINN);
            this.textBoxWorkerINN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPressINN);
            // 
            // textBoxWorkerPosition
            // 
            this.textBoxWorkerPosition.Location = new System.Drawing.Point(129, 65);
            this.textBoxWorkerPosition.Name = "textBoxWorkerPosition";
            this.textBoxWorkerPosition.Size = new System.Drawing.Size(292, 20);
            this.textBoxWorkerPosition.TabIndex = 2;
            this.textBoxWorkerPosition.TextChanged += new System.EventHandler(this.textChangedPosition);
            this.textBoxWorkerPosition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyPressPosition);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(231, 188);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(122, 188);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButtonClick);
            // 
            // AddEditWorkerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 229);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.textBoxWorkerPosition);
            this.Controls.Add(this.textBoxWorkerINN);
            this.Controls.Add(this.textBoxWorkerFIO);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddEditWorkerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавить работника";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxWorkerFIO;
        private System.Windows.Forms.TextBox textBoxWorkerINN;
        private System.Windows.Forms.TextBox textBoxWorkerPosition;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
    }
}