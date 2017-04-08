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
            PopulateXCOMList,
         PopulateVials,
            PopulateGeometry};

            return populatorArray;
        }

        public Action[] PMBasic()
        {
            Action[] populatorArray = null;
            populatorArray = new Action[]   {
                       PopulateColumnExpresions,
                PopulateUserDirectories,
                PopulatePreferences,
                //PopulateSSFPreferences,
                SavePreferences
              };

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
            populatorArray = PMBasic();
            populatorArray = populatorArray.Union(PMMatrix());

            populatorArray = populatorArray.Union(PMDetect());
            return populatorArray;
        }
    }
}