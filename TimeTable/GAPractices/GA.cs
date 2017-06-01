using System;
using System.Collections;
using System.IO;
using System.Threading;
using TimeTable.Work;

namespace btl.generic
{

	public delegate double GAFunction(int[,] pract, GroupClass.Practice practData);

    public delegate int GAFunction<T>(T obj);

    /// <summary>
    /// Genetic Algorithm class
    /// </summary>
    public class GA
	{
		/// <summary>
		/// Default constructor sets mutation rate to 5%, crossover to 80%, population to 100,
		/// and generations to 2000.
		/// </summary>
		public GA()
		{
			InitialValues();
			m_mutationRate = 0.35;
			m_crossoverRate = 0.80;
			m_populationSize = 100;
			m_generationSize = 2000;
			m_strFitness = "";
		}
        
		public GA(double crossoverRate, double mutationRate, int populationSize, int generationSize)
		{
			InitialValues();
			m_mutationRate = mutationRate;
			m_crossoverRate = crossoverRate;
			m_populationSize = populationSize;
			m_generationSize = generationSize;
            //m_genomeSize = genomeSize;
			m_strFitness = "";
		}

		public GA(int days, int clases)
		{
			InitialValues();
			;
		}


		public void InitialValues()
		{
			m_elitism = false;
		}


		/// <summary>
		/// Method which starts the GA executing.
		/// </summary>
		public void Go(out Genome uotGenome, GroupClass.Practice _practice)
        {
			if (getFitness == null && FitnessFunction == null)
				throw new ArgumentNullException("Need to supply fitness function");
			

			//  Create the fitness table.
			m_fitnessTable = new ArrayList();
			m_thisGeneration = new ArrayList(m_generationSize);
			m_nextGeneration = new ArrayList(m_generationSize);
			Genome.MutationRate = m_mutationRate;
            m_practice = _practice;
            Genome.max = 100;
            CreateGenesAgain = CreateGenomes;
            
            CreateGenomes();

            if (_practice.codes.Length != 1)
            {
                Genome.Fin = true;
                RankPopulation();

                StreamWriter outputFitness = null;
                bool write = true;
                if (m_strFitness != "")
                {
                    write = true;
                    outputFitness = new StreamWriter(m_strFitness);
                }
                //int i=0;
                //for (int i = 0; i < m_generationSize; i++)
                if(m_totalFitness!=0)
                while (Genome.Fin)
                {
                    CreateNextGeneration();
                    RankPopulation();


                    if (write)
                    {
                        if (outputFitness != null)
                        {
                            double d = (double)((Genome)m_thisGeneration[m_populationSize - 1]).Fitness;
                            //outputFitness.WriteLine("{0},{1}", i, d);
                        }
                    }

                    Genome a = (Genome)m_thisGeneration[m_populationSize - 1];
                    Genome.TryMax(a);                    
                    StreamWriter file = new StreamWriter("out.txt");
                    file.Close();

                }
                Genome _outGenom = (Genome)m_thisGeneration[m_populationSize - 1];
                //int fit = _outGenom.Fitness;
                //_outGenom.m_ttable = Genome.maxFitnesTTable;
                //double t = FitnessFunction(_outGenom.TTable, _outGenom.PractData);
                uotGenome = _outGenom;

                if (outputFitness != null)
                    outputFitness.Close();
            }
            else
                uotGenome = (Genome)m_thisGeneration[m_populationSize - 1];
        }

        //Predicate<Genome> predec = 
        static int numbIter = 0, gener=0;
        public void IsEnd(Genome uotGenome)
        {
            
            Genome g = ((Genome)m_thisGeneration[m_populationSize-1]);
            if (g.Fitness < 36)
            {
                //TODO if it'll need
            }
            else
                numbIter++;
            if (numbIter > 100)
            {
                gener++;
                if (gener > 200)
                    fin = true;

                if (fin != true)
                {
                    numbIter = 0;
                    Go(out uotGenome,m_practice);
                }
            }            
        }
        public void Vivod()
        {
            if (outVars.Count > 6)
            {
                outVars.Sort((a,b) => {

                    if (a.m_genes[0] > b.m_genes[0] ) return 1;
                    if (a.m_genes[0] < b.m_genes[0] ) return -1;
                    return 0;

                            });
                foreach (Genome x in outVars)
                {
                    Genome g = x; //((Genome)m_thisGeneration[m_populationSize - 1]);
                    int[] a = g.Genes();
                    Console.WriteLine("Ответ {0} + 2*({1})+ 3*({2})+ 4*({3})+ 5*({4}) = 30", a[0], a[1], a[2], a[3],a[4]);
                }
                Console.ReadLine();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Ответы не найдены");
                gener = 0;
                fin = false;
            }
        }

