using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Windows.Forms;

using Word = Microsoft.Office.Interop.Word;

namespace TimeTable
{
    public partial class FillinBD : Form
    {
        BindingSource practices = new BindingSource();
        BindingSource groups = new BindingSource();
        string groupsQuery = "SELECT Group.Name FROM[Group]  ORDER BY Group.Name;";

        public FillinBD()
        {
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FillinBD_Load(object sender, EventArgs e)
        {
            groups.DataSource = BD.getListFromBD("SELECT Group.Name FROM[Group]  ORDER BY Group.Name; ");
            comboBox1.DataSource = groups;


            listBox1.Items.AddRange(BD.getListFromBD("SELECT Practices.Name FROM Practices").ToArray());
            listBoxPractice.DataSource = BD.getListFromBD("select name from practices");
            ReloadTabs();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();                                 

            List <string> avelablePractices = BD.getListFromBD("SELECT  Practices.Name FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \""+comboBox1.Text+"\")); ");
            if(avelablePractices!=null)
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (avelablePractices.Contains(listBox1.Items[i].ToString()))
                    {
                        listBox2.Items.Add(listBox1.Items[i]);
                    }
                }           
            
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            BD DataBase = new BD();
            Regex r = new Regex(@"\d");
            string code;
            if (r.Match(textBox1.Text).Index != 0)
            {
                code = (textBox1.Text.Remove(r.Match(textBox1.Text).Index));
            }
            else
                code = textBox1.Text;




            int mask = 0;
            byte [] biteMask = Encoding.ASCII.GetBytes(code); 
            for (int i = 0; i < code.Length; i++)
            {
                mask += biteMask[i];
            }
            code = mask.ToString();
            code += (r.Match(textBox1.Text).Index != 0) ? (textBox1.Text.Remove(0, r.Match(textBox1.Text).Index)) : "" ;
            DataBase.Add("INSERT INTO [Group] ( Name, Code ) VALUES(\"" + textBox1.Text + "\", " + code + ");");
            ReloadTabs();
            ReloadTabs();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BD DataBase = new BD();
            DataBase.Delete("DELETE EmploymentPupils.* FROM[Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode WHERE(((Group.Name) = \"" + comboBox1.Text + "\")); ");
            DataBase.Delete("DELETE Group.Name FROM[Group] WHERE(((Group.Name) = \"" + comboBox1.Text + "\")); ");
            ReloadTabs();
            ReloadTabs();
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            
            bool notRepeating=true;
            foreach (var item in listBox2.Items)
            {
                if (listBox1.SelectedItem == item)
                    notRepeating = false;
            }
            if (notRepeating)
            {
                int groupIndex=0, npodGroup=1;
                string dateBegin="", dateEnd="";
                switch (OpenPracticeAndGroup(comboBox1.Text, listBox1.SelectedItem.ToString(), out groupIndex, out npodGroup, out dateBegin, out dateEnd))
                {
                    case DialogResult.OK:
                        {
                            try
                            {

                                BD DataBase = new BD();

                                string groupCode = DataBase.SelectFirstQery("SELECT Group.Code FROM[Group]  where ((Group.Name) = \"" + comboBox1.Text + "\")");
                                string practiceCode = DataBase.SelectFirstQery("SELECT Practices.Code FROM Practices WHERE (((Practices.Name) = \"" + listBox1.SelectedItem.ToString() + "\"))");
                                int recordCode = Convert.ToInt16(DataBase.SelectFirstQery("SELECT Max(EmploymentPupils.Code) FROM EmploymentPupils;")) + 1;


                                DataBase.Add("INSERT INTO [EmploymentPupils] ( GroupCode, PracticeCode, Code, NPudgroup, workGroupCode, Begining, ending ) VALUES(" + groupCode + ", " + practiceCode + ", " + practiceCode + ", "+npodGroup+", "+groupIndex+", " + (dateBegin==""? "NULL": "\""+dateBegin+ "\"") + ", " + (dateEnd==""? "NULL": "\""+dateEnd+ "\"") + ");");
                                listBox2.Items.Add(listBox1.SelectedItem);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            break;
                        }
                    case DialogResult.Cancel: break;
                }

            }
        }

        private DialogResult OpenPracticeAndGroup(string groupName, string practiceName, out int groupIndex, out int npodGroup, out string dateBegin, out string dateEnd)
        {
            PracticeAndGroup form = new PracticeAndGroup();

            BD DataBase = new BD();

            form.Group = groupName;
            form.Practice = practiceName;

            string boofer = DataBase.SelectFirstQery("SELECT EmploymentPupils.Begining FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + groupName + "\") AND((Practices.Name) = \"" + practiceName + "\"));");

            try
            {
                form.DataBegin = boofer.Remove(boofer.IndexOf(" "));
            }
            catch
            {
                form.DataBegin = boofer;
            }
            boofer = DataBase.SelectFirstQery("SELECT EmploymentPupils.ending FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + groupName + "\") AND((Practices.Name) = \"" + practiceName + "\"));");
            try
            {
                form.DataEnd = boofer.Remove(boofer.IndexOf(" "));
            }
            catch
            {
                form.DataEnd = boofer;
            }

            try
            {
                form.NpodGroup = Convert.ToInt16(DataBase.SelectFirstQery("SELECT EmploymentPupils.NPudgroup FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + groupName + "\") AND((Practices.Name) = \"" + practiceName + "\"));"));
                form.GroupIndex = Convert.ToInt16(DataBase.SelectFirstQery("SELECT EmploymentPupils.workGroupCode FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + groupName + "\") AND((Practices.Name) = \"" + practiceName + "\"));"));
            }
            catch
            {

            }
            form.ShowDialog();
            groupIndex = form.GroupIndex;
            dateBegin = form.DataBegin;
            dateEnd = form.DataEnd;
            npodGroup = form.NpodGroup;
            return form.DialogResult;
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            BD DataBase = new BD();
            int groupIndex = 0, npodGroup = 1;
            string dateBegin = "", dateEnd = "";
            try
            {
                switch (OpenPracticeAndGroup(comboBox1.Text, listBox2.SelectedItem.ToString(), out groupIndex, out npodGroup, out dateBegin, out dateEnd))
                {
                    case DialogResult.OK:
                        {

                            string groupCode = DataBase.SelectFirstQery("SELECT Group.Code FROM[Group]  where ((Group.Name) = \"" + comboBox1.Text + "\")");
                            string practiceCode = DataBase.SelectFirstQery("SELECT Practices.Code FROM Practices WHERE (((Practices.Name) = \"" + listBox2.SelectedItem.ToString() + "\"))");
                            string recordCode = DataBase.SelectFirstQery("SELECT EmploymentPupils.Code FROM Practices INNER JOIN ([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + comboBox1.Text + "\") AND((Practices.Name) = \"" + listBox2.SelectedItem.ToString() + "\"))");

                            DataBase.Add("UPDATE [EmploymentPupils] SET [EmploymentPupils].NPudgroup = " + npodGroup + ", [EmploymentPupils].workGroupCode = " + groupIndex + ", [EmploymentPupils].Begining = " + (dateBegin == "" ? "NULL" : "\""+ dateBegin +"\"") + ", [EmploymentPupils].ending = " + (dateEnd == "" ? "NULL" : "\""+dateEnd+ "\"") + " WHERE (((EmploymentPupils.Code)=" + recordCode + "));");

                            break;
                        }
                    case DialogResult.No:
                        {
                            string code = DataBase.SelectFirstQery("SELECT EmploymentPupils.Code FROM Practices INNER JOIN ([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + comboBox1.Text + "\") AND((Practices.Name) = \"" + listBox2.SelectedItem.ToString() + "\"))");
                            DataBase.Delete("DELETE Group.Name FROM[EmploymentPupils] WHERE(((Code) = " + code + ")); ");
                            listBox2.Items.Remove(listBox2.SelectedItem);
                            break;
                        }
                }
                //string code =DataBase.SelectFirstQery("SELECT EmploymentPupils.Code FROM Practices INNER JOIN ([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + comboBox1.Text + "\") AND((Practices.Name) = \"" + listBox2.SelectedItem.ToString() + "\"))");
                //DataBase.Delete("DELETE Group.Name FROM[EmploymentPupils] WHERE(((Code) = " + code + ")); ");
            }
            catch
            {

            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SizeChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            List<string> avelablePractices = BD.getListFromBD("SELECT  Practices.Name FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + comboBox1.Text + "\")); "); 
            if (avelablePractices != null)
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (avelablePractices.Contains(listBox1.Items[i].ToString()))
                    {
                        listBox2.Items.Add(listBox1.Items[i]);
                    }
                }
        }
        

        private void сделатьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BD dataBase = new BD();
            

            
            object oMissing = System.Reflection.Missing.Value;

            Word._Application oWord;
            Word._Document oDoc;
            oWord = new Word.Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);            

