using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Rsx;

namespace DB.UI
{
    public interface IPicker
    {
        void DeLinkDGVs();

        System.Windows.Forms.DataGridView FromDgv { get; set; }
        System.Data.DataTable FromDt { get; set; }

        void LinkDGVs();

        System.Data.DataRelation Relation { get; set; }

        bool Take();

        System.Windows.Forms.DataGridView ToDgv { get; set; }
        System.Data.DataTable ToDt { get; set; }
    }
}