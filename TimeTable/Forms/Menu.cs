using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace TimeTable
{
    public partial class Menu : Form
    {
        BD dataBase = new BD();
        public Menu()
        {
            InitializeComponent();            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ShowFillForm();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowFillForm();
        }
        void ShowFillForm()
        {
            FillinBD form = new FillinBD();
            try
            {
                form.Visible=false;
            }
            catch
            {

            }
            Visible = false;
            form.ShowDialog();            
            
            var sleep = form.DialogResult;
            Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }
        void ShowMainForm()
        {
            Main form = new Main();
            try
            {
                form.Visible = false;
            }
            catch
            {

            }
            Visible = false;
            form.ShowDialog();
            

            var sleep = form.DialogResult;
            Visible = true;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
                        
        }
    }
}
