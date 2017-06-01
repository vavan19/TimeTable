using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using btl.generic;
using System.Threading;
using System.IO;
using TimeTable.Work;
using TimeTable;

namespace TimeTable.GATTable
{
	/// <summary>
	/// Summary description for Genome.
	/// </summary>
	public class Genome
	{
        

        public Genome()
		{          
            //m_groups = groups; //7;
            

            CreateGenes();
            //
            // TODO: Add constructor logic here !checked!
            //
        }
        
        static Random r = new Random();

        void CreateGenes()
        {
            tTList = new List<TTPractice>();
            foreach (GroupClass group in GroupsList.TotalWorks)
            {                
                
                foreach (GroupClass.Practice practice in group.Practs)
                {
                    if (practice.lenght!=0)
                    {
                        int totalDays = (GroupClass.EndDay - GroupClass.StartDay).Days-((GroupClass.EndDay - GroupClass.StartDay).Days/7);
                        int minRandom = 0, maxRandom = (totalDays - practice.lenght); //- 6;//(totalDays/6);
                        if (practice.dayBegin != 0)
                            minRandom = practice.dayBegin;
                        if (practice.dayEnd != 0)
                            maxRandom = practice.dayEnd;
                        if (maxRandom <= 0)
                            maxRandom = 1;

                        int randomDay = m_random.Next(minRandom, maxRandom);
                        randomDay -= (randomDay + 1) % 3;
                        if (randomDay < practice.dayBegin)
                            randomDay += 6;
                        TTPractice PracticePosition = new TTPractice();
                        PracticePosition.SetStruct(practice.dayBegin, randomDay, maxRandom, practice.lenght, practice.practCode, Convert.ToInt16(group.CodeGroup), practice.podGrups);

                        tTList.Add(PracticePosition); //set randomDay + ":" + (practice.lenght) + ":" + group.CodeGroup + "," + group.Name });
                    }
                }
                
            }
        }

        List<int> CreateRandomOrder(int number)
        {
            List<int> newList = new List<int>();
            for (int i = 0; i < number; i++) 
            {
                newList.Add(i);
            }

            List<int> returnedList = new List<int>();

            while (newList.Count!=0)
            {
                int randIndex = m_random.Next(0, newList.Count);
                returnedList.Add(newList[randIndex]);
                newList.Remove(newList[randIndex]);
            }

            return returnedList;
        }
        public Genome(List<TTPractice> _tTList, bool createGenes)
        {
            //tTList = _tTList;
            if (createGenes)
                CreateGenes();
        }

        public void Crossover(ref Genome genome2, out Genome child1, out Genome child2)
		{
            int count = m_random.Next(1, tTList.Count-1);
            int pos1 = m_random.Next(1,tTList.Count-count);
            
            //int pos = m_random.Next(m_days);
            child1 = new Genome(tTList, false);
			child2 = new Genome(tTList, false);

            tTList.Sort(new PracticeComparer());
            genome2.tTList.Sort(new PracticeComparer());
            for (int i = 0; i < tTList.Count; i++)
            {
                if (i < pos1)
                {
                    child1.tTList.Add(tTList[i]);
                    child2.tTList.Add(genome2.tTList[i]);
                }
                else
                    if(i<pos1+count)
                {
                    child1.tTList.Add(genome2.tTList[i]);
                    child2.tTList.Add(tTList[i]);
                }
                else
                {
                    child1.tTList.Add(tTList[i]);
                    child2.tTList.Add(genome2.tTList[i]);
                }

            }
            
		}


		public void Mutate()
		{
            int fitnes=m_fitness;

            //m_mutationRate = 1;
            for (int index = 0 ; index < tTList.Count; index++)
			{
                if (m_random.NextDouble() < m_mutationRate)
                {
                    tTList[index].reRandDayBegin(m_fitness);
                    
                    //tTList[index].Remove();

                }               
            }
        }

        void WriteInFile(string[] s, string[] s1)
        {
            StreamWriter file = new StreamWriter("out.txt",true);
            foreach (string str in s)
                file.WriteLine(str);
            file.WriteLine("------------------------------");
            foreach (string str in s1) //not actual
                file.WriteLine(str);
            file.WriteLine("++++++++++++++++++++++++++++++");
            file.Close();
        }

        public int[] Genes()
		{
			return m_genes;
		}

        public struct TTPractice : IComparable,IComparer
        {
            int leftBorderDay,dayBegin, rightBorderDay,lenght,codeOfPractice, groupCode, podGroups;
            
