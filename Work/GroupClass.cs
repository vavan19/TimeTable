using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using TimeTable;

namespace TimeTable.Work
{    
    public class GroupClass
    {
        BD dataBase = new BD();
        string nameGroup,codeGroup;
        static DateTime startDay = new DateTime(/*DateTime.Now.Month < 9 ? DateTime.Now.Year : DateTime.Now.Year+1*/2017, 9, 1);
        static DateTime endDay = new DateTime(/*DateTime.Now.Month < 9 ? DateTime.Now.Year+1 : DateTime.Now.Year+2*/2018, 7, 1);              
        List<Practice> practices= new List<Practice>();        
                
        public GroupClass(string _name, string _code)
        {
            nameGroup = _name;
            codeGroup = _code;            
            getPractices();
        }

        
        void getPractices()
        {
            List<string> codes = BD.getListFromBD("SELECT Practices.Code FROM Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((EmploymentPupils.GroupCode) = " + codeGroup + "));");

            foreach (string code in codes)
            {
                try
                {
                    Practice pract = new Practice() { practCode = Convert.ToInt16(code), groupCode = Convert.ToInt16(codeGroup) };

                    pract.fillFields(codeGroup);

                    practices.Add(pract);
                }
                catch
                {
                    MessageBox.Show("Ошибка чтения данных. Запись группы "+nameGroup);
                }
            }
        }
        public struct Practice
        {
            public int practCode, groupCode;
            int hoursInDay;                     
            public int podGrups, lenght;
            public int[] teachers;
            public int[] timings, codes, classes;
            public double proportion;
            public int[,] tTable;
            public int workgroup;
            public int dayBegin, dayEnd;

            public void setMassValues(int NPractice)
            {
                codes = new int[NPractice];
                timings = new int[NPractice];
                classes = new int[NPractice];
                teachers = new int[NPractice];
            }
            public void Clear(string codeGroup, int massValue=0)
            {
                codes = new int[massValue];
                timings = new int[massValue];
                classes = new int[massValue];
                teachers = new int[massValue];
                tTable = new int[massValue, massValue];
                groupCode = Convert.ToInt16(codeGroup);
            }

            public void fillFields(string codeGroup)
            {

                hoursInDay = 6;

                DataSet DS = BD.getDataSetFromBD("SELECT SubWorks.Class, SubWorks.Timing, SubWorks.Name, SubWorks.Code, SubWorks.PractiseCode, EmploymentPupils.GroupCode FROM(Practices INNER JOIN([Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode) ON Practices.Code = EmploymentPupils.PracticeCode) INNER JOIN SubWorks ON Practices.Code = SubWorks.PractiseCode WHERE(((SubWorks.PractiseCode) = " + practCode + ") AND((EmploymentPupils.GroupCode) = " + codeGroup + ")); ");

                BD dataBase = new BD();
                podGrups = Convert.ToInt16(dataBase.SelectFirstQery("SELECT EmploymentPupils.NPudgroup FROM EmploymentPupils WHERE(((EmploymentPupils.PracticeCode) = " + practCode + ") and ((EmploymentPupils.GroupCode)=" + codeGroup + ")); "));


                setMassValues(DS.Tables[0].Rows.Count);


                for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    classes[i] = Convert.ToInt16(DS.Tables[0].Rows[i][0].ToString());
                    timings[i] = Convert.ToInt16(DS.Tables[0].Rows[i][1].ToString()) / hoursInDay / podGrups;
                    lenght += timings[i];
                    codes[i] = Convert.ToInt16(DS.Tables[0].Rows[i][3].ToString());
                }

                int maxTiming = 0;
                foreach (int timin in timings)
                {
                    maxTiming = (maxTiming < timin) ? maxTiming = timin : maxTiming;
                }
                if (maxTiming != 0)
                {
                    if (lenght % maxTiming != 0)
                    {
                        proportion = 1.52;
                    }
                    else proportion = 0;
                }
                else
                    throw new Exception();


                DateTime dateTime = new DateTime();
                if (DateTime.TryParse(dataBase.SelectFirstQery("SELECT EmploymentPupils.Begining FROM Practices INNER JOIN EmploymentPupils ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((EmploymentPupils.PracticeCode) = " + practCode + ") AND((EmploymentPupils.GroupCode) = " + groupCode + "));"), out dateTime))
                {
                    dayBegin = (dateTime - StartDay).Days;
                    dayBegin -= dayBegin / 7;
                }
                if (DateTime.TryParse(dataBase.SelectFirstQery("SELECT EmploymentPupils.ending FROM Practices INNER JOIN EmploymentPupils ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((EmploymentPupils.PracticeCode) = " + practCode + ") AND((EmploymentPupils.GroupCode) = " + groupCode + "));"), out dateTime))
                {
                    dayEnd = (dateTime - startDay).Days;
                    dayEnd -= dayEnd / 7;
                    dayEnd -= lenght;
                }
                string workgr = dataBase.SelectFirstQery("SELECT EmploymentPupils.workGroupCode FROM Practices INNER JOIN EmploymentPupils ON Practices.Code = EmploymentPupils.PracticeCode WHERE(((EmploymentPupils.PracticeCode) = " + practCode + ") AND((EmploymentPupils.GroupCode) = " + groupCode + "))");
                if (workgr != "")
                { 
                    workgroup = Convert.ToInt16(workgr);
                }
            }

