namespace ProductionModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.ThoughtLinePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.FCbutton = new System.Windows.Forms.RadioButton();
            this.BackwardReasoningButton = new System.Windows.Forms.RadioButton();
            this.ForwardReasoningButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(483, 17);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(639, 470);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(580, 514);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 46);
            this.label1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(27, 525);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "Load File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(27, 17);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(425, 470);
            this.flowLayoutPanel1.TabIndex = 5;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(147, 525);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 30);
            this.button2.TabIndex = 6;
            this.button2.Text = "Calculate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Run_Clicked);
            // 
            // ThoughtLinePanel
            // 
            this.ThoughtLinePanel.AutoScroll = true;
            this.ThoughtLinePanel.BackColor = System.Drawing.Color.White;
            this.ThoughtLinePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ThoughtLinePanel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ThoughtLinePanel.Location = new System.Drawing.Point(27, 576);
            this.ThoughtLinePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ThoughtLinePanel.Name = "ThoughtLinePanel";
            this.ThoughtLinePanel.Size = new System.Drawing.Size(1096, 173);
            this.ThoughtLinePanel.TabIndex = 7;
            this.ThoughtLinePanel.WrapContents = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.FCbutton);
            this.groupBox2.Controls.Add(this.BackwardReasoningButton);
            this.groupBox2.Controls.Add(this.ForwardReasoningButton);
            this.groupBox2.Location = new System.Drawing.Point(263, 494);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(311, 79);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reasoning";
            // 
            // FCbutton
            // 
            this.FCbutton.AutoSize = true;
            this.FCbutton.Location = new System.Drawing.Point(119, 24);
            this.FCbutton.Name = "FCbutton";
            this.FCbutton.Size = new System.Drawing.Size(152, 23);
            this.FCbutton.TabIndex = 2;
            this.FCbutton.TabStop = true;
            this.FCbutton.Text = "Forward Confidence";
            this.FCbutton.UseVisualStyleBackColor = true;
            // 
            // BackwardReasoningButton
            // 
            this.BackwardReasoningButton.AutoSize = true;
            this.BackwardReasoningButton.Location = new System.Drawing.Point(6, 51);
            this.BackwardReasoningButton.Name = "BackwardReasoningButton";
            this.BackwardReasoningButton.Size = new System.Drawing.Size(88, 23);
            this.BackwardReasoningButton.TabIndex = 1;
            this.BackwardReasoningButton.TabStop = true;
            this.BackwardReasoningButton.Text = "Backward";
            this.BackwardReasoningButton.UseVisualStyleBackColor = true;
            // 
            // ForwardReasoningButton
            // 
            this.ForwardReasoningButton.AutoSize = true;
            this.ForwardReasoningButton.Location = new System.Drawing.Point(6, 24);
            this.ForwardReasoningButton.Name = "ForwardReasoningButton";
            this.ForwardReasoningButton.Size = new System.Drawing.Size(80, 23);
            this.ForwardReasoningButton.TabIndex = 0;
            this.ForwardReasoningButton.TabStop = true;
            this.ForwardReasoningButton.Text = "Forward";
            this.ForwardReasoningButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 763);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ThoughtLinePanel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Production Model - The Simpsons";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FlowLayoutPanel ThoughtLinePanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton FCbutton;
        private System.Windows.Forms.RadioButton BackwardReasoningButton;
        private System.Windows.Forms.RadioButton ForwardReasoningButton;
    }
}

