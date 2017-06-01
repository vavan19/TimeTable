

using System;
using System.Collections;
using btl.generic;

namespace TimeTable.GATTable
{
	/// <summary>
	/// Compares genomes by fitness
	/// </summary>
	public sealed class GenomeComparer : IComparer
	{
		public GenomeComparer()
		{
		}
		public int Compare( object x, object y)
		{
			if ( !(x is Genome) || !(y is Genome))
				throw new ArgumentException("Not of type Genome");

			if (((Genome) x).Fitness > ((Genome) y).Fitness)
				return 1;
			else if (((Genome) x).Fitness == ((Genome) y).Fitness)
				return 0;
			else
				return -1;

		}
	}
    public sealed class TTableComparer:IComparer
    {
        public TTableComparer()
        {
        }

        
        public int Compare(object x, object y)
        {            
            if (!(x is Genome.TTPractice) || !(y is Genome.TTPractice))
                throw new ArgumentException("Not of type Genome");
            if (((Genome.TTPractice)x).DayBegin > ((Genome.TTPractice)y).DayBegin)
                return 1;
            else if (((Genome)x).Fitness == ((Genome)y).Fitness)
                return 0;
            else
                return -1;

        }
    }
}
