using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DB.Tools
{
    public partial class ToDo
    {
        private double alpha0 = 0;

        private DataColumn[] arrOfColToPush = null;

        private LINAA.ToDoAvgDataTable clone = null;

        private double f0 = 0;

        private FitParameters fit;

        private IEnumerable<LINAA.ToDoAvgRow> iAvgs = null;

        private Interface Interface;

        private LINAA Linaa = null;

        private bool locked = false;

        private short minPosition = 3;

        private bool optimize = false;

        private BindingSource sRBS = null;

        private DataGridView sRDGV = null;

        private LINAA.ToDoType tocalculate;

        private IEnumerable<LINAA.ToDoRow> ToDoes = null;

        private bool useRef = false;
    }
}