        /// <summary>
        /// After ranking all the genomes by fitness, use a 'roulette wheel' selection
        /// method.  This allocates a large probability of selection to those with the 
        /// highest fitness.
        /// </summary>
        /// <returns>Random individual biased towards highest fitness</returns>
        private int RouletteSelection()
		{
			double randomFitness = m_random.NextDouble() * m_totalFitness;
			int idx = -1;
			int mid;
			int first = 0;
			int last = m_populationSize -1;
			mid = (last - first)/2;

			//  ArrayList's BinarySearch is for exact values only
			//  so do this by hand.
			while (idx == -1 && first <= last)
			{
				if (randomFitness < (double)m_fitnessTable[mid])
				{
					last = mid;
				}
				else if (randomFitness > (double)m_fitnessTable[mid])
				{
					first = mid;
				}
				mid = (first + last)/2;
				//  lies between i and i+1
				if ((last - first) == 1)
					idx = last;
			}
			return idx;
		}

		/// <summary>
		/// Rank population and sort in order of fitness.
		/// </summary>
		private void RankPopulation()
		{
			m_totalFitness = 0;
			for (int i = 0; i < m_populationSize; i++)
			{
				Genome g = ((Genome) m_thisGeneration[i]);
                g.Fitness = Convert.ToInt16(FitnessFunction(g.TTable, g.GetSetPracticeData));
				m_totalFitness += g.Fitness;
			}
			m_thisGeneration.Sort(new GenomeComparer());

			//  now sorted in order of fitness.
			double fitness = 0.0;
			m_fitnessTable.Clear();
			for (int i = 0; i < m_populationSize; i++)
			{
				fitness += ((Genome)m_thisGeneration[i]).Fitness;
                try
                {
                    m_fitnessTable.Add((double)fitness);
                }
                catch(StackOverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                }
			}
		}

		/// <summary>
		/// Create the *initial* genomes by repeated calling the supplied fitness function
		/// </summary>
		private void CreateGenomes()
		{
            m_thisGeneration.Clear();
			for (int i = 0; i < m_populationSize ; i++)
			{
				Genome g = new Genome(m_practice);
				m_thisGeneration.Add(g);
			}
		}

		private void CreateNextGeneration()
		{
			m_nextGeneration.Clear();
			Genome g = null;
			if (m_elitism)
				g = (Genome)m_thisGeneration[m_populationSize - 1];

			for (int i = 0 ; i < m_populationSize ; i+=2)
			{
				int pidx1 = RouletteSelection();
				int pidx2 = RouletteSelection();
				Genome parent1, parent2, child1, child2;
				parent1 = ((Genome) m_thisGeneration[pidx1]);
				parent2 = ((Genome) m_thisGeneration[pidx2]);

				if (m_random.NextDouble() < m_crossoverRate)
				{
					parent1.Crossover(ref parent2, out child1, out child2);
				}
				else
				{
					child1 = parent1;
					child2 = parent2;
				}
				child1.Mutate();
				child2.Mutate();

				m_nextGeneration.Add(child1);
				m_nextGeneration.Add(child2);
			}
			if (m_elitism && g != null)
				m_nextGeneration[0] = g;

			m_thisGeneration.Clear();
			for (int i = 0 ; i < m_populationSize; i++)
				m_thisGeneration.Add(m_nextGeneration[i]);
		}

        GroupClass.Practice m_practice;

        private double m_mutationRate;
		private double m_crossoverRate;
		private int m_populationSize;
		private static int m_generationSize;
		private double m_totalFitness;
		private string m_strFitness;
		private bool m_elitism;
        private bool fin;
		
		private ArrayList m_thisGeneration;
        private System.Collections.Generic.List<Genome> outVars = new System.Collections.Generic.List<Genome>();
        private ArrayList m_nextGeneration;
		private ArrayList m_fitnessTable;

        public delegate void CreateGenes();
        static public CreateGenes CreateGenesAgain;
		
		static Random m_random = new Random();



		static private GAFunction getFitness;
        //static private GAFunction<Genome.Vars> getVarsFitnes;
		public GAFunction FitnessFunction
		{
			get	
			{
				return getFitness;
			}
			set
			{
				getFitness = value;
			}
		}
        /*public GAFunction<Genome.Vars> VarsFitnessFunction
        {
            get
            {
                return getVarsFitnes;
            }
            set
            {
                getVarsFitnes = value;
            }
        }*/

        //  Properties
        public int PopulationSize
		{
			get
			{
				return m_populationSize;
			}
			set
			{
				m_populationSize = value;
			}
		}

		public static int Generations
		{
			get
			{
				return m_generationSize;
			}
			set
			{
				m_generationSize = value;
			}
		}

		/*public int GenomeSize
		{
			get
			{
				return m_genomeSize;
			}
			set
			{
				m_genomeSize = value;
			}
		}*/

		public double CrossoverRate
		{
			get
			{
				return m_crossoverRate;
			}
			set
			{
				m_crossoverRate = value;
			}
		}
		public double MutationRate
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

		public string FitnessFile
		{
			get
			{
				return m_strFitness;
			}
			set
			{
				m_strFitness = value;
			}
		}

		/// <summary>
		/// Keep previous generation's fittest individual in place of worst in current
		/// </summary>
		public bool Elitism
		{
			get
			{
				return m_elitism;
			}
			set
			{
				m_elitism = value;
			}
		}


	}
}
