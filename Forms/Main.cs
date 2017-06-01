using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using btl.generic;
using TimeTable.Work;
using System.Collections;
using TimeTables.GATTable;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

using System.Threading;

namespace TimeTable
{
    public partial class Main : Form
    {
        BD dataBase = new BD();
        
        public delegate void SelectDelegat(string s);
        //static SelectDelegat PrintMethod;
        //static int i=0;
        Genome test;
        public Main()
        {
            InitializeComponent();
        }

        string SelectFirstQery(string qery)
        {
            string selectetItem = dataBase.SelectFirstQery(qery);
            return selectetItem;
        }
        private void Main_Load(object sender, EventArgs e)
        {
            GroupsList.ClearTotalWorks();

            tabControl1.SelectedIndex = 1;
            backgroundWorker1.RunWorkerAsync();
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        void OrganizeAndPrint()
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {

        }
        

        public void PrintGroupTTables(GroupClass test)
        {
            /*richTextBox1.Text += "Группа - " + test.Name + Environment.NewLine;
            foreach (GroupClass.Practice practice in test.Practs)
            {
                richTextBox1.Text += dataBase.SelectFirstQery("SELECT Practices.Name FROM Practices WHERE(((Practices.Code) = " + practice.practCode + "));") + " практика - идеальный вид" + Environment.NewLine;
                int[,] tTable = practice.tTable;
                string[] text = new string[tTable.GetLength(0)];                

                for (int i = 0; i < tTable.GetLength(1); i++)
                {

                    for (int j = 0; j < tTable.GetLength(0); j++)
                    {
                        text[j] += tTable[j, i];
                    }

                }
                for (int i = 0; i < tTable.GetLength(0); i++)
                    PrintMethod(text[i]);               
            }
            i++;*/
        }
        
        public static double theActualFunction(int[,] pract, GroupClass.Practice practData)
        {
            /*if (values.GetLength(2) == 2)
                throw new ArgumentOutOfRangeException("should only have 2 args");*/

            double fitnes = 0;
            int count = pract.GetLength(0);
            Stack[] numbersOrder = new Stack[count];
            for (int i = 0; i < count; i++)
                numbersOrder[i] = new Stack();

            for (int i = 0; i < pract.GetLength(1); i++)
            {
                ArrayList number = new ArrayList();

                for (int j = 0; j < count; j++)
                {
                    if (!number.Contains(pract[j, i]))
                    {
                        number.Add(pract[j, i]);
                    }
                    if (!numbersOrder[j].Contains(pract[j, i]))
                        numbersOrder[j].Push(pract[j, i]);
                    else
                        if (numbersOrder[j].Contains(pract[j, i]) && (int)numbersOrder[j].Peek() != pract[j, i])
                    {
                        fitnes += 1;
                    }
                }
                if (number.Count >= count - 1)
                    fitnes += (count - number.Count) * 0.5;
                else
                    fitnes += 20;
            }
            return fitnes;

        }
        void printInTextbox(string text)
        {
           // richTextBox1.Text += text + Environment.NewLine;           
        }
        internal  BD DataBase
        {
            get { return dataBase; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*PrintOrganizedGenome(24);
            GATTableGoMethod gm = new GATTableGoMethod();
            gm.theActualFunction(GroupsList.OrginizedPractices);
            int weeks = 30;
            DateTime dayNow = new DateTime(DateTime.Now.Year, 8, 31);
            while (dayNow.Month != 6 || dayNow.Day != 5) // new DateTime(DateTime.Now.Year, 10, 5))
            {
                richTextBox2.Text += Environment.NewLine + "\t";
                richTextBox1.Text += "\t";
                for (int i = 0; i < weeks; i++)
                {
                    dayNow = dayNow.AddDays(1);
                    if (dayNow.DayOfWeek == DayOfWeek.Sunday)
                        dayNow = dayNow.AddDays(1);
                    richTextBox2.Text += "[" + dayNow.Day + "," + dayNow.Month + "]";   
                    //t++;
                }
            }
         }*/

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FillinBD fillForm = new FillinBD();
            fillForm.Show();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            OrganizePratices();
        }
        void OrganizePratices()
        {
            GroupsList works = new GroupsList();
            label1.Text = "Получение данных из базы";
            works.GetWorks();

            backgroundWorker1.ReportProgress(30);
            //label1.Text = "Организация практик";
            GAGoMethod ga = new GAGoMethod();
            int i = 0, j = 0;
            for (i = 0; i < GroupsList.TotalWorks.Count; i++)
            {
                for (j = 0; j < GroupsList.TotalWorks[i].Practs.Count; j++)
                {

                    GAGoMethod.GoGa(0.8, 0.75, 60, 20, out test, GroupsList.TotalWorks[i].Practs[j]);
                    var str = GroupsList.TotalWorks[i].Practs[j];
                    str.TTable = test.TTable;
                    GroupsList.TotalWorks[i].Practs[j] = str;
                }
            }
            //progressBar1.Value = 80;
            backgroundWorker1.ReportProgress(80);

            //label1.Text = "Совмещение";
            GroupsList.Organized();

            backgroundWorker1.ReportProgress(100);
            //progressBar1.Value = 100;

            Thread.Sleep(1000);                        
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            //label1.Text = "Составление рассписания. Для остановки нажмите кнопку";
            //progressBar1.Value = 0;            
            GATTable.Genome organizedGenome = new GATTable.Genome();
            organizedGenome.Fitness = 5;
            GATTableGoMethod ga = new GATTableGoMethod();
            ga.GoGa(0.8, 0.65, 60, 10, out organizedGenome);
            //if(!GATTable.Genome.stop)
            GroupsList.OrginizedPractices = organizedGenome;
            
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GroupsList.SetDays(GroupsList.OrginizedPractices);
            PrintInWord();
            Close();
        }

        private void PrintInWord()
        {
            BD dataBase = new BD();
            GroupsList.TotalWorks.Sort(new GroupsList());

            DateTime date = new DateTime(2016,9,1);
            int daysInLine = 65;

            var excelApp = new Excel.Application();
            // Make the object visible.           
            
            excelApp.Workbooks.Add();

            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            //Excel.Style style = excelApp.ThisWorkbook.Styles.Add("BoldTable");

            int startColumn = 2, currentRow = 5;

            

            excelApp.ActiveWindow.Zoom = 60;
            //workSheet.
            int index = 0;
            while (date < new DateTime(2017, 7, 1))
            {
                ((Excel.Range)workSheet.Range[workSheet.Cells[currentRow, startColumn + 1], workSheet.Cells[currentRow + 1, startColumn + daysInLine]]).EntireColumn.ColumnWidth = 2.5;
                ((Excel.Range)workSheet.Range[workSheet.Cells[currentRow + 1, startColumn], workSheet.Cells[currentRow + 1, startColumn + daysInLine + 1]]).Orientation = "90";
                ((Excel.Range)workSheet.Range[workSheet.Cells[currentRow + 1, startColumn], workSheet.Cells[currentRow + 1, startColumn + daysInLine + 1]]).RowHeight = 54;

                ((Excel.Range)workSheet.Range[workSheet.Cells[currentRow, startColumn + 1], workSheet.Cells[currentRow + 1, startColumn + 1]]).Orientation = "90";
                ((Excel.Range)workSheet.Range[workSheet.Cells[currentRow, startColumn + 1], workSheet.Cells[currentRow + 1, startColumn + 1]]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                ((Excel.Range)workSheet.Range[workSheet.Cells[currentRow, startColumn + 1], workSheet.Cells[currentRow + 1, startColumn + 1]]).VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;
                ((Excel.Range)workSheet.Range[workSheet.Cells[currentRow, startColumn], workSheet.Cells[currentRow, startColumn + daysInLine + 1]]).RowHeight = 15;

                workSheet.Cells[currentRow, startColumn] = "Группа";
                workSheet.Cells[currentRow, startColumn + 1] = "Подгруппы";

                workSheet.Range[workSheet.Cells[currentRow, startColumn], workSheet.Cells[currentRow + 1, startColumn]].Merge();
                workSheet.Range[workSheet.Cells[currentRow, startColumn + 1], workSheet.Cells[currentRow + 1, startColumn + 1]].Merge();

                //workSheet.Cells.Merge(exCells);

                for (int collumn = 1; collumn <= daysInLine; collumn++)
                {
                    if (date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        workSheet.Cells[currentRow, (collumn + startColumn + 1)] = date.DayOfWeek.ToString().Remove(2);
                        workSheet.Cells[currentRow + 1, (collumn + startColumn + 1)] = date.Day + " " + date.Month.ToString();
                        index++;
                    }
                    else
                    {
                        collumn -= 1;
                    }
                    date = date.AddDays(1);
                }
                currentRow+=2;

                foreach (var group in GroupsList.TotalWorks)
                {
                    foreach (var practice in group.Practs)
                    {
                        bool mayBeConflict = false;
                        if (index-65 <= practice.dayBegin
                            && index >= practice.dayBegin)
                            mayBeConflict = true;
                        else
                            if (index-65 <= practice.dayEnd
                                && index >= practice.dayEnd)
                            mayBeConflict = true;
                        else
                            if (index - 65 <= practice.dayBegin
                                && index >= practice.dayEnd)
                            mayBeConflict = true;

                        if (mayBeConflict)
                        {
                            //workSheet.Range[workSheet.Cells[currentRow, startColumn], workSheet.Cells[currentRow + practice.TTable.GetLength(0), startColumn]].Merge();
                            AddGroupLineToExcel(workSheet, practice, index, daysInLine, currentRow, startColumn);
                                
                            currentRow += practice.TTable.GetLength(0);
                        }                        
                    }                    
                }
                currentRow += 4;

            }
            Excel.Worksheet ObjWorkSheet;
            ObjWorkSheet = (Excel.Worksheet)excelApp.Sheets[1];
            object miss = Type.Missing;

            List<Color> colours = new List<Color>();
            colours.Add(Color.Yellow);
            colours.Add(Color.Green);
            colours.Add(Color.Blue);
            colours.Add(Color.Brown);
            colours.Add(Color.DarkOliveGreen);
            colours.Add(Color.Gray);


            foreach (KeyValuePair<int, string> className in GroupsList.ClassNames)
            {

                Excel.Range range = workSheet.Cells.Find(className.Value, miss, miss, Excel.XlLookAt.xlWhole, miss, Excel.XlSearchDirection.xlNext, miss, miss, miss);
                range.Interior.Color = colours[1];
                /*var firstCell = range.Cells[1, 1];

                do
                {
                    range.Interior.Color = colours[className.Key];
                    range = workSheet.Cells.Find(className.Value, range.Cells[1, 1], miss, Excel.XlLookAt.xlWhole);
                } while (firstCell != range.Cells[1, 1]);*/
            }
            
            //workSheet.UsedRange;
            //workSheet.UsedRange.Find("T", miss, Excel.XlFindLookIn.xlValues, miss, miss, Excel.XlSearchDirection.xlNext, true);

            //range.Cells.Interior.Color = Color.Red;

            //range.FindNext().Interior.Color = Color.Red;
            //workSheet.gin

            excelApp.Visible = true;
        }

        private void AddGroupLineToExcel(Excel._Worksheet worksheet, GroupClass.Practice conflictedPractic,
            int numberOfDays, int howManyDaysInLine , int currentRow, int currentColumn)
        {
            worksheet.Cells[currentRow, currentColumn]=conflictedPractic.GroupName;
            for (int row = 0; row < conflictedPractic.TTable.GetLength(0); row++)
            {
                worksheet.Cells[currentRow + row, currentColumn + 1] = row + 1;

                int selectedCollumn = currentColumn + 2;

                int practiceColumn = 0;
                int startDay = conflictedPractic.dayBegin - (numberOfDays-howManyDaysInLine);

                if (startDay < 0)
                {
                    practiceColumn = (numberOfDays - howManyDaysInLine) - conflictedPractic.dayBegin; //inc the past line days
                    startDay = 0;   //set pointer at the begining
                }

                int endDay = howManyDaysInLine;
                if(conflictedPractic.dayEnd < numberOfDays)
                {
                    endDay -= numberOfDays - conflictedPractic.dayEnd;  // 
                }
                try
                {
                    int debuging = 0;
                    for (int collumnNumber = startDay; collumnNumber < endDay; collumnNumber++)
                    {    ////////////////[Error OutOfRangeException]
                        string className = GroupsList.ClassNames[conflictedPractic.TTable[row, practiceColumn]];

                        worksheet.Cells[currentRow + row, selectedCollumn + collumnNumber] = className;

                        practiceColumn++;
                    }
                }
                catch
                {

                }
                int debug = 0;
                
            }

            //stiling rows
            //string alphabet = "abcdefghijklmnopqrstuvwxyz";
            //Excel.Style style = .Styles.Add("NewStyle");
            for (int column = 0; column < howManyDaysInLine; column+=6)
            {
                /*string columnLetter= "";
                if (column > alphabet.Length)
                {
                    columnLetter += alphabet[currentColumn + (column / alphabet.Length)];
                    columnLetter += alphabet[currentColumn + (column % alphabet.Length)];
                }
                string startRange = columnLetter + currentRow;
                string endRange = columnLetter + currentRow;*/
                var startCell =worksheet.Cells[currentRow+2, currentColumn];
                var endCell = worksheet.Cells[currentRow+2+conflictedPractic.TTable.GetLength(0), currentColumn + column];
                Excel.Range range = worksheet.Range[startCell, endCell];
                range.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
            }
            
            
            //string column = 
            //
        }
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            GATTable.Genome.stop = true;
            //backgroundWorker2.CancelAsync();
            /*GroupsList.OrginizedPractices = new GATTable.Genome();
            GroupsList.OrginizedPractices.GetSetTTPracticeList = GATTable.Genome.GetSetMaxGenom;
            GroupsList.SetDays(GroupsList.OrginizedPractices);
            PrintInWord();
            Close();*/
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex==1)
            {
                backgroundWorker1.RunWorkerAsync();

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Visible = true;
            label1.Text= "Составление рассписания. Для остановки нажмите кнопку";
            backgroundWorker2.RunWorkerAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker2.CancelAsync();
        }
    }
}
