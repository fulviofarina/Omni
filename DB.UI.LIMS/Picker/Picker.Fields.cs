using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DB.UI
{
    public partial class Picker
    {
        private DataGridView fromdgv = null;

        public DataGridView FromDgv
        {
            get { return fromdgv; }
            set { fromdgv = value; }
        }

        private DataGridView todgv = null;

        public DataGridView ToDgv
        {
            get { return todgv; }
            set { todgv = value; }
        }

        private DataTable from = null;

        public DataTable FromDt
        {
            get { return from; }
            set { from = value; }
        }

        private DataTable to = null;

        public DataTable ToDt
        {
            get { return to; }
            set { to = value; }
        }

        private bool toAdd = false;
        private DataRelation relation = null;

        public DataRelation Relation
        {
            get { return relation; }
            set { relation = value; }
        }

        private LINAA.ExceptionsDataTable exceptions;
        private DataGridView[] fromDgvs;

        private List<DataRow> ToRowList;
    }
}