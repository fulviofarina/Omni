using System;

using System.Collections.Generic;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        public Action[] PMMatrix()
        {
            Action[] populatorArray = null;

            populatorArray = new Action[]   {
            PopulateCompositions ,
        PopulateMatrix,
        //    PopulateMUESList,
         PopulateVials,
            PopulateGeometry};

            return populatorArray;
        }

        public IEnumerable<Action> PMThree()
        {
            Action[] populatorOther = null;

            populatorOther = new Action[]   {
                    PopulateCOIList,
                      PopulateToDoes,
                   PopulateScheduledAcqs};

            IEnumerable<Action> populatorArray = null;
            populatorArray = populatorOther;
            // populatorArray.Union(PMNAA());

            return populatorArray;
        }

        public IEnumerable<Action> PMTwo()
        {
            IEnumerable<Action> populatorArray = null;
            populatorArray = PMMatrix();
            //   populatorArray = populatorArray.Union(PMMatrix());

            populatorArray = populatorArray.Union(PMDetect());
            return populatorArray;
        }
    }
}