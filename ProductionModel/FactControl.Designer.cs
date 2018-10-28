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
            this.FactText.Location = new System.Drawing.Point(3, 10);
            this.FactText.Name = "FactText";
            this.FactText.Size = new System.Drawing.Size(49, 13);
            this.FactText.TabIndex = 0;
            this.FactText.Text = "FactText";
            // 
            // FactValueControl
            // 
            this.FactValueControl.Location = new System.Drawing.Point(3, 26);
            this.FactValueControl.Maximum = 100;
            this.FactValueControl.Name = "FactValueControl";
            this.FactValueControl.Size = new System.Drawing.Size(383, 45);
            this.FactValueControl.TabIndex = 1;
            this.FactValueControl.TickFrequency = 10;
            // 
            // FactControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FactValueControl);
            this.Controls.Add(this.FactText);
            this.Name = "FactControl";
            this.Size = new System.Drawing.Size(388, 66);
            ((System.ComponentModel.ISupportInitialize)(this.FactValueControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label FactText;
        public System.Windows.Forms.TrackBar FactValueControl;
    }
}
