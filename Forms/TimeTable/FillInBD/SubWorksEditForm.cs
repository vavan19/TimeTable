using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTable.Forms.FillInBD
{
    public partial class SubWorksEditForm : Form
    {
        public SubWorksEditForm()
        {
            InitializeComponent();
        }

        private void GroupLabel_Click(object sender, EventArgs e)
        {

        }

        private void SubWorksEditForm_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = BD.getListFromBD("SELECT Name From Class");
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            Close();            
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        public string SubWorksName
        {
            set
            {
                textBox1.Text = value;
                SubWorkLabel.Text = value;
            }
            get
            {
                return textBox1.Text;
            }
        }
        public string PracticeName
        {
            set
            {
                PracticeLabel.Text = value;                
            }
            get
            {
                return textBox1.Text;
            }
        }
        public string Timing
        {
            set
            {
                numericUpDown1.Text = value;
            }
            get
            {
                return numericUpDown1.Text;
            }
        }
        public string ClassName
        {
            set
            {
                comboBox1.Text = value;
            }
            get
            {
                return comboBox1.SelectedItem.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SubWorkLabel.Text = textBox1.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
