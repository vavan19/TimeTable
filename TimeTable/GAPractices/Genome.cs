
using System;
using System.Collections;
using System.Collections.Generic;
using btl.generic;
using System.Threading;
using System.IO;
using TimeTable.Work;
using TimeTable;

namespace btl.generic
{
	/// <summary>
	/// Summary description for Genome.
	/// </summary>
	public class Genome
	{
        

        public Genome(GroupClass.Practice pract)
		{
            GetSetPracticeData = pract;            
            //m_groups = groups; //7;
            

            CreateGenes(pract);
            //
            // TODO: Add constructor logic here !check!
            //
        }
        

        void CreateGenes(GroupClass.Practice pract)
        {
            int FirstShift = m_random.Next(0, 1);
            
            for (int podGrop = 0; podGrop < pract.podGrups; podGrop++)
            {
                List<int> randomOrder = CreateRandomOrder(pract.codes.Length);

                
                int i = m_random.Next(m_days);
                //int shift = FirstShift + i / 6;
                int k = 0;

                for (int index=0; index<randomOrder.Count; index++)
                {
                    
                    int timing = pract.timings[randomOrder[index]];
                    int schoolClass = pract.classes[randomOrder[index]];

                    //while (k % pract.timings[randomOrder[index]] != pract.timings[randomOrder[index]])
                    int beginingDay = k;
                    
                    for (; k < beginingDay + timing; k++, i++)                   
                    {                                                                        
                        m_ttable[podGrop, i % m_days] = schoolClass;  // how it write for enothers 2 groups?
                        /*if (k % 7 == 0)
                            shift++;*/
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
    /// <summary>
    /// ex constructor (below)
    /// </summary>
    /// <param name="length"></param>
    /// <param name="createGenes"></param>
        /*int day = m_random.Next(0, m_ttable.GetLength(1) - group.Duration);
                foreach (Group.Practise pract in group.Practs)
                {
                    List<int> timings = new List<int>();
                    timings.AddRange(pract.timings);
                    //to do... write some var. to save the current long of practice
                    // and manage how to spread the prictice time
                    while (timings.Capacity!=0)
                    {
                        int randNumb = m_random.Next(timings.Capacity);
                        int chosedTiming = timings[randNumb];  //choosing the pract.
                        int thisPracticeDay = day;
                        int shift=1;
                        //if (m_ttable[pract.nclass, day, 1] == 0 && m_ttable[pract.nclass, day + chosedTiming, 1] == 0 && m_ttable[pract.nclass, day + (chosedTiming / 2), 1] == 0)
                            //shift = 0;

                        for (; day < day + chosedTiming; day++)
                            m_ttable[pract.nclass, day,shift] = pract.codes[randNumb];

                        timings.Remove(timings[randNumb]);

                    }                
                    //
                } */

        /*private void CreateVGenes()
        {
            CreateVGenes();
            for (int i=0; i<m_length; i++)
            {
                m_genes[i] = r.Next(1,30);
            }
                
        }*/

        /*public Genome(int length)
		{
			m_length = length;
			m_genes = new double[ length ];
			CreateGenes();
		}
		public Genome(int length, bool createGenes)
		{
			m_length = length;
			m_genes = new double[ length ];
			if (createGenes)
				CreateGenes();
		}

		public Genome(ref double[] genes)
		{
			m_length = genes.GetLength(0);
			m_genes = new double[ m_length ];
			for (int i = 0 ; i < m_length ; i++)
				m_genes[i] = genes[i];
		}
		 

		private void CreateGenes()
		{
			// DateTime d = DateTime.UtcNow;
			for (int i = 0 ; i < m_length ; i++)
				m_genes[i] = m_random.NextDouble();
		}*/
        public Genome(GroupClass.Practice _practData, bool createGenes)
        {
            GetSetPracticeData = _practData;
            if (createGenes)
                CreateGenes(practData);
        }

        public void Crossover(ref Genome genome2, out Genome child1, out Genome child2)
		{
            int row = m_random.Next(m_ttable.GetLength(0));
            //int pos = m_random.Next(m_days);
			child1 = new Genome(practData, false);
			child2 = new Genome(practData, false);
            for (int rowIndex = 0; rowIndex < m_ttable.GetLength(0); rowIndex++)
            {
                if (rowIndex<row)
                for (int i = 0; i < m_days; i++)
                {
                    {
                        child1.m_ttable[rowIndex,i] = m_ttable[rowIndex, i];
                        child2.m_ttable[rowIndex, i] = genome2.m_ttable[rowIndex, i];
                    }
                }
                else
                {
                    for (int i = 0; i < m_days; i++)
                        {
                            child1.m_ttable[rowIndex, i] = genome2.m_ttable[rowIndex, i];
                            child2.m_ttable[rowIndex, i] = m_ttable[rowIndex, i];
                        }

                }
            }
		}


		public void Mutate()
		{
            string[] s=null , s1=null ;

            int fitnes=m_fitness;
            if (m_fitness==0)
            {
                fitnes = 37;
            }

            if (fitnes == max)// && false)
            {
                TakeIt(ref s);                
            }
            //m_mutationRate = 1;
            for (int row = 0 ; row < m_podGroups; row++)
			{
                if (m_random.NextDouble() < m_mutationRate)
                {
                    int maxRandomValue= fitnes / (m_days / m_podGroups / 2);
                    if (maxRandomValue == 0)
                        maxRandomValue=2;
                    int move = m_random.Next(1, maxRandomValue);/// m_fitness);
                    if (m_random.Next(-1, 1) < 0)
                        move = Math.Abs(m_days - move);
                    int[] a = new int[m_ttable.GetLength(1)];
                        
                    for (int index = 0; index < m_ttable.GetLength(1); index++)
                    {                        
                        a[index] = m_ttable[row, (index+move)% m_ttable.GetLength(1)];
                            
                    }
                    for (int index = 0; index < m_ttable.GetLength(1); index++)
                    {
                        m_ttable[row,index] = a[index];
                    }                
                        //for (int index = 0; index < m_ttable.GetLength(1); index++)
                        //    m_ttable[row, index] = m_ttable[row, (index + move) % m_ttable.GetLength(1)];
                    

                }               
            }
            if (fitnes == max) //&& f && false)
            {
                TakeIt(ref s1);
                WriteInFile(s,s1);
            }
        }
        void TakeIt(ref string[] text)
        {            
            text = new string[m_ttable.GetLength(0)];



            

            for (int i = 0; i < m_ttable.GetLength(1); i++)
            {

                for (int j = 0; j < m_ttable.GetLength(0); j++)
                {
                    text[j] += m_ttable[j, i];
                }

            }
        }
        void WriteInFile(string[] s, string[] s1)
        {
            StreamWriter file = new StreamWriter("out.txt",true);
            foreach (string str in s)
                file.WriteLine(str);
            file.WriteLine("------------------------------");
            foreach (string str in s1)
                file.WriteLine(str);
            file.WriteLine("++++++++++++++++++++++++++++++");
            file.Close();
        }
        //else
        //{
        //    int[] a = new int[m_ttable.GetLength(1)];
        //    int k=0;
        //    for (int index = m_ttable.GetLength(1)+move; index >0+ move; index--)
        //    {

        //        a[k] = m_ttable[row, Math.Abs((m_ttable.GetLength(1)-(index)))];
        //        k++;
        //    }
        //    for (int index = 0; index < m_ttable.GetLength(1); index++)
        //    {
        //        m_ttable[row, index] = a[index];
        //    }
        //    //m_ttable[row, index% m_ttable.GetLength(1)] = m_ttable[row, (index + move) % m_ttable.GetLength(1)];
        //}
        public void MutateV()
        {
            //TODO
        }

        public int[] Genes()
		{
			return m_genes;
		}

        /*public Vars VGenes()
        {
            return vars;
        }*/
        
        
        public int[,]m_ttable;
        public int[] move;
        GroupClass.Practice practData;
        public int[] m_genes;
		private int m_fitness;
        static int m_days;
        int m_podGroups;
        static public int upIter=0;
        static Random m_random = new Random();
        static public int max = 0;
        static double proportion;
        private static double m_mutationRate;
        static public int[,] maxFitnesTTable;

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

        static public int TryMax(Genome g)
        {
            if (max > g.Fitness || max == 0)
            {
                max = g.Fitness;
                maxFitnesTTable = g.TTable;
                //maxFitnesTTable = ;
                upIter = 0;
                if (max == 0)
                    upIter = GA.Generations;
            }
            else
                if (max != g.Fitness)
            {
                upIter++;
                if (upIter > GA.Generations * 2)
                {
                    GA.CreateGenesAgain();
                    upIter = 0;
                }
            }

            return 0;
        }
        public static int Max
        {
            get { return max; }
            set
            {
               /* if ( max>value || max == 0)
                {
                    max = value;
                    //maxFitnesTTable = ;
                    upIter = 0;
                    if (max == 0)
                        upIter = GA.Generations;
                }  
                else
                if (max!=value)
                {
                    upIter++;
                    if (upIter > GA.Generations*2)
                    {
                        GA.CreateGenesAgain();
                        upIter = 0;
                    }
                }*/
            }
        }

        public static bool Fin
        {
            get
            {
                if ( max*(m_days*0.001)<= proportion)
                    return false;
                return true;
            }
            set
            {
                
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

        public GroupClass.Practice PractData
        {
            get
            {
                return practData;
            }
        }

        public int[,] TTable
        {
            get { return m_ttable; }
        }
        public GroupClass.Practice GetSetPracticeData
        {
            get { return practData; }
            set
            {
                practData = value;
                m_podGroups = practData.podGrups; //5;  //<-- = 3
                m_days = practData.lenght ; //261;
                m_ttable = new int[m_podGroups, m_days];
                move = new int[m_podGroups];
                proportion = practData.proportion;
            }
        }
	}
}
