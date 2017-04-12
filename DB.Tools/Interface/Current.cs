using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Security.Principal;
using System.Windows.Forms;
using DB.Properties;
using Msn;
using Rsx;
using static DB.LINAA;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

      public partial class Current 
    {

    

        private BindingSources bs;

    

        private Interface Interface;

        public Current(ref BindingSources bss, ref Interface interfaces)
        {
            bs = bss;
            Interface = interfaces;
        }

     

        public DataRow SubSample
        {
            get
            {
                return (bs.SubSamples.Current as DataRowView).Row;
            }
        }

        public IEnumerable<DataRow> SubSamples
        {
            get
            {
                return (bs.SubSamples.List as DataView).Table.AsEnumerable().OfType<DataRow>();
            }
        }

        public DataRow Unit
        {
            get
            {
                return (bs.Units.Current as DataRowView).Row;
            }
        }

        public IEnumerable<DataRow> Units
        {
            get
            {
                return (bs.Units.List as DataView).Table.AsEnumerable().OfType<DataRow>();
            }
        }

      

    
      
    }
}