            public void GetCodes(out int _practCode, out int _groupCode)
            {
                _practCode = codeOfPractice; _groupCode = groupCode;
            }
            public void SetStruct(int _leftBorderDay, int _dayBegin, int _endDay, int _lenght, int _numberOfPractice, int _groupCode, int _podGroups)
            {
                leftBorderDay = _leftBorderDay;
                dayBegin = _dayBegin;
                rightBorderDay = _endDay;
                lenght = _lenght;
                codeOfPractice = _numberOfPractice;
                groupCode = _groupCode;
                podGroups = _podGroups;
            }
            public void reRandDayBegin(int fitnes)
            {
                
                double move =(rightBorderDay-leftBorderDay);
                if (move == 0)
                    move = rightBorderDay - leftBorderDay;
                int shift = m_random.Next(1,3);
                //int minRandom = Math.Abs(dayBegin + (shift * (m_random.Next(-1, 1) == 0 ? 1 : -1)) )%(rightBorderDay-leftBorderDay);
                int newDay = (dayBegin+ (shift*3* (m_random.Next(-1, 1) == 0 ? 1 : -1)))%rightBorderDay;
                newDay += (newDay+1) % 3;
                while (newDay < leftBorderDay)
                    newDay += 3;
                dayBegin = newDay;
                
                
                //dayBegin = m_random.Next(leftBorderDay, rightBorderDay);
                dayBegin -= dayBegin % 3;
                if (leftBorderDay > dayBegin)
                    dayBegin += 6;
            }
            public int DayBegin
            {
                get { return dayBegin; }
            }
            public int DayEnd
            {
                get { return dayBegin+lenght; }
            }
            public int Lenght
            {
                get { return lenght; }
            }

            public int GroupCode
            {
                get
                {
                    return groupCode;
                }

                set
                {
                    groupCode = value;
                }
            }

            public int CodeOfPractice
            {
                get
                {
                    return codeOfPractice;
                }

                set
                {
                    codeOfPractice = value;
                }
            }
            public int PodGroups
            {
                get { return podGroups; }
            }

            public int CompareTo(object y)
            {
                if (!(y is Genome.TTPractice))
                    throw new ArgumentException("Not of type Genome");
                if (DayBegin > ((Genome.TTPractice)y).DayBegin)
                    return 1;
                else
                if (DayBegin == ((TTPractice)y).DayBegin)
                    return 0;
                else
                    return -1;
            }
            public int Compare(object x, object y)
            {
                if (!(x is Genome.TTPractice) || !(y is Genome.TTPractice))
                    throw new ArgumentException("Not of type Genome");
                if (((Genome.TTPractice)x).groupCode == ((Genome.TTPractice)y).groupCode &&
                    ((Genome.TTPractice)x).DayBegin == ((Genome.TTPractice)y).DayBegin)
                    return 0;
                else
                    return -1;
                

            }
            
        }
        
        class PracticeComparer: IComparer<TTPractice>
        {
            public int Compare(TTPractice x, TTPractice y)
            {                
                if (x.GroupCode > y.GroupCode)
                    return 1;
                else
                    if (x.GroupCode < y.GroupCode)
                    return -1;
                else
                    if (x.CodeOfPractice > y.CodeOfPractice)
                    return 1;
                else
                    return -1;
            }
        }
        
        List<TTPractice> tTList = new List<TTPractice>();
        public int[] m_genes;
		private int m_fitness;
        static public int upIter=0;
        static Random m_random = new Random();
        static public int max=20000;
        private static double m_mutationRate;
        static public List<TTPractice> maxGenome;
        public static bool stop = false;

        public int Fitness
		{
			get
			{
				return m_fitness;
			}
			set
			{
                m_fitness = value;
			}
		}

        public static List<TTPractice> GetSetMaxGenom
        {
            get { return maxGenome; }
            set { maxGenome = value; }
        }
        public static int Max
        {
            get { return max; }
            set
            {
                if ( max>value)
                {
                    max = value;
                    upIter = 0;
                }
                //else
                //if (max <= value)
                //{
                //    upIter++;
                //    if (upIter > GA.Generations * 20)
                //    {
                //        GA.CreateGenesAgain();
                //        upIter = 0;
                //    }
                //}
            }
        }

        public static bool Fin
        {
            get
            {
                if (!stop)
                    if (max < 5)
                    {
                        //&& max*(m_days*0.001)<= proportion)
                        stop = false;
                        return false;
                    }
                    else;
                else
                {
                    return false;
                }
                return true;
            }
        }


		public static double MutationRate
		{
			get
			{
				return m_mutationRate;
			}
			set
			{
				m_mutationRate = value;
			}
		}

        public List<TTPractice> GetSetTTPracticeList
        {
            get
            {
                return tTList;
            }
            set
            {
                tTList = value;
            }
        }

	}
}
