namespace TimeTable
{
    partial class Menu
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFillMasters = new System.Windows.Forms.Label();
            this.labelDistributionWorks = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::TimeTable.Properties.Resources._0_12062b_929f39d2_orig;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(423, 260);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TimeTable.Properties.Resources.w512h5121377940132185095settingsstreamline;
            this.pictureBox1.Location = new System.Drawing.Point(420, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(431, 260);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Mongolian Baiti", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(40, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(365, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Составление расписания";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Mongolian Baiti", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(494, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(252, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Заполнение базы";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // labelFillMasters
            // 
            this.labelFillMasters.AutoSize = true;
            this.labelFillMasters.BackColor = System.Drawing.Color.Transparent;
            this.labelFillMasters.Font = new System.Drawing.Font("Mongolian Baiti", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelFillMasters.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelFillMasters.Location = new System.Drawing.Point(517, 367);
            this.labelFillMasters.Name = "labelFillMasters";
            this.labelFillMasters.Size = new System.Drawing.Size(321, 32);
            this.labelFillMasters.TabIndex = 1;
            this.labelFillMasters.Text = "Заполнение мастеров";
            this.labelFillMasters.Click += new System.EventHandler(this.labelFillMasters_Click);
            // 
            // labelDistributionWorks
            // 
            this.labelDistributionWorks.AutoSize = true;
            this.labelDistributionWorks.BackColor = System.Drawing.Color.Transparent;
            this.labelDistributionWorks.Font = new System.Drawing.Font("Mongolian Baiti", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDistributionWorks.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelDistributionWorks.Location = new System.Drawing.Point(55, 367);
            this.labelDistributionWorks.Name = "labelDistributionWorks";
            this.labelDistributionWorks.Size = new System.Drawing.Size(327, 32);
            this.labelDistributionWorks.TabIndex = 1;
            this.labelDistributionWorks.Text = "Составление нагрузки";
            this.labelDistributionWorks.Click += new System.EventHandler(this.labelDistributionWorks_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 481);
            this.Controls.Add(this.labelDistributionWorks);
            this.Controls.Add(this.labelFillMasters);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFillMasters;
        private System.Windows.Forms.Label labelDistributionWorks;
    }
}

