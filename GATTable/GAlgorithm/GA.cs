using System;
using System.Collections;
using System.IO;
using System.Threading;
using TimeTable.Work;

namespace TimeTable.GATTable
{

    public delegate double GAFunction(Genome g);

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
        }


        public void InitialValues()
        {
            m_elitism = false;
        }


        /// <summary>
        /// Method which starts the GA executing.
        /// </summary>
        public void Go(out Genome outGenome)
        {
            if (getFitness == null && FitnessFunction == null)
                throw new ArgumentNullException("Need to supply fitness function");


            //  Create the fitness table.
            m_fitnessTable = new ArrayList();
            m_thisGeneration = new ArrayList(m_generationSize);
            m_nextGeneration = new ArrayList(m_generationSize);
            Genome.MutationRate = m_mutationRate;
            Genome.max = 20000;
            CreateGenesAgain = CreateGenomes;
            Genome bestGenome = new Genome();

            CreateGenomes();

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

            int i=0;
            while (Genome.Fin)
            {
                CreateNextGeneration();
                
                if (i%2 ==0)
                {
                    for (int j = 0; j < m_thisGeneration.Count/4; j++)
                    {
                        Genome g = new Genome();
                        m_thisGeneration.Add(g);
                    }
                }

                RankPopulation();

                if (write)
                {
                    if (outputFitness != null)
                    {
                        double d = (double)((Genome)m_thisGeneration[m_populationSize - 1]).Fitness;
                        //outputFitness.WriteLine("{0},{1}", i, d);
                    }
                }

                Genome famouseGenome = (Genome)m_thisGeneration[0];
                Genome.Max = famouseGenome.Fitness;
                if (Genome.max == famouseGenome.Fitness)
                {
                    Genome.maxGenome = famouseGenome.GetSetTTPracticeList;
                    bestGenome = famouseGenome;
                }
                i++;

                if (i > 1200)
                    break;
            }
            //System.Console.Beep(1000,1000);
            int t = Convert.ToInt16(FitnessFunction(bestGenome));
            outGenome = new Genome();
            outGenome.Fitness = Genome.Max;
            outGenome.GetSetTTPracticeList = Genome.maxGenome;

            if (outputFitness != null)
                outputFitness.Close();


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
            int last = m_populationSize - 1;
            mid = (last - first) / 2;

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
                mid = (first + last) / 2;
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
            for (int i = 0; i < m_thisGeneration.Count; i++)
            {
                Genome g = ((Genome)m_thisGeneration[i]);
                g.Fitness = Convert.ToInt16(FitnessFunction(g));
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
                catch (StackOverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            m_thisGeneration.RemoveRange(m_populationSize, m_thisGeneration.Count - m_populationSize);
        }

        /// <summary>
        /// Create the *initial* genomes by repeated calling the supplied fitness function
        /// </summary>
        private void CreateGenomes()
        {
            m_thisGeneration.Clear();
            for (int i = 0; i < m_populationSize; i++)
            {
                Genome g = new Genome();
                m_thisGeneration.Add(g);
            }
        }

        private void CreateNextGeneration()
        {
            m_nextGeneration.Clear();
            Genome g = null;
            if (m_elitism)
                g = (Genome)m_thisGeneration[m_populationSize - 1];

            for (int i = 0; i < m_populationSize; i += 2)
            {
                int pidx1 = RouletteSelection();
                int pidx2 = RouletteSelection();
                Genome parent1, parent2, child1, child2;
                parent1 = ((Genome)m_thisGeneration[pidx1]);
                parent2 = ((Genome)m_thisGeneration[pidx2]);

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
            for (int i = 0; i < m_populationSize; i++)
                m_thisGeneration.Add(m_nextGeneration[i]);
        }

        //GroupClass.Practice m_practice;

        private double m_mutationRate;
        private double m_crossoverRate;
        private int m_populationSize;
        private static int m_generationSize;
        private double m_totalFitness;
        private string m_strFitness;
        private bool m_elitism;

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

		/*public void GetBest(out double[] values, out double fitness)
		{
			Genome g = ((Genome)m_thisGeneration[m_populationSize-1]);
			values = new double[g.Length];
			g.GetValues(ref values);
			fitness = (double)g.Fitness;
		}

		public void GetWorst(out double[] values, out double fitness)
		{
			GetNthGenome(0, out values, out fitness);
		}

		public void GetNthGenome(int n, out double[] values, out double fitness)
		{
			if (n < 0 || n > m_populationSize-1)
				throw new ArgumentOutOfRangeException("n too large, or too small");
			Genome g = ((Genome)m_thisGeneration[n]);
			values = new double[g.Length];
			g.GetValues(ref values);
			fitness = (double)g.Fitness;
		}*/
	}
}
