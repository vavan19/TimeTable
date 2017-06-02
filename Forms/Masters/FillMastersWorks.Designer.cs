namespace TimeTable.Forms
{
    partial class FillMastersWorks
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
            this.listBoxMasters = new System.Windows.Forms.ListBox();
            this.listBoxClasses = new System.Windows.Forms.ListBox();
            this.listBoxMasterClasses = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxMasters
            // 
            this.listBoxMasters.FormattingEnabled = true;
            this.listBoxMasters.Location = new System.Drawing.Point(12, 53);
            this.listBoxMasters.Name = "listBoxMasters";
            this.listBoxMasters.Size = new System.Drawing.Size(129, 173);
            this.listBoxMasters.TabIndex = 0;
            this.listBoxMasters.Click += new System.EventHandler(this.listBoxMasters_Click);
            // 
            // listBoxClasses
            // 
            this.listBoxClasses.FormattingEnabled = true;
            this.listBoxClasses.Location = new System.Drawing.Point(384, 53);
            this.listBoxClasses.Name = "listBoxClasses";
            this.listBoxClasses.Size = new System.Drawing.Size(137, 173);
            this.listBoxClasses.TabIndex = 0;
            this.listBoxClasses.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxClasses_MouseDoubleClick);
            // 
            // listBoxMasterClasses
            // 
            this.listBoxMasterClasses.FormattingEnabled = true;
            this.listBoxMasterClasses.Location = new System.Drawing.Point(189, 53);
            this.listBoxMasterClasses.Name = "listBoxMasterClasses";
            this.listBoxMasterClasses.Size = new System.Drawing.Size(137, 173);
            this.listBoxMasterClasses.TabIndex = 0;
            this.listBoxMasterClasses.SelectedIndexChanged += new System.EventHandler(this.listBoxMasterClasses_SelectedIndexChanged);
            // 
            // FillMastersWorks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 287);
            this.Controls.Add(this.listBoxMasterClasses);
            this.Controls.Add(this.listBoxClasses);
            this.Controls.Add(this.listBoxMasters);
            this.Name = "FillMastersWorks";
            this.Text = "FillMastersWorks";
            this.Load += new System.EventHandler(this.FillMastersWorks_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxMasters;
        private System.Windows.Forms.ListBox listBoxClasses;
        private System.Windows.Forms.ListBox listBoxMasterClasses;
    }
}