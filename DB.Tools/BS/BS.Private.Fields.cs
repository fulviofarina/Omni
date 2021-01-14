

using Rsx.Dumb;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using static DB.LINAA;

/// <summary>
/// 
/// </summary>
namespace DB.Tools
{
    
   


    public partial class BS
    {

       
        private Hashtable bindings;

        private bool enabledControls = false;
        private Interface Interface;

        /// <summary>
        /// Notifies the controls the BS is busy calculating
        /// </summary>
        private bool isCalculating = false;

        /// <summary>
        /// checks for errors according to the IRow delegate
        /// </summary>
      //  private CheckerDelegate hasErrorsMethod = null;

      //  private delegate bool CheckerDelegate();

        private bool showErrors = true;


    }


  
}