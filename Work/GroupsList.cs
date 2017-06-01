using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.GATTable;
using System.Collections;

namespace TimeTable.Work
{
    public class GroupsList: IComparer<GroupClass>
    {
        static List<GroupClass> totalWorks= new List<GroupClass>();

        static GATTable.Genome orginizedPractices;

        static Dictionary<int,string> classNames = new Dictionary<int,string>();

        public GroupsList()
        {

        }

        public void GetWorks()
        {
            DataSet allGroupsData;
            allGroupsData = BD.getDataSetFromBD("SELECT Group.Name, EmploymentPupils.GroupCode FROM[Group] INNER JOIN EmploymentPupils ON Group.Code = EmploymentPupils.GroupCode GROUP BY Group.Name, EmploymentPupils.GroupCode;");
            for (int i = 0; i < allGroupsData.Tables[0].Rows.Count; i++)
            {
                totalWorks.Add(new GroupClass(allGroupsData.Tables[0].Rows[i][0].ToString(), allGroupsData.Tables[0].Rows[i][1].ToString()));
            }

            foreach (DataRow item in BD.getDataTableFromBD("SELECT Class.Код, Class.Name FROM Class").Rows)
            {
                classNames[Convert.ToInt16(item.ItemArray[0])] = item.ItemArray[1].ToString().Remove(1).ToUpper();
            }            
        }

        public static void Organized()
        {
            BD dataBase = new BD();
            List<string> groupCodesList = BD.getListFromBD("SELECT EmploymentPupils.GroupCode, EmploymentPupils.workGroupCode FROM EmploymentPupils GROUP BY EmploymentPupils.workGroupCode, EmploymentPupils.GroupCode, EmploymentPupils.workGroupCode HAVING(((EmploymentPupils.workGroupCode) <> 0)); ") ;
            List<string> groupWorkList = BD.getListFromBD("SELECT EmploymentPupils.workGroupCode FROM EmploymentPupils GROUP BY EmploymentPupils.workGroupCode, EmploymentPupils.GroupCode, EmploymentPupils.workGroupCode HAVING(((EmploymentPupils.workGroupCode) <> 0)); ");
            for (int countIndex = 0; countIndex <groupWorkList.Count; countIndex++)
            {
                int i = 0;
                while(totalWorks[i].CodeGroup!=groupCodesList[countIndex])
                {
                    i++;
                }

                bool areThereWorkGroupsMarks = false;
                foreach (GroupClass.Practice practElement in totalWorks[i].Practs)
                    if (practElement.workgroup.ToString() == groupWorkList[countIndex])
                        areThereWorkGroupsMarks = true;
                if(areThereWorkGroupsMarks)
                {
                    List<GroupClass.Practice> practs = new List<GroupClass.Practice>();
                    List<int> newCodes = new List<int>();
                    GroupClass.Practice pract = new GroupClass.Practice();
                    pract.Clear(totalWorks[i].CodeGroup);

                    List<int> codesToDelete = new List<int>();


                    for (int j = 0; j < totalWorks[i].Practs.Count; j++)
                    {
                        if (totalWorks[i].Practs[j].groupCode != Convert.ToInt16(groupCodesList[countIndex]))
                        {
                            practs.Add(totalWorks[i].Practs[j]);
                            codesToDelete.Add(totalWorks[i].Practs[j].practCode);
                            
                        }
                        else
                        {
                            pract.Uning(totalWorks[i].Practs[j]);
                            newCodes.AddRange(totalWorks[i].Practs[j].classes.ToArray());
                        }
                    }

                    foreach (GroupClass.Practice practToDelete in totalWorks[i].Practs)
                    {
                        if (codesToDelete.Contains(practToDelete.groupCode))
                            totalWorks[i].Practs.Remove(practToDelete);
                    }
                    practs.Add(pract);
                    var work = totalWorks[i];
                    var pr = practs[0];
                    pr.lenght = work.Practs[0].TTable.GetLength(1);
                    pr.codes = newCodes.ToArray();
                    practs[0] = pr;
                    work.Practs = practs;
                    totalWorks.Remove(totalWorks[i]);
                    totalWorks.Add(work);
                }
            }
        }
        public static void SetDays(GATTable.Genome listElements)
        {
            foreach(GATTable.Genome.TTPractice organizedPractice in listElements.GetSetTTPracticeList)
            {
                int i = 0, j = 0;
                FindIndexOfPracticeInList(organizedPractice.GroupCode, organizedPractice.CodeOfPractice, out i, out j);
                GroupClass.Practice newPract = TotalWorks[i].Practs[j];
                newPract.dayBegin = organizedPractice.DayBegin;
                newPract.dayEnd = organizedPractice.DayBegin+ organizedPractice.Lenght;
                TotalWorks[i].Practs[j] = newPract;
            }

        }
       public static void FindIndexOfPracticeInList(int groupCode, int practiceCode, out int listIndex, out int practiceIndex)
        {
            listIndex = 0; practiceIndex = 0;
            for (int i = 0; i < GroupsList.TotalWorks.Count; i++)
            {
                if (Convert.ToInt16(TotalWorks[i].CodeGroup) == groupCode)
                {
                    for (int j = 0; j < GroupsList.TotalWorks[i].Practs.Count; j++)
                    {
                        if (GroupsList.TotalWorks[i].Practs[j].practCode == practiceCode)
                        {
                            listIndex = i; practiceIndex = j;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        internal static void ClearTotalWorks()
        {
            totalWorks.Clear();
        }

        internal static List<GroupClass> TotalWorks
        {
            get
            {
                return totalWorks;
            }
        }

        public static Genome OrginizedPractices
        {
            get
            {
                return orginizedPractices;
            }

            set
            {
                orginizedPractices = value;
            }
        }

        public static Dictionary<int, string> ClassNames
        {
            get
            {
                return classNames;
            }
        }

        public int Compare(GroupClass x, GroupClass y)
        {
            /*if (!(x is GroupClass) || !(y is GroupClass))
                throw new ArgumentException("Not of type GroupClass");*/

            int xLength = ((GroupClass)x).Name.Length, yLength = ((GroupClass)y).Name.Length;
            for (int letter = 0; letter < ( xLength>yLength ? ((GroupClass)x).Name.Length : ((GroupClass)y).Name.Length); letter++)
            {           
                if (((GroupClass)x).Name.ToCharArray()[letter] > ((GroupClass)y).Name.ToCharArray()[letter])
                    return 1;
                else if (((GroupClass)x).Name.ToCharArray()[letter] < ((GroupClass)y).Name.ToCharArray()[letter])
                    return -1;
            }
            return 0;
        }
    }
    
}
