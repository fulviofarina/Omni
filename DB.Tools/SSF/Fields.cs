using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Rsx;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.Tools
{
    public partial class MatSSF
    {
        protected internal static char[] ch = new char[] { ',', '(', '#', ')' };
        protected internal static string cmd = "cmd";
        protected internal static string exefile = "matssf2.exe";
        protected internal static string inPutExt = ".in";
        protected internal static string outPutExt = ".out";
        protected internal static string startupPath = string.Empty;
        protected static System.Collections.Hashtable processTable;
        protected EventHandler callBack = null;
        protected bool cancelCalculations = false;
        protected Interface Interface;
        protected Action<int> resetProgress;
        protected EventHandler showProgress;
        protected IList<LINAA.UnitRow> units = null;
    }

   
}