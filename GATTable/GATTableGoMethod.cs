using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Work;
using System.Collections;
using TimeTable.GATTable;

namespace TimeTables.GATTable
{
    class GATTableGoMethod
    {
        int[,] fitnesTable = new int[10,5];
        public  double theActualFunction(Genome g)
        {
            /*if (values.GetLength(2) == 2)
                throw new ArgumentOutOfRangeException("should only have 2 args");*/

            List<Genome.TTPractice> copyTTList = g.GetSetTTPracticeList;
            copyTTList.Sort();

            double fitnes = 0;
            foreach (Genome.TTPractice selectedElement in copyTTList)
            {
                foreach(Genome.TTPractice comparableElement in copyTTList)
                {                   
                    if (selectedElement.CodeOfPractice != comparableElement.CodeOfPractice)
                    {
                        bool mayBeConflict = false;
                        if (selectedElement.DayBegin >= comparableElement.DayBegin
                            && selectedElement.DayBegin <= comparableElement.DayEnd)
                            mayBeConflict = true;
                        else
                            if (selectedElement.DayBegin >= comparableElement.DayBegin                                
                                && selectedElement.DayEnd <= comparableElement.DayEnd)
                                mayBeConflict = true;
                        else
                            if (selectedElement.DayBegin <= comparableElement.DayBegin 
                            && selectedElement.DayEnd >= comparableElement.DayBegin)
                            mayBeConflict = true;
                        if (mayBeConflict)
                        {
                            fitnes += CompareGenomes(selectedElement, comparableElement);
                        }
                    }
                }
            }

            fitnes++;

            
            return fitnes;
        }

        double CompareGenomes(Genome.TTPractice g1, Genome.TTPractice g2)
        {
            double fitnes=0;
            int beginingIndex,endingIndex;
            GroupClass.Practice p1 = new GroupClass.Practice(), p2 = new GroupClass.Practice();
            
            int[,] tt1 = new int[0,0], tt2 = new int [0,0];

            if (g1.DayBegin < g2.DayBegin)
            {
                beginingIndex = g2.DayBegin - g1.DayBegin;
            }
            else
            {
                beginingIndex = g1.DayBegin - g2.DayBegin;
            }

            if (g1.DayBegin + g1.Lenght < g2.DayBegin + g2.Lenght) // what lenght is bigest? // There are bugs here
            {
                endingIndex = g1.Lenght;
                int code = 0, group = 0;
                g1.GetCodes(out code, out group);
                
                p1 = p1.GetPracticeByCode(group, code); 

                g2.GetCodes(out code, out group);
                p2 = p2.GetPracticeByCode(group, code);
            }
            else // else -> take another ended lenght
            {
                endingIndex = g2.Lenght;
                int code = 0, group = 0;
                g2.GetCodes(out code, out group);


                p1 = p1.GetPracticeByCode(group, code);

                g1.GetCodes(out code, out group);
                p2 = p2.GetPracticeByCode(group, code);
            }

            bool repition = true; // are there same clases or not
            /*foreach(int codeP1 in p1.classes)
                foreach(int codeP2 in p2.classes)
                {
                    if (codeP1 == codeP2)
                        repition = true;
                }*/

            List<int> number = new List<int>();
            if (repition)
            {
                for (int index = beginingIndex, j = 0; index < endingIndex; index++, j++)
                {
                    int count = 0;
                    for (int podgroupP1 = 0; podgroupP1 < p1.TTable.GetLength(0); podgroupP1++)
                    {
                        for (int podgroupP2 = 0; podgroupP2 < p2.TTable.GetLength(0); podgroupP2++)
                            if (p1.TTable[podgroupP1, index] == p2.TTable[podgroupP2, j])
                            {
                                count++;
                            }

                    }
                    fitnes += count;
                }
            }
                return fitnes;
        }
        int Factorial(int numb)
        {
            int res = 1;
            for (int i = 0; i < numb; i++)
                res *= 2;
            return res==1?0:res;
        }
        public void GoGa(double Crossover, double Mutation, int Population, int Generations, out Genome outGenome)
        {
            Genome.stop = false;
            outGenome = new Genome();
            //Crossover = 80 %
            //Mutation = 5 %
            //Population size = 100
            //Generations = 2000
            //Genome size = 2
            GA ga = new GA(Crossover, Mutation, Population, Generations);

            ga.FitnessFunction = new GAFunction(theActualFunction);

            //ga.FitnessFile = @"H:\fitness.csv";
            ga.Elitism = true;
            //uotGenome = null;
            ga.Go(out outGenome);

        }
    }
}
