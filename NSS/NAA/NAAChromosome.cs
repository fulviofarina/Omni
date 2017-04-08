using System;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;

namespace DB
{
    public sealed class NAAChromo : GeneticSharp.Domain.Chromosomes.ChromosomeBase
    {
        private int numberOfGenes;

        // private HashSet<int> nonRepeated;
        private int eqNumber = 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sizeOfChromosome"></param>
        /// <param name="numOfGenes"></param>
        public NAAChromo(int numOfGenes, int numberEqs) : base(numOfGenes)
        {
            // nonRepeated = new HashSet<int>();

            numberOfGenes = numOfGenes; // do I need the values? nope I think, only indexes

            eqNumber = numberEqs;

            for (int i = 0; i < numberOfGenes; i++)
            {
                Gene g = GenerateGene(i);
                ReplaceGene(i, g);
            }
        }

        public override Gene GenerateGene(int geneIndex)
        {
            object o = 0;
            o = RandomizationProvider.Current.GetFloat(0.0f, 1.0f);
            decimal d = Decimal.Round(Convert.ToDecimal(o), 3);
            return new Gene(d);
        }

        /// <summary>
        /// Creates a new chromosome using the same structure of this.
        /// </summary>
        /// <returns>The new chromosome.</returns>
        public override IChromosome CreateNew()
        {
            return new NAAChromo(this.numberOfGenes, this.eqNumber);
        }

        /// <summary>
        /// Creates a clone.
        /// </summary>
        /// <returns>The chromosome clone.</returns>
        public override IChromosome Clone()
        {
            NAAChromo c = base.Clone() as NAAChromo;
            c.numberOfGenes = this.numberOfGenes;
            c.eqNumber = this.eqNumber;
            return c;
        }
    }
}