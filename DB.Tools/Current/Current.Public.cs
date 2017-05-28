using System.Collections.Generic;
using System.Data;
using Rsx.Dumb;
using Rsx;
using System.Windows.Forms;

namespace DB.Tools
{
    /// <summary>
    /// This class gives the current row shown by a Binding Source
    /// </summary>

    public partial class Current  
    {

     

        /// <summary>
        /// Constructor
        /// </summary>
        public Current()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Current(ref BS bss, ref Interface interfaces)
        {
            bs = bss;
            Interface = interfaces;
        }
    }
   
}