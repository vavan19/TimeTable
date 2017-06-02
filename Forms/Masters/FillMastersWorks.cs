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
    public partial class FillMastersWorks : Form
    {
        public static int a;
        private Masters.MasterModel mastersModel;
        public FillMastersWorks()
        {
            InitializeComponent();
        }

        private void FillMastersWorks_Load(object sender, EventArgs e)
        {
            listBoxMasters.Items.AddRange(mastersModel.Masters.ToArray());
            listBoxClasses.Items.AddRange(mastersModel.Classes.ToArray());
        }

        public Masters.MasterModel MastersModel
        {
            get
            {
                return mastersModel;
            }
            set
            {
                mastersModel = value;
            }
        }

        private void listBoxMasters_Click(object sender, EventArgs e)
        {
            listBoxMasterClasses.Items.Clear();
            if(listBoxMasters.SelectedItem!=null)
            {
                var range = mastersModel.GetRelatedClassesByMasteName(listBoxMasters.SelectedItem.ToString());
                if (range!=null)
                    listBoxMasterClasses.Items.AddRange(range.ToArray());           
            }
        }

        private void listBoxClasses_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxMasters.SelectedItem != null && listBoxMasters.SelectedItem != null)
            {
                foreach (var item in listBoxMasterClasses.Items)
                {
                    if (item==listBoxClasses.SelectedItem)
                    {
                        return;
                    }
                }
                mastersModel.AddClassToMaster(listBoxMasters.SelectedItem.ToString(), listBoxClasses.SelectedItem.ToString());
                listBoxMasterClasses.Items.Add(listBoxClasses.SelectedItem);
            }
        }

        private void listBoxMasterClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMasterClasses.SelectedItem != null)
            {
                mastersModel.RemoveClassFromMaster(listBoxMasters.SelectedItem.ToString(), listBoxMasterClasses.SelectedItem.ToString());
                listBoxMasterClasses.Items.Remove(listBoxMasterClasses.SelectedItem);
            }
        }

    }
}
