using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTable
{
    public partial class PracticeAndGroup : Form
    {
        static string  dataBegin, dataEnd;
        public PracticeAndGroup()
        {
            InitializeComponent();
            
        }

        private void PracticeAndGroup_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Forms.Calendar form = new Forms.Calendar();
            form.Owner = this;
            form.ShowDialog();
            if(form.DialogResult == DialogResult.OK)
            {
                ((TextBox) sender).Text = Forms.Calendar.Data;
            }
        }

        private void PracticeAndGroup_Shown(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataBegin = textBox1.Text;
        }

        private void PracticeAndGroup_Load_1(object sender, EventArgs e)
        {

        }

        private void PracticeAndGroup_Activated(object sender, EventArgs e)
        {
            
        }

        public string DataBegin
        {
            set
            {
                dataBegin = value;
                textBox1.Text = dataBegin;
            }
            get
            {
                return textBox1.Text;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        public string DataEnd
        {
            set
            {
                dataEnd = value;
                textBox2.Text = dataEnd;
            }
            get
            {
                return textBox2.Text;
            }
        }
        public int GroupIndex
        {
            set
            {                
                numericUpDown2.Value = value;
            }
            get
            {
                return Convert.ToInt16(numericUpDown2.Value);
            }
        }
        public int NpodGroup
        {
            set
            {
                numericUpDown1.Value = value;
            }
            get
            {
                return Convert.ToInt16(numericUpDown1.Value);
            }
        }
        public string Practice
        {
            set
            {
                PracticeLabel.Text = value;
            }
            get
            {
                return PracticeLabel.Text;
            }
        }

        private void PracticeAndGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        public string Group
        {
            set
            {
                GroupLabel.Text = value;

            }
            get
            {
                return GroupLabel.Text;
            }
        }
    }
}
