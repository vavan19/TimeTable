using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Work;
using System.Collections;

namespace btl.generic
{
    class GAGoMethod
    {
        public static double theActualFunction(int[,] pract, GroupClass.Practice practData)
        {
            /*if (values.GetLength(2) == 2)
                throw new ArgumentOutOfRangeException("should only have 2 args");*/

            double fitnes=0;
            int count= pract.GetLength(0);
            Stack[] numbersOrder = new Stack[count];
            for (int i=0; i<count; i++)
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
                        fitnes+=2;
                    }
                }
                if(number.Count>= count - 1)
                    fitnes += (count - number.Count)*0.5;
                else
                    fitnes += 20;
            }            
            return fitnes;
        }

        public static void GoGa(double Crossover, double Mutation, int Population, int Generations, out Genome uotGenome,GroupClass.Practice pract)
        {
            //  Crossover		= 80%
            //  Mutation		=  5%
            //  Population size = 100
            //  Generations		= 2000
            //  Genome size		= 2
            GA ga = new GA(Crossover, Mutation, Population, Generations);

            ga.FitnessFunction = new GAFunction(theActualFunction);

            //ga.FitnessFile = @"H:\fitness.csv";
            ga.Elitism = true;
            uotGenome = null;
            ga.Go(out uotGenome,pract);

            /*double[] values;
            double fitness;
            ga.GetBest(out values, out fitness);
            System.Console.WriteLine("Best ({0}):", fitness);
            for (int i = 0; i < values.Length; i++)
                System.Console.WriteLine("{0} ", values[i]);

            ga.GetWorst(out values, out fitness);
            System.Console.WriteLine("\nWorst ({0}):", fitness);
            for (int i = 0; i < values.Length; i++)
                System.Console.WriteLine("{0} ", values[i]);*/

        }
    }
}