            Word.Range range = oDoc.Range(0,0);

            //Указываем таблицу в которую будем помещать данные (таблица должна существовать в шаблоне документа!)
            Microsoft.Office.Interop.Word.Table tbl = oWord.ActiveDocument.Tables.Add(range,comboBox1.Items.Count,2);

            //Заполняем в таблицу - 10 записей.
            int i=0;
            foreach (var item in comboBox1.Items)
            {
                try
                {

                    string count = dataBase.SelectFirstQery("SELECT Practices.Name AS [Count-Name] FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode where (((Group.Name) = \"" + item.ToString() + "\"));");
                    if (count != "")
                    {
                        i++;
                        tbl.Rows.Add();//Добавляем в таблицу строку.
                                       //Обычно саздаю только строку с заголовками и одну пустую для данных.
                        tbl.Rows[i].Cells[1].Range.Text = "Группа " + item.ToString();
                        foreach (var practice in BD.getListFromBD("SELECT Practices.Name FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + item.ToString() + "\"));"))
                        {
                            tbl.Rows[i].Cells[2].Range.Text += practice.ToString() + Environment.NewLine;
                        }
                    }
                }
                catch
                {

                }
            }
                     
            tbl.ApplyStyleRowBands = true;
            //Открываем документ для просмотра.
            oWord.Visible = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }

