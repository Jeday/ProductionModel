namespace ProductionModel
{
    partial class FactControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.FactText = new System.Windows.Forms.Label();
            this.FactValueControl = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.FactValueControl)).BeginInit();
            this.SuspendLayout();
            // 
            // FactText
            // 
            this.FactText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FactText.AutoSize = true;
            this.FactText.Location = new System.Drawing.Point(4, 12);
            this.FactText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FactText.Name = "FactText";
            this.FactText.Size = new System.Drawing.Size(62, 17);
            this.FactText.TabIndex = 0;
            this.FactText.Text = "FactText";
            // 
            // FactValueControl
            // 
            this.FactValueControl.LargeChange = 10;
            this.FactValueControl.Location = new System.Drawing.Point(4, 32);
            this.FactValueControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FactValueControl.Maximum = 100;
            this.FactValueControl.Name = "FactValueControl";
            this.FactValueControl.Size = new System.Drawing.Size(429, 56);
            this.FactValueControl.TabIndex = 1;
            this.FactValueControl.TickFrequency = 5;
            // 
            // FactControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FactValueControl);
            this.Controls.Add(this.FactText);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FactControl";
            this.Size = new System.Drawing.Size(436, 81);
            ((System.ComponentModel.ISupportInitialize)(this.FactValueControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label FactText;
        public System.Windows.Forms.TrackBar FactValueControl;
    }
}
