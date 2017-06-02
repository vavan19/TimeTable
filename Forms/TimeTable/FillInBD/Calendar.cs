using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace TimeTable.Forms
{
    public partial class Calendar : Form
    {
        private static string data;
        public static string Data { get { return data; } set { data = value; } }
        
        public Calendar()
        {
            InitializeComponent();
        }

        private void Calendar_Load(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            Data = e.Start.Date.ToShortDateString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
