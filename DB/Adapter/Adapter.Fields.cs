using DB.LINAATableAdapters;
using System.Collections;

namespace DB
{
    public partial class LINAA : IAdapter
    {
        protected Hashtable adapters;

        /// <summary>
        /// Queries of this dataset
        /// </summary>
        protected QTA qTA;

        /// <summary>
        /// The master Table Adapter Manager of this dataset
        /// </summary>
        protected TableAdapterManager tAM;

        protected System.Exception tAMException = null;

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        protected internal void adapter_FillError(object sender, System.Data.FillErrorEventArgs e)
        {
            try
            {
                object[] o = e.Values;
            }
            catch (System.SystemException ex)
            {
                this.AddException(ex);
            }
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        protected internal void adapter_RowUpdating(object sender, System.Data.SqlClient.SqlRowUpdatingEventArgs e)
        {
            try
            {
                object o = e.Row;
            }
            catch (System.SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}