        private void listBoxPractice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxNamePractice.Text = listBoxPractice.SelectedItem.ToString();

                listBoxSubWorks.DataSource = BD.getListFromBD("SELECT SubWorks.Name as \"Имя работы\" FROM Class INNER JOIN(Practices INNER JOIN SubWorks ON Practices.Code = SubWorks.PractiseCode) ON Class.Код = SubWorks.Class WHERE(((Practices.Name) = \"" + listBoxPractice.SelectedItem.ToString() + "\")); ");

                /*BindingSource bS = new BindingSource();
                bS.DataSource = BD.getDataSetFromBD("SELECT SubWorks.Name as \"Имя работы\", SubWorks.Timing, Class.Name FROM Class INNER JOIN(Practices INNER JOIN SubWorks ON Practices.Code = SubWorks.PractiseCode) ON Class.Код = SubWorks.Class WHERE(((Practices.Name) = \"" + listBoxPractice.SelectedItem.ToString() + "\")); ").Tables[0];
                dataGridView1.DataSource = bS;*/
            }
            catch
            {

            }
            
        }

        private void textBoxNamePractice_TextChanged(object sender, EventArgs e)
        {
            
        }
        
        private void buttonPracticeRename_Click(object sender, EventArgs e)
        {
            BD DataBase = new BD();
            DataBase.Add("update [Practices] SET name=\"" + textBoxNamePractice.Text+ "\" where name = \"" + listBoxPractice.SelectedItem.ToString() + "\"");
            ReloadTabs();
        }

        private void buttonPracticeAdd_Click(object sender, EventArgs e)
        {
            BD DataBase = new BD();
            int code = Convert.ToInt16(DataBase.SelectFirstQery("SELECT Max(Practices.Code) FROM Practices;")) + 1;
            DataBase.Add("insert into [Practices] (name,code) Values (\"" + textBoxPracticeAdd.Text + "\", " + code + " )");
            ReloadTabs();
            textBoxPracticeAdd.Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            BD DataBase = new BD();
            try
            {
                DataBase.Delete("DELETE Practices.Name FROM[Practices] WHERE(((Practices.Name) = \"" + listBoxPractice.SelectedItem.ToString() + "\")); ");
            }
            catch 
            {
                MessageBox.Show("Выберите практику из списка");
            }
            ReloadTabs();
        }

        void ReloadTabs()
        {
            ReloadGropBox();
            ReloadGroupList();
            ReloadPracticeList();
            ReloadSubwWorksList();
            ReloadClassListBox();
        }
        void ReloadPracticeList()
        {
            listBoxPractice.DataSource = BD.getListFromBD("select name from practices");
        }
        void ReloadGroupList()
        {
            groups.DataSource = BD.getListFromBD(groupsQuery);
        }
        private void ReloadGropBox()
        {
            listBox2.Items.Clear();

            List<string> avelablePractices = BD.getListFromBD("SELECT  Practices.Name FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((Group.Name) = \"" + comboBox1.Text + "\")); ");
            if (avelablePractices != null)
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (avelablePractices.Contains(listBox1.Items[i].ToString()))
                    {
                        listBox2.Items.Add(listBox1.Items[i]);
                    }
                }
            
        }

        void ReloadSubwWorksList()
        {
            try
            {
                listBoxSubWorks.DataSource = BD.getListFromBD("SELECT SubWorks.Name as \"Имя работы\" FROM Class INNER JOIN(Practices INNER JOIN SubWorks ON Practices.Code = SubWorks.PractiseCode) ON Class.Код = SubWorks.Class WHERE(((Practices.Name) = \"" + listBoxPractice.SelectedItem.ToString() + "\")); ");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void ReloadClassListBox()
        {
            listBoxClasses.DataSource = BD.getListFromBD("Select name from class");
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridCell cell = (DataGridCell)sender;
            //if (cell.RowNumber)
        }



        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Add_Click(object sender, EventArgs e)
        {
            string timing="", className="", subWorkName="", name="";
            if (OpenEditSubWorks(listBoxPractice.SelectedItem.ToString(), name,out name,out timing, out className, out subWorkName) == DialogResult.OK)
                try
                {
                    BD DataBase = new BD();
                    int recordCode = Convert.ToInt16(DataBase.SelectFirstQery("SELECT Max(SubWorks.Code) FROM SubWorks;")) + 1;
                    string practiceCode = DataBase.SelectFirstQery("SELECT Practices.Code FROM Practices WHERE(((Practices.Name) = \"" + listBoxPractice.SelectedItem.ToString() +"\")); ");
                    DataBase.Add("INSERT INTO SubWorks (class, timing, name, code, practiseCode) VALUES (" + className + ", " + timing + ", \"" + name + "\", " + recordCode + ", " + practiceCode + " )");
                }
                catch
                {
                    MessageBox.Show("Ошибка при добавлении работы");
                }
            ReloadTabs();
        }

        private void listBoxSubWorks_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SubWoksMenu.Show(MousePosition);
            }
        }
        private DialogResult OpenEditSubWorks(string practiceName, string subWokNameNow, out string subWorkName, out string timing, out string className, out string subWokNameCorrected)
        {
            Forms.FillInBD.SubWorksEditForm form = new Forms.FillInBD.SubWorksEditForm();

            BD DataBase = new BD();


            subWorkName = subWokNameNow;
            form.SubWorksName = subWokNameNow;
            if (subWorkName == "")
                form.SubWorksName = "Введите имя";

            form.PracticeName = practiceName;           
            
            try
            {
                form.Timing = DataBase.SelectFirstQery("SELECT SubWorks.Timing FROM Practices INNER JOIN SubWorks ON Practices.Code = SubWorks.PractiseCode WHERE(((Practices.Name) = \"" + practiceName +"\") and (subworks.name = \"" + subWorkName +"\"));");
                form.ClassName = DataBase.SelectFirstQery("SELECT Class.Name FROM Class INNER JOIN (Practices INNER JOIN SubWorks ON Practices.Code = SubWorks.PractiseCode) ON Class.Код = SubWorks.Class WHERE(((Practices.Name) = \"" + practiceName + "\") and (subworks.name = \"" + subWorkName + "\"));");
            }
            catch
            {

            }
            form.ShowDialog();

            timing = form.Timing;
            subWokNameCorrected = form.SubWorksName;
            className = DataBase.SelectFirstQery("SELECT Код From Class where name = \"" + form.ClassName + "\"");
            subWorkName = form.SubWorksName;
            return form.DialogResult;
        }

        private void Редактировать_Click(object sender, EventArgs e)
        {

            try
            {
                string timing = "", className = "", subWorkName = "", name = listBoxSubWorks.SelectedItem.ToString();
                if (OpenEditSubWorks(listBoxPractice.SelectedItem.ToString(), name, out name, out timing, out className, out subWorkName) == DialogResult.OK)

                    try
                    {
                        BD DataBase = new BD();
                        int recordCode = Convert.ToInt16(DataBase.SelectFirstQery("SELECT SubWorks.Code FROM SubWorks where Name = \""+listBoxSubWorks.SelectedItem.ToString()+"\";"));
                        string practiceCode = DataBase.SelectFirstQery("SELECT Practices.Code FROM Practices WHERE(((Practices.Name) = \"" + listBoxPractice.SelectedItem.ToString() + "\")); ");
                        DataBase.Add("UPDATE [SubWorks] SET class = " + className + ", timing = " + timing + ", name = \"" + name + "\", code = " + recordCode + ", PractiseCode=" + practiceCode + " WHERE (((Code)=" + recordCode + "));");
                        
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при добавлении работы");
                    }
                ReloadTabs();
            }
            catch 
            {
                MessageBox.Show("Выберите нужную работу");
            }
        }

        private void Удалить_Click(object sender, EventArgs e)
        {
            BD DataBase = new BD();
            DataBase.Delete("DELETE Subworks.* FROM[Subworks] WHERE(((Subworks.Name) = \"" + listBoxSubWorks.SelectedItem.ToString() + "\")); ");
            ReloadTabs();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BD DataBase = new BD();

            int recordCode = Convert.ToInt16(DataBase.SelectFirstQery("SELECT Max(Class.Код) FROM class;")) + 1;
            DataBase.Add("INSERT INTO [Class] ( Name, Код ) VALUES(\"" + textBoxClassToAdd.Text + "\", " + recordCode + ");");
            ReloadTabs();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BD DataBase = new BD();

            int recordCode = Convert.ToInt16(DataBase.SelectFirstQery("SELECT Max(Class.Код) FROM class;")) + 1;
            DataBase.Add("Delete Class.* from class where name=\"" + listBoxClasses.SelectedItem.ToString() + "\";");
            ReloadTabs();
        }
    }    
}
