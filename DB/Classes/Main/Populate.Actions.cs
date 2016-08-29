using System;

using System.Collections.Generic;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        public Action[] PMBasic()
        {
            Action[] populatorArray = null;
            populatorArray = new Action[]   {
                       PopulateColumnExpresions,
                PopulateUserDirectories,
                PopulatePreferences,
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


        public IEnumerable<Action> PMZero()
        {
            IEnumerable<Action> populatorArray = null;
            populatorArray = PMLIMS();
            populatorArray = populatorArray.Union(PMMatrix());
            populatorArray = populatorArray.Union(PMStd());
            populatorArray = populatorArray.Union(PMDetect());
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