            public Practice GetPracticeByCode(int groupCode, int practiceCode)
            {
                foreach(GroupClass group in GroupsList.TotalWorks)
                {
                    if(Convert.ToInt16(group.codeGroup)==groupCode)
                        foreach(GroupClass.Practice pract in group.Practs)
                            if (pract.practCode == practiceCode)
                                return (pract);
                }
                MessageBox.Show("Практика с кодом "+practCode+", у группы с кодом "+groupCode+" не найдена!");
                return new GroupClass.Practice();
            }
            public Practice SetPracticeDaysByCode(int groupCode, int practiceCode, int dayBegin, int dayEnd)
            {
                int i=0, j=0;
                //FindNumberOfPracticeInList(groupCode, practCode, out i, out j);

                Practice newPract = GroupsList.TotalWorks[i].practices[j];
                newPract.dayBegin = dayBegin;
                newPract.dayEnd = dayEnd;

                GroupsList.TotalWorks[i].practices[j] = newPract;                

                MessageBox.Show("Практика с кодом " + practCode + ", у группы с кодом " + groupCode + " не найдена!");
                return new GroupClass.Practice();
            }
            
            public void Uning(Practice pract)
            {
                practCode = pract.practCode + (groupCode*2);                
                Array.Resize(ref codes, codes.Length + pract.codes.Length);
                for(int i = codes.Length; i< pract.codes.Length+ codes.Length; i++)
                {
                    plusMases(codes, pract.codes);
                    plusMases(classes, pract.classes);
                    plusMases(timings, pract.timings);
                    lenght = 0;
                    foreach (int time in timings)
                        lenght += time;
                    if (tTable.GetLength(1)<1)
                    {
                        tTable = pract.tTable;
                    }
                    else
                    {
                        UnicTTable(pract.tTable);
                    }

                }
            }

            void UnicTTable( int[,] ttable)
            {
                int[,] newTTable = new int[tTable.GetLength(0) + ttable.GetLength(0), tTable.GetLength(1)];
                int rows = tTable.GetLength(0);
                for (int i = 0; i < tTable.GetLength(0); i++)
                    for (int j = 0; j < tTable.GetLength(1); j++)
                        newTTable[i, j] = tTable[i, j];


                for (int newRow = tTable.GetLength(0); newRow < tTable.GetLength(0) + ttable.GetLength(0); newRow++) 
                {
                    for (int newElement = 0; newElement < ttable.GetLength(1); newElement++)
                        newTTable[newRow, newElement] = ttable[newRow- tTable.GetLength(0), newElement];
                }
                tTable = newTTable;
            }
            void plusMases(int[] m1, int[] m2)
            {
                List<int> newMass = new List<int>();
                for (int i = 0; i < m1.Length; i++)
                    if (m1[i] != 0)
                    {
                        newMass.Add(m1.Length);
                    }
                for (int i = 0; i < m2.Length; i++)
                    if (m2[i] != 0)
                    {
                        newMass.Add(m2.Length);
                    }
                m1 = newMass.ToArray();
            }
            public void SetTTable(int[,] _tTable)
            {
                TTable = new int[_tTable.GetLength(0), _tTable.GetLength(1)];
            }
            public int[,] TTable
            {
                get { return tTable; }
                set
                {
                    tTable = value;   
                }
            }
            public string GroupName
            {
                get
                {
                    foreach(GroupClass group in GroupsList.TotalWorks)
                    {
                        if (groupCode == Convert.ToInt16(group.codeGroup))
                            return group.Name;
                    }
                    return null;
                }
            }
        }
        
        public static DateTime StartDay
        {
            get { return startDay; }
        }
        public List<Practice> Practs
        {
            get
            {
                return practices;
            }
            set
            {
                practices = value;
            }
        }        
        public string Name
        {
            get { return nameGroup; }
        }

        public string CodeGroup
        {
            get
            {
                return codeGroup;
            }

            set
            {
                codeGroup = value;
            }
        }

        public static DateTime EndDay
        {
            get
            {
                return endDay;
            }
        }
    }

}
