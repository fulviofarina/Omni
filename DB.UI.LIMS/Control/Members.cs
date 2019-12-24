using DB.Tools;
using Rsx.DGV;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VTools;

namespace DB.UI
{
    public partial class LIMSUI
    {
        private static IAboutBox aboutBox;

        public static Interface Interface = null;

        public static IList<object> UserControls
        {
            get
            {
                return Creator.UserControls;
            }
        }

        public static void CreateAppForm(string productName, ref UserControl control, bool showAlready)
        {
            Creator.CreateAppForm(productName, ref control, showAlready);

        }
    }


    public partial class LIMSUI
    {
        /// <summary>
        /// only kept for compatibility with k0-X, remove when possible
        /// make necessary changes
        /// </summary>
        public static LINAA Linaa = null;

        public static IFind IFind = null;
        public static LIMS Form = null;
    }

    /// <summary>
    /// TRASH EVENTUALLY
    /// </summary>
    public partial class LIMSUI
    {
        public static void SaveWorkspaceXML(string file)
        {
            try
            {
                // this.Linaa.ToDoRes.Clear(); this.Linaa.ToDoResAvg.Clear();
                // this.Linaa.ToDoAvg.Clear(); this.Linaa.ToDoData.Clear();

                Linaa.WriteXml(file, System.Data.XmlWriteMode.WriteSchema);
                Interface.IReport.Msg("Workspace was saved on " + file, "Saved Workspace!", true);
            }
            catch (SystemException)
            {
                Interface.IReport.Msg("Workspace was NOT saved on " + file, "Not Saved Workspace!", false);
            }
        }
    }
}