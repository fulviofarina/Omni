using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB.Tools
{
    public partial class MatSSF
    {

        protected internal static System.Collections.Hashtable _processTable;

        protected internal bool _bkgCalculation = false;
        protected internal static string _startupPath = string.Empty;
        protected internal EventHandler _callBack = null;
        protected internal Interface Interface;
        protected internal Action<int> _resetProgress;
        protected internal EventHandler _showProgress;
        protected internal IList<LINAA.UnitRow> _units = null;
    }
  